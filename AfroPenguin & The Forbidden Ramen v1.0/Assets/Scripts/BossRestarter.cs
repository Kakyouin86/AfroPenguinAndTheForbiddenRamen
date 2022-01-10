using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BossRestarter : MonoBehaviour
{
    public GameObject[] everythingYouNeedRestarted;
    public GameObject[] everythingYouNeedDisabled;
    public GameObject[] hPYouNeedRestored;
    public int hPValue;
    public GameObject theBoss;
    public Slider theBossSlider;
    public Transform TheBossPositionTransform;
    public Vector3 TheBossPositionVector;

    // Start is called before the first frame update
    void Start()
    {
        TheBossPositionVector =
            new Vector2(TheBossPositionTransform.position.x, TheBossPositionTransform.position.y);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player") || player.CompareTag("Invulnerable"))
        {
            AudioManager.instance.PlayBGM();
            for (int i = 0; i < everythingYouNeedRestarted.Length; i++)
            {
                everythingYouNeedRestarted[i].SetActive(true);
            }

            for (int i = 0; i < everythingYouNeedDisabled.Length; i++)
            {
                everythingYouNeedDisabled[i].SetActive(false);
            }

            for (int i = 0; i < hPYouNeedRestored.Length; i++)
            {
                hPYouNeedRestored[i].GetComponent<EnemyHP>().currentHP = hPValue;
            }

            theBossSlider.value = hPValue;

            //theBoss.transform.position = TheBossPositionVector;
            theBoss.transform.position = TheBossPositionVector;
        }

    }
}