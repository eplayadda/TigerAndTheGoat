using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	public static InputHandler instance;
	public Queue<InputPacket> myCurrTurnInput = new Queue<InputPacket>();
    int tryCount;
	int tokenID= 0;

	public class InputPacket
	{
		public int packetID;
		public int input;
		public InputPacket(int pPacketID,int pInput)
		{
			packetID = pPacketID;
			input = pInput;
		}
	}
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
			tokenID++;
			InputPacket ip = new InputPacket (tokenID,pData);
			myCurrTurnInput.Enqueue(ip);
			//ConnectionManager.Instance.OnSendMeAnswer (pData+"");
            StartCoroutine("WaitAndSendData");
        }
	}

    public void OnInputByAI(int pData)
    {
        BordManager.instace.OnInputByUser(pData);
        if (GameManager.instance.currGameMode == eGameMode.vServerMulltiPlayer)
        {
			tokenID++;
            Debug.Log("Input By User " + pData);
			InputPacket ip = new InputPacket (tokenID,pData);
			myCurrTurnInput.Enqueue(ip);
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

	public void AcknowledgementByServer(int packetID)
    {
		tryCount = 0;
		if (myCurrTurnInput.Count > 0) {
			InputPacket ip = myCurrTurnInput.Peek();
			if (ip.packetID == packetID) {
				myCurrTurnInput.Dequeue();
				StopCoroutine("WaitAndSendData");
				StartCoroutine("WaitAndSendData");
			}
			Debug.Log(ip.packetID+" "+packetID+"AcknowledgementByServer" + myCurrTurnInput.Count);
		}

    }

    IEnumerator WaitAndSendData()
    {
        ConnectionManager.Instance.isFriendLive = false;
        if (myCurrTurnInput.Count <= 0)
        {
            tryCount = 0;
            StopCoroutine("WaitAndSendData");
            ConnectionManager.Instance.isFriendLive = true;
			yield break;
           // ConnectionManager.Instance.connectionMsg.text = "";
        }
        
        if (myCurrTurnInput.Count > 0)
        {
            tryCount++;
			InputPacket ip = myCurrTurnInput.Peek();
			Debug.Log("Data mised Send Again"+ip.input);
			ConnectionManager.Instance.OnSendMeAnswer(ip.input + " "+ip.packetID);
            if (tryCount > 3)
            {
                FriendNetStatus();
            }
        }
        yield return new WaitForSeconds(4f);
        StartCoroutine("WaitAndSendData");
    }

    void FriendNetStatus()
    {
        ConnectionManager.Instance.isFriendLive = false;
		return;
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
