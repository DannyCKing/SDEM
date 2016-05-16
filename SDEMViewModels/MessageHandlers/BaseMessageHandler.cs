using System;
using System.Linq;
using System.Xml.Linq;
using SDEMViewModels.Global;

namespace SDEMViewModels.MessageHandlers
{
    public abstract class BaseMessageHandler : IMessageHandler
    {
        public abstract string MessageHeaderType { get; }
        public abstract void HandleMessageType(MainChatViewModel mainChatViewModel, string message);

        public string MessageHeader
        {
            get
            {
                return MessageHeaderType;
            }
        }

        public void HandleMessage(MainChatViewModel mainChatViewModel, string message)
        {
            HandleMessageType(mainChatViewModel, message);
        }

        public bool IsMessageThisType(string message)
        {
            XDocument doc;
            try
            {
                doc = XDocument.Parse(message);
            }
            catch (Exception e)
            {
                return false;
            }
            var messageElement = doc.Root.Descendants().FirstOrDefault(x => x.Name == Constants.MESSAGE_TYPE_HEADER);

            if (messageElement.Value == MessageHeader)
                return true;
            else
                return false;

        }
    }
}
