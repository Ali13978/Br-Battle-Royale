using System;
using System.Collections;
using UnityEngine.Advertisements;
using UnityEngine;

public class UnityAdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static UnityAdsManager Instance { get; private set; }

    private string GAME_ID = "5446226"; //replace with your gameID from dashboard. note: will be different for each platform.

    private const string BANNER_PLACEMENT = "Banner_Android";
    private const string VIDEO_PLACEMENT = "Interstitial_Android";
    private const string REWARDED_VIDEO_PLACEMENT = "Rewarded_Android";

    private bool testMode = false;

    //utility wrappers for debuglog
    public delegate void DebugEvent(string msg);
    public static event DebugEvent OnDebugLog;

    private Action<bool> onAdCompleteAction;

    private void Awake()
    {
        Initialize();

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Initialize()
    {
        if (Advertisement.isSupported)
        {
            DebugLog(Application.platform + " supported by Advertisement");
        }
        Advertisement.Initialize(GAME_ID, testMode, this);
    }

    public void ShowBanner(BannerPosition bannerPosition)
    {
        Advertisement.Banner.SetPosition(bannerPosition);
        Advertisement.Banner.Show(BANNER_PLACEMENT);

    }

    public void HideBanner()
    {
        Advertisement.Banner.Hide(false);
    }

    public void LoadRewardedAd(Action<bool> onCompleteAction)
    {
        onAdCompleteAction = onCompleteAction;
        Advertisement.Load(REWARDED_VIDEO_PLACEMENT, this);
    }

    private void ShowRewardedAd()
    {
        Advertisement.Show(REWARDED_VIDEO_PLACEMENT, this);
    }

    public void LoadNonRewardedAd()
    {
        Advertisement.Load(VIDEO_PLACEMENT, this);
    }

    private void ShowNonRewardedAd()
    {
        Advertisement.Show(VIDEO_PLACEMENT, this);
    }

    #region Interface Implementations
    public void OnInitializationComplete()
    {
        DebugLog("Init Success");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        DebugLog($"Init Failed: [{error}]: {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == "Interstitial_Android")
        {
            ShowNonRewardedAd();
        }
        else if (placementId == "Rewarded_Android")
        {
            ShowRewardedAd();
        }
        DebugLog($"Load Success: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        DebugLog($"Load Failed: [{error}:{placementId}] {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        DebugLog($"OnUnityAdsShowFailure: [{error}]: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        DebugLog($"OnUnityAdsShowStart: {placementId}");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        DebugLog($"OnUnityAdsShowClick: {placementId}");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        DebugLog($"OnUnityAdsShowComplete: [{showCompletionState}]: {placementId}");
        if(showCompletionState == UnityAdsShowCompletionState.COMPLETED && placementId == "Rewarded_Android")
        {
            onAdCompleteAction?.Invoke(true);
            onAdCompleteAction = null;
        }
        else if(showCompletionState == UnityAdsShowCompletionState.SKIPPED && placementId == "Rewarded_Android")
        {
            onAdCompleteAction?.Invoke(false);
            onAdCompleteAction = null;
        }
    }
    #endregion

    public void OnGameIDFieldChanged(string newInput)
    {
        GAME_ID = newInput;
    }

    public void ToggleTestMode(bool isOn)
    {
        testMode = isOn;
    }

    //wrapper around debug.log to allow broadcasting log strings to the UI
    void DebugLog(string msg)
    {
        OnDebugLog?.Invoke(msg);
        Debug.Log(msg);
    }
}
