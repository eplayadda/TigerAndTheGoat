using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eGameStatus
{
	none,
	mainmenu,
	playerselection,
	play,
	pause,
	setting,
	exit,
	gameover
}

public enum eGameMode
{
	none = 0,
	vCPU,
	vLocalMulltiPlayer,
	vServerMulltiPlayer
}

public enum eTurnStatus
{
	none,
	my,
	friend
}

public enum ePlayerIdentity
{
	none,
	host,
	client
}

public enum eAnimalType
{
	none = 0,
	tiger = 1,
	goat = 2
}

public enum GameType
{
	None = 0,
	OnLine = 1,
	OffLine = 2
}

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public eGameStatus currGameStatus;
	public eGameMode currGameMode;
	public eTurnStatus currTurnStatus;
	public ePlayerIdentity currPlayerIdentity;
	public eAnimalType myAnimalType;
	public eAnimalType friendAnimalType;
	public GameType currentGameType;
	public int totalNoOfGoat;
	public int totalNoOfTiger;
	public bool isRandomPlayer = false;
	public bool showTutorial = false;
	public Toggle _toggle;


	void Awake ()
	{
		if (instance == null)
			instance = this;
		
	}

	public void OnGameModeSelected (int pMode)
	{
		currGameMode = (eGameMode)pMode;
		currPlayerIdentity = ePlayerIdentity.host;
		BordManager.instace.OnGameStart ();
	}

	void Start ()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		StartCoroutine (GameAllow ());
	}

	public void TutorialAllow ()
	{
		Debug.Log (_toggle.isOn);
		if (_toggle.isOn) {
			showTutorial = true;
			PlayerPrefs.SetInt ("Tutorials", 1);
		} else {
			showTutorial = false;
			PlayerPrefs.SetInt ("Tutorials", 0);
		}
	}
	public bool isAllowPlay = true;
	IEnumerator GameAllow ()
	{
		if (PlayerPrefs.GetInt ("Tutorials") == 1) {
			_toggle.isOn = true;
			showTutorial = true;
		} else {
			_toggle.isOn = false;
			showTutorial = false;
		}
		WWW www = new WWW ("http://www.eplayadda.com/datacheck/api/values");
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			string str = www.data [2].ToString ();
			if (str == "0") {
				isAllowPlay = false;
				Application.Quit ();
			}
		}
	
	}

}
