using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteUI : MonoBehaviour
{
	GameManager gameManager;
	UIManager uiManager;
	public Text playerNameTxt;
	public int friendAnimalType;

	void OnEnable ()
	{
		gameManager = GameManager.instance;
		uiManager = UIManager.instance;
	}

	public void OnInviteClicked (bool isAccepted)
	{
		gameObject.SetActive (false);
		SocialManager.Instance.SetDefaultAvatar ();
		SocialManager.Instance.UpdateFriendName ("");
		UIManager.instance.mainMenuUI.selectFriendPopup.text = "";
		SocialManager.Instance.isFriendsSelected = false;
		UIManager.instance.mainMenuUI.selectFriendPopup.text = "";
		if (isAccepted) {
			GameManager.instance.currGameStatus = eGameStatus.play;
			GameManager.instance.currGameMode = eGameMode.vServerMulltiPlayer;
			GameManager.instance.currPlayerIdentity = ePlayerIdentity.client;
			GameManager.instance.myAnimalType = (eAnimalType)friendAnimalType;
			if (friendAnimalType == 1) {
				GameManager.instance.currTurnStatus = eTurnStatus.friend;
				friendAnimalType = 2;
				uiManager.gamePlayUI.tigerText.text = "You";
				uiManager.gamePlayUI.goatText.text = "Friend";
			} else {
				uiManager.gamePlayUI.tigerText.text = "Friend";
				uiManager.gamePlayUI.goatText.text = "You";
				friendAnimalType = 1;
				GameManager.instance.currTurnStatus = eTurnStatus.my;
			}
			GameManager.instance.friendAnimalType = (eAnimalType)friendAnimalType;
			ConnectionManager.Instance.IacceptChallage (1);
			uiManager.OnFriendInviteAccepted ();
			uiManager.friendDecliendPanel.SetActive (false);
			if (uiManager.pausePanel.activeInHierarchy) {
				Time.timeScale = 1.0f;
				uiManager.pauseMenuUI.PauseMenuUICallBack ();
			}
			if (uiManager.mainMenuUI.settingPanle.activeInHierarchy) {
				uiManager.mainMenuUI.OnClickBackSetting ();
			}
			if (uiManager.mainMenuUI.selectPlayerPanel.activeInHierarchy) {
				uiManager.mainMenuUI.PlayerSelectionBack ();
			}
		} else {
			ConnectionManager.Instance.IacceptChallage (0);

		}
//		playerNameTxt.text = "Friend";
	}

}
