using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class bannerAds : MonoBehaviour
{
    public string androidAdUnitId;
    public string iosAdUnitId;

    public string adUnitId;

    BannerPosition bannerPosition = BannerPosition.BOTTOM_CENTER;

    //[SerializeField] Text logs;

    private void Start()
    {
#if UNITY_IOS
        adUnitId = iosAdUnitId;
#elif UNITY_ANDROID
        adUnitId = androidAdUnitId;
#endif

        Advertisement.Banner.SetPosition(bannerPosition);

        LoadBanner();
            
        //showBannerAd();
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
        //logs.text += "Banner Ads Loaded.\n";

        showBannerAd();
    }

    private void OnBannerLoadError(string error)
    {
        //logs.text += "Banner Ads Failed: " + error + '\n';
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

        //logs.text += "Banner Ads Showed.\n";
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
