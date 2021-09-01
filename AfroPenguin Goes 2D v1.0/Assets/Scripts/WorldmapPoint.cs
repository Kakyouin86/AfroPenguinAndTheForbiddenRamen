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
    public int gemsCollected, totalGems;
    public float bestTime, targetTime;
    public GameObject gemBadge, timeBadge;

    // Start is called before the first frame update
    void Start()
    {
        if (isLevel && levelToLoad != null)
        {
            if (PlayerPrefs.HasKey(levelToLoad + "_gems"))
            {
                gemsCollected = PlayerPrefs.GetInt(levelToLoad + "_gems");
            }

            if (PlayerPrefs.HasKey(levelToLoad + "_time"))
            {
                bestTime = PlayerPrefs.GetFloat(levelToLoad + "_time");
            }

            if (gemsCollected >= totalGems)
            {
                gemBadge.SetActive(true);
            }
            if (bestTime <= targetTime && bestTime != 0.0f)
            {
                timeBadge.SetActive(true);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
