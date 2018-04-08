using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{
	GameManager gameManager;
	UIManager uiManager;
	public GameObject waitingPanel;
	public Button waittingPanelBtn;
	public GameObject waittingMsgPnl;
	public Text tigerText;
	public Text goatText;

	void OnEnable ()
	{
		AdsHandler.Instance.HideBannerAdsMenuPage ();
		AdsHandler.Instance.HideBannerAdsPausePage ();
		gameManager = GameManager.instance;
		uiManager = UIManager.instance;
		waitingPanel.SetActive (false);

	}

	public void OnBackClicked ()
	{
		AdsHandler.Instance.HideBannerAdsMenuPage ();
		AdsHandler.Instance.HideBannerAdsPausePage ();
		AudioManager.Instance.PlaySound (AudioManager.SoundType.ButtonClick);

		if (GameManager.instance.currGameMode != eGameMode.vServerMulltiPlayer) {
			uiManager.BackButtonClickSingePlayer ();
		} else {
			uiManager.OnMenuBttnClicked ();
			ConnectionManager.Instance.OnGameOverSendData ();

		}
	}

	public void WaittingFriendBtn ()
	{
		waittingMsgPnl.SetActive (true);
		waittingPanelBtn.gameObject.SetActive (true);
		waittingPanelBtn.interactable = false;
		waitingPanel.SetActive (true);
	}

	public void OnServerPlayerAccepted (int a)
	{
		if (a == 1) {
			waittingPanelBtn.interactable = true;
			waittingMsgPnl.SetActive (false);
			gameManager.currGameStatus = eGameStatus.play;
		} else {
			Debug.Log ("Not accpetd");
			UIManager.instance.OnDicliend ();
		}


	}

	public void OnGameStart ()
	{
		gameManager.currGameStatus = eGameStatus.play;
		waitingPanel.SetActive (false);
		ConnectionManager.Instance.OnServerGameStart ();
	}

	public void OnServerGameStart ()
	{
		waitingPanel.SetActive (false);
		gameManager.currGameStatus = eGameStatus.play;
	}

	public void OnInvieAcceptedByME ()
	{
		waitingPanel.SetActive (true);
		waittingMsgPnl.SetActive (true);
		waittingPanelBtn.gameObject.SetActive (false);
	}

}
