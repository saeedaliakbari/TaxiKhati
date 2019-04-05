namespace Tapsell.Base
{
    public interface IMessageHandler
    {
        void HandleMessage(TapsellMessage message, ICallbackData data);
    }
}