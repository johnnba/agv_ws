using AGV_WS.src.utils;
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
        private ServiceManager()
        {
            _server = null;
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
                    _server.Accept();
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
            try
            {
                if (_server != null)
                {
                    _server.Dispose();
                    _server = null;
                }
                else
                {
                    Logger.Debug("do Start first.");
                }
            }
            catch (Exception e)
            {
                Logger.Error(e.ToString());
            }

        }

        private void accept(IAsyncResult iar)
        {
            //还原传入的原始套接字
            Socket server = (Socket)iar.AsyncState;
            //在原始套接字上调用EndAccept方法，返回新的套接字
            Socket socket = server.EndAccept(iar);
        }

    }
}

