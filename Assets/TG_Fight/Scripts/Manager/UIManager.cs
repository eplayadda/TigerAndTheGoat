using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager instance;
	public MainMenuUI mainMenuUI;
	public GamePlayUI gamePlayUI;
	public GameOverUI gameOverUI;
	public GameObject loginPanel;
	public InviteUI inviteUI;
	public GameObject fbFriendsPanel;
	public GameObject pausePanel;
	public PauseMenuUI pauseMenuUI;

	public GameObject internetCheckPanel;
	public GameObject fbLoginCheckPanel;
	public GameObject panelLoading;

	public GameObject tutorialLeft;
	public GameObject tutorialRight;
	public GameObject tutorialParent;
	public GameObject friendDecliendPanel;
	public Transform pauseEntryPos;
	public Transform pauseEndPos;

	public GameObject exitPanel;
	public Transform exitStartPos;
	public Transform exitEndPos;
	private bool isTutorialDone = false;

	private float tutorialDelay = 5.0f;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		panelLoading.SetActive (true);
		Invoke ("LoadingDisable", 2.0f);
	}

	void LoadingDisable ()
	{
		panelLoading.SetActive (false);
		SocialManager.Instance.facebookManager.mStart ();
		TutorialReset ();
		AdsHandler.Instance.ShowBannerAdsMenuPage ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			if (isTutorialDone) {
				if (GameManager.instance.currGameStatus == eGameStatus.play) {
					//if (GameManager.instance.currGameMode != eGameMode.vServerMulltiPlayer) 
					{
						//pausePanel.SetActive (true);
						//GameManager.instance.currGameStatus = eGameStatus.pause;
						//Time.timeScale = 0;
						gamePlayUI.OnBackClicked ();
					} 
//					else {
//					GameManager.instance.currGameStatus = eGameStatus.none;
//					pausePanel.SetActive (false);
//					gamePlayUI.gameObject.SetActive (false);
//					mainMenuUI.gameObject.SetActive (true);
//					ConnectionManager.Instance.OnGameOverSendData ();
//					Time.timeScale = 1;
//				}

				}
				if (GameManager.instance.currGameStatus == eGameStatus.pause) {
					pauseMenuUI.OnClickResume ();
				
				}
				if (GameManager.instance.currGameStatus == eGameStatus.setting) {
					mainMenuUI.OnClickBackSetting ();
				
				}
				if (GameManager.instance.currGameStatus == eGameStatus.playerselection) {
					mainMenuUI.PlayerSelectionBack ();
				} 
				if (GameManager.instance.currGameStatus == eGameStatus.mainmenu || GameManager.instance.currGameStatus == eGameStatus.gameover) {
					ExitPanleActive ();
				}
			}
//			if (GameManager.instance.currGameStatus == eGameStatus.exit) {
//				OnClickNo ();
//			}
		}
	}

	public void ExitPanleActive ()
	{
		//GameManager.instance.currGameStatus = eGameStatus.exit;
		exitPanel.SetActive (true);
		UIAnimationController.Instance.ExitPanleAnimation (exitPanel, exitEndPos.localPosition.x);
	}

	public void FriendGameOver ()
	{
		OnDicliend ();
		Debug.Log ("Friend Game Quit");
	}

	public void OnGameOver ()
	{
		Invoke ("GameOverInvoke", 1f);
	}

	void GameOverInvoke ()
	{
		gameOverUI.gameObject.SetActive (true);
	}

	public void DisableAllUI ()
	{
		mainMenuUI.gameObject.SetActive (false);
		loginPanel.SetActive (false);
		gameOverUI.gameObject.SetActive (false);
		inviteUI.gameObject.SetActive (false);
		fbFriendsPanel.gameObject.SetActive (false);
	}

	public void OnClickAsGuest ()
	{
		AudioManager.Instance.PlaySound (AudioManager.SoundType.ButtonClick);
		GameManager.instance.currentGameType = GameType.OffLine;
		ConnectionManager.Instance.MakeConnection ();
		loginPanel.SetActive (false);
		mainMenuUI.gameObject.SetActive (true);
	}

	public void OnSendRequest (int price, int type, string pName)
	{
		Debug.Log (">>>>>>>>>>>>>>>>>>>>>>>." + pName);
		inviteUI.playerNameTxt.text = pName;

		inviteUI.gameObject.SetActive (true);
		inviteUI.friendAnimalType = type;
	}

	public void OnGameStartOnServer ()
	{
		gamePlayUI.gameObject.SetActive (true);
		gamePlayUI.OnServerGameStart ();

	}

	public void OnChallangeAccepted (int a)
	{
		gamePlayUI.OnServerPlayerAccepted (a);
	}

	public void OnFriendInviteAccepted ()
	{
		DisableAllUI ();
		gamePlayUI.gameObject.SetActive (true);
		gamePlayUI.OnInvieAcceptedByME ();
		BordManager.instace.OnGameStart ();

	}

	public void OnCancleFriendList ()
	{
		UIManager.instance.mainMenuUI.selectFriendPopup.text = "";
		SocialManager.Instance.isFriendsSelected = false;
		SocialManager.Instance.SetDefaultAvatar ();
		SocialManager.Instance.isFriendsSelected = false;
		SocialManager.Instance.UpdateFriendName ("");

		fbFriendsPanel.SetActive (false);
	}

	public void OnCloseFbCheck ()
	{
		fbLoginCheckPanel.SetActive (false);
	}

	public void NoINternetDisplay ()
	{
		internetCheckPanel.SetActive (true);
		UIAnimationController.Instance.InternetCheckPanel (internetCheckPanel, 0.25f);
	}

	public void OnCloseInternetCheck ()
	{
		internetCheckPanel.SetActive (false);
		internetCheckPanel.transform.localScale = Vector3.zero;
	}

	public void DisplayTutorial ()
	{
		if (GameManager.instance.showTutorial) {
			isTutorialDone = false;
			tutorialParent.SetActive (true);
			tutorialLeft.SetActive (true);
			UIAnimationController.Instance.TutorialAnimation (tutorialLeft);
			Invoke ("DisplayRightTutorial", tutorialDelay);
		}

	}

	private void DisplayRightTutorial ()
	{
		tutorialLeft.SetActive (false);
		tutorialRight.SetActive (true);
		UIAnimationController.Instance.TutorialAnimation (tutorialRight);
		Invoke ("TutorialReset", tutorialDelay);
	}

	private void TutorialReset ()
	{
		tutorialParent.SetActive (false);
		tutorialLeft.SetActive (false);
		tutorialRight.SetActive (false);
		tutorialLeft.transform.localScale = Vector3.zero;
		tutorialRight.transform.localScale = Vector3.zero;
		isTutorialDone = true;
	}

	public void OnDicliend ()
	{
		inviteUI.gameObject.SetActive (false);
		friendDecliendPanel.SetActive (true);
	}

	public void OnMenuBttnClicked ()
	{
		GameManager.instance.currGameStatus = eGameStatus.none;
		pausePanel.SetActive (false);
		gamePlayUI.gameObject.SetActive (false);
		mainMenuUI.gameObject.SetActive (true);
		Time.timeScale = 1;
		friendDecliendPanel.SetActive (false);
		inviteUI.gameObject.SetActive (false);
		mainMenuUI.ServerRoomPanel.SetActive (false);
		fbFriendsPanel.SetActive (false);
		mainMenuUI.settingPanle.SetActive (false);
		mainMenuUI.selectPlayerPanel.SetActive (false);
	}

	public void BackButtonClickSingePlayer ()
	{
		pausePanel.SetActive (true);
		Invoke ("PauseDelay", 0.2f);
	}

	void PauseDelay ()
	{
		GameManager.instance.currGameStatus = eGameStatus.pause;
		Time.timeScale = 0;
	}

	public void OnClickYes ()
	{
		Application.Quit ();
	}

	public void OnClickNo ()
	{
		exitPanel.SetActive (false);
		exitPanel.transform.localPosition = exitStartPos.localPosition;
		if (gameOverUI.isGameOver) {
			GameManager.instance.currGameStatus = eGameStatus.gameover;
		}
	}

}
