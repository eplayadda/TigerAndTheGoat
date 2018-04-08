using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
	UIManager uiManager;

	public Text msgTxt;
	public Text winStatus;
	public GameObject[] WinnerImage;
	public GameObject winnerLogo;
	public Sprite[] winnerSprites;
	public GameObject shareButton;
	public GameObject replayBtn1;
	public GameObject replayBtn2;
	public bool isGameOver;

	void OnEnable ()
	{
		isGameOver = true;
		AdsHandler.Instance.ShowBannerAdsMenuPage ();
		AdsHandler.Instance.HideBannerAdsPausePage ();
		uiManager = UIManager.instance;
		winnerLogo.GetComponent<Image> ().sprite = winnerSprites [(int)GameManager.instance.myAnimalType - 1];
		if (BordManager.instace.currWinStatus == BordManager.eWinStatus.tiger) {
			GameManager.instance.currGameStatus = eGameStatus.gameover;
			msgTxt.text = "Tiger Win The Game";
		} else {
			
			GameManager.instance.currGameStatus = eGameStatus.gameover;	
			msgTxt.text = "Goat Win The Game";
		}
		GameResult ();
		AdsHandler.Instance.HideBannerAdsMenuPage ();
		AdsHandler.Instance.ShowBannerAdsPausePage ();

	}

	void GameResult ()
	{
		if (BordManager.instace.currWinStatus.ToString () == GameManager.instance.myAnimalType.ToString ()) {
			ScoreHandler.instance.SetScore ();
			winStatus.text = "You Won";
			AudioManager.Instance.PlaySound (AudioManager.SoundType.Success);
			WinnerImage [0].SetActive (true);
			WinnerImage [1].SetActive (true);
			shareButton.SetActive (true);
			replayBtn1.SetActive (true);
			replayBtn2.SetActive (false);

		} else {
			winStatus.text = "You Lost";
			shareButton.SetActive (false);
			replayBtn1.SetActive (false);
			replayBtn2.SetActive (true);
			WinnerImage [0].SetActive (false);
			WinnerImage [1].SetActive (false);
			AudioManager.Instance.PlaySound (AudioManager.SoundType.GameOver);

		}
	}

	public void OnReplay ()
	{
		AudioManager.Instance.PlaySound (AudioManager.SoundType.ButtonClick);
		uiManager.gamePlayUI.gameObject.SetActive (false);
		uiManager.mainMenuUI.gameObject.SetActive (true);
		uiManager.gameOverUI.gameObject.SetActive (false);
//		Application.LoadLevel (0);
	}
}
