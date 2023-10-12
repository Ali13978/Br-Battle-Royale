/*
 * In First setup create your account on https://developers.facebook.com/
 * Next create an app there in dashboard 
     * While creating apps select other first
     * Give app type of gaming
     * Give your app a name, add email
     
 * Select your app from My Apps
 * From Add products in your app add Audience network in your app
 * Start setup your Audience network
 * Create Ad Space
 * In your Ad space Start creating placements
 * Create one placement for Banner and one for Interstitial Ads
 * Now Select Integration < Testing from left side
 * Enable testing there
 * check the use real advertiser content
 * Now here add your testing device
     * You can get your AAID from here in your mobile Settings > Google > Ads > This device advertising id
     * Now give your newly created test device an ad type of IMAGE; Ratio: 16:9; CTA: Appinstall 
 */

using AudienceNetwork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FbAdsManager : MonoBehaviour
{
    InterstitialAd interstitialAd;
    public static FbAdsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        // Initialize the Facebook Audience Network SDK.
        AudienceNetworkAds.Initialize();
        //LoadBanner(AdPosition.BOTTOM);
    }


    // Load a banner ad.
    public void LoadBanner(AdPosition bannerPosition)
    {
        // Create a banner ad view with a specific placement ID and size.
        AdView adView = new AdView("298527799599885_298538522932146", AdSize.BANNER_HEIGHT_50); // Change the string with your Banner Placement id from dashboard
        adView.Register(this.gameObject);

        // Set event handlers for ad loading and interactions.
        adView.AdViewDidLoad = (delegate () {
            Debug.Log("Banner loaded.");
            adView.Show(bannerPosition);
        });
        adView.AdViewDidFailWithError = (delegate (string error) {
            Debug.Log("Banner failed to load with error: " + error);
        });
        adView.AdViewWillLogImpression = (delegate () {
            Debug.Log("Banner logged impression.");
        });
        adView.AdViewDidClick = (delegate () {
            Debug.Log("Banner clicked.");
        });

        // Load the banner ad.
        adView.LoadAd();
    }

    // Load an interstitial ad.
    public void LoadInterstitial()
    {
        // Create an interstitial ad with a specific placement ID.
        interstitialAd = new InterstitialAd("298527799599885_298538816265450"); // Change the string with your Interstitial Placement id from dashboard
        interstitialAd.Register(this.gameObject);

        // Set event handlers for interstitial ad loading and interactions.
        interstitialAd.InterstitialAdDidLoad = (delegate () {
            Debug.Log("Interstitial ad loaded.");
            ShowInterstitial();
        });
        interstitialAd.InterstitialAdDidFailWithError = (delegate (string error) {
            Debug.Log("Interstitial ad failed to load with error: " + error);
        });
        interstitialAd.InterstitialAdWillLogImpression = (delegate () {
            Debug.Log("Interstitial ad logged impression.");
        });
        interstitialAd.InterstitialAdDidClick = (delegate () {
            Debug.Log("Interstitial ad clicked.");
        });

        interstitialAd.interstitialAdDidClose = (delegate () {
            Debug.Log("Interstitial ad did close.");
            if (interstitialAd != null)
            {
                interstitialAd.Dispose();
            }
        });

        // Load the interstitial ad.
        interstitialAd.LoadAd();
    }

    // Show the interstitial ad.
    public void ShowInterstitial()
    {
        this.interstitialAd.Show();
    }
}