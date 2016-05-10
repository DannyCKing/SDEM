using System;
using System.Linq;
using System.Windows;
using SDEMViewModels.Global;
using SDEMViewModels.Messages;
using SDEMViewModels.Models;

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
            AliveMessageContent messageContent = MessageParser.ParseMessage(message) as AliveMessageContent;
            if (messageContent.SenderId == Settings.Instance.UserId)
            {
                // this message is from yourself
                // you can ignore it
            }
            else
            {
                // this message is from another
                var fromUser = mainChatViewModel.ChatUsers.FirstOrDefault(x => x.UserId == messageContent.SenderId);
                if (fromUser == null)
                {
                    fromUser = new ChatUser() { UserId = messageContent.SenderId, UserName = messageContent.Username };
                    Application.Current.Dispatcher.Invoke(new Action(() => { mainChatViewModel.ChatUsers.Add(fromUser); }));
                }

                fromUser.UserStatus = messageContent.CurrentStatus;
            }
        }
    }
}
