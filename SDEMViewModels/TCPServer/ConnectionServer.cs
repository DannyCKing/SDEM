using Crypt;
using SDEMViewModels.Global;
using SDEMViewModels.MessageHandlers;
using SDEMViewModels.Messages;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SDEMViewModels.TCPServer
{
    public class ConnectionServer
    {
        private SimpleTcpServer SimpleTcpServer;

        public string IPAddress;
        public int Port;
        private Guid Guid;
        private string Username;
        private MainChatViewModel MainChat;
        private readonly MessageHandlerFactory MessageHandlerFactory = new MessageHandlerFactory();
        private readonly PasswordConverter Coder = new PasswordConverter();

        public ConnectionServer(string myIPAddress, int myPort, Guid myGuid, string myUsername, MainChatViewModel mainChat)
        {
            IPAddress = myIPAddress;
            Port = myPort;
            Guid = myGuid;
            Username = myUsername;
            MainChat = mainChat;

            SimpleTcpServer = new SimpleTcpServer();
            SimpleTcpServer.ClientConnected += Server_ClientConnected;
            SimpleTcpServer.ClientDisconnected += Server_ClientDisconnected;
            SimpleTcpServer.DataReceived += SimpleTcpServer_DataReceived;

            try
            {
                SimpleTcpServer.Start(Settings.Instance.FinderTCPServerPort);
            }
            catch(Exception e)
            {
                // Server already started, likely 2 instances on one machine
                Logger.Log("Unable to start server.  Already started.  ", e);
                SimpleTcpServer.Start(Settings.Instance.FinderTCPServerPort + 1);
            }
        }

        private void SimpleTcpServer_DataReceived(object sender, Message e)
        {
            //get message
            var message = e.Data;
            var decodedMessage = XMLUtils.FormatXML(message, Coder);
            Logger.Log("SERVER received message " + decodedMessage);
            var handler = MessageHandlerFactory.GetMessageHandler(decodedMessage);
            handler.HandleMessage(MainChat, decodedMessage);
        }

        private void Server_ClientDisconnected(object sender, TcpClient e)
        {
            // for now do nothing
        }

        private void Server_ClientConnected(object sender, TcpClient e)
        {
            Logger.Log("SERVER - a client connected to server");
            var messageCreator = new AliveMessageCreator();
            var myIPAddress = IPAddress;
            var myPort = Port;
            var myId = Guid;
            var myUsername = Username;
            var messageContent = new AliveMessageContent(myIPAddress, myPort, myId, myUsername);
            var message = messageCreator.CreateMessage(messageContent);
            var encryptedString = new PasswordConverter().Encrypt(message);
            var aliveMessage = Encoding.ASCII.GetBytes(encryptedString);

            try
            {
                e.Client.Send(aliveMessage);
                Logger.Log("SERVER message sent to client " + message);
            }
            catch(Exception sendMessageException)
            {
                Logger.Log("SERVER Unable to send connect message", sendMessageException);
            }
        }

    }
}
