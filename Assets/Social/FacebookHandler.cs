using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using Facebook.MiniJSON;
using System;
using System.Linq;
using UnityEngine.UI;


public class FacebookHandler : MonoBehaviour
{
	public GameObject FriendPrefab;
	public Transform parentObject;
	private ToggleGroup toggleGroup;
	//public GameObject FriendPrefabRoom;
	//public Transform parentRoom;
	//public Text debugText;
	private string userId;
	//private Texture profilePic;
	string appStoreLink = "https://play.google.com/store/apps/details?id=renewin.com.huliya&hl=en";
	string inviteAppLinkUrl = "https://fb.me/350820032015040";
	private bool IsInternetAvailabe = false;
	//public Text testText;
	private List<string> FriendsIdList = new List<string> ();
	private List<GameObject> FriendsObjectList = new List<GameObject> ();

	void Start ()
	{
		PlayerPrefs.DeleteAll ();

	}

	public void mStart ()
	{ 
		int loginValue = PlayerPrefs.GetInt ("IsFbLogedIn");
		if (loginValue == 1) {
			UIManager.instance.loginPanel.SetActive (false);
			UIManager.instance.mainMenuUI.gameObject.SetActive (true);
		}
		FB.Init (OnInitComplete, OnHideUnity);
		toggleGroup = SocialManager.Instance.GetComponent<ToggleGroup> ();
	}


	private void OnInitComplete ()
	{
		if (FB.IsInitialized) {
			FB.ActivateApp ();
		}

		Debug.Log ("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
		if (FB.IsLoggedIn) {
			GameManager.instance.currentGameType = GameType.OnLine;
			UIManager.instance.loginPanel.SetActive (false);
			UIManager.instance.mainMenuUI.gameObject.SetActive (true);
			var token = Facebook.Unity.AccessToken.CurrentAccessToken;
			userId = token.UserId.ToString ();
			ConnectionManager.Instance.myID = userId;
			UserProfile ();
			ConnectionManager.Instance.MakeConnection ();
		} else {
			
		}
	}

	//Facebook Login
	public void OnFacebookLogin ()
	{
		if (!FB.IsLoggedIn) { 
			CallFBLogin (); 
		} else {
			PlayerPrefs.SetInt ("IsFbLogedIn", 1);
			GameManager.instance.currentGameType = GameType.OnLine;
			var token = Facebook.Unity.AccessToken.CurrentAccessToken;
			userId = token.UserId.ToString ();
			ConnectionManager.Instance.myID = userId;
			UserProfile ();
			ConnectionManager.Instance.MakeConnection ();
			UIManager.instance.mainMenuUI.gameObject.SetActive (true);
			UIManager.instance.loginPanel.SetActive (false);
		}
	}

	private void OnHideUnity (bool isGameShown)
	{
		Debug.Log ("Is Fb Hide");
		if (!isGameShown) {
			// Pause the game - we will need to hide
			Time.timeScale = 0;
		} else {
			// Resume the game - we're getting focus again
			Time.timeScale = 1;
		}
	}

	private void CallFBLogin ()
	{
		Debug.Log ("CallFBLogin Methohd");
		FB.LogInWithReadPermissions (new List<string> (){ "public_profile", "email", "user_friends" }, this.FBLoginCallBack);
		//FB.LogInWithPublishPermissions (new List<string> () { "publish_actions" }, this.FBLoginCallBack);
	}

	private void FBLoginCallBack (ILoginResult result)
	{
		Debug.Log ("Result " + result.ToString ());
		if (result.Error == null) {
			//debugText.text += "Login Success";
			var token = Facebook.Unity.AccessToken.CurrentAccessToken;
			userId = token.UserId.ToString ();
			Debug.Log ("LoginSuccess" + userId);
			ConnectionManager.Instance.myID = userId;
			//debugText.text += "\n" + userId;
			UserProfile ();
			PlayerPrefs.SetInt ("IsFbLogedIn", 1);
			UIManager.instance.mainMenuUI.gameObject.SetActive (true);
			UIManager.instance.loginPanel.SetActive (false);
			ConnectionManager.Instance.MakeConnection ();
			UIManager.instance.fbLoginCheckPanel.SetActive (false);
		} else if (result.Error != null) {
			//debugText.text += "\n Error" + result.Error.ToString ();
			Debug.Log ("Error in Login");
		}

	}

	public void GetFriends ()
	{
//		if (GameManager.instance.isRandomPlayer) {
//			return;
//		}
		if (FB.IsLoggedIn) {
			FB.API ("me?fields=id,name,friends.limit(50){name,picture}", HttpMethod.GET, this.GetFreindCallback);
		} else {
			GetFriendAsGuest ();
//			LoginForFriendsList ();
		}

	}

	private void LoginForFriendsList ()
	{
		FB.LogInWithReadPermissions (new List<string> (){ "public_profile", "email", "user_friends" }, this.FBLoginGetFriendCallBack);
		//FB.LogInWithPublishPermissions (new List<string> () { "publish_actions" }, this.FBLoginGetFriendCallBack);
	}

	void FBLoginGetFriendCallBack (ILoginResult result)
	{
		if (string.IsNullOrEmpty (result.Error)) {
			PlayerPrefs.SetInt ("IsFbLogedIn", 1);
			GetFriends ();
		}	
	}

	[HideInInspector]
	bool isFrndsAvials = false;

	void GetFriendAsGuest ()
	{
		DestroyFriendsList ();


		List<string> onlyGuest = new List<string> ();
		for (int i = 0; i < ConnectionManager.Instance.onlineFriends.Count; i++) {
			string str = ConnectionManager.Instance.onlineFriends [i];
			if (str [0] == 'G') {
				onlyGuest.Add (str);
				Debug.Log (str + "____________________________::::::::::");
				//instance friends
				GameObject g = Instantiate (FriendPrefab) as GameObject;
				g.SetActive (true);
				g.transform.SetParent (parentObject);
				g.transform.localScale = Vector3.one;
				g.transform.position = Vector3.zero;
				FriendsObjectList.Add (g);
				g.GetComponent<FriendsDetails> ().Name.text = str;
				Toggle btn = g.GetComponentInChildren<Toggle> ();
				btn.group = toggleGroup;
				string id = str;
				//g.GetComponent<FriendsDetails> ().ID = System.Convert.ToInt64 (id);
				AddListener (btn, id, str);
				if (ConnectionManager.Instance.onlineFriends.Contains (id)) {
					g.GetComponent<FriendsDetails> ().SetOnline (true);
				} else {
					g.GetComponent<FriendsDetails> ().SetOnline (false);
				}
			}
		}

	}

	void GetFreindCallback (IResult result)
	{
		DestroyFriendsList ();
		if (GameManager.instance.isRandomPlayer)
			GetFriendAsGuest ();
		string resposne = result.RawResult;
		Debug.Log (resposne);
		var data = (Dictionary<string, object>)result.ResultDictionary;
		var tagData = data ["friends"] as Dictionary<string,object>;
		var resultData = tagData ["data"] as List<object>;
		for (int i = 0; i < resultData.Count; i++) {
			var resultValue = resultData [i] as Dictionary<string, object>;
			var picture = resultValue ["picture"] as Dictionary<string ,object>;
			var picData = picture ["data"] as Dictionary<string,object>;
			string url = picData ["url"].ToString ();
			Debug.Log ("url : " + url);
			GameObject g = Instantiate (FriendPrefab) as GameObject;
			g.SetActive (true);
			g.transform.SetParent (parentObject);
			g.transform.localScale = Vector3.one;
			g.transform.position = Vector3.zero;
			FriendsObjectList.Add (g);
			string name = resultValue ["name"].ToString ();
			g.GetComponent<FriendsDetails> ().Name.text = name;
			Toggle btn = g.GetComponentInChildren<Toggle> ();
			btn.group = toggleGroup;
			Debug.Log (resultValue ["name"].ToString () + "  , " + resultValue ["id"].ToString ());
			string id = resultValue ["id"].ToString ();
			g.GetComponent<FriendsDetails> ().ID = System.Convert.ToInt64 (id);
			AddListener (btn, id, name);
			if (ConnectionManager.Instance.onlineFriends.Contains (id)) {
				g.GetComponent<FriendsDetails> ().SetOnline (true);
			} else {
				g.GetComponent<FriendsDetails> ().SetOnline (false);
			}
			if (!string.IsNullOrEmpty (id)) {
				FB.API ("https" + "://graph.facebook.com/" + id + "/picture?width=128&height=128", HttpMethod.GET, delegate(IGraphResult avatarResult) {
					if (avatarResult.Error != null) {
						Debug.Log (avatarResult.Error);
					} else {

						g.GetComponent<FriendsDetails> ().ProfilePic.sprite = Sprite.Create (avatarResult.Texture, new Rect (0, 0, 128, 128), new Vector2 (0.5f, 0.5f));
						;
					}
				});
			}
			isFrndsAvials = true;
		}
	}

	private void DestroyFriendsList ()
	{
		for (int i = 0; i < FriendsObjectList.Count; i++) {
			Destroy (FriendsObjectList [i]);
		}
		FriendsObjectList.Clear ();
	}

	string friendName = "";

	private void AddListener (Toggle btn, string fbID, string name)
	{
		btn.onValueChanged.AddListener ((bool value) => {
			if (value) {
				
				SetFriendsId (fbID);
				friendName = name;
				SocialManager.Instance.UpdateFriendName (name);
			}
		});

	}

	//	private void OnGUI ()
	//	{
	//		if (GUI.Button (new Rect (100, 100, 100, 50), "Login")) {
	//			Login ();
	//		}
	//
	//		if (GUI.Button (new Rect (100, 200, 100, 50), "GetFriends")) {
	//			GetFriends ();
	//		}
	//
	//		if (GUI.Button (new Rect (100, 300, 100, 50), "Invite")) {
	//			AppRequest ();
	//		}
	//	}

	public void SetFriendsId (string id)
	{
		SocialManager.Instance.isFriendsSelected = true;
		ConnectionManager.Instance.friedID = id;
		Debug.Log ("SetFriendsId : " + id);
		FB.API ("https" + "://graph.facebook.com/" + id + "/picture?width=128&height=128", HttpMethod.GET, delegate(IGraphResult avatarResult) {
			if (avatarResult.Error != null) {
				SocialManager.Instance.SetDefaultAvatar ();
				Debug.Log (avatarResult.Error);
			} else {

				SocialManager.Instance.UpdateFriendProfilePic (Sprite.Create (avatarResult.Texture, new Rect (0, 0, 128, 128), new Vector2 (0.5f, 0.5f)));

			}
		});

	}




	public void DownloadImageByID (string id)
	{
		Debug.Log ("ID " + id);
		FB.API ("https" + "://graph.facebook.com/" + id + "/picture?width=128&height=128", HttpMethod.GET, delegate(IGraphResult avatarResult) {
			if (avatarResult.Error != null) {
				Debug.Log (avatarResult.Error);
			} else {

				// = Sprite.Create (avatarResult.Texture, new Rect (0, 0, 128, 128), new Vector2 (0.5f, 0.5f));

			}
		});
	}


	//Share On Facebook.
	public void OnFacebookShare ()
	{
		Debug.Log ("OnFacebookShare");
		if (FB.IsLoggedIn) {
			FB.ShareLink (new System.Uri (appStoreLink), "Huliya", "want to bit me ? Download and play the Game", null, callback: ShareCallBck);

		} else {
			Debug.Log ("Please Login");
			CallFBLogin ();
		}
	}

	private void ShareCallBck (IShareResult result)
	{
		if (result.Cancelled || !string.IsNullOrEmpty (result.RawResult)) {
			Debug.Log ("Share Error : " + result.Error);
		} else if (!string.IsNullOrEmpty (result.PostId)) {
			Debug.Log (result.PostId);
		} else {
			Debug.Log ("Share success");
		}
	}

	public void UserProfile ()
	{
		if (FB.IsLoggedIn) {
			Debug.Log ("GetPlayerInfo");
			string queryString = "/me?fields=id,first_name,picture.width(128).height(128)";
			FB.API (queryString, HttpMethod.GET, GetPlayerInfoCallback);
		} else {
			Debug.Log ("Login to facebook");
		}
	}

	public string firstName = "";
	//
	private void GetPlayerInfoCallback (IGraphResult result)
	{
		Debug.Log ("GetPlayerInfoCallback");
		if (result.Error != null) {
			Debug.LogError (result.Error);
			return;
		}
		Debug.Log (result.RawResult);
	
		Dictionary<string,object> resultData = (Dictionary<string,object>)result.ResultDictionary;
		UIManager.instance.mainMenuUI.username.text = resultData ["first_name"].ToString ();
		string playerImgUrl = DeserializePictureURL (result.ResultDictionary);
		Debug.Log ("playerImgUrl " + playerImgUrl);
		SocialManager.Instance.UpdateUserProfile (playerImgUrl);

	}



	public static string DeserializePictureURL (object userObject)
	{
		var user = userObject as Dictionary<string, object>;
	
		object pictureObj;
		if (user.TryGetValue ("picture", out pictureObj)) {
			var pictureData = (Dictionary<string, object>)(((Dictionary<string, object>)pictureObj) ["data"]);
			return (string)pictureData ["url"];
		}
		return null;
	}

	public bool IsFbLogedin ()
	{
		return FB.IsLoggedIn;
	}

	public void GetScoreFB ()
	{
		foreach (GameObject g in friendsList) {
			Destroy (g);
		}

		if (FB.IsLoggedIn) {
			
			//UIAnimationController.Instance.loading = true;
			//UIController.Instance.ActiveUI (UIAnimationController.Instance.loadingImage);
			//UIController.Instance.DeactiveUI (SocialManager.Instance.facebookPanel);
			//SetScoreToFB (ScoreHandler.instance.GetCurrentScore ().ToString ());
			FB.API ("/app/scores?fields=score,user.limit(30)", HttpMethod.GET, GetScoreCallBack);
		} else {
			//UIAnimationController.Instance.loading = false;
			//UIController.Instance.ActiveUI (SocialManager.Instance.facebookPanel);

			//CreateLeaderBoard ("Me", ScoreHandler.instance.GetCurrentScore ().ToString (), "");
		}
	}

	public void LoginWithFacebook ()
	{
		FB.LogInWithReadPermissions (new List<string> (){ "public_profile", "email", "user_friends" }, this.APICallback);
		FB.LogInWithPublishPermissions (new List<string> () { "publish_actions" }, this.APICallback);
	}

	void APICallback (ILoginResult result)
	{
		if (result.Error != null) {
			Debug.Log ("Error in login ");
			//UIController.Instance.ActiveUI (SocialManager.Instance.facebookPanel);
		} else {
			GetScoreFB ();
		}

	}

	private void GetScoreCallBack (IResult result)
	{
		foreach (GameObject g in friendsList) {
			Destroy (g);
		}
		friendsList.Clear ();
		Debug.Log (result.RawResult);
		if (FB.IsLoggedIn) {
			var dict = Json.Deserialize (result.RawResult) as Dictionary<string,object>;
			List<object> scores = dict ["data"] as List<object>;
			for (int i = 0; i < scores.Count; i++) {
				Dictionary<string , object> scoreData = scores [i] as Dictionary<string , object>;
				object score = scoreData ["score"];
				var user = scoreData ["user"] as Dictionary<string , object>;
				var userName = user ["name"];
				//debugText.text = score.ToString () + "-" + userName.ToString ();
				Debug.Log (score.ToString () + "-" + userName.ToString ());
				//		UIAnimationController.Instance.loading = false;
				//		UIController.Instance.DeactiveUI (UIAnimationController.Instance.loadingImage);
				CreateLeaderBoard (userName.ToString (), score.ToString (), user ["id"].ToString ());

			}
		}
	}

	public void SetScoreToFB (string score)
	{
		if (!IsInternetAvailabe)
			return;
		if (FB.IsLoggedIn) {
			var scoreData = new Dictionary<string,string> ();
			scoreData ["score"] = score;
			FB.API ("/me/scores", HttpMethod.POST, delegate(IGraphResult result) {
				Debug.Log ("result Submit + " + result.RawResult);	
				//debugText.text = "result Submit + " + result.RawResult;
			}, scoreData);
		} 
	}

	//public GameObject playerInfoPrefab;
	//public Transform parentProfile;
	// info;
	List<GameObject> friendsList = new List<GameObject> ();

	public void CreateLeaderBoard (string userName, string score, string id)
	{
		/*GameObject profile = Instantiate (playerInfoPrefab) as GameObject;
		profile.SetActive (true);
		friendsList.Add (profile);
		profile.transform.SetParent (parentProfile);
		profile.transform.localScale = Vector3.one;
		Image avatar = profile.transform.Find ("ProfilePic").GetComponent<Image> ();
		//info = profile.GetComponent<UserInfo> ();
	
		//info.UpdateUserInfo (userName, score);
		if (!string.IsNullOrEmpty (id)) {
			FB.API ("https" + "://graph.facebook.com/" + id + "/picture?width=128&height=128", HttpMethod.GET, delegate(IGraphResult avatarResult) {
				if (avatarResult.Error != null) {
					Debug.Log (avatarResult.Error);
				} else {
				
					avatar.sprite = Sprite.Create (avatarResult.Texture, new Rect (0, 0, 128, 128), new Vector2 (0.5f, 0.5f));
					;
				}
			});
		}
*/
	}

	public void DeleteAllScoreFromFB ()
	{
		if (FB.IsLoggedIn) {
			FB.API ("/me/scores", HttpMethod.DELETE, delegate(IGraphResult result) {
				if (result.Error != null)
					Debug.Log (result.Error);
				else
					Debug.Log (result.RawResult);
			});
		}
	}

	public void UpdateTopScorrer ()
	{
		if (FB.IsLoggedIn) {
			//UIAnimationController.Instance.gameoverLoadning = true;
			//UIController.Instance.ActiveUI (UIAnimationController.Instance.loadingGameOverImage);
			FB.API ("/app/scores?fields=score,user.limit(1)", HttpMethod.GET, GetTopScorrer);
		} else {
			//ScoreHandler.instance.ScoreAI ();
		}
	}

	private void GetTopScorrer (IResult result)
	{
		var dict = Json.Deserialize (result.RawResult) as Dictionary<string,object>;
		List<object> scores = dict ["data"] as List<object>;
		Dictionary<string , object> scoreData = scores [0] as Dictionary<string , object>;
		object score = scoreData ["score"];
		var user = scoreData ["user"] as Dictionary<string , object>;
		var userName = user ["name"];
		//testText.text = userName.ToString () + " | " + score.ToString ();
		Debug.Log ("UserName " + userName.ToString ());
		PlayerPrefs.SetString ("FriendName", userName.ToString ());
		PlayerPrefs.SetInt ("FriendScore", Convert.ToInt32 (score.ToString ()));
		//UIAnimationController.Instance.gameoverLoadning = false;
		//UIController.Instance.DeactiveUI (UIAnimationController.Instance.loadingGameOverImage);
		//ScoreHandler.instance.ScoreAI ();

		Debug.Log (PlayerPrefs.GetString ("FriendName") + " " + PlayerPrefs.GetInt ("FriendScore"));
	}


	public void InviteFriends ()
	{
		if (FB.IsLoggedIn) {
			FB.Mobile.AppInvite (new Uri (appStoreLink), null, this.InviteCallback);
		} else {
			FB.LogInWithReadPermissions (new List<string> (){ "public_profile", "email", "user_friends" }, this.InviteFreindLoginCallback);
			FB.LogInWithPublishPermissions (new List<string> () { "publish_actions" }, this.InviteFreindLoginCallback);
		}
	}

	private void InviteCallback (IAppInviteResult result)
	{
		if (string.IsNullOrEmpty (result.Error)) {
			Debug.Log (result.Error);
		} else {
			Debug.Log ("Successfully Invited");
		}
	}

	private void InviteFreindLoginCallback (ILoginResult result)
	{
		if (string.IsNullOrEmpty (result.Error)) {
			PlayerPrefs.SetInt ("IsFbLogedIn", 1);
			InviteFriends ();
		}
	}

	public void LogOutFB ()
	{
		FB.LogOut ();
	}

	public void GetFriendsNameByID (string id)
	{
		FB.API ("me?fields=friends.limit(30){id,name}", HttpMethod.GET, delegate(IGraphResult result) {
			if (result.Error != null) {
				Debug.Log (result.Error);
			} else {
				
				//Debug.Log (result.RawResult);
				var data = (Dictionary<string, object>)result.ResultDictionary;
				var tagData = data ["friends"] as Dictionary<string,object>;
				var resultData = tagData ["data"] as List<object>;
				for (int i = 0; i < resultData.Count; i++) {
					var resultValue = resultData [i] as Dictionary<string, object>;

					string friendID = resultValue ["id"].ToString ();
					if (id == friendID) {
						UIManager.instance.inviteUI.playerNameTxt.text = resultValue ["name"].ToString ();
						//Debug.Log (resultValue ["name"].ToString ());
					}
				}
			}
		});
	}

	public void FacebookLike ()
	{
		Application.OpenURL ("https://www.facebook.com/akash.renewin");
	}

	public string GetSelectedFriend ()
	{
		return friendName;
	}
}



