using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginPanelView : MonoBehaviour {

	void OnEnable()
	{
		Debug.Log ("Login Panel Ads Request");
		AdsHandler.Instance.ShowInterstitialAds();
	}
}
