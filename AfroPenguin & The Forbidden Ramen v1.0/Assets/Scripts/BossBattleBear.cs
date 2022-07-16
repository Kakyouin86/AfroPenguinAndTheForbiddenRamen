using System.Collections;
using NiceIO.Sysroot;
using UnityEngine;

public class BossBattleBear : MonoBehaviour
{
    public Transform player;
    public Animator theAnimator;
    public EnemyHP enemyHP;
    public bool faceRight;
    public GameObject[] prizes;
    public GameObject theBoss;
    public GameObject theHurtbox;
    public GameObject[] invisibleWalls;
    public bool isGrounded;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;
    public Vector2 bottomOffset;
    public Transform bearLeftPoint;
    public Transform bearRightPoint;

    void Start()
    {
        theAnimator = GetComponentInChildren<Animator>();
        enemyHP = GetComponentInChildren<EnemyHP>();
        theHurtbox = transform.GetChild(0).gameObject;
        theBoss = transform.gameObject;
        bearLeftPoint = GameObject.FindGameObjectWithTag("Boss Wander Left").GetComponent<Transform>();
        bearRightPoint = GameObject.FindGameObjectWithTag("Boss Wander Right").GetComponent<Transform>();
        bearLeftPoint.parent = null;
        bearRightPoint.parent = null;
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        for (int i = 0; i < invisibleWalls.Length; i++)
        {
            invisibleWalls[i].SetActive(true);
        }
        faceRight = true;
    }

    void Update()
    {
        Slash();
        WhereToLook();
        GroundCheck();

        //Health related
        if (enemyHP.currentHP > 0 && enemyHP.currentHP <= 6)
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

    public void Slash()
    {
        if (Vector2.Distance(player.position, theBoss.transform.position) <= 5f)
        {
            theBoss.GetComponent<Animator>().SetTrigger("goAway");
        }
    }
    public void WhereToLook()
    {
        if (transform.position.x < player.transform.position.x && !faceRight)
        {
            faceRight = !faceRight;
            transform.Rotate(0f, 180f, 0);
        }
        else if (transform.position.x > player.transform.position.x && faceRight)
        {
            faceRight = !faceRight;
            transform.Rotate(0f, 180f, 0);
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