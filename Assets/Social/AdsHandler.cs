using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsHandler : MonoBehaviour
{
	public static AdsHandler Instance;

    BannerView bannerViewMenuPage;
	BannerView bannerViewPausePage;
	InterstitialAd interstitialPause;
	InterstitialAd interstitialMainMenu;
    RewardBasedVideoAd rewardVideoAds;
	RewardBasedVideoAd rewardBasedVideoAds;
	private string testDeviceId = "6EEC9FCA858EF9B40DC6BCA19FD51036";

	void Awake ()
	{
        
		if (Instance == null)
			Instance = this;
        //RequestBannerMenuPage ();
        //RequestBannerPausePage ();
        //RequestVideoAds ();
        //RequestRewardedVideoAds ();
	}
	// Use this for initialization
	void Start ()
	{
        RequestInterstitialPause();
        RequestInterstitialMainMenu();

    }

    public void RequestBannerMenuPage ()
	{
        string adUnitId;
#if UNITY_EDITOR
        adUnitId = "unused";
#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-1224519388650129/3670556784";

#elif UNITY_IPHONE
		adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";

#else
		adUnitId = "unexpected_platform";
#endif
        bannerViewMenuPage = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);
		AdRequest request = new AdRequest.Builder ().AddTestDevice (testDeviceId).Build ();
		//AdRequest request = new AdRequest.Builder ().Build ();
		// Load the banner with the request.
		bannerViewMenuPage.LoadAd (request);
	}

	public void RequestBannerPausePage ()
	{
        string adUnitId;
#if UNITY_EDITOR
        adUnitId = "unused";
#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-1224519388650129/1133035115";

#elif UNITY_IPHONE
		adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";

#else
		adUnitId = "unexpected_platform";
#endif
        bannerViewPausePage = new BannerView (adUnitId, AdSize.Banner, AdPosition.Bottom);
		AdRequest request = new AdRequest.Builder ().AddTestDevice (testDeviceId).Build ();
		//AdRequest request = new AdRequest.Builder ().Build ();
		// Load the banner with the request.
		bannerViewPausePage.LoadAd (request);
	}

	public void RequestInterstitialPause ()
	{
        string adUnitId;

#if UNITY_EDITOR
        adUnitId = "unused";
#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-1224519388650129/7665979716";

#elif UNITY_IPHONE
		adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";

#else
		adUnitId = "unexpected_platform";
#endif
        interstitialPause = new InterstitialAd (adUnitId);
		AdRequest request = new AdRequest.Builder ().AddTestDevice (testDeviceId).Build ();
		//AdRequest request = new AdRequest.Builder ().Build ();
		// Load the interstitial with the request.
		interstitialPause.LoadAd (request);
	}

    public void RequestInterstitialMainMenu()
    {
        string adUnitId;

#if UNITY_EDITOR
        adUnitId = "unused";

#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-1224519388650129/5632032682";

#elif UNITY_IPHONE
		adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";

#else
		adUnitId = "unexpected_platform";
#endif
        interstitialMainMenu = new InterstitialAd(adUnitId);
  		AdRequest request = new AdRequest.Builder ().AddTestDevice (testDeviceId).Build ();
        //AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        interstitialMainMenu.LoadAd(request);
        interstitialMainMenu.OnAdFailedToLoad += InterstitialMainMenu_OnAdFailedToLoad;
        interstitialMainMenu.OnAdLoaded += Interstitial_OnAdLoaded;
        
    }

    private void InterstitialMainMenu_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        Debug.Log("Faild To Load : >> "+e.ToString());
    }

    //private void OnGUI()
    //{
    //    if(GUI.Button(new Rect(10,10,100,50),"Requst Ad"))
    //    {
    //        RequestInterstitialMainMenu();
    //    }

    //    if (GUI.Button(new Rect(10, 70, 100, 50), "Show Ad"))
    //    {
    //        ShowInterstitialAds();
    //    }

    //    if (GUI.Button(new Rect(10, 130,100, 50), "Destroy Ad"))
    //    {
    //        interstitialMainMenu.Destroy();
    //    }
    //}

    private void Interstitial_OnAdLoaded(object sender, System.EventArgs e)
    {
        Debug.Log(sender.ToString() + " >>>   " + e.ToString());
    }

    public void RequestVideoAds ()
	{
        string adUnitId;

#if UNITY_EDITOR
        adUnitId = "unused";
#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-1224519388650129/8867043142";

#elif UNITY_IPHONE
		adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";

#else
		adUnitId = "unexpected_platform";
#endif
        rewardVideoAds = RewardBasedVideoAd.Instance;
//		AdRequest request = new AdRequest.Builder ().AddTestDevice (testDeviceId).Build ();
		AdRequest request = new AdRequest.Builder ().Build ();
		rewardVideoAds.LoadAd (request, adUnitId);
	}

	public void RequestRewardedVideoAds ()
    {
        string adUnitId;

		#if UNITY_EDITOR
		adUnitId = "unused";
		#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-1064698863475177/8118956696";

		#elif UNITY_IPHONE
		adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";

		#else
		adUnitId = "unexpected_platform";
		#endif
		rewardBasedVideoAds = RewardBasedVideoAd.Instance;
//		AdRequest request = new AdRequest.Builder ().AddTestDevice (testDeviceId).Build ();
		AdRequest request = new AdRequest.Builder ().Build ();
		rewardBasedVideoAds.LoadAd (request, adUnitId);
		rewardBasedVideoAds.OnAdClosed += RewardVideoAds_OnAdClosed;
	}

	void RewardVideoAds_OnAdClosed (object sender, System.EventArgs e)
	{
//		if (GameManager.instance.currentGameState == GameManager.eGameState.GameOver)
//			return;
//		Debug.Log ("Rewarded");
//		InputHandler.instance.GameStartAfterReward ();
//		RequestRewardedVideoAds ();
	}

	public void ShowBannerAdsMenuPage ()
	{
		Debug.Log ("showMenu");
		if (bannerViewMenuPage != null)
			bannerViewMenuPage.Show ();
	}

	public void ShowBannerAdsPausePage ()
	{
		Debug.Log ("showPause");
		if (bannerViewPausePage != null)
			bannerViewPausePage.Show ();
	}

	public void HideBannerAdsMenuPage ()
	{
		Debug.Log ("HideMenu");
		if (bannerViewMenuPage != null)
			bannerViewMenuPage.Hide ();
	}

	public void HideBannerAdsPausePage ()
	{
		Debug.Log ("HidePause");
		if (bannerViewPausePage != null)
			bannerViewPausePage.Hide ();
	}

	public void ShowInterstitialAds ()
	{
        Debug.Log("Add Loaded : "+interstitialMainMenu.IsLoaded());
		if (interstitialMainMenu.IsLoaded ())
            interstitialMainMenu.Show ();
	}

    public void ShowInterstitialPauseAds()
    {
        Debug.Log("Add Loaded : " + interstitialPause.IsLoaded());
        if (interstitialPause.IsLoaded())
            interstitialPause.Show();
    }

    public void ShowVideoAds ()
	{
		if (rewardVideoAds.IsLoaded ()) {
			rewardVideoAds.Show ();
		}
	}

	public void OnRestartGame ()
	{
		interstitialPause.Destroy ();
		bannerViewMenuPage.Destroy ();
		bannerViewPausePage.Destroy ();
	}

	public void ShowRewardedVideo ()
	{
//		if (rewardBasedVideoAds.IsLoaded ()) {
//			Debug.Log ("Reward Video Load");
//			rewardBasedVideoAds.Show ();
//		} else {
//			Debug.Log ("Video not loaded");
//			UIController.Instance.ActiveUI (GameManager.instance.connectionPanel);
//			UIAnimationController.Instance.ConnectionPanelAnimation (GameManager.instance.connectionPanel);
//		}

	}


}
