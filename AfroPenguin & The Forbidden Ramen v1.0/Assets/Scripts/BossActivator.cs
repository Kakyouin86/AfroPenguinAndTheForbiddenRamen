using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossActivator : MonoBehaviour
{
    public GameObject theBossBattle, theBossSlider, cameraAnimator;
    public GameObject bossPosition;
    public Vector3 positionToSave;

    // Start is called before the first frame update
    void Start()
    {
        positionToSave = bossPosition.transform.position;
        if (bossPosition.transform.position != positionToSave)
        {
            bossPosition.transform.position = positionToSave;
        }
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