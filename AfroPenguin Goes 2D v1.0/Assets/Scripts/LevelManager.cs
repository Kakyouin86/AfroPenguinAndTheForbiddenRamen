using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public float waitToRespawn = 1.5f;
    public int gemsCollected;
    public string levelToLoad;
    public float timeInLevel;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        timeInLevel = 0.0f;
    }

    void Update()
    {
        timeInLevel += Time.deltaTime;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

    //this is a coroutine. It happens outside the normal time of Unity. LESSON 41 WITH JAMES DOYLE.
    private IEnumerator RespawnCo()
    {
        //this is a CoRoutine. It happens outside the normal execution time of Unity.
        PlayerController.instance.gameObject.SetActive(false);

        AudioManager.instance.PlaySFX(8);

        yield return new WaitForSeconds(waitToRespawn - (1.0f / UIController.instance.fadeSpeed));
        //Yield return new wait for seconds: an instruction to the respawn Coroutine, to wait for a RETURN (a value to be true).
        //When it finishes, it continues below

        UIController.instance.FadeToBlack();

        yield return new WaitForSeconds((1.0f / UIController.instance.fadeSpeed) + 0.25f);

        UIController.instance.FadeFromBlack();

        PlayerController.instance.gameObject.SetActive(true);

        PlayerController.instance.sr.flipX = false;

        PlayerController.instance.gameObject.transform.position = CheckpointController.instance.spawnPoint;

        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;

        UIController.instance.UpdateHealthUpdate();
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

    public IEnumerator EndLevelCo()
    {

        AudioManager.instance.PlayLevelVictory();
        PlayerController.instance.stopInput = true;
        CameraController.instance.stopFollow = true;
        UIController.instance.levelCompleteText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        UIController.instance.FadeToBlack();
        //If I don't put any yield, then I won't be able to test anything.
        yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) + 3f);
        //We will store information: PlayerPrefs. Unlocked is 1.
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);
        PlayerPrefs.SetString("CurrentLevel", (SceneManager.GetActiveScene().name));
        
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_gems"))
        {
            if (gemsCollected > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_gems"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_gems", gemsCollected);
            }
        }

        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_gems", gemsCollected);
        }

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_time"))
        {
            if (timeInLevel < PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel))
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
        }
        SceneManager.LoadScene(levelToLoad);
    }
}

