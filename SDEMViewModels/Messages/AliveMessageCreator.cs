using System;
using SDEMViewModels.Global;

namespace SDEMViewModels.Messages
{
    public class AliveMessageCreator : IMessageCreator
    {
        private const string ALIVE_MESSAGE =
                @"<Message>
                    <MessageType>{0}</MessageType>
                    <MessageDetails>
                        <SenderId>{1}</SenderId>
                        <IPAddress>{2}</IPAddress>
                        <Port>{3}</Port>
                        <Username>{4}</Username>
                        <Date>{5}</Date>
                        <CurrentStatus>{6}</CurrentStatus>
                    </MessageDetails>
                </Message>";

        public string CreateMessage(IMessageContent arguments)
        {
            AliveMessageContent msg = arguments as AliveMessageContent;

            return string.Format(ALIVE_MESSAGE, Constants.ALIVE_MESSAGE_HEADER, msg.SenderId, msg.TCPServerAddress, msg.TCPPort, msg.Username, DateTime.Now, msg.CurrentStatus);
        }
    }
}
