using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class initializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    public string androidGameId;
    public string iosGameId;

    public bool isTestingMode = false;

    string gameId;

    void Awake()
    {
        InitializeAds();
    }

    private void InitializeAds()
    {
#if UNITY_IOS
        gameId = iosGameId;
#elif UNITY_ANDROID
        gameId = androidGameId;
#elif UNITY_EDITOR
        gameId = androidGameId;
#endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTestingMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Ads Initialized.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Ads Failed to Initialize: " + error);
    }
}
