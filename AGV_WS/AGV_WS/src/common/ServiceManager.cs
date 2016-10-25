﻿using AGV_WS.src.utils;
using RemotePLC.src.comm.protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace AGV_WS.src.common
{
    public class ServiceManager
    {
        private Socket _server;
        private List<AgvSession> _threads;

        private ServiceManager()
        {
            _server = null;
            _threads = new List<AgvSession>();
        }
        public static readonly ServiceManager instance = new ServiceManager();

        public void Start()
        {
            try
            {
                if (_server == null)
                {
                    _server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint iep = new IPEndPoint(IPAddress.Any, Properties.Settings.Default.SocketPort);
                    _server.Bind(iep);

                    _server.Listen(20);
                    _server.BeginAccept(new AsyncCallback(accept), _server);
                }
                else
                {
                    Logger.Debug("do Stop first.");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }

        }

        public void Stop()
        {
            if (_server != null)
            {
                _server.Close();
                _server = null;
            }
            else
            {
                Logger.Debug("do Start first.");
            }
        }

        private void accept(IAsyncResult iar)
        {
            try
            {
                //还原传入的原始套接字
                Socket server = (Socket)iar.AsyncState;
                //在原始套接字上调用EndAccept方法，返回新的套接字
                Socket socket = server.EndAccept(iar);

                Logger.Debug("Accept [{0}]", socket.RemoteEndPoint.ToString());

                Receive(socket);

            }
            catch (Exception e)
            {
                Logger.Debug(e.ToString());
            }
            finally
            {
                if (_server != null && _server.IsBound)
                {
                    _server.BeginAccept(new AsyncCallback(accept), _server);
                }
            }
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
                        UInt64 agv_id;
                        if (_getAgvId(state.buffer, bytesRead, out agv_id))
                        {
                            SessionManager.instance.Sessions.Add(new AgvSession(agv_id, client));
                        }
                        else
                        {
                            // Get the rest of the data.     
                            client.BeginReceive(state.buffer, 0, Properties.Settings.Default.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }
        }

        private bool _getAgvId(byte[] buffer, int length, out UInt64 agv_id)
        {
            agv_id = 0;
            Package package = Package.decode(buffer, length);
            if (package != null)
            {
                byte[] src = package.Src;
                Array.Reverse(src);
                agv_id = BitConverter.ToUInt64(src, 0);
                return true;
            }
            return false;

        }
    }
}

