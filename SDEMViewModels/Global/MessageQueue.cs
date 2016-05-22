using System;
using System.Collections.Generic;
using System.Linq;
using SDEMViewModels.MessageHandlers;
using SDEMViewModels.Messages;

namespace SDEMViewModels.Global
{
    public class MessageQueue
    {

        private static object syncRoot = new object();

        #region Singleton Instance

        private static volatile MessageQueue instance;


        public static MessageQueue Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MessageQueue();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Singleton Properties

        private Dictionary<Guid, List<DirectMessageContent>> DirectMessageQueue;

        #endregion

        #region Constructors

        private MessageQueue()
        {
            DirectMessageQueue = new Dictionary<Guid, List<DirectMessageContent>>();
        }

        #endregion

        public void AddMessage(DirectMessageContent messageContent, MainChatViewModel chatViewModel)
        {
            if (DirectMessageQueue.ContainsKey(messageContent.MessageId))
            {
                DirectMessageQueue[messageContent.MessageId].Add(messageContent);
            }
            else
            {
                var list = new List<DirectMessageContent>() { messageContent };
                DirectMessageQueue.Add(messageContent.MessageId, list);
            }

            if (HasAllMessages(messageContent.MessageId, messageContent.TotalMessages))
            {
                var message = CombineMessagesForMessage(messageContent.MessageId);
                new DirectMessageHandler().HandleMessage(chatViewModel, message);
            }
        }

        private bool HasAllMessages(Guid messageId, int totalCount)
        {
            if (DirectMessageQueue.ContainsKey(messageId))
            {
                var list = DirectMessageQueue[messageId];
                return list.Count == totalCount;
            }
            else
            {
                return false;
            }
        }

        private DirectMessageContent CombineMessagesForMessage(Guid messageId)
        {
            string entireMessage = "";

            var messages = DirectMessageQueue[messageId];
            Guid senderId = Guid.Empty;
            Guid combinedMessageId = Guid.Empty;
            DateTime createdDate = DateTime.Now;

            foreach (var message in messages.OrderBy(x => x.MessageNumber))
            {
                entireMessage += message.Message;
                senderId = message.SenderId;
                combinedMessageId = message.MessageId;
                createdDate = message.MessageCreatedDate;
            }

            return new DirectMessageContent(senderId, entireMessage, combinedMessageId, createdDate, 1, 1);
        }
    }
}
