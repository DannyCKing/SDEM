using System;
using System.Linq;
using System.Windows;
using SDEMViewModels.Global;
using SDEMViewModels.Messages;
using SDEMViewModels.Models;
using SDEMViewModels.TCPClient;

namespace SDEMViewModels.MessageHandlers
{
    public class AliveMessageHandler : BaseMessageHandler
    {
        private readonly AliveMessageParser MessageParser = new AliveMessageParser();
        public override string MessageHeaderType
        {
            get { return Constants.ALIVE_MESSAGE_HEADER; }
        }

        public override void HandleMessageType(MainChatViewModel mainChatViewModel, string message)
        {
            AliveMessageContent msg = MessageParser.ParseMessage(message) as AliveMessageContent;
            //Console.WriteLine(msg.Username);
            if (msg.SenderId == Settings.Instance.UserId)
            {
                // this message is from yourself
                // you can ignore it
            }
            else
            {
                // this message is from another
                var fromUser = mainChatViewModel.ChatUsers.FirstOrDefault(x => x.UserId == msg.SenderId);
                if (fromUser == null)
                {
                    var tcplistener = new TCPClientListener(msg.TCPServerAddress, msg.TCPPort);
                    fromUser = new ChatUser(msg.SenderId, msg.Username, tcplistener);
                    fromUser.TCPClient.StartClient();
                    Application.Current.Dispatcher.Invoke(new Action(() => { mainChatViewModel.ChatUsers.Add(fromUser); }));
                }

                fromUser.UserStatus = msg.CurrentStatus;
            }
        }
    }
}
