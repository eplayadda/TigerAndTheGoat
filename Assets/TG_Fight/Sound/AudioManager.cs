﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
	public enum SoundType
	{
		None = 0,
		GameStart,
		Success,
		GameOver,
		ButtonClick,
		LevelComplete
	}

	private static AudioManager _instance;

	public static AudioManager Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<AudioManager> ();
				//DontDestroyOnLoad (_instance);
			}
			return _instance;
		}
		set {
			_instance = value;
		}
	}

	public Toggle _toggleMute;

    public GameObject[] togglePanel;

	public Slider musicSlider;
//	public Slider soundSlider;

	public AudioClip[] gameSoundClips;
	public AudioSource[] audioSources;

	void Start ()
	{
       // PlayerPrefs.DeleteAll();
		audioSources = GetComponents<AudioSource> ();
		StartGamePlayAudio ();
		Debug.Log (PlayerPrefs.GetInt ("Once")+"????");
		if (PlayerPrefs.GetInt ("Once") == 0) {
			PlayerPrefs.SetFloat ("music", 1f);
			musicSlider.value = 1f;
			PlayerPrefs.SetInt ("Once", 1);
		} else {
			musicSlider.value = PlayerPrefs.GetFloat ("music");
		}
		if (PlayerPrefs.GetInt ("mute") == 0) {
            _toggleMute.isOn = false;
		}
		else{
            _toggleMute.isOn = true;
		}
		SetValue ();
	}

	void SetValue()
	{
		audioSources [0].volume = PlayerPrefs.GetFloat ("music");
        if (_toggleMute.isOn)
        {
            foreach (AudioSource source in audioSources)
            {
                source.mute = true;
            }
        }
        else
        {
            foreach (AudioSource source in audioSources)
            {
                source.mute = false;
            }
        }

	}
	public void GameplaySoundOnOff ()
	{
		Debug.Log (_toggleMute.isOn);

		if (_toggleMute.isOn) {
			PlayerPrefs.SetInt ("mute",1) ;
			Debug.Log ("Pause "+PlayerPrefs.GetInt("mute"));

            //togglePanel [0].SetActive (true);
            //	togglePanel [1].SetActive (false);
            foreach (AudioSource source in audioSources) {
				source.mute = true;
			}

		} else {
			PlayerPrefs.SetInt ("mute",0) ;
			Debug.Log ("Play "+PlayerPrefs.GetInt("mute"));
		//	togglePanel [0].SetActive (false);
		//	togglePanel [1].SetActive (true);
			foreach (AudioSource source in audioSources) {
				source.mute = false;
			}
		}
	}

	public void StartGamePlayAudio ()
	{
		audioSources [0].loop = true;
		audioSources [0].clip = gameSoundClips [0];
		audioSources [0].Play ();
	}

	public void ChangeMusicVolume ()
	{
		audioSources [0].volume = musicSlider.value <= 0.5f ? musicSlider.value : 0.5f;
		PlayerPrefs.SetFloat ("music",musicSlider.value);
		Debug.Log ("musicSlider.value"+musicSlider.value);

	}

	public void ChangeSoundVolume ()
	{
//		audioSources [1].volume = soundSlider.value; 
//		PlayerPrefs.SetFloat ("sound",soundSlider.value);


	}

	public void PlaySound (SoundType type)
	{
		switch (type) {

		case SoundType.Success:
			{
				audioSources [1].clip = gameSoundClips [3];
			}
			break;
		case SoundType.GameStart:
			{
				//audioSources [1].clip = gameSoundClips [2];
			}
			break;
		case SoundType.ButtonClick:
			{
				audioSources [1].clip = gameSoundClips [1];
			}
			break;
		case SoundType.GameOver:
			{
				audioSources [1].clip = gameSoundClips [2];
			}
			break;
		case SoundType.LevelComplete:
			{
				audioSources [1].clip = gameSoundClips [4];
			}
			break;
		}
		if (audioSources [1].isPlaying)
			audioSources [1].Stop ();
		audioSources [1].Play ();
	}
		
}
