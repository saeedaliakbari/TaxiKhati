using System;
using Tapsell.Base;

namespace Tapsell
{
    public class RequestNativeAdMessageHandler : IMessageHandler
    {
        private const string AdReadyEvent = "adReady";
        private const string ErrorEvent = "error";
        private const string NoAdEvent = "noAdAvailable";
        private const string NoNetworkEvent = "noNetwork";
    
        private const string IdKey = "id";
        private const string CodeKey = "code";
        private const string MessageKey = "message";
        private const string TitleKey = "title";
        private const string DescriptionKey = "description";
        private const string CallToActionKey = "callToAction";
        private const string PortraitKey = "portraitBannerImage";
        private const string LandScapeKey = "landscapeBannerImage";
        private const string IconKey = "icon";
    
    
        public void HandleMessage(TapsellMessage message, ICallbackData data)
        {
            var requestAdData = data as RequestNativeAdCallbackData;

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
                    _handleAdReady(message, requestAdData);
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

        private void _handleAdReady(TapsellMessage message, RequestNativeAdCallbackData callback)
        {
            TapsellPlusNativeBannerAd ad = new TapsellPlusNativeBannerAd
            {
                adId = message.Data[IdKey],
                title = message.Data[TitleKey],
                description = message.Data[DescriptionKey],
                callToActionText = message.Data[CallToActionKey],
                portraitStaticImageUrl = message.Data[PortraitKey],
                landscapeStaticImageUrl = message.Data[LandScapeKey],
                iconUrl = message.Data[IconKey]
            };

            TapsellPlus.onNativeBannerRequestFilled(callback.MonoBehaviourItem, ad,
                callback.RequestFilledAction, callback.ErrorAction);
        }

        private void _handleError(TapsellMessage message, Action<long> action)
        {
            if (action != null)
            {
                action(message.Data[CodeKey].AsInt);   
            }
        }
    }
}
