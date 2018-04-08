using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUI : MonoBehaviour
{

	public void OnClickResume ()
	{
		AudioManager.Instance.PlaySound (AudioManager.SoundType.ButtonClick);

		Time.timeScale = 1;
		GameManager.instance.currGameStatus = eGameStatus.play;
		UIManager.instance.pausePanel.SetActive (false);
		//UIAnimationController.Instance.PausePanleAnimation (UIManager.instance.pausePanel, UIManager.instance.pauseEndPos.localPosition.y);


	}

	public void OnClickRestart ()
	{
		AudioManager.Instance.PlaySound (AudioManager.SoundType.ButtonClick);

		GameManager.instance.currTurnStatus = BordManager.instace.lstGameTurn;
		GameManager.instance.currGameStatus = eGameStatus.play;
		GameManager.instance.OnGameModeSelected ((int)GameManager.instance.currGameMode);
		UIManager.instance.pausePanel.SetActive (false);
		Time.timeScale = 1;

	}

	public void OnClickMainMenu ()
	{
		AudioManager.Instance.PlaySound (AudioManager.SoundType.ButtonClick);

		GameManager.instance.currGameStatus = eGameStatus.none;
		UIManager.instance.pausePanel.SetActive (false);
		UIManager.instance.gamePlayUI.gameObject.SetActive (false);
		UIManager.instance.mainMenuUI.gameObject.SetActive (true);
		Time.timeScale = 1;

	}

	public void OnClickQuit ()
	{
		Time.timeScale = 1;
		UIManager.instance.ExitPanleActive ();
	}


	public void OnClickSetting ()
	{
		Time.timeScale = 1;
		UIManager.instance.mainMenuUI.OnSettingActive ();
	}

	public void PauseMenuUICallBack ()
	{
		if (GameManager.instance.currGameStatus != eGameStatus.pause) {
			Debug.Log ("Pause to Play");
			UIManager.instance.pausePanel.SetActive (false);
			//	UIManager.instance.pausePanel.transform.localPosition = UIManager.instance.pauseEntryPos.localPosition;
		}
	}
}
