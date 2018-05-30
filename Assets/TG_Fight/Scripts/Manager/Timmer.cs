using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timmer : MonoBehaviour {
    Image timmerImg;
   public bool isStart;
	// Use this for initialization
	void Awake () {
        timmerImg = gameObject.GetComponent<Image>();
	}

    public void ResetTimmer()
    {
        isStart = true;
        timmerImg.fillAmount = 1;
    }

    public void Stop()
    {
        isStart = false;
        timmerImg.fillAmount = 0;
    }
	// Update is called once per frame
	void Update () {
        if (isStart)
        {
            timmerImg.fillAmount -= Time.deltaTime * .1f;
            if (timmerImg.fillAmount <= 0)
            {
                isStart = false;
                BordManager.instace.TimeUPStartAI();
            }
        }
	}
}
