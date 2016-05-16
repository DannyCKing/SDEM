﻿using SDEMViewModels.Global;

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
                        <Other>{5}</Other>
                    </MessageDetails>
                </Message>";

        public string CreateMessage(IMessageContent arguments)
        {
            DirectMessageContent msg = arguments as DirectMessageContent;

            var senderId = msg.SenderId;
            var messageId = msg.MessageId;
            string encodedXml = System.Security.SecurityElement.Escape(msg.Message);
            var timeString = msg.MessageCreatedDate.ToLongDateString();
            var xml = string.Format(DIRECT_MESSAGE, Constants.DIRECT_MESSAGE_HEADER, senderId, messageId, encodedXml, timeString, MessageHelper.GetExtraFluff());
            return xml;
        }
    }
}
