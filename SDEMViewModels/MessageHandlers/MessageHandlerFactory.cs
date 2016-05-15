
namespace SDEMViewModels.MessageHandlers
{
    public class MessageHandlerFactory
    {
        private readonly AliveMessageHandler MyAliveMessageHandler = new AliveMessageHandler();

        private readonly DirectMessageHandler MyDirectMessageHandler = new DirectMessageHandler();

        public IMessageHandler GetMessageHandler(string message)
        {
            var messageType = GetMessageType(message);

            if (messageType == MessageType.Alive)
            {
                return MyAliveMessageHandler;
            }
            else if (messageType == MessageType.DirectMessage)
            {
                return MyDirectMessageHandler;
            }
            else if (messageType == MessageType.ReadReciept)
            {

            }
            // other unknown type

            return null;
        }

        private MessageType GetMessageType(string message)
        {
            if (MyAliveMessageHandler.IsMessageThisType(message))
                return MessageType.Alive;
            else if (this.MyDirectMessageHandler.IsMessageThisType(message))
                return MessageType.DirectMessage;
            else
                return MessageType.Unknown;

        }
    }
}
