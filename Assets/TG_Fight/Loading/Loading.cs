using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour {
	public GameObject[] loading;
	 int i;
	WaitForSeconds waitTime = new WaitForSeconds(.3f);

	void Start () 
	{
		StartCoroutine(LoadingImg());
	}

	IEnumerator LoadingImg()
	{
		while(true)
		{
			if (i % 3 == 0) {
				loading[0].SetActive(false);
				loading[1].SetActive(false);
				loading[2].SetActive(false);
			}
			loading[i%3].SetActive(true);
			i++;
			yield return waitTime;
		}

	}

}
