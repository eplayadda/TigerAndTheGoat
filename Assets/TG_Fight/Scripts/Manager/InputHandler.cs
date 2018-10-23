﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	public static InputHandler instance;
    public Queue<int> myCurrTurnInput = new Queue<int>();
    int tryCount;
    void Awake()
	{
		if (instance == null)
			instance = this;
	}
	public void OnInputTaken(int pData)
	{
        Debug.Log("Current Input >>> "+pData);
        if (GameManager.instance.isTimeUp)
            return;
		if (!GameManager.instance.isAllowPlay)
			QuitGame ();
		AudioManager.Instance.PlaySound (AudioManager.SoundType.ButtonClick);

		if (GameManager.instance.currGameMode == eGameMode.vServerMulltiPlayer && GameManager.instance.currTurnStatus != eTurnStatus.my)
			return;
		if (GameManager.instance.currGameMode == eGameMode.vCPU && GameManager.instance.currTurnStatus != eTurnStatus.my)
			return;
        BordManager.instace.OnInputByUser(pData);
		if (GameManager.instance.currGameMode == eGameMode.vServerMulltiPlayer)
        {
            Debug.Log("Input By User "+pData);
            myCurrTurnInput.Enqueue(pData);
		//	ConnectionManager.Instance.OnSendMeAnswer (pData+"");
            StartCoroutine("WaitAndSendData");
        }
	}

    public void OnInputByAI(int pData)
    {
        BordManager.instace.OnInputByUser(pData);
        if (GameManager.instance.currGameMode == eGameMode.vServerMulltiPlayer)
        {
            Debug.Log("Input By User " + pData);
            myCurrTurnInput.Enqueue(pData);
         //   ConnectionManager.Instance.OnSendMeAnswer(pData + "");
            StartCoroutine("WaitAndSendData");
        }
    }
	public void OnInputTakenBYServer(int pData)
	{
		if (!GameManager.instance.isAllowPlay)
			QuitGame ();
        myCurrTurnInput.Clear();
        StopCoroutine("WaitAndSendData");
		BordManager.instace.OnInputByUser(pData);
        Debug.Log("Input By Friend " + pData);

        //		GameManager.instance.currTurnStatus = eTurnStatus.my;

    }

    public void AcknowledgementByServer()
    {
        if(myCurrTurnInput.Count >0)
        myCurrTurnInput.Dequeue();
        Debug.Log("AcknowledgementByServer" + myCurrTurnInput.Count);

        StartCoroutine("WaitAndSendData");
    }

    IEnumerator WaitAndSendData()
    {
        ConnectionManager.Instance.isFriendLive = false;
        if (myCurrTurnInput.Count <= 0)
        {
            tryCount = 0;
            StopCoroutine("WaitAndSendData");
            ConnectionManager.Instance.isFriendLive = true;
            ConnectionManager.Instance.connectionMsg.text = "";
        }
        
        if (myCurrTurnInput.Count > 0)
        {
            tryCount++;
            int data = myCurrTurnInput.Peek();
            Debug.Log("Data mised Send Again"+data);
            ConnectionManager.Instance.OnSendMeAnswer(data + "");
            if (tryCount > 3)
            {
                FriendNetStatus();
            }
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine("WaitAndSendData");
    }

    void FriendNetStatus()
    {
        ConnectionManager.Instance.isFriendLive = false;
        if(ConnectionManager.Instance.isIamLive)
            ConnectionManager.Instance.connectionMsg.text = "Friend Offline";
        else
            ConnectionManager.Instance.connectionMsg.text = "Check your Connection";

    }
    void QuitGame()
	{
		Application.Quit ();
	}
}
