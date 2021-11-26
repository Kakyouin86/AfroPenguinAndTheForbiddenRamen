using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldmapPoint : MonoBehaviour
{
    public WorldmapPoint up, down, left, right;
    public bool isLevel;
    public bool isLocked;
    public string levelToLoad;
    public string levelToCheck;
    public string levelName;
    public int starsCollected, livesLost;
    public float bestTime;
    public GameObject ramenStarsBadge, noLivesLostBadge;

    // Start is called before the first frame update
    void Start()
    {
        if (isLevel && levelToLoad != null)
        {
            if (PlayerPrefs.HasKey(levelToLoad + "_stars"))
            {
                starsCollected = PlayerPrefs.GetInt(levelToLoad + "_stars");
            }

            if (PlayerPrefs.HasKey(levelToLoad + "_time"))
            {
                bestTime = PlayerPrefs.GetFloat(levelToLoad + "_time");
                ramenStarsBadge.SetActive(true);
            }

            if (livesLost == 0 && PlayerPrefs.GetInt("_unlocked") == 1)
            {
                noLivesLostBadge.SetActive(true);
            }

            isLocked = true;
            if (levelToCheck != null)
            {
                if (PlayerPrefs.HasKey("_unlocked"))
                {
                    if (PlayerPrefs.GetInt("_unlocked") == 1)
                    {
                        isLocked = false;
                    }
                }
            }
            if (levelToLoad == levelToCheck)
            {
                isLocked = false;
            }
        }
    }
}
