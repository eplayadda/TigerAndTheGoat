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
        return 0;
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
        ConnectionId = dic["ClientId"].ToString();
        isPlaying = (bool)dic["ClientId"];
    }

    public bool IsContains(string id)
    {
        if (id.Equals(ClientId))
            return true;
        else
            return false;
    }
}
