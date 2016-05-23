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

            MessageQueue.Instance.AddMessage(msg, mainChatViewModel);
            //HandleMessage(mainChatViewModel, msg);
        }

        public void HandleMessage(MainChatViewModel mainChatViewModel, DirectMessageContent directMessage)
        {
            //find conversation
            var chatUser = mainChatViewModel.ChatUsers.FirstOrDefault(x => x.UserId == directMessage.SenderId);
            var conversation = mainChatViewModel.Conversations[chatUser];
            var sameSender = conversation.Messages.Last().Sender == chatUser.Username;
            DispatchService.Invoke(new Action(() =>
            {
                conversation.Messages.Add(new MessageViewModel(chatUser.Username, directMessage.Message, directMessage.MessageCreatedDate, sameSender));
            }));

            //ShowToast(chatUser.Username, directMessage.Message);
            ShowToast("SDEM", chatUser.Username + " sent you a message");

        }

        private void ShowToast(string sender, string message)
        {
            //if (Settings.Instance.DesktopNotifications == false)
            //    return;

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
