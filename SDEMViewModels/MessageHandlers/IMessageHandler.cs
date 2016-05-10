
namespace SDEMViewModels.MessageHandlers
{
    public interface IMessageHandler
    {
        bool IsMessageThisType(string message);

        string MessageHeader { get; }

        void HandleMessage(MainChatViewModel mainChatViewModel, string message);
    }
}
