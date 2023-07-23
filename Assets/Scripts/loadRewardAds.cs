using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Advertisements;

public class loadRewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
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
    }

    public void LoadAd()
    {
        Debug.Log("Reward Ads Loaded.");

        Advertisement.Load(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(adUnitId))
        {
            Debug.Log("On Unity Reward Ads Ad Loaded.");

            ShowAds();
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log("On Unity Reward Ads Ad Failed: " + error);
    }

    public void ShowAds()
    {
        Debug.Log("Reward Ads Show Ads.");

        Advertisement.Show(adUnitId, this);
    }


    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("On Unity Reward Ads Show Clicked.");

        gameController.GetToken(1);
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            Debug.Log("On Unity Reward Ads Show Complete.");

            gameController.GetToken(1);
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("On Unity Reward Ads Show Failed: " + error);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("On Unity Reward Ads Show Started.");
    }
}
