﻿using System.Collections;
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
	public Toggle _toggleTutorial;
	public Toggle _toggleVivration;
    public bool isTimeUp;
    public bool isVibrateAlow;

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
        //PlayerPrefs.DeleteAll();
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		GameAllow ();
	}

	public void TutorialAllow ()
	{
		Debug.Log (_toggleTutorial.isOn);
		if (_toggleTutorial.isOn) {
			showTutorial = true;
			PlayerPrefs.SetInt ("Tutorials", 1);
		} else {
			showTutorial = false;
			PlayerPrefs.SetInt ("Tutorials", 0);
		}
	}

    public void IsVibrateAlow()
    {
        if (_toggleVivration.isOn)
        {
            isVibrateAlow = true;
            PlayerPrefs.SetInt("isVibrateAlow", 1);
        }
        else
        {
            isVibrateAlow = false;
            PlayerPrefs.SetInt("isVibrateAlow", 0);
        }
    }
    public bool isAllowPlay = true;
	void GameAllow ()
	{
        if (PlayerPrefs.GetInt("TutorialInit", 0) == 0)
        {
            PlayerPrefs.SetInt("Tutorials", 1);
            PlayerPrefs.SetInt("TutorialInit",1);
        }
		if (PlayerPrefs.GetInt ("Tutorials") == 1) {
			_toggleTutorial.isOn = true;
			showTutorial = true;
		} else {
			_toggleTutorial.isOn = false;
			showTutorial = false;
		}
        /*
		WWW www = new WWW ("http://www.eplayadda.com/datacheck/api/values");
		yield return www;
		if (string.IsNullOrEmpty (www.error)) {
			string str = www.text [2].ToString ();
			if (str == "0") {
				isAllowPlay = false;
				Application.Quit ();
			}
		}*/
	
	}

}
