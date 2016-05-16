using System;
using System.Linq;
using System.Windows;
using SDEMViewModels.Global;
using SDEMViewModels.Messages;

namespace SDEMViewModels.MessageHandlers
{
    public class DirectMessageHandler : BaseMessageHandler
    {
        private readonly DirectMessageParser MessageParser = new DirectMessageParser();
        public override string MessageHeaderType
        {
            get { return Constants.DIRECT_MESSAGE_HEADER; }
        }

        public override void HandleMessageType(MainChatViewModel mainChatViewModel, string message)
        {
            DirectMessageContent msg = MessageParser.ParseMessage(message) as DirectMessageContent;

            //find conversation
            var chatUser = mainChatViewModel.ChatUsers.FirstOrDefault(x => x.UserId == msg.SenderId);
            var conversation = mainChatViewModel.Conversations[chatUser];
            var sameSender = conversation.Messages.Last().Sender == chatUser.Username;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                conversation.Messages.Add(new MessageViewModel(chatUser.Username, msg.Message, msg.MessageCreatedDate, sameSender));
            }));
        }
    }
}
