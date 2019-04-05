using System;
using Tapsell.Base;
using UnityEngine;

namespace Tapsell
{
    public class RequestNativeAdCallbackData : ICallbackData
    {
        public readonly MonoBehaviour MonoBehaviourItem;
        public readonly Action<TapsellPlusNativeBannerAd> RequestFilledAction;
        public readonly Action<long> ErrorAction;
        public readonly Action NoAdAction;
        public readonly Action NoNetworkAction;

        public RequestNativeAdCallbackData(MonoBehaviour monoBehaviour, Action<TapsellPlusNativeBannerAd> onRequestFilled, Action<long> errorAction, Action noAdAction, Action noNetworkAction)
        {
            MonoBehaviourItem = monoBehaviour;
            RequestFilledAction = onRequestFilled;
            ErrorAction = errorAction;
            NoAdAction = noAdAction;
            NoNetworkAction = noNetworkAction;
        }
    }
}