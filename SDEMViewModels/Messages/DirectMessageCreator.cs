using System.Collections.Generic;
using System.Linq;
using SDEMViewModels.Global;

namespace SDEMViewModels.Messages
{
    public class DirectMessageCreator : IMessageCreator
    {
        private const string DIRECT_MESSAGE =
                @"<?xml version=""1.0"" encoding=""ASCII"" standalone=""yes"" ?>
                <Message>
                    <MessageType>{0}</MessageType>
                    <MessageDetails>
                        <SenderId>{1}</SenderId>
                        <MessageId>{2}</MessageId>
                        <Message>{3}</Message>
                        <DateCreated>{4}</DateCreated>
                        <MessageNumber>{5}</MessageNumber>
                        <TotalMessages>{6}</TotalMessages>
                        <Other>{7}</Other>
                    </MessageDetails>
                </Message>";

        public string[] CreateMessage(IMessageContent arguments)
        {
            List<string> xmlStrings = new List<string>();
            DirectMessageContent msg = arguments as DirectMessageContent;

            var senderId = msg.SenderId;
            var messageId = msg.MessageId;
            var entireXml = msg.Message;
            var list = Split(entireXml, 100);
            var timeString = msg.MessageCreatedDate.ToLongDateString();
            int intPart = 1;
            int totalParts = list.Count();
            foreach (var part in list)
            {
                var encodedMessage = System.Security.SecurityElement.Escape(part);
                var xml = string.Format(DIRECT_MESSAGE, Constants.DIRECT_MESSAGE_HEADER, senderId, messageId, encodedMessage, timeString, intPart, totalParts, MessageHelper.GetExtraFluff());
                xmlStrings.Add(xml);
                intPart++;
            }
            return xmlStrings.ToArray();
        }

        static IEnumerable<string> Split(string entireString, int chunkSize)
        {
            List<string> stringsToReturn = new List<string>();
            int start = 0;
            while (entireString.Length > 0)
            {
                string portion = "";
                if (entireString.Length > chunkSize)
                {
                    portion = entireString.Substring(start, chunkSize);
                    entireString = entireString.Remove(start, chunkSize);
                }
                else
                {
                    portion = entireString;
                    entireString = "";
                }

                stringsToReturn.Add(portion);

            }

            return stringsToReturn;
        }

        string IMessageCreator.CreateMessage(IMessageContent arguments)
        {
            throw new System.NotImplementedException();
        }
    }
}
