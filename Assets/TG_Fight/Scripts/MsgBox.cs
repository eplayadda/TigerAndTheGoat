using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MsgBox : MonoBehaviour {
    public static MsgBox instance;
    public List<string> setOfMsg = new List<string>();
    public Text myTxt;
    public Text friendTxt;
    public GameObject msgOptionsGo;
    bool isOptionAlow;
    public InputField customInput;
    private void Start()
    {
        if (instance == null)
            instance = this;
        Init();
    }

    void Init()
    {
        setOfMsg.Add("Fast");
        setOfMsg.Add("Super");
    }
    public void MsgClicked()
    {
        isOptionAlow = !isOptionAlow;
        msgOptionsGo.SetActive(isOptionAlow);
    }

    public void OnPreDefiendMsg(int index)
    {
        // myTxt.text = setOfMsg[index].ToString();
        DisplayMyMsg(setOfMsg[index].ToString());
        ConnectionManager.Instance.SendMsg(setOfMsg[index].ToString());
        msgOptionsGo.SetActive(false);
        CancelInvoke("DisableMsg");
        Invoke("DisableMsg",5f);
    }

    public void CustomMsg()
    {
        string str = customInput.text;
        // myTxt.text = str;
        DisplayMyMsg(str);
        ConnectionManager.Instance.SendMsg(str);
        msgOptionsGo.SetActive(false);
        CancelInvoke("DisableMsg");
        Invoke("DisableMsg", 5f);

    }
    //server
    public void ShowMsg(string str)
    {
        //friendTxt.text = str;
        DisplayFriendMsg(str);
        Invoke("DisableMsg", 5f);

    }

    void DisableMsg()
    {
        myTxt.text = "";
        friendTxt.text = "";
    }

    void DisplayMyMsg(string str)
    {
        if (GameManager.instance.myAnimalType == eAnimalType.goat)
        {
            myTxt.text = str;
        }
        else
        {
            friendTxt.text = str;
        }
    }

    void DisplayFriendMsg(string str)
    {
        if (GameManager.instance.myAnimalType == eAnimalType.goat)
        {
            friendTxt.text = str;
        }
        else
        {
            myTxt.text = str;
        }
    }

}
