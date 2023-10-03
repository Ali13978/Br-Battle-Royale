using admob;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class ADMOB : MonoBehaviour
{
	//public static ADMOB instance;

	//public static Admob ads;

	//private string bannerId = "ca-app-pub-8068100050444250/5148271527";

	//private string intersitialId = "ca-app-pub-8068100050444250/6625004727";

	//private string nativeBannerId = "ca-app-pub-8068100050444250/5148271527";

	//private void Start()
	//{
	//	UnityEngine.Debug.Log("start unity demo-------------");
	//	UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	//	instance = this;
	//	InitAdmob();


 //   }

	//private void InitAdmob()
	//{
	//	ads = Admob.Instance();
	//	ads.bannerEventHandler += onBannerEvent;
	//	ads.interstitialEventHandler += onInterstitialEvent;
	//	ads.nativeBannerEventHandler += onNativeBannerEvent;
	//	ads.initAdmob(bannerId, intersitialId);
	//	StartCoroutine(InitAdMobCoroutine());
	//	UnityEngine.Debug.Log("admob inited -------------");
	//}

	//private IEnumerator InitAdMobCoroutine()
	//{
	//	yield return new WaitForSeconds(0.5f);
	//	instance.ShowBanner(AdPosition.BOTTOM_CENTER);
	//	yield return new WaitForSeconds(0.5f);
	//	ads.loadInterstitial();
	//}

	//public void ShowBanner(int bannerPosition)
	//{
	//	ads.showBannerRelative(AdSize.SmartBanner, bannerPosition, 0);
	//}

	//public void ShowNativeBanner(int bannerPosition)
	//{
	//	ads.showNativeBannerRelative(AdSize.SmartBanner, bannerPosition, 0, nativeBannerId);
	//}

	//public void HideBanner()
	//{
	//	ads.removeBanner();
	//}

	//public bool ShowIntersitial()
	//{
	//	if (ads.isInterstitialReady())
	//	{
	//		ads.showInterstitial();
	//		return true;
	//	}
	//	return false;
	//}

	////public bool ShowRewardVideo(Action<ShowResult> handle)
	////{
	////	if (Advertisement.IsReady())
	////	{
	////		ShowOptions showOptions = new ShowOptions();
	////		showOptions.resultCallback = handle;
	////		Advertisement.Show("rewardedVideo", showOptions);
	////		return true;
	////	}
	////	return false;
	////}

	//public void HandleShowResult(ShowResult result)
	//{
	//	switch (result)
	//	{
	//	case ShowResult.Finished:
	//		UnityEngine.Debug.Log("Video completed. Offer a reward to the player.");
	//		RewardVideo(isSuccess: true);
	//		break;
	//	case ShowResult.Skipped:
	//		UnityEngine.Debug.LogWarning("Video was skipped.");
	//		RewardVideo(isSuccess: false);
	//		break;
	//	case ShowResult.Failed:
	//		UnityEngine.Debug.LogError("Video failed to show.");
	//		RewardVideo(isSuccess: false);
	//		break;
	//	}
	//}

	//private void RewardVideo(bool isSuccess)
	//{
	//	if (SceneManager.GetActiveScene().name == "Main Menu")
	//	{
	//		GameObject.Find("MainMenu").GetComponent<MainMenu>().VideoAdsHandle(isSuccess);
	//	}
	//}

	//private void onInterstitialEvent(string eventName, string msg)
	//{
	//	UnityEngine.Debug.Log("handler onAdmobEvent---" + eventName + "   " + msg);
	//	if (eventName == AdmobEvent.onAdClosed || eventName == AdmobEvent.onAdFailedToLoad)
	//	{
	//		ads.loadInterstitial();
	//	}
	//}

	//private void onBannerEvent(string eventName, string msg)
	//{
	//	UnityEngine.Debug.Log("handler onAdmobBannerEvent---" + eventName + "   " + msg);
	//}

	//private void onNativeBannerEvent(string eventName, string msg)
	//{
	//	UnityEngine.Debug.Log("handler onAdmobNativeBannerEvent---" + eventName + "   " + msg);
	//}
}
