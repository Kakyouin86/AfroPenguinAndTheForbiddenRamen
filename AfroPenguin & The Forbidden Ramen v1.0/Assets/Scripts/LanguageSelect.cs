using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LanguageSelect : MonoBehaviour
{
    public static LanguageSelect instance;
    public int language;
    public Image fadeScreen;
    public float fadeSpeed = 1.0f;
    public bool shouldFadeToBlack, shouldFadeFromBlack;
    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        FadeFromBlack();
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

    public void EnglishLanguage()
    {
        PlayerPrefs.SetInt("Language", 0);
        language = PlayerPrefs.GetInt("Language");
        SceneManager.LoadScene("Main Menu");
    }

    public void SpanishLanguage()
    {
        PlayerPrefs.SetInt("Language", 1);
        language = PlayerPrefs.GetInt("Language");
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void FadeFromBlack()
    {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
    }
}
