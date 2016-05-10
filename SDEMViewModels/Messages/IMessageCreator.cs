
namespace SDEMViewModels.Messages
{
    public interface IMessageCreator
    {
        string CreateMessage(IMessageContent arguments);
    }
}
