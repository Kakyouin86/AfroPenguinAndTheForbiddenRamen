using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContinueAndFadeScreens : MonoBehaviour
{
    public Image fadeScreen;
    public float fadeSpeed = 1.0f;
    public bool shouldFadeToBlack, shouldFadeFromBlack;
    public string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        FadeFromBlack();
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

    public void Update()
    {
        if (shouldFadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b,
                Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                shouldFadeFromBlack = false;
            }
        }

        if (Input.anyKeyDown)
        {
            SceneManager.LoadSceneAsync(sceneName);
        }
    }

    public void FadeFromBlack()
    {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
    }

    void ChangeScene()

    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}