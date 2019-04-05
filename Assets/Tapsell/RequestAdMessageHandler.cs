using System;
using Tapsell.Base;

namespace Tapsell
{
    public class RequestAdMessageHandler : IMessageHandler
    {
        private const string AdReadyEvent = "adReady";
        private const string ErrorEvent = "error";
        private const string NoAdEvent = "noAdAvailable";
        private const string NoNetworkEvent = "noNetwork";
    
        private const string IdKey = "id";
        private const string CodeKey = "code";
        private const string MessageKey = "message";
    
    
        public void HandleMessage(TapsellMessage message, ICallbackData data)
        {
            var requestAdData = data as RequestAdCallbackData;

            if (requestAdData == null)
            {
                return;
            }
        
            string eventType = message.EventType;
            switch (eventType)
            {
                case null:
                    return;
                case AdReadyEvent:
                    _handleAdReady(message, requestAdData.AdReadyAction);
                    return;
                case ErrorEvent:
                    _handleError(message, requestAdData.ErrorAction);
                    return;
                case NoAdEvent:
                    var noAdAction = requestAdData.NoAdAction;
                    if (noAdAction != null)
                    {
                        noAdAction();
                    }
                    return;
                case NoNetworkEvent:
                    var noNetAction = requestAdData.NoNetworkAction;
                    if (noNetAction != null)
                    {
                        noNetAction();
                    }
                    return;
            }
        }

        private void _handleAdReady(TapsellMessage message, Action<string> action)
        {
            if (action != null)
            {
                action(message.Data[IdKey].Value);   
            }
        }

        private void _handleError(TapsellMessage message, Action<long, string> action)
        {
            if (action != null)
            {
                action(message.Data[CodeKey].AsInt, message.Data[MessageKey].Value);   
            }
        }
    }
}
