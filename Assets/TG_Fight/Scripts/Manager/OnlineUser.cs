using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineUser  {

    public static List<User> users = new List<User>();
    public static int IsContains(string id)
    {
        for (int i = 0; i < users.Count; i++)
        {
            if (users[i].IsContains(id))
            {
                  return i;
            }
        }
        return -1;
    }
}

public class User
{
    public string ClientId;
    public string ConnectionId;
    public bool isPlaying;
    public User(Dictionary<string, object> dic)
    {
        ClientId = dic["ClientId"].ToString();
        ConnectionId = dic["ConnectionId"].ToString();
        isPlaying = (bool)dic["isPlaying"];
    }

    public bool IsContains(string id)
    {
        if (id.Equals(ClientId))
            return true;
        else
            return false;
    }
}
