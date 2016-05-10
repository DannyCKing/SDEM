
namespace SDEMViewModels.MessageHandlers
{
    public class GetMessageHandlerFactory
    {
        private readonly AliveMessageHandler MyAliveMessageHandler = new AliveMessageHandler();

        public IMessageHandler GetMessageHandler(string message)
        {
            var messageType = GetMessageType(message);

            if (messageType == MessageType.Alive)
            {
                return MyAliveMessageHandler;
            }
            else if (messageType == MessageType.DirectMessage)
            {

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
            else
                return MessageType.Unknown;

        }
    }
}
