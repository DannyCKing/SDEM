using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Caching; // add this library from the reference tab
using System.Text;
using System.Threading.Tasks;
using Crypt;
using SDEMViewModels.Global;
using SDEMViewModels.MessageHandlers;
using SDEMViewModels.Messages;

namespace SDEMViewModels
{
    public class Discoverer
    {
        private static MessageHandlerFactory MessageHandlerFactory = new MessageHandlerFactory();

        private static AliveMessageCreator AliveMessageCreator = new AliveMessageCreator();

        private static string MulticastIPAddress;

        private static int MulticastPort;

        private static string TCPIPAddress;

        private static int TCPPort;

        private static Guid MyIdentifier;

        private static PasswordConverter Coder = new PasswordConverter();

        static UdpClient _UdpClient;
        static MemoryCache _Peers = new MemoryCache("_PEERS_");

        public static Action<string> PeerJoined = null;
        public static Action<string> PeerLeft = null;

        public static Action<string> MessageRecieved = null;

        public static void Start(string multiCastIP, int multicastPort, Guid identifier, string tcpAddress, int tcpPort)
        {
            MulticastIPAddress = multiCastIP;
            MulticastPort = multicastPort;
            MyIdentifier = identifier;
            TCPIPAddress = tcpAddress;
            TCPPort = tcpPort;

            _UdpClient = new UdpClient();

            try
            {
                _UdpClient.ExclusiveAddressUse = false;
                _UdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                _UdpClient.Client.Bind(new IPEndPoint(IPAddress.Any, MulticastPort));

                _UdpClient.JoinMulticastGroup(IPAddress.Parse(MulticastIPAddress));

                Task.Run(() => Receiver());
                Task.Run(() => Sender());
            }
            catch
            {
                Settings.IsTestAccount = true;
                // Just start tcp chat server
                _UdpClient.ExclusiveAddressUse = false;
                _UdpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                _UdpClient.Client.Bind(new IPEndPoint(IPAddress.Any, MulticastPort));

                _UdpClient.JoinMulticastGroup(IPAddress.Parse(MulticastIPAddress));

                Task.Run(() => Receiver());
                Task.Run(() => Sender());
            }
        }

        static void Sender()
        {
            var aliveMessageContent = new AliveMessageContent(TCPIPAddress, TCPPort, Settings.Instance.UserId, Settings.Instance.Username);
            var aliveMessageString = AliveMessageCreator.CreateMessage(aliveMessageContent);
            var encryptedString = Coder.Encrypt(aliveMessageString);
            var aliveMessage = Encoding.ASCII.GetBytes(encryptedString);
            IPEndPoint mcastEndPoint = new IPEndPoint(IPAddress.Parse(MulticastIPAddress), MulticastPort);

            while (true)
            {
                _UdpClient.Send(aliveMessage, aliveMessage.Length, mcastEndPoint);
                Task.Delay(1000).Wait();
            }
        }

        static void Receiver()
        {
            var from = new IPEndPoint(0, 0);
            while (true)
            {
                var message = _UdpClient.Receive(ref from);
                var messageAsString = XMLUtils.FormatXML(message, Coder);
                if (MessageRecieved != null)
                {
                    MessageRecieved(messageAsString);
                }

                //Console.WriteLine(messageAsString);
                //var handler = MessageHandlerFactory.GetMessageHandler(messageAsString);

                // cache item
                var cacheItem = new CacheItem(from.Address.ToString(), from);

                // cache policy
                var expiration = TimeSpan.FromSeconds(20);
                var cachePolity = new CacheItemPolicy
                {
                    SlidingExpiration = expiration,
                    RemovedCallback = RemovedCallback
                };

                if (_Peers.Add(cacheItem, cachePolity))
                {
                    if (PeerJoined != null)
                    {
                        PeerJoined(from.Address.ToString());
                    }
                }

                //Console.WriteLine(from.Address.ToString());
            }
        }

        private static void RemovedCallback(CacheEntryRemovedArguments removeArgs)
        {
            if (PeerLeft != null)
            {
                PeerLeft(removeArgs.CacheItem.Key);
            }
        }
    }
}
