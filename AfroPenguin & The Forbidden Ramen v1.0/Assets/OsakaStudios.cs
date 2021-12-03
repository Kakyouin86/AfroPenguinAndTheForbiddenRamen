using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OsakaStudios : MonoBehaviour
{
    public string nextScene;
    public Image fadeScreen;
    public float fadeSpeed = 1.0f;
    public bool shouldFadeToBlack, shouldFadeFromBlack;

    // Start is called before the first frame update
    void Start()
    {
        FadeFromBlack();
        StartCoroutine(EndOfTheClip());
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

        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit") || Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    public void FadeFromBlack()
    {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
    }

    public void GoToNextScreen()
    {
        SceneManager.LoadScene(nextScene);
    }

    IEnumerator EndOfTheClip()
    {
        yield return new WaitForSeconds(11f);
        SceneManager.LoadScene(nextScene);
    }

}
