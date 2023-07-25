using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class loadInterstitial : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private GameController gameController;

    public string androidAdUnitId;
    public string iosAdUnitId;

    public string adUnitId;

    void Awake()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif

        InvokeRepeating("ShowAds", 180f, 180f);
    }

    public void LoadAd()
    {
        Debug.Log("Interstitial Ads Loaded.");

        Advertisement.Load(adUnitId, this); 
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("On Unity Interstitial Ads Ad Loaded.");

        ShowAds();
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("On Unity Interstitial Ads Ad Load Failed: " + error);
    }

    public void ShowAds()
    {
        Debug.Log("Interstitial Ads Show Ads.");

        Advertisement.Show(adUnitId, this); 
    }


    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("On Unity Interstitial Ads Show Clicked.");

        gameController.GetToken(1);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("On Unity Interstitial Ads Show Complete.");

        // gameController.GetToken(1);
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("On Unity Interstitial Ads Show Failed: " + error);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("On Unity Interstitial Ads Show Started.");
    }
}
