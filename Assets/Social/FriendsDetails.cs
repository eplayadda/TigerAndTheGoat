using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsDetails : MonoBehaviour
{
	public Text Name;
	public Image ProfilePic;
	public long ID;
	public Image onlineIcon;
	void OnEnable()
	{
		onlineIcon = transform.GetChild (3).GetComponent<Image>();
	}
	public void SetOnline(bool isOnline)
	{
		if (isOnline) {
			Debug.Log ("on");
			onlineIcon.color = Color.yellow;
		} else {
			Debug.Log ("off");
			onlineIcon.color = Color.red;
		}

	}
}
