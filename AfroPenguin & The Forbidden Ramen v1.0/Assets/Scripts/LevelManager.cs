using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Animator theAnimator;
    public PhysicsMaterial2D physicsMaterial;
    public float waitToRespawn = 1.5f;
    public float sumLostLife;
    public int starsCollected;
    public string levelToLoad;
    public float timeInLevel;
    public bool isPlayingIntro;
    public bool isPlayingLevelEnd;
    public GameObject[] enemiesToRespawn;
    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        isPlayingLevelEnd = false;
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
    }

    void Update()
    {
        if (theAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1)
        {
            isPlayingIntro = true;
            PlayerController.instance.GetComponent<CapsuleCollider2D>().sharedMaterial = null;
        }
        else
        {
            isPlayingIntro = false;
            PlayerController.instance.GetComponent<CapsuleCollider2D>().sharedMaterial = physicsMaterial;
            timeInLevel += Time.deltaTime;
        }
    }

    public void SumLostLife()
    {
        sumLostLife++;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

    //this is a coroutine. It happens outside the normal time of Unity. LESSON 41 WITH JAMES DOYLE.
    public IEnumerator RespawnCo()
    {
        //this is a CoRoutine. It happens outside the normal execution time of Unity.
        PlayerController.instance.gameObject.SetActive(false);

        //AudioManager.instance.PlaySFX(8);

        yield return new WaitForSeconds(waitToRespawn - (1.0f / UIController.instance.fadeSpeed));
        //Yield return new wait for seconds: an instruction to the respawn Coroutine, to wait for a RETURN (a value to be true).
        //When it finishes, it continues below

        UIController.instance.FadeToBlack();

        yield return new WaitForSeconds((1.0f / UIController.instance.fadeSpeed) + 0.25f);

        UIController.instance.FadeFromBlack();

        PlayerController.instance.gameObject.SetActive(true);

        PlayerController.instance.theSR.flipX = false;

        PlayerController.instance.gameObject.transform.position = CheckpointController.instance.spawnPoint;

        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;

        UIController.instance.UpdateHealthUpdate();

        for (int i = 0; i < enemiesToRespawn.Length; i++)
        {
            enemiesToRespawn[i].gameObject.SetActive(true);
        }
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

    public IEnumerator EndLevelCo()
    {
        AudioManager.instance.PlayLevelVictory();
        isPlayingLevelEnd = true;
        //CameraController.instance.stopFollow = true;
        FindObjectOfType<CameraFollowMegaMan>().enabled = false;
        UIController.instance.levelCompleteText.SetActive(true);
        yield return new WaitForSeconds(2f);
        UIController.instance.FadeToBlack();
        //If I don't put any yield, then I won't be able to test anything.
        yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) + 1);
        //We will store information: PlayerPrefs. Unlocked is 1.
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);
        PlayerPrefs.SetString("CurrentLevel", (SceneManager.GetActiveScene().name));
        
        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_stars"))
        {
            if (starsCollected > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_stars"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_stars", starsCollected);
            }
        }

        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_stars", starsCollected);
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

