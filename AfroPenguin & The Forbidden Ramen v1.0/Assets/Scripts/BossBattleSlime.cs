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
    public LayerMask whatIsInvisibleWall;
    public float groundCheckRadius = 0.2f;
    public float wallCheckRadius = 0.2f;
    public Vector2 bottomOffset;
    public float timeToMove = 5f;
    public float overshoot = 50f;

    public Vector3 targetPosition;

    private bool isPastThePlayer;

    void Start()
    {
        theAnimator = GetComponentInChildren<Animator>();
        enemyHP = GetComponentInChildren<EnemyHP>();
        theHurtbox = transform.GetChild(0).gameObject;
        theBoss = transform.gameObject;
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        targetPosition = player.position;
        targetPosition.x = int.MinValue;
    }

    private void OnDisable()
    {
        if(invisibleWalls == null)
        {
            return;
        }
        for (int i = 0; i < invisibleWalls.Length; i++)
        {
            invisibleWalls[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < invisibleWalls.Length; i++)
        {
            invisibleWalls[i].SetActive(true);
        }
    }

    void Update()
    {
        bool wasPastThePlayer = isPastThePlayer;
        isPastThePlayer = player.transform.position.x > transform.position.x;
        if (isPastThePlayer != wasPastThePlayer)
        {
            Invoke("MoveToPosition", timeToMove);
        }
        GroundCheck();
        OnTouchInvisibleWall();


        //Health related
        if (enemyHP.currentHP > 0 && enemyHP.currentHP <= 5)
        {
            theAnimator.SetTrigger("stageTwo");
        }

        if (enemyHP.currentHP <= 0)
        {
            AudioManager.instance.PlayBGM();
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

    public void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, groundCheckRadius, whatIsGround);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, groundCheckRadius);
    }

    void OnTouchInvisibleWall()
    {
        bool isTouchingWall = Physics2D.IsTouchingLayers(theHurtbox.GetComponent<Collider2D>(), whatIsInvisibleWall);
        if (isTouchingWall)
        {
            targetPosition.x = transform.position.x + (isPastThePlayer ? -overshoot : overshoot);
            Debug.Log("Touching Wall or Player!");
            Invoke("MoveToPosition", timeToMove);
        }

    }

    void MoveToPosition()
    {
        targetPosition.x = (isPastThePlayer ? int.MaxValue : int.MinValue);
        theHurtbox.GetComponentInChildren<SpriteRenderer>().flipX = isPastThePlayer;
        Debug.Log("Target changed");
    }
}