﻿using UnityEngine;
using System.Collections;
using System.Threading;
using System.Net.Sockets;
using System.IO;
using System.Net.NetworkInformation;
using System.Net;
using System;
using System.Text;
using BestHTTP.SignalR;
using BestHTTP;
using BestHTTP.SignalR.Hubs;
using BestHTTP.SignalR.Messages;
using BestHTTP.SignalR.JsonEncoders;
using System.Collections.Generic;

public class ConnectionManager : MonoBehaviour
{
	public static ConnectionManager Instance;
	string HUB_NAME = "eLarning";
	string CLIENTID = "ClientId";
	string GETREQUEST = "GetRequest";
	string ACK_CONNECTED = "receiveAcknowledgement";
	string CHALLENGEACCEPTED = "ChallengeAccepted";
	string INPUTRECIVEC = "OnInputRecived";
    string ReceiveOneSignalID = "receiveOneSignalID";
    //	string baseUrl = "http://52.11.67.198/SignalRDemo/";
    // "http://localhost:1921/SignalRDemo";// "http://52.33.40.224/SignalRDemo";//"http://localhost:1921/SignalRDemo";
    //string baseUrl = "http://localhost:1921/SignalRDemo2/";//"http://52.11.67.198/SignalRDemo";// "http://52.33.40.224/SignalRDemo";
    //string baseUrl = "http://huliyawebapp.azurewebsites.net/SignalR/eLarningHub/hubs";
    //"http://52.11.67.198/eLarningHub/";
    string baseUrl = "http://www.eplayadda.com/SignalR/eLarningHub/hubs";//"http://localhost:30359/SignalR/eLarningHub/hubs";//
   // string baseUrl = "http://localhost:30359/SignalR/eLarningHub/hubs";//
    public string myID = "1";
	string guestID;
    public string inviteGuestID;
	public string friedID = "1";
	public List<string> onlineFriends = new List<string> ();
	bool isLatestOnline;
    public bool isIamLive;
    public bool isFriendLive;
    public bool isMutiplayerPlaying;
	List<int> recivedPacketID = new List<int>();
    public int friendCount;
    public enum SignalRConectionStatus
	{
		None = 0,
		DisConnected,
		Connected,
	}

	private SignalRConectionStatus curSignalRConectionStatus;
	public static Connection signalRConnection;
	public static Hub _newHub;
	public Coroutine signalRCoroutine;
    public UnityEngine.UI.Text connectionMsg;
    
    void Awake ()
	{
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (this.gameObject);
			myID = GetRandomID ();
			UIManager.instance.mainMenuUI.username.text = myID;
            UIManager.instance.mainMenuUI.playerName.text = myID;
            guestID = myID;
		} else {
			DestroyImmediate (this.gameObject);
			return;
		}
//		MakeConnection ();
	}

	string GetRandomID ()
	{
		string str = "";
		str = "Guest " + UnityEngine.Random.Range (0, 10) + UnityEngine.Random.Range (0, 10) + UnityEngine.Random.Range (0, 10) + UnityEngine.Random.Range (0, 10) + UnityEngine.Random.Range (0, 10) + "";
		return str;
	}

	public void SetConnectionID (bool p)
	{
		if (p) {
			myID = "1";
			friedID = "2";
		} else {
			myID = "2";
			friedID = "1";
		}
	}

	public void MakeConnection ()
	{
		signalRConnection = null;

		if (signalRConnection == null) {
			try {
				Uri uri = new Uri (baseUrl);
				_newHub = new Hub (HUB_NAME);
				signalRConnection = new Connection (uri, _newHub);
				signalRConnection.JsonEncoder = new LitJsonEncoder ();

				signalRConnection.OnStateChanged += OnSignalRStatusChange;
				signalRConnection.OnError += OnSignalRErrorOccur;
				signalRConnection.OnConnected += OnSignalRConnected;
				signalRConnection.OnClosed += (con) => OnSignalRClosed ();
				signalRConnection.OnReconnected += onSignalRReconnected;

				Dictionary<string, string> dict = new Dictionary<string, string> ();
				dict.Add (CLIENTID, myID);
				signalRConnection.AdditionalQueryParams = dict;
				signalRCoroutine = StartCoroutine ("OpenSignalRConnection");
				AllOperations (); 

			} catch (Exception e) {
			}
		}      
	}

	void OnSignalRStatusChange (Connection conection, ConnectionStates oldState, ConnectionStates newState)
	{

	}

	void OnSignalRErrorOccur (Connection connection, string error)
	{
	}

	void  OnSignalRConnected (Connection connection)
	{
		//curSignalRConectionStatus = SignalRConectionStatus.Connected;
	}

	public void OnSignalRClosed ()
	{

	}

	void OnApplicationQuit ()
	{
		if (signalRConnection != null) {
            if (isMutiplayerPlaying)
                OnGameOverSendData(friedID);
            signalRConnection.Close ();
			signalRConnection = null;
           
        }
		Debug.Log ("Application Quit");
	}


	void onSignalRReconnected (Connection connection)
	{
		Debug.Log ("Signal R connection Reconnected");
	}

	public IEnumerator OpenSignalRConnection ()
	{
		signalRConnection.Open ();
		while (true) {
			yield return new WaitForSeconds (1f);
                    //Debug.Log("Open COnnection"+signalRConnection.State);
			try {
                if (signalRConnection.State == ConnectionStates.Connected)
                {
                    isIamLive = true;
                }
                else
                {
                    isIamLive = false;
                }

                if (signalRConnection.State != ConnectionStates.Connected) {
					signalRConnection.Open ();
				}
			} catch (Exception e) {
				Debug.LogError ("Exception SignalR Open " + e.Message);
			}
		}
	}

	public void AllOperations ()
	{
		signalRConnection [HUB_NAME].On (GETREQUEST, OnReceiveMatchDetails);
		signalRConnection [HUB_NAME].On (ACK_CONNECTED, Ack);
		signalRConnection [HUB_NAME].On (CHALLENGEACCEPTED, ChallengeAccepted);
		signalRConnection [HUB_NAME].On (INPUTRECIVEC, OnInputRecived);
		signalRConnection [HUB_NAME].On (ReceiveOneSignalID, OnReceiveOneSignalID);

     
    }

    List <string> usersID = new List<string> ();

	// Sending Request
	public void OnSendRequest (string pTablePrice, string pCurrSubjectType,string pName)
	{
		usersID.Clear ();
		usersID.Add (myID);
		usersID.Add (friedID);
		usersID.Add (pTablePrice);
		usersID.Add (pCurrSubjectType);
		usersID.Add (pName);
		Debug.Log (myID + "Send Request" + friedID);
		signalRConnection [HUB_NAME].Call ("SendRequest", usersID);
		GameManager.instance.currGameMode = eGameMode.vServerMulltiPlayer;
		int a = Convert.ToInt32 (pCurrSubjectType);
		GameManager.instance.currPlayerIdentity = ePlayerIdentity.host;
		GameManager.instance.friendAnimalType = (eAnimalType)a;
		if (a == 1) {
			GameManager.instance.currTurnStatus = eTurnStatus.my;
			a = 2;
		} else {
			a = 1;
			GameManager.instance.currTurnStatus = eTurnStatus.friend;
		}
		GameManager.instance.myAnimalType = (eAnimalType)a;

	}

    public void SendMsg(string str)
    {
        inputData.Clear();
        inputData.Add(friedID);
        inputData.Add(str);
        inputData.Add(4 + "");
        signalRConnection[HUB_NAME].Call("InPutTaken", inputData);
    }
	// Request Came
	public void OnReceiveMatchDetails (Hub hub, MethodCallMessage msg)
	{

		Debug.Log ("Request came");
		var str = msg.Arguments [0] as object[];
        //friedID = str [0].ToString ();
        inviteGuestID = str[0].ToString();
		int tablePrice = Convert.ToInt32 (str [2].ToString ());
		int subjectType = Convert.ToInt32 (str [3].ToString ());
		string pName = str [4].ToString ();
		Debug.Log ("Request came"+pName);

		UIManager.instance.OnSendRequest (tablePrice, subjectType,pName);

	}
    void OnReceiveOneSignalID(Hub hub, MethodCallMessage msg)
    {
        var str = msg.Arguments[0] as object;
        string onSignalID = str.ToString();
        Debug.Log("OnSignalID------???"+onSignalID);
    }

    public void IacceptChallage (int a)
	{
		usersID.Clear ();
		usersID.Add (myID);
		usersID.Add (inviteGuestID);
		usersID.Add (a + "");
        Debug.Log("Invite Accepted Sent to server" );
        if (a == 1)
        {
            string temp = friedID;
            friedID = inviteGuestID;
            inviteGuestID = temp;
            if(isMutiplayerPlaying)
              OnGameOverSendData(inviteGuestID);
        }

        signalRConnection[HUB_NAME].Call ("IacceptedChallenge", usersID);


	}

	public void ChallengeAccepted (Hub hub, MethodCallMessage msg)
	{
        isIamLive = true;
        isFriendLive = true;
		var str = msg.Arguments [0] as object[];
		int a = Convert.ToInt16 (str [2].ToString ());
		Debug.Log (str [2].ToString ());
		Debug.Log ("Chalage accepted");
		UIManager.instance.OnChallangeAccepted (a);
	}

	List <string> inputData = new List<string> ();

	public void GetOnlineFriend ()
	{
//        isLatestOnline = true;
		signalRConnection [HUB_NAME].Call ("SendOnlineFriend", myID);
	}

	public void OnServerGameStart ()
	{
		recivedPacketID.Clear ();
		inputData.Clear ();
		inputData.Add (friedID);
		inputData.Add ("");
		inputData.Add (3 + "");
		signalRConnection [HUB_NAME].Call ("InPutTaken", inputData);
	}

	public void OnSendMeAnswer (string ansCount)
	{
		inputData.Clear ();
		inputData.Add (friedID);
		inputData.Add (ansCount);
		inputData.Add (0 + "");
		signalRConnection [HUB_NAME].Call ("InPutTaken", inputData);
	}

	public void OnGameOverSendData (string id)
	{
		//Game Over
		inputData.Clear ();
		inputData.Add (id);
		inputData.Add ("");
		inputData.Add (1 + "");
        isMutiplayerPlaying = false;
		signalRConnection [HUB_NAME].Call ("InPutTaken", inputData);
	}
	void DataRecivedACK(int packetID)
    {
        inputData.Clear();
        inputData.Add(friedID);
		inputData.Add(packetID+"");
        inputData.Add(2 + "");
        signalRConnection[HUB_NAME].Call("InPutTaken", inputData);
    }


    public void OnInputRecived (Hub hub, MethodCallMessage msg)
	{
		var str = msg.Arguments [0] as object[];
		Debug.Log (str [2].ToString ());


		if (str [2].ToString () == "0") {
			if (GameManager.instance.currGameStatus == eGameStatus.play) {
				string[] packet = str [1].ToString ().Split(' ');
				int a = Convert.ToInt32 (packet[0]);
				int packetID = Convert.ToInt32 (packet [1]);
				Debug.Log (recivedPacketID.Contains(packetID) + " packet ID "+packetID);
				if (!recivedPacketID.Contains(packetID)) {
					InputHandler.instance.OnInputTakenBYServer (a);
					recivedPacketID.Add(packetID);
					Debug.Log (a + " ");
				}
				DataRecivedACK(packetID);

			}
		} else if (str [2].ToString () == "1") {
            isMutiplayerPlaying = false;
			//int a = Convert.ToInt32(str[1]);
			UIManager.instance.FriendGameOver ();
		} else if (str [2].ToString () == "2") {
			InputHandler.instance.AcknowledgementByServer(Convert.ToInt32 (str [1].ToString ()));
		} else if (str [2].ToString () == "3") {
			UIManager.instance.OnGameStartOnServer ();
		}
        else if (str[2].ToString() == "4")
        {
            MsgBox.instance.ShowMsg(str[1].ToString());
        }

    }

	public void Ack (Hub hub, MethodCallMessage msg)
	{
		Debug.Log ("ACk");
        OnlineUser.users.Clear();
		onlineFriends.Clear ();
        //        UIManager.instance.OnSignalRConnected ();
        object[] str = msg.Arguments[0] as object[];
        Debug.Log(str.Length);
        friendCount = str.Length;
        for (int i = 0; i < str.Length; i++)
        {

            object tempData = str[i];
            Dictionary<string, object> dic = (Dictionary<string, object>)tempData;
            Debug.Log(dic["ClientId"].ToString()+"Playing " + (bool)dic["isPlaying"]);
            if (myID != dic["ClientId"].ToString() && guestID != dic["ClientId"].ToString())
            {
                User newUser = new User(dic);
                OnlineUser.users.Add(newUser);
                Debug.Log(" CLientID  >>>>>>>> " + dic["ClientId"]);
                onlineFriends.Add(dic["ClientId"].ToString());
            }
            
        }
        SocialManager.Instance.facebookManager.GetFriends();
		if (isLatestOnline) {
			Debug.Log ("Onlime Friend");
			SocialManager.Instance.facebookManager.GetFriends ();
			isLatestOnline = false;
		}
      //  signalRConnection[HUB_NAME].Call("InsertOnSignalData", new List<string>() { "FB_55000", "S5_6000", "30", "3", "3" });
       // signalRConnection[HUB_NAME].Call("UpdateScore", new List<string>() { "FB_55000", "S5_5000", "3022", "5553", "3" });
        signalRConnection[HUB_NAME].Call("GetOneSignalID", "FB_55000");
        Debug.Log("+++++++++++++++++++Inserdata"+ friendCount);
        if (friendCount == 1)
        {
            Debug.Log("Count Is Zero........");
        }
    }

}


 