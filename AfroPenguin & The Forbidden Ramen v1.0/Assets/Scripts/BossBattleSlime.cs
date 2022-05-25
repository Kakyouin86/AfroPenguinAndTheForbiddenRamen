using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BossBattleSlime : MonoBehaviour
{
    public Transform player;
    public Animator theAnimator;
    public EnemyHP enemyHP;
    public GameObject[] prizes;
    public GameObject theBoss;
    public GameObject theHurtbox;
    public GameObject[] invisibleWalls;
    public bool isGrounded;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;
    public Vector2 bottomOffset;

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
        GroundCheck();


        //Health related
        if (enemyHP.currentHP >0 && enemyHP.currentHP <= 5)
        {
            theAnimator.SetTrigger("stageTwo");
        }

        if (enemyHP.currentHP <= 0)
        {
            AudioManager.instance.PlayBGM();
            for (int i = 0; i < invisibleWalls.Length; i++)
            {
                invisibleWalls[i].SetActive(false);
            }

            for (int i = 0; i < prizes.Length; i++)
            {
                prizes[i].SetActive(true);
            }

            theAnimator.SetTrigger("death");
            StartCoroutine(DeathSequence());
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
        yield return new WaitForSeconds(1f);
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

    public void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, groundCheckRadius, whatIsGround);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, groundCheckRadius);
    }
}
