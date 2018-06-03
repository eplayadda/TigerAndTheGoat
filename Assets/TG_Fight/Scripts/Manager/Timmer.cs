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
            if(GameManager.instance.currGameStatus == eGameStatus.play)
                 timmerImg.fillAmount -= Time.deltaTime * .06f;
            if (timmerImg.fillAmount <= 0)
            {
                isStart = false;
                if (GameManager.instance.currGameMode == eGameMode.vServerMulltiPlayer && GameManager.instance.currTurnStatus != eTurnStatus.my)
                    return;
                BordManager.instace.TimeUPStartAI();
            }
        }
	}
}
