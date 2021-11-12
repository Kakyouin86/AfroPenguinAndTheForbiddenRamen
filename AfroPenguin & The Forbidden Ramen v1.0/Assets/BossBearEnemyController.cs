using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using UnityEngine;

public class BossBearEnemyController : MonoBehaviour
{
    [Header("Components")]
    //public Transform theBoss;
    public Rigidbody2D theRB;
    public Animator theAnimator;

    [Header("Movement")]
    public float walkSpeed = 10f;
   // public float runSpeed;
    //public float timeBetweenRun;
    //public Transform leftPoint;
    // public Transform rightPoint;

    [Header("Positions")]
    //public bool isOnTheRight;
    public Transform leftPointWanderLeft;
    public Transform rightPointWanderLeft;
    public Vector2 newWanderLeft;
    public int chargingTimesBetweenCycles;
    public float invisibleLengthCharge;
    public float invisibleCounterCharge;
    public float waitTimeCharge = 0.06f;
    public float waitTimeCounterCharge;

    [Header("Ground Check")]
    public bool isGrounded;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;
    public Vector2 bottomOffset;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        theAnimator = GetComponent<Animator>();
        invisibleCounterCharge = invisibleLengthCharge;
        //leftPoint.parent = null;
        //rightPoint.parent = null;
        leftPointWanderLeft.parent = null;
        rightPointWanderLeft.parent = null;
    }

    void Update()
    {

        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, groundCheckRadius, whatIsGround);
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                invisibleLengthCharge = Random.Range(3f, 6f);
                invisibleCounterCharge = invisibleLengthCharge;
                waitTimeCounterCharge = waitTimeCharge;
                WanderOnTheLeft();
            }

            if (invisibleCounterCharge > 0)
            {
                invisibleCounterCharge -= Time.deltaTime;
                theAnimator.SetBool("isWalking", true);
                theRB.position = Vector2.MoveTowards(transform.position, new Vector2(newWanderLeft.x, transform.position.y), walkSpeed * Time.deltaTime);

                if (Vector2.Distance(transform.position, new Vector2(newWanderLeft.x, transform.position.y)) < 0.1f)
                {
                    if (waitTimeCounterCharge <= 0.1f)
                    {
                        WanderOnTheLeft();
                        waitTimeCounterCharge = waitTimeCharge;
                    }
                    else
                    {
                        theAnimator.SetBool("isWalking", false);
                        waitTimeCounterCharge -= Time.deltaTime;
                    }
                }
            }

            else
            {
                theAnimator.SetBool("isWalking", false);
            }
        }
    }

    void WanderOnTheLeft()
    {
        newWanderLeft.x = Mathf.RoundToInt(Random.Range(leftPointWanderLeft.position.x, rightPointWanderLeft.position.x));
    }

    #region Ground Check Gizmo
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, groundCheckRadius);
    }
    #endregion
}
