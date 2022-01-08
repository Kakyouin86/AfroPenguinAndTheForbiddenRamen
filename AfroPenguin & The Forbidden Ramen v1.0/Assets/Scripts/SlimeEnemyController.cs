using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemyController : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;
    public Animator anim;
    public KnockbackEnemies theKnockback;

    [Header("Movement")]
    public bool canMove;
    public Transform leftPoint;
    public Transform rightPoint;
    public bool moveRight;
    public float moveSpeed = 3.0f;

    [Header("Ground Check")]
    public bool isGrounded;
    public bool cameFromTheGround;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;
    public Vector2 bottomOffset;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        theSR = GetComponentInChildren<SpriteRenderer>();
        theKnockback = GetComponentInChildren<KnockbackEnemies>();
        leftPoint.parent = null;
        rightPoint.parent = null;
        moveRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMoveOrInteract() == false)
            return;
        GroundCheck();
    }

    #region Can Move Or Interact
    public bool CanMoveOrInteract()
    {
        canMove = true;
        if (theKnockback.isKnockback)
            canMove = false;
        return canMove;
    }
    #endregion

    #region Ground Check
    public void GroundCheck()
    {
        cameFromTheGround = isGrounded;
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, groundCheckRadius, whatIsGround);
        if (isGrounded)
        {
            anim.SetBool("isMoving", true);
            if (moveRight)
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);

                theSR.flipX = true;

                if (transform.position.x > rightPoint.position.x)
                {
                    moveRight = false;
                }
            }
            else
            {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);

                theSR.flipX = false;

                if (transform.position.x < leftPoint.position.x)
                {
                    moveRight = true;
                }
            }
        }

        else
        {
            canMove = false;
        }
    }
    #endregion

    #region Ground Check Gizmo
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, groundCheckRadius);
    }
    #endregion
}