using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsHandler : MonoBehaviour
{
	public static AdsHandler Instance;

	string adUnitId;

	BannerView bannerViewMenuPage;
	BannerView bannerViewPausePage;
	InterstitialAd interstitial;
	RewardBasedVideoAd rewardVideoAds;
	RewardBasedVideoAd rewardBasedVideoAds;
	private string testDeviceId = "6EEC9FCA858EF9B40DC6BCA19FD51036";

	void Awake ()
	{
		if (Instance == null)
			Instance = this;

		RequestBannerMenuPage ();
		RequestBannerPausePage ();
		//RequestInterstitial ();
		//RequestVideoAds ();
		//RequestRewardedVideoAds ();
	}
	// Use this for initialization
	void Start ()
	{
		
	}

	public void RequestBannerMenuPage ()
	{
		#if UNITY_EDITOR
		adUnitId = "unused";
		#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-8278120811341322/9571915451";

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
		#if UNITY_EDITOR
		adUnitId = "unused";
		#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-8278120811341322/7373536644";

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

	public void RequestInterstitial ()
	{
		#if UNITY_EDITOR
		adUnitId = "unused";
		#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-1064698863475177/1158101173";

		#elif UNITY_IPHONE
		adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";

		#else
		adUnitId = "unexpected_platform";
		#endif
		interstitial = new InterstitialAd (adUnitId);
//		AdRequest request = new AdRequest.Builder ().AddTestDevice (testDeviceId).Build ();
		AdRequest request = new AdRequest.Builder ().Build ();
		// Load the interstitial with the request.
		interstitial.LoadAd (request);
	}

	public void RequestVideoAds ()
	{
		#if UNITY_EDITOR
		adUnitId = "unused";
		#elif UNITY_ANDROID
		adUnitId = "ca-app-pub-1064698863475177/8569332113";

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
		if (interstitial.IsLoaded ())
			interstitial.Show ();
	}

	public void ShowVideoAds ()
	{
		if (rewardVideoAds.IsLoaded ()) {
			rewardVideoAds.Show ();
		}
	}

	public void OnRestartGame ()
	{
		interstitial.Destroy ();
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
