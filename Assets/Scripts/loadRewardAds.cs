using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class loadRewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private GameController gameController; 

    public string androidAdUnitId;
    public string iosAdUnitId;

    public string adUnitId;

    //[SerializeField] Text logs;

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
        Advertisement.Load(adUnitId, this);

        //logs.text += "Reward Ads Loaded.\n";
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId.Equals(adUnitId))
        {
            //logs.text += "On Unity Reward Ads Ad Loaded.\n";

            ShowAds();
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        //logs.text += "On Unity Reward Ads Ad Failed: " + error + '\n';
    }

    public void ShowAds()
    {
        Advertisement.Show(adUnitId, this);

        //logs.text += "Reward Ads Show Ads.\n";
    }


    public void OnUnityAdsShowClick(string placementId)
    {
        //logs.text += "On Unity Reward Ads Show Clicked.\n";

        gameController.GetToken(1); 
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
        {
            //logs.text += "On Unity Reward Ads Show Complete.\n";

            gameController.GetToken(1);
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        //logs.text += "On Unity Reward Ads Show Failed: " + error + '\n';
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        //logs.text += "On Unity Reward Ads Show Started.\n";

        // gameController.GetToken(1);
    }
}
