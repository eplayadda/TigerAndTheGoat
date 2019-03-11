using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Utility : MonoBehaviour {
    [MenuItem("Utility/DeleteKeys")]
    public static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
