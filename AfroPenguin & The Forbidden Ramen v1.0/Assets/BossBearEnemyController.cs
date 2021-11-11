using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Options;
using JetBrains.Annotations;
using UnityEngine;

public class BossBearEnemyController : MonoBehaviour
{
    public Transform theBoss;
    public Animator theAnimator;
    public float walkSpeed = 2f;
    public float runSpeed;
    public float timeBetweenRun;
    //public Transform leftPoint;
   // public Transform rightPoint;
    public Transform leftPointWanderLeft;
    public Transform rightPointWanderLeft;
    public Vector2 newWanderLeft;
    public float invisibleLength = 2f;
    public float invisibleCounter;

    [Header("Ground Check")]
    public bool isGrounded;
    public bool cameFromTheGround;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;
    public Vector2 bottomOffset;

    void Start()
    {
        theAnimator = GetComponent<Animator>();
        invisibleCounter = invisibleLength;
        //leftPoint.parent = null;
        //rightPoint.parent = null;
        leftPointWanderLeft.parent = null;
        rightPointWanderLeft.parent = null;

    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, groundCheckRadius, whatIsGround);

        if (Input.GetKeyDown(KeyCode.I))
        {
            WanderOnTheLeft();
        }
        if (invisibleCounter > 0)
        {
            invisibleCounter -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(newWanderLeft.x, transform.position.y), walkSpeed * Time.deltaTime);
        }
    }

    void WanderOnTheLeft()
    {
        newWanderLeft.x = Random.Range(leftPointWanderLeft.position.x, rightPointWanderLeft.position.x);
        invisibleLength = Random.Range(1f, 2f);
        invisibleCounter = invisibleLength;
    }

    #region Ground Check Gizmo
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, groundCheckRadius);
    }
    #endregion
}
