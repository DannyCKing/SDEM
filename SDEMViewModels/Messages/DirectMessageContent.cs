using System;
using SDEMViewModels.Global;

namespace SDEMViewModels.Messages
{
    public class DirectMessageContent : IMessageContent
    {
        public const string MessageType = Constants.DIRECT_MESSAGE_HEADER;

        public DateTime MessageCreatedDate { get; private set; }

        public Guid MessageId { get; private set; }

        public Guid SenderId { get; set; }

        public string Message { get; set; }

        public DirectMessageContent(Guid senderId, string message)
        {
            SenderId = senderId;
            Message = message;
            MessageId = Guid.NewGuid();
            MessageCreatedDate = DateTime.Now;
        }

        public DirectMessageContent(Guid senderId, string message, Guid messageId, DateTime createdDate)
        {
            SenderId = senderId;
            Message = message;
            MessageId = messageId;
            MessageCreatedDate = createdDate;
        }
    }
}
