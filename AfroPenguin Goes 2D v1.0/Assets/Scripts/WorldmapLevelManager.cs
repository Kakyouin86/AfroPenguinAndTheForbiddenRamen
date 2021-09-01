using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldmapLevelManager : MonoBehaviour
{
    public WorldmapPlayer worldmapPlayer;

    private WorldmapPoint[] allPoints;

    void Start()
    {
        allPoints = FindObjectsOfType<WorldmapPoint>();
        if(PlayerPrefs.HasKey("CurrentLevel"))
        {
            foreach(WorldmapPoint point in allPoints)
            //when we don't know the specific number of the map points. We don't need to know which point on out array has the value.
            //we call POINT to each point in that script
            {
                if (point.levelToLoad == PlayerPrefs.GetString("CurrentLevel")) //BE CAREFUL HERE. SAME NAME, SAME CAPS AS IN LEVEL MANAGER!
                {
                    worldmapPlayer.transform.position = point.transform.position;
                    worldmapPlayer.currentPoint = point;
                }
            }
        }    
    }

    public void LoadLevel()
    {
        StartCoroutine(LoadLevelCo());
    }

    public IEnumerator LoadLevelCo()
    {
        AudioManager.instance.PlaySFX(4);

        WorldmapUIController.instance.FadeToBlack();

        yield return new WaitForSeconds((1.0F / WorldmapUIController.instance.fadeSpeed) + 0.25f);

        SceneManager.LoadScene(worldmapPlayer.currentPoint.levelToLoad);
    }    
}
