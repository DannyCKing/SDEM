using System;
using System.Xml.Linq;

namespace SDEMViewModels.Messages
{
    public class DirectMessageParser : IMessageParser
    {
        //        private const string ALIVE_MESSAGE =
        //        @"<Message>
        //                    <MessageType>{0}</MessageType>
        //                    <MessageDetails>
        //                        <SenderId>{1}</SenderId>
        //                        <MessageId>{2}</MessageId>
        //                        <Message>{3}</Message>
        //                        <DateCreated>{3}</DateCreated>
        //                        <Other>{4}</Other>
        //                    </MessageDetails>
        //                </Message>";

        public IMessageContent ParseMessage(string message)
        {
            var xml = XDocument.Parse(message);
            var details = xml.Root.Element("MessageDetails");
            var senderId = Guid.Parse(details.Element("SenderId").Value.ToString());
            var messageId = Guid.Parse(details.Element("MessageId").Value.ToString());
            var messageContents = details.Element("Message").Value.ToString();
            var dateCreated = DateTime.Now;// DateTime.Parse(details.Element("DateCreated").Value.ToString());
            var parsedMessage = new DirectMessageContent(senderId, messageContents, messageId, dateCreated);

            return parsedMessage;
        }
    }
}
