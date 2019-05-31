using Crypt;
using SDEMViewModels.Global;
using SDEMViewModels.MessageHandlers;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDEMViewModels.TCPClient
{
    class ConnectionClient : BaseSocketClient
    {
        MainChatViewModel MainChat = null;

        public ConnectionClient(string ipAddress, int port, MainChatViewModel mainChat, byte[] messageToSend)
            :base(ipAddress, port, "Connection Client")
        {
            MessageToSend = messageToSend;
            MainChat = mainChat;
        }

        public override void ParseMessage(byte[] message)
        {
            var decodedMessage =  XMLUtils.FormatXML(message, Coder);
            Logger.Log("CLIENT message from server " + decodedMessage);
            var handler = MessageHandlerFactory.GetMessageHandler(decodedMessage);
            handler.HandleMessage(MainChat, decodedMessage);
        }

        private readonly MessageHandlerFactory MessageHandlerFactory = new MessageHandlerFactory();
        private readonly PasswordConverter Coder = new PasswordConverter();
    }
}
