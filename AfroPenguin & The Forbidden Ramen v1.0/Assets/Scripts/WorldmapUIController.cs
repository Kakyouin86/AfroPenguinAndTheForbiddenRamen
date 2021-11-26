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

    public void ShowLevelInfo(WorldmapPoint worldMapPongLevelInfo) //I'm giving you the WorldmapPoint variable to use to get information. LevelInfo is made up.
    {
        levelName.text = worldMapPongLevelInfo.levelName;
        starsCollected.text = "Stars collected: " + worldMapPongLevelInfo.starsCollected;
        livesLost.text = "Lives lost: " + worldMapPongLevelInfo.livesLost; //no need because there is a string before. NO NEED ->.ToString();

        if (worldMapPongLevelInfo.bestTime == 0)
        {
            bestTime.text = "Best time: ---";
        }
        else
        {
            bestTime.text = "Best time: " + worldMapPongLevelInfo.bestTime.ToString("F2") + "s";
        }

        levelInfoPanel.SetActive(true);
    }

    public void HideLevelInfo()
    {
        levelInfoPanel.SetActive(false);
    }
}
