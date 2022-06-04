using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageMassiveController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        switch (PlayerPrefs.GetInt("Language"))
        {
            case 1:
                GameObject[] englishGameObjects = GameObject.FindGameObjectsWithTag("English Text");
                foreach (GameObject go in englishGameObjects)
                {
                    go.SetActive(false);
                }

                break;

            default:
                GameObject[] españolGameObjects = GameObject.FindGameObjectsWithTag("Español Text");
                foreach (GameObject go in españolGameObjects)
                {
                    go.SetActive(false);
                }

                break;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
