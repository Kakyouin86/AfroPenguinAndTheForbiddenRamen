using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Transform leftPoint;
    public Transform rightPoint;
    public bool moveRight;
    private Rigidbody2D theRB;
    public SpriteRenderer theSR;
    private Animator anim;
    public float moveTime = 3.0f;
    public float waitTime = 2.0f;
    public float moveCount;
    public float waitCount;

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

       leftPoint.parent= null;
       rightPoint.parent = null;

        moveRight = true;

        moveCount = moveTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveCount >= 0)
        {
            moveCount -= Time.deltaTime;

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

            if (moveCount <= 0)
            {
                waitCount = Random.Range(waitTime * .75f, waitTime * 1.25f);
            }

            anim.SetBool("isMoving", true);
        }
        else if (waitCount >= 0)
        {
            waitCount -= Time.deltaTime;
            theRB.velocity = new Vector2(0f, theRB.velocity.y);

            if (waitCount <= 0)
            {
                moveCount = Random.Range(moveTime * .75f, waitTime * .75f);
            }
            anim.SetBool("isMoving", false);
        }
    }
}