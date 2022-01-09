using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject theBossBattle, theBossSlider, cameraAnimator;
    //public Animator cameraAnimator;

    // Start is called before the first frame update
    void Start()
    {
        //cameraAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Invulnerable")
        {
            cameraAnimator.GetComponent<Animator>().SetTrigger("shake");
            theBossBattle.SetActive(true);
            theBossSlider.SetActive(true);
            gameObject.SetActive(false);
            AudioManager.instance.PlayBossMusic();
        }
    }
}