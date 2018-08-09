using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class OnlineUser  {

    public RootObjects rootObj;
}
[System.Serializable]
public class RootObjects
{
    public string ClientId;
    public string ConnectionId;
    public bool isPlaying;
}
