using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;


public class AdManager : MonoBehaviour
{
    public static AdManager instance;
    [Header("Unity AD ID")]
    public string UnityGameID;
    [Header("Admob Id----")]
    public string bannerAdId, interstitialAdId, rewardVideoAdId;
    [Header("BannerPos----")]
    public bool isOnTop;
    public RewardedAd rewardedAd;
    private static BannerView bannerView;
    private static InterstitialAd interstitial;
    void Start()
    {
        gameObject.name = "AdManager";
        //loadrewardedAd();
    }

    public void loadrewardedAd()
    {
        this.rewardedAd = new RewardedAd(rewardVideoAdId);
        this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        //if(UiManager.instance)
        //{
        //    UiManager.instance.Skipbutton.SetActive(true);
        //}
    }
    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.Message);
        loadrewardedAd();
    }
    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
    }
    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);
    }
    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        //if (UiManager.Instance)
        //{
        //    UiManager.Instance.nextlevel();
        //}
        //loadrewardedAd();
    }
    void Awake()
    {
        if (instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
    }
    public void unityadload()
    {
        Advertisement.Initialize(UnityGameID, true);
    }
    
    //TODO We need placementId & Advertisement doesnt have property "IsReady"
    public void showunityads()
    {
        // if (Advertisement.IsReady())
        // {
        //     Advertisement.Show();
        //     Debug.Log("it ready");
        // }
        // else
        // {
        //
        //     Debug.Log("yo its not loaded");
        // }
    }

    public void showBannerAd()
    {

        if (isOnTop)
        {
            if (bannerView != null)
            {
                Debug.Log("from here");
                bannerView.Destroy();
            }
            bannerView = new BannerView(bannerAdId, AdSize.Banner, AdPosition.Top);
            AdRequest request = new AdRequest.Builder().Build();
            bannerView.LoadAd(request);

        }
        else
        {
            if (bannerView != null)
            {
                Debug.Log("from here");
                bannerView.Destroy();
            }
            bannerView = new BannerView(bannerAdId, AdSize.Banner, AdPosition.Bottom);
            AdRequest request = new AdRequest.Builder().Build();
            bannerView.LoadAd(request);

        }

    }
    public void loadInterstitial()
    {
        interstitial = new InterstitialAd(interstitialAdId);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }
    public void showInterstitial()
    {
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }

    }
    public void hideBannerAd()
    {
        bannerView.Hide();
    }
    public void showRewardVideo()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }

}