using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;
    public string levelSelect;
    public string mainMenu;
    public GameObject pauseScreen;
    public bool isPaused;
    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }

    public void PauseUnpause()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1.0f;
        }

        else
        {
            isPaused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0.0f;   
        }
    }

    public void LevelSelect()
    {
        PlayerPrefs.SetString("CurrentLevel", (SceneManager.GetActiveScene().name));
        SceneManager.LoadScene(levelSelect);
        Time.timeScale = 1.0f;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1.0f;
    }
}
