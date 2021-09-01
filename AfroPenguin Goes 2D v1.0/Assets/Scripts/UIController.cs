using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Image heart01, heart02, heart03;
    public Sprite heartFull, heartEmpty, heartHalf;
    public Text gemText;
    public Image fadeScreen;
    public float fadeSpeed = 3.0f;
    public bool shouldFadeToBlack, shouldFadeFromBlack;
    public GameObject levelCompleteText;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateGemCount();
        FadeFromBlack();
    }

    // Update is called once per frame
    void Update()
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

    public void UpdateHealthUpdate()
    {
        switch(PlayerHealthController.instance.currentHealth)
        {
            case 6:
                heart01.sprite = heartFull;
                heart02.sprite = heartFull;
                heart03.sprite = heartFull;
                break;
            case 5:
                heart01.sprite = heartFull;
                heart02.sprite = heartFull;
                heart03.sprite = heartHalf;
                break;

            case 4:
                heart01.sprite = heartFull;
                heart02.sprite = heartFull;
                heart03.sprite = heartEmpty;
                break;
            case 3:
                heart01.sprite = heartFull;
                heart02.sprite = heartHalf;
                heart03.sprite = heartEmpty;
                break;

            case 2:
                heart01.sprite = heartFull;
                heart02.sprite = heartEmpty;
                heart03.sprite = heartEmpty;
                break;
            case 1:
                heart01.sprite = heartHalf;
                heart02.sprite = heartEmpty;
                heart03.sprite = heartEmpty;
                break;


            case 0:
                heart01.sprite = heartEmpty;
                heart02.sprite = heartEmpty;
                heart03.sprite = heartEmpty;
                break;

            default:
                heart01.sprite = heartEmpty;
                heart02.sprite = heartEmpty;
                heart03.sprite = heartEmpty;
                break;
        }
    }

    public void UpdateGemCount()
    {
        gemText.text = LevelManager.instance.gemsCollected.ToString();
        //converts the numbers into strings.
    }

    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromBlack = false;
    }

    public void FadeFromBlack()
    {
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
    }

}
