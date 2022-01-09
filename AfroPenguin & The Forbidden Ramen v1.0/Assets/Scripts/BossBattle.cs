using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattle : MonoBehaviour
{
    public Transform player;
    public Animator theAnimator;
    public EnemyHP enemyHP;
    public GameObject[] prizes;
    public GameObject theBoss;
    public GameObject theHurtbox;
    public GameObject[] invisibleWalls;

    void Start()
    {
        theAnimator = GetComponentInChildren<Animator>();
        enemyHP = GetComponentInChildren<EnemyHP>();
        theHurtbox = transform.GetChild(0).gameObject;
        theBoss = transform.gameObject;
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        for (int i = 0; i < invisibleWalls.Length; i++)
        {
            invisibleWalls[i].SetActive(true);
        }
    }

    void Update()
    {
        WhereToLook();
        //Health related
        if (enemyHP.currentHP <= 3)
        {
            theAnimator.SetTrigger("stageTwo");
        }

        if (enemyHP.currentHP <= 0)
        {
            for (int i = 0; i < invisibleWalls.Length; i++)
            {
                invisibleWalls[i].SetActive(false);
            }
            theAnimator.SetTrigger("death");

            StartCoroutine(DeathSequence());
            theBoss.GetComponent<BoxCollider2D>().enabled = false;
            theHurtbox.tag = "Boss Hurtbox";
        }

        //Hurtbox related
        if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Boss Slime - 01 - Idle Stage 01")
            || theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Boss Slime - 01 - Idle Stage 02"))
        {
            theHurtbox.tag = "Boss Hurtbox";
        }

        if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Boss Slime - 01 - Jump Stage 01") 
            || theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Boss Slime - 01 - Jump Stage 02") 
            || theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Boss Slime - 01 - Intro") 
            || theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Boss Slime - 01 - Big Jump"))
        {
            theHurtbox.tag = "Enemy";
        }
    }

    IEnumerator DeathSequence()
    {
        for (int i = 0; i < prizes.Length; i++)
        {
            prizes[i].SetActive(true);
        }
        yield return new WaitForSeconds(1.0f);
        theBoss.SetActive(false);
    }

    public void WhereToLook()
    {
        if (player.transform.position.x > transform.position.x)
        {
            theHurtbox.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }

        else
        {
            theHurtbox.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }


    }
}
