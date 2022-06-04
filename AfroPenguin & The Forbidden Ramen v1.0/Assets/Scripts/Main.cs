using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Runs before a scene gets loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        GameObject main = GameObject.Instantiate(Resources.Load("Main")) as GameObject;
        GameObject.DontDestroyOnLoad(main);
        PlayerPrefs.SetInt("Language", 0);
    }
}
