using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string startScene, continueScene;
    public GameObject continueButton;
    public Image fadeScreen;
    public float fadeSpeed = 1.0f;
    public bool shouldFadeToBlack, shouldFadeFromBlack;

    // Start is called before the first frame update
    void Start()
    {
        FadeFromBlack();
        switch (LanguageSelect.instance.language)
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

        if (PlayerPrefs.HasKey(startScene+ "_unlocked"))
        {
            continueButton.SetActive(true);
        }

        else
        {
            continueButton.SetActive(false);
        }
    }

    public void Update()
    {
        if (shouldFadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFadeFromBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
            if (fadeScreen.color.a == 0f)
            {
                shouldFadeFromBlack = false;
            }
        }
    }

    public void StartGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("Language", LanguageSelect.instance.language);
        StartCoroutine("LittleFadeStart");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        PlayerPrefs.SetInt("Language", LanguageSelect.instance.language);
        StartCoroutine("LittleFadeContinue");
    }

    public void FadeFromBlack()
    {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
    }
    IEnumerator LittleFadeStart()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(startScene);
    }

    IEnumerator LittleFadeContinue()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(continueScene);
    }
}

