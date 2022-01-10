using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public AudioSource[] soundEffects;
    public AudioSource BGM, levelEndMusic, bossBattleMusic, fishItemMusic;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySFX(int soundToPlay)
    {
        soundEffects[soundToPlay].Stop();
        //soundEffects[soundToPlay].pitch = Random.Range(0.9f, 1.1f);
        soundEffects[soundToPlay].Play();
    }
    public void PlayBGM()
    {
        if (!fishItemMusic.isPlaying)
        {
            if (!BGM.isPlaying)
            {
                bossBattleMusic.Stop();
                BGM.Play();
            }
        }
    }

    public void PlayNormalBGM()
    {
        BGM.Play();
    }

    public void PlayLevelVictory()
    {
        BGM.Stop();
        levelEndMusic.Play();
    }
    public void PlayBossMusic()
    {
        fishItemMusic.Stop();
        BGM.Pause();
        bossBattleMusic.Play();
    }

    public void PlayFishItemMusic()
    {
        fishItemMusic.Play();
        BGM.Stop();
    }

    public void StopBossMusic()
    {
        bossBattleMusic.Stop();
        BGM.UnPause();
    }
}

