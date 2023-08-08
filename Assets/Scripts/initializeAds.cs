using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class initializeAds : MonoBehaviour, IUnityAdsInitializationListener
{
    public string androidGameId;
    public string iosGameId;

    public bool isTestingMode = false;

    //[SerializeField] Text logs;

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
        //logs.text += "Ads Initialized.\n";
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        //logs.text += "Ads Failed to Initialize: " + error + '\n';
    }
}
