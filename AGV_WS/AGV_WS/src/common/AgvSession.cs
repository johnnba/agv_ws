using AGV_WS.src.model;
using AGV_WS.src.protocol;
using AGV_WS.src.utils;
using RemotePLC.src.comm.protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

        //private Thread t;
        //private bool _finish;
        //private bool _stopflag;
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
            AgvInfoManager.instance.SetOnline(agvId, true);

            _agv_id = agvId;

            _socket = socket;
            //t = new Thread(run);
            //_finish = true;
            //_stopflag = false;

            Receive(_socket);
        }

        //public void Start()
        //{
        //    if (_finish)
        //    {
        //        _finish = false;
        //        _stopflag = false;

        //        t.Start();
        //    }
        //}

        //public void Stop()
        //{
        //    _stopflag = true;

        //    try
        //    {
        //        _socket.Dispose();
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.Error(e.ToString());
        //    }
        //}

        //private void run()
        //{
        //    Receive(_socket);

        //    while (!_stopflag)
        //    {

        //    }
        //    _finish = true;
        //}

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
            List<Package> packages = Package.decode(buffer, length);
            foreach (Package package in packages)
            {
                if (package.Id == Protocol.ID_STATUS_INFO_NTC)
                {
                    MemoryStream ms = new MemoryStream(package.Payload);
                    byte[] lastStatus = new byte[2];
                    ms.Read(lastStatus, 0, lastStatus.Length);
                    byte[] currentStatus = new byte[2];
                    ms.Read(currentStatus, 0, currentStatus.Length);
                    Array.Reverse(currentStatus);
                    UInt16 status = BitConverter.ToUInt16(currentStatus, 0);

                    AgvInfoManager.instance.SetAgvStatus(_agv_id, status);
                }
                else if (package.Id == Protocol.ID_STATION_INFO_NTC)
                {
                    MemoryStream ms = new MemoryStream(package.Payload);
                    byte[] cardId = new byte[2];
                    ms.Read(cardId, 0, cardId.Length);
                    Array.Reverse(cardId);
                    UInt16 card_id = BitConverter.ToUInt16(cardId, 0);

                    AgvInfoManager.instance.SetAgvPosition(_agv_id, card_id);
                }
                else if (package.Id == Protocol.ID_USONIC_INFO_NTC)
                {
                    MemoryStream ms = new MemoryStream(package.Payload);
                    byte[] usonic = new byte[2];
                    ms.Read(usonic, 0, usonic.Length);
                    Array.Reverse(usonic);
                    UInt16 ultrasonic = BitConverter.ToUInt16(usonic, 0);

                    AgvInfoManager.instance.SetAgvUltraSonic(_agv_id, ultrasonic);
                }
                else if (package.Id == Protocol.ID_SPEED_INFO_NTC)
                {
                    MemoryStream ms = new MemoryStream(package.Payload);
                    byte[] speed = new byte[2];
                    ms.Read(speed, 0, speed.Length);
                    Array.Reverse(speed);
                    UInt16 uSpeed = BitConverter.ToUInt16(speed, 0);

                    AgvInfoManager.instance.SetAgvSpeed(_agv_id, uSpeed);
                }
                else if (package.Id == Protocol.ID_VOLTAGE_INFO_NTC)
                {
                    MemoryStream ms = new MemoryStream(package.Payload);
                    byte[] voltage = new byte[2];
                    ms.Read(voltage, 0, voltage.Length);
                    Array.Reverse(voltage);
                    UInt16 uVoltage = BitConverter.ToUInt16(voltage, 0);

                    AgvInfoManager.instance.SetAgvVoltage(_agv_id, uVoltage);
                }
            }
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

                AgvInfoManager.instance.SetAgvPlan(_agv_id, plan.planid);
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
