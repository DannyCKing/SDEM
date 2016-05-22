using System;
using System.Xml.Linq;

namespace SDEMViewModels.Messages
{
    public class DirectMessageParser : IMessageParser
    {
        //                @"<?xml version=""1.0"" encoding=""ASCII"" standalone=""yes"" ?>
        //                <Message>
        //                    <MessageType>{0}</MessageType>
        //                    <MessageDetails>
        //                        <SenderId>{1}</SenderId>
        //                        <MessageId>{2}</MessageId>
        //                        <Message>{3}</Message>
        //                        <DateCreated>{4}</DateCreated>
        //                        <Other>{5}</Other>
        //                        <MessageNumber>{6}</MessageNumber>
        //                        <TotalMessages><{7}</TotalMessages>
        //                    </MessageDetails>
        //                </Message>";

        public IMessageContent ParseMessage(string message)
        {
            var xml = XDocument.Parse(message);
            var details = xml.Root.Element("MessageDetails");
            var senderId = Guid.Parse(details.Element("SenderId").Value.ToString());
            var messageId = Guid.Parse(details.Element("MessageId").Value.ToString());
            var messageContents = details.Element("Message").Value.ToString();
            var part = int.Parse(details.Element("MessageNumber").Value.ToString());
            var total = int.Parse(details.Element("TotalMessages").Value.ToString());
            var dateCreated = DateTime.Now;// DateTime.Parse(details.Element("DateCreated").Value.ToString());
            var parsedMessage = new DirectMessageContent(senderId, messageContents, messageId, dateCreated, part, total);

            return parsedMessage;
        }
    }
}
