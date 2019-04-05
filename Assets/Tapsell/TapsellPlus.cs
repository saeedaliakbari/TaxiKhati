using Tapsell.Base;
using UnityEngine;
using System;
using System.Collections;

namespace Tapsell
{
	public abstract class TapsellPlus
	{
		private static GameObject _adManager;
		private static TapsellMessageHandler _messageHandler;
		private static ShowAdMessageHandler _showAdMessageHandler;

		private const string ShowAdAction = "showAd";
		private const string RequestAdAction = "requestAd";
		private const string RequestNativeAdAction = "requestNativeBannerAd";
		private const string AdManagerName = "TapsellPlusManager";
		
		private static readonly AndroidJavaClass TapsellPlusClass = new AndroidJavaClass ("ir.tapsell.sdk.plus.unitywrapper.TapsellPlusUnity");

		public static void EnableDebug()
		{
			TapsellPlusClass.CallStatic("enableDebug");
			Debug.Log("ok");
		}
		
		public static void Initialize(string apiKey, string sign)
		{
			if (_adManager != null)
			{
				return;
			}		
			
			Debug.Log("Initializing TapsellPlus.");
			TapsellPlusClass.CallStatic("initialize", apiKey, sign);
			
			_adManager = new GameObject(AdManagerName);
			UnityEngine.Object.DontDestroyOnLoad(_adManager);
			_messageHandler = _adManager.AddComponent<TapsellMessageHandler>();
			
			_messageHandler.AddHandler(RequestAdAction, new RequestAdMessageHandler());
			_messageHandler.AddHandler(RequestNativeAdAction, new RequestNativeAdMessageHandler());

			_showAdMessageHandler = new ShowAdMessageHandler();
			_messageHandler.AddHandler(ShowAdAction, _showAdMessageHandler);
		}
		
		public static void RequestAd(string abrZoneId, Action<string> onAdready, Action<long, string> onError, Action onNoAdAvailable, Action onNoNetwork)
		{
			if (_adManager == null)
			{
				return;
			}
        
			int id = _messageHandler.NewItem(new RequestAdCallbackData(onAdready, onError, onNoAdAvailable, onNoNetwork));
			TapsellPlusClass.CallStatic("requestAd", abrZoneId, id, AdManagerName, TapsellMessageHandler.MethodName);
		}

		public static void RequestNativeBannerAD(MonoBehaviour monoBehaviour,string abrZoneId, Action<TapsellPlusNativeBannerAd> onRequestFilled, Action<long> onError, Action onNoAdAvailable, Action onNoNetwork){
			if (_adManager == null)
			{
				return;
			}
        
			int id = _messageHandler.NewItem(new RequestNativeAdCallbackData(monoBehaviour, onRequestFilled, onError, onNoAdAvailable, onNoNetwork));
			TapsellPlusClass.CallStatic("requestNativeAd", abrZoneId, id, AdManagerName, TapsellMessageHandler.MethodName);
		}

		public static void ShowAd(string adId, Action<long, string> onError, Action<bool> onAdClosed, Action<string, string> onReward)
		{
			if (_adManager == null)
			{
				return;
			}
        
			int id = _messageHandler.NewItem(new ShowAdCallbackData(onError, onAdClosed, onReward));
			TapsellPlusClass.CallStatic("showAd", adId, id, AdManagerName, TapsellMessageHandler.MethodName);
		}

		public static void SetRewardCallback(Action<string, string> rewardAction)
		{
			if (_adManager == null)
			{
				return;
			}
			_showAdMessageHandler.SetRewardCallback(rewardAction);
		}
		
		public static void onNativeBannerRequestFilled(MonoBehaviour monoBehaviour, 
			TapsellPlusNativeBannerAd result, Action<TapsellPlusNativeBannerAd> onRequestFilled,
			Action<long> onError)
		{
			if (monoBehaviour != null && monoBehaviour.isActiveAndEnabled) {
				monoBehaviour.StartCoroutine (loadNativeBannerAdImages(result, onRequestFilled));
			}
			else 
			{
				if(onError != null)
					onError(-2);
			}
		}
		
		static IEnumerator loadNativeBannerAdImages(TapsellPlusNativeBannerAd result, Action<TapsellPlusNativeBannerAd> onRequestFilled)
		{
			if(result.iconUrl!=null && !result.iconUrl.Equals(""))
			{
				WWW wwwIcon = new WWW (result.iconUrl);
				yield return wwwIcon;
				if(wwwIcon.texture!=null)
				{
					result.iconImage = wwwIcon.texture;
				}
			}
			if(result.portraitStaticImageUrl!=null && !result.portraitStaticImageUrl.Equals(""))
			{
				WWW wwwPortrait = new WWW (result.portraitStaticImageUrl);
				yield return wwwPortrait;
				if(wwwPortrait.texture!=null)
				{
					result.portraitBannerImage = wwwPortrait.texture;
				}
			}
			if(result.landscapeStaticImageUrl!=null && !result.landscapeStaticImageUrl.Equals(""))
			{
				WWW wwwLandscape = new WWW (result.landscapeStaticImageUrl);
				yield return wwwLandscape;
				if(wwwLandscape.texture!=null)
				{
					result.landscapeBannerImage = wwwLandscape.texture;
				}
			}
			if(onRequestFilled != null)
				onRequestFilled(result);
		}
		
		public static void onNativeBannerAdClicked(string adId)
		{
			TapsellPlusClass.CallStatic("onNativeClicked",adId);
		}

		public static void onNativeBannerAdShown(string adId)
		{
			TapsellPlusClass.CallStatic("onNativeShowed",adId);
		}

	}

	[Serializable]
	public class TapsellPlusNativeBannerAd
	{
		public string adId;
		public string title;
		public string description;
		public string iconUrl;
		public string callToActionText;
		public string portraitStaticImageUrl;
		public string landscapeStaticImageUrl;

		public bool shownReported = false;

		public Texture2D portraitBannerImage;
		public Texture2D landscapeBannerImage;
		public Texture2D iconImage;

		public string getTitle()
		{
			return title;
		}

		public string getDescription()
		{
			return description;
		}

		public string getCallToAction()
		{
			return callToActionText;
		}

		public Texture2D getPortraitBannerImage()
		{
			return portraitBannerImage;
		}

		public Texture2D getLandscapeBannerImage()
		{
			return landscapeBannerImage;
		}

		public Texture2D getIcon()
		{
			return iconImage;
		}

		public void onShown()
		{
			if (!this.shownReported)
			{
				TapsellPlus.onNativeBannerAdShown (this.adId);
				this.shownReported = true;
			}
		}

		public void onClicked()
		{
			TapsellPlus.onNativeBannerAdClicked (this.adId);
		}
	}
}
