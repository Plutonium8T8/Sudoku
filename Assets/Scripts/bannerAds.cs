using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.InputSystem.Android;
using UnityEngine.InputSystem;
using UnityEditor.PackageManager;

public class bannerAds : MonoBehaviour
{
    public string androidAdUnitId;
    public string iosAdUnitId;

    public string adUnitId;

    BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

    private void Start()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif

        Advertisement.Banner.SetPosition(bannerPosition);

        LoadBanner();
            
        showBannerAd();
    }

    public void LoadBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions()
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerLoadError,
        };

        Advertisement.Banner.Load(adUnitId, options);
    }

    private void OnBannerLoaded()
    {
        Debug.Log("Banner Ads Loaded.");

        showBannerAd();
    }

    private void OnBannerLoadError(string error)
    {
        Debug.Log("Banner Ads Failed: " + error);
    }

    public void showBannerAd()
    {
        BannerOptions options = new BannerOptions()
        {
            showCallback = OnBannerShown,
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden
        };

        Advertisement.Banner.Show(adUnitId, options);
    }

    private void OnBannerShown()
    {

    }

    private void OnBannerClicked()
    {

    }

    private void OnBannerHidden()
    {

    }
}
