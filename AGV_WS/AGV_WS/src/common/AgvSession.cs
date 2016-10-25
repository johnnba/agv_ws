using AGV_WS.src.model;
using AGV_WS.src.protocol;
using AGV_WS.src.utils;
using RemotePLC.src.comm.protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace AGV_WS.src.common
{
    public class StateObject
    {
        // Client   socket.
        public Socket workSocket = null;
        // Receive buffer.
        public byte[] buffer = new byte[Properties.Settings.Default.BufferSize];
    }
    public class AgvSession : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private UInt64 _agv_id;

        private Thread t;
        private bool _finish;
        private bool _stopflag;
        private Socket _socket;

        public UInt64 AgvId { get { return _agv_id; } }
        public string SocketInfo
        {
            get
            {
                if (_socket != null && _socket.Connected)
                {
                    return _socket.RemoteEndPoint.ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        public AgvSession(UInt64 agvId, Socket socket)
        {
            _agv_id = agvId;

            _socket = socket;
            t = new Thread(run);
            _finish = true;
            _stopflag = false;
        }

        public void Start()
        {
            if (_finish)
            {
                _finish = false;
                _stopflag = false;

                t.Start();
            }
        }

        public void Stop()
        {
            _stopflag = true;

            try
            {
                _socket.Close();
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

        private void run()
        {
            while (!_stopflag)
            {

            }
            _finish = true;
        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.     
                StateObject state = new StateObject();
                state.workSocket = client;
                // Begin receiving the data from the remote device.     
                client.BeginReceive(state.buffer, 0, Properties.Settings.Default.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket     
                // from the asynchronous state object.     
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;
                // Read data from the remote device.     
                if (client != null && client.Connected)
                {
                    int bytesRead = client.EndReceive(ar);
                    if (bytesRead > 0)
                    {
                        // There might be more data, so store the data received so far.     
                        _doMessageProcess(state.buffer, bytesRead);

                        // Get the rest of the data.     
                        client.BeginReceive(state.buffer, 0, Properties.Settings.Default.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

        public void _doMessageProcess(byte[] buffer, int length)
        {
        }


        public void Send(byte[] buffer, int length)
        {
            try
            {
                _socket.Send(buffer, length, SocketFlags.None);
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

        public override string ToString()
        {
            return _socket.RemoteEndPoint.ToString();
        }

        public void SendPlan(AgvPlan plan)
        {
            try
            {
                Package package = Package.encode(_agv_id, Protocol.ID_SENDPLAN_NTC, plan.ToJsonBytes());
                _socket.Send(package.ToSocketData());
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }
        public void print()
        {
            Logger.Info("session: agv_id:[{0}] {1}", _agv_id, _socket.RemoteEndPoint);
        }
    }
}
