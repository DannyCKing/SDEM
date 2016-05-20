using System;
using System.Linq;
using SDEMNotifications;
using SDEMViewModels.Global;
using SDEMViewModels.Messages;
using Windows.UI.Notifications;

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
            DispatchService.Invoke(new Action(() =>
            {
                conversation.Messages.Add(new MessageViewModel(chatUser.Username, msg.Message, msg.MessageCreatedDate, sameSender));
            }));

            ShowToast(chatUser.Username, msg.Message);
        }

        private void ShowToast(string sender, string message)
        {
            // Get a toast XML template
            Windows.Data.Xml.Dom.XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);

            var xml = DirectMessageNotification.GetDirectNotificationXml(sender, message);
            // Specify the absolute path to an image
            //String imagePath = "file:///" + Path.GetFullPath("toastImageAndText.png");
            //XmlNodeList imageElements = toastXml.GetElementsByTagName("image");

            var xmlDocument = new Windows.Data.Xml.Dom.XmlDocument();
            xmlDocument.LoadXml(xml);
            ToastNotification toast = new ToastNotification(xmlDocument);
            //toast.ExpirationTime = DateTimeOffset.Now;

            ToastNotificationManager.CreateToastNotifier("SDEM").Show(toast);
        }
    }
}
