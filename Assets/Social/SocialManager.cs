using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System;

public class SocialManager : MonoBehaviour
{
	private static SocialManager mInstance;

	public static SocialManager Instance {
		get {
			if (mInstance == null)
				mInstance = FindObjectOfType<SocialManager> ();
			return mInstance;
		}
		set {
			mInstance = value;
		}
	}

	public FacebookHandler facebookManager;
	public ShareApp shareApplication;
	public Image userProfile;
	public Image friendProfile;
	public Sprite defaultAvatar;
	public bool isFriendsSelected;

	// Use this for initialization
	void Start ()
	{
		
	}

	public void CheckInternetConnection ()
	{
		StartCoroutine (InternetConnectionCheck (InternetConnectionCallBack));
		//return isInternetAvl;
	}

	public void InternetConnectionCallBack (bool isConnected)
	{
		if (isConnected) {
			
			GameManager.instance.currentGameType = GameType.OnLine;
			facebookManager.OnFacebookLogin ();
		} else
			UIManager.instance.NoINternetDisplay ();
	}

	private IEnumerator InternetConnectionCheck (Action<bool> action)
	{
		WWW www = new WWW ("https://www.google.com");
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			action (true);
		} else
			action (false);
	}

	public void LoginWithFB ()
	{
		AudioManager.Instance.PlaySound (AudioManager.SoundType.ButtonClick);
		CheckInternetConnection ();
//		ConnectionManager.Instance.CheckInternetConnection ();
//		if (ConnectionManager.Instance.isInternetAvl) {
		//GameManager.instance.currentGameType = GameType.OnLine;
		//facebookManager.OnFacebookLogin ();
//		} else
//			UIManager.instance.NoINternetDisplay ();
	}

	public void ShareWithFacebook ()
	{
		StartCoroutine (InternetConnectionCheck (InternetCallBack));

	}

	public void InternetCallBack (bool isConnected)
	{
		if (isConnected) {

			facebookManager.OnFacebookShare ();
		}
	}

	public void RateUs ()
	{
		PlayerPrefs.SetInt ("RateUs", 1);
		Debug.Log ("RetUs");
		//GameManager.instance.ShowRateUsPanel (false);
		Application.OpenURL ("market://details?id=com.eplayadda.mindssmash");
	}

	public void GetFriendFB ()
	{
		SocialManager.Instance.UpdateFriendName ("");
		SocialManager.Instance.SetDefaultAvatar ();
		UIManager.instance.mainMenuUI.selectFriendPopup.text = "";
		isFriendsSelected = false;
		UIManager.instance.fbFriendsPanel.SetActive (true);
		facebookManager.GetFriends ();
	}

	public void OnClickInvite ()
	{
		UIManager.instance.fbFriendsPanel.SetActive (false);
	}

	public void LikeWithFacebook ()
	{
		#if UNITY_ANDROID
		facebookManager.FacebookLike ();
		#endif
	}

	public void InviteFacebookFriend ()
	{
		facebookManager.InviteFriends ();
	}

	public void UpdateUserProfile (string url)
	{
		Debug.Log (url);
		StartCoroutine (DownloadImage (url));
	}

	private IEnumerator DownloadImage (string url)
	{
		WWW www = new WWW (url);
		yield return www;
		Debug.Log (www.isDone + " " + www.error);
		if (string.IsNullOrEmpty (www.error)) {
			userProfile.sprite =	Sprite.Create (www.texture, new Rect (0, 0, www.texture.width, www.texture.height), new Vector2 (0.5f, 0.5f));
			UIManager.instance.mainMenuUI.ProfilePic.sprite = userProfile.sprite;
		}

	}

	public void UpdateFriendProfilePic (Sprite profilePic)
	{
		friendProfile.sprite = profilePic;
	}

	public void SetDefaultAvatar ()
	{
		friendProfile.sprite = defaultAvatar;
	}

	public void ShareWithWhatsApp ()
	{
		shareApplication.shareText ();
	}

	public void UpdateFriendName (string name)
	{
		UIManager.instance.mainMenuUI.selectedFriendName.text = name;
	}

}
