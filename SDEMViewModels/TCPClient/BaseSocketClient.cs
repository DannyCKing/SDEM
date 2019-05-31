using SDEMViewModels.Global;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace SDEMViewModels.TCPClient
{
    public abstract class BaseSocketClient 
    {
        protected SimpleTcpClient Client;

        protected byte[] MessageToSend = null;
        private Timer ConnectTimer;

        private string IPAddress;
        private int Port;

        private bool _Connected = false;

        BackgroundWorker _ConnectBackgroundWorker;


        public bool Connected
        {
            get
            {
                return _Connected;
            }
            private set
            {
                _Connected = value;
            }
        }

        private string _Name;

        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        public BaseSocketClient(string ipAddress, int port, string socketName) : base()
        {
            IPAddress = ipAddress;
            Port = port;
            Name = socketName;

            Client = new SimpleTcpClient();
            Client.DataReceived += Client_DataReceived;

            _ConnectBackgroundWorker = new BackgroundWorker();
            _ConnectBackgroundWorker.DoWork += _ConnectBackgroundWorker_DoWork;
            _ConnectBackgroundWorker.RunWorkerCompleted += _ConnectBackgroundWorker_RunWorkerCompleted;
            _ConnectBackgroundWorker.RunWorkerAsync();

            ConnectTimer = new Timer(5000);
            ConnectTimer.Elapsed += ConnectTimer_Elapsed;
            ConnectTimer.Interval = 5000; // in miliseconds
            ConnectTimer.Start();
            ConnectTimer_Elapsed(this, null);
        }

        private void ConnectTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_ConnectBackgroundWorker.IsBusy)
            {
                //don't try to connect again if already trying
                return;
            }

            if (!GetIsConnected())
            {
                Connected = false;
                try
                {
                    _ConnectBackgroundWorker = new BackgroundWorker();
                    _ConnectBackgroundWorker.DoWork += _ConnectBackgroundWorker_DoWork;
                    _ConnectBackgroundWorker.RunWorkerCompleted += _ConnectBackgroundWorker_RunWorkerCompleted;
                    _ConnectBackgroundWorker.RunWorkerAsync();
                    //Connected = true;
                }
                catch (Exception ex)
                {
                    Connected = false;
                    Logger.Log("Could not connect to " + IPAddress + " port " + Port, ex);
                }
            }
            else
            {
                Connected = true;
            }
        }

        bool firstTime = true;
        private void _ConnectBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Connected = (bool)e.Result;
            if(Connected && firstTime)
            {
                Logger.Log("CLIENT Connected");

                firstTime = false;
                try
                {
                    Client.Write(MessageToSend);
                }
                catch(Exception connectError)
                {
                    Logger.Log("CLIENT Could not send connect message", connectError);
                }
            }
        }

        private void _ConnectBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Client.Connect(IPAddress, Port);
                e.Result = true;

            }
            catch (Exception ex)
            {
                Logger.Log("CLIENT Could not connect to " + IPAddress + " port " + Port, ex);
                e.Result = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_ConnectBackgroundWorker.IsBusy)
            {
                //don't try to connect again if already trying
                return;
            }

            if (!GetIsConnected())
            {
                Connected = false;
                try
                {
                    _ConnectBackgroundWorker = new BackgroundWorker();
                    _ConnectBackgroundWorker.DoWork += _ConnectBackgroundWorker_DoWork;
                    _ConnectBackgroundWorker.RunWorkerCompleted += _ConnectBackgroundWorker_RunWorkerCompleted;
                    _ConnectBackgroundWorker.RunWorkerAsync();
                    //Connected = true;
                }
                catch (Exception ex)
                {
                    Connected = false;
                    Logger.Log("Could not connect to " + IPAddress + " port " + Port, ex);
                }
            }
            else
            {
                Connected = true;
            }
        }

        public bool GetIsConnected()
        {
            if (Client == null || Client.TcpClient == null)
            {
                return false;
            }
            TcpClient client = Client.TcpClient;
            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections().Where(x => x.LocalEndPoint.Equals(client.Client.LocalEndPoint) && x.RemoteEndPoint.Equals(client.Client.RemoteEndPoint)).ToArray();

            if (tcpConnections != null && tcpConnections.Length > 0)
            {
                TcpState stateOfConnection = tcpConnections.First().State;
                if (stateOfConnection == TcpState.Established)
                {
                    // Connection is OK
                    return true;
                }
                else
                {
                    // No active tcp Connection to hostName:port
                    return false;
                }

            }
            return false;
        }

        private void Client_DataReceived(object sender, Message e)
        {
            var dataReceived = e.MessageString;
            try
            {
                ParseMessage(e.Data);
            }
            catch (Exception parseException)
            {
                Logger.Log("CLIENT Exception on parse.", parseException);
            }
        }

        public abstract void ParseMessage(byte [] data);

        internal void Disconnect()
        {
            try
            {
                Client.Disconnect();
                Connected = false;
            }
            catch (Exception e)
            {
                Logger.Log("CLIENT Error disconnect socket.");
            }
        }
    }
}
