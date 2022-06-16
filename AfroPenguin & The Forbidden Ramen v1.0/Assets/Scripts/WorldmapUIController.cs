using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldmapUIController : MonoBehaviour
{
    public static WorldmapUIController instance;
    public Image fadeScreen;
    public float fadeSpeed = 1.0f;
    public bool shouldFadeToBlack, shouldFadeFromBlack;
    public GameObject levelInfoPanel;
    public Text levelName, livesLost, starsCollected, bestTime;

    private void Awake()
    {
        instance = this;
    }
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

    public void ShowLevelInfo(WorldmapPoint worldMapPointLevelInfo) //I'm giving you the WorldmapPoint variable to use to get information. LevelInfo is made up.
    {
        switch (PlayerPrefs.GetInt("Language"))
        {
            case 1:
                levelName.text = worldMapPointLevelInfo.levelNameSpanish;
                break;

            default:
                levelName.text = worldMapPointLevelInfo.levelNameEnglish;
                break;
        }

        starsCollected.text = "Stars collected: " + worldMapPointLevelInfo.starsCollected;
        livesLost.text = "Lives lost: " + worldMapPointLevelInfo.livesLost; //no need because there is a string before. NO NEED ->.ToString();

        if (worldMapPointLevelInfo.bestTime == 0)
        {
            bestTime.text = "Best time: ---";
        }
        else
        {
            bestTime.text = "Best time: " + worldMapPointLevelInfo.bestTime.ToString("F2") + "s";
        }

        levelInfoPanel.SetActive(true);
    }

    public void HideLevelInfo()
    {
        levelInfoPanel.SetActive(false);
    }
}
