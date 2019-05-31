using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Crypt;
using SDEMViewModels.Global;
using SDEMViewModels.MessageHandlers;
using SDEMViewModels.Messages;
using SDEMViewModels.Models;
using SDEMViewModels.TCPClient;
using SDEMViewModels.TCPServer;

namespace SDEMViewModels
{
    public class MainChatViewModel : NotifyPropertyChanged
    {
        private ConnectionClient CurrentConnectionClient;
        public ConnectionServer ConnectionServer;

        private Dictionary<ChatUser, ConversationViewModel> _Conversations;
        private ObservableCollection<ChatUser> _ChatUsers;
        private ConversationViewModel _CurrentConversation;
        private string _MulticastIpAddress;
        private int _MulticastPort;
        public static Thread _SocketThread;
        public ChatUser _SelectedChatUser;
        private TCPServerListener _TCPServer;
        private string _TCPServerAddress;
        private int _TCPServerPort;
        private string _Username;
        private readonly MessageHandlerFactory MessageHandlerFactory = new MessageHandlerFactory();

        public Guid MyIdentifier { get; set; }

        #region Convesersations

        public Dictionary<ChatUser, ConversationViewModel> Conversations
        {
            get
            {
                if (_Conversations == null)
                {
                    _Conversations = new Dictionary<ChatUser, ConversationViewModel>();
                }

                if (_Conversations.Count == 1)
                    CurrentConversation = _Conversations[ChatUsers[0]];

                return _Conversations;
            }
            set
            {
                if (_Conversations == value)
                    return;

                _Conversations = value;
                RaisePropertyChanged("Conversations");
            }
        }

        #endregion

        #region Chat Users

        public ObservableCollection<ChatUser> ChatUsers
        {
            get
            {
                if (_ChatUsers == null)
                {
                    _ChatUsers = new ObservableCollection<ChatUser>();
                }

                return _ChatUsers;
            }
            set
            {
                if (value == _ChatUsers)
                    return;

                _ChatUsers = value;

                RaisePropertyChanged("ChatUsers");
            }
        }

        #endregion

        public ConversationViewModel CurrentConversation
        {
            get
            {
                return _CurrentConversation;
            }
            set
            {
                if (_CurrentConversation == value)
                    return;

                _CurrentConversation = value;
                RaisePropertyChanged("CurrentConversation");

            }
        }

        #region MulticastIPAddress

        public string MulticastIPAddress
        {
            get
            {
                return _MulticastIpAddress;
            }
            set
            {
                if (_MulticastIpAddress == value)
                    return;

                _MulticastIpAddress = value;
                RaisePropertyChanged("MulticastIPAddress");
            }
        }

        #endregion

        #region MulticastPort

        public int MulticastPort
        {
            get
            {
                return _MulticastPort;
            }
            set
            {
                if (_MulticastPort == value)
                    return;

                _MulticastPort = value;
                RaisePropertyChanged("MulticastPort");
            }
        }

        #endregion

        #region SelectedChatUser

        public ChatUser SelectedChatUser
        {
            get
            {
                return _SelectedChatUser;
            }
            set
            {
                if (value == _SelectedChatUser)
                    return;

                _SelectedChatUser = value;
                RaisePropertyChanged("SelectedChatUser");

                // Update main conversation view model 
                // to the selected user
                if (!Conversations.ContainsKey(_SelectedChatUser))
                {
                    Conversations.Add(_SelectedChatUser, new ConversationViewModel(_SelectedChatUser));
                }
                CurrentConversation = Conversations[_SelectedChatUser];
            }
        }

        #endregion

        #region TCPServerAddress

        public string TCPServerAddress
        {
            get
            {
                return _TCPServerAddress;
            }
            set
            {
                if (value == _TCPServerAddress)
                    return;

                _TCPServerAddress = value;
                RaisePropertyChanged("TCPServerAddress");
            }
        }

        #endregion

        #region TCPServerPort

        public int TCPServerPort
        {
            get
            {
                return _TCPServerPort;
            }
            set
            {
                if (value == _TCPServerPort)
                    return;

                _TCPServerPort = value;
                RaisePropertyChanged("TCPServerPort");
            }
        }

        #endregion

        #region Username

        public string Username
        {
            get
            {
                return _Username;
            }
            set
            {
                if (_Username == value)
                    return;

                _Username = value;
                RaisePropertyChanged("Username");
            }
        }

        #endregion

        public static Action<string> PeerJoined = (x) =>
        {

        };

        public MainChatViewModel(string multicastIPAddress, int multiCastPort, int tcpPort)
        {
            Username = Settings.Instance.Username;
            MulticastIPAddress = multicastIPAddress;
            MulticastPort = multiCastPort;

            Discoverer.PeerJoined = PeerJoined;
            Discoverer.PeerLeft = PeerLeftRoom;
            Discoverer.MessageRecieved = this.MessageRecieved;

            MyIdentifier = Settings.Instance.UserId;

            TCPServerPort = tcpPort;
            TCPServerAddress = GetPublicIP();
            _TCPServer = new TCPServerListener(this, tcpPort);
            _SocketThread = new Thread(_TCPServer.StartListener);
            _SocketThread.Start();
            //Discoverer.Start(MulticastIPAddress, MulticastPort, MyIdentifier, TCPServerAddress, TCPServerPort);

            Logger.Log("Starting server with guid " + MyIdentifier);
            ConnectionServer = new ConnectionServer(GetPublicIP(), Settings.Instance.FinderTCPServerPort, MyIdentifier, Settings.Instance.Username, this);
        }

        public void MessageRecieved(string message)
        {
            var handler = MessageHandlerFactory.GetMessageHandler(message);
            handler.HandleMessage(this, message);
        }

        private void PeerLeftRoom(string message)
        {

        }

        public string GetPublicIP()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("10.0.2.4", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return localIP;
        }

        public void ConnectToServer(string ipAddress, int port)
        {
            var messageCreator = new AliveMessageCreator();
            var myIPAddress = GetPublicIP();
            var myPort = Settings.Instance.TCPServerPort;
            var myId = Settings.Instance.UserId;
            var myUsername = Settings.Instance.Username;
            var messageContent = new AliveMessageContent(myIPAddress, myPort, myId, myUsername);
            var message = messageCreator.CreateMessage(messageContent);
            var encryptedString = new PasswordConverter().Encrypt(message);
            var aliveMessage = Encoding.ASCII.GetBytes(encryptedString);
            CurrentConnectionClient = new ConnectionClient(ipAddress, port, this, aliveMessage);

        }
    }
}
