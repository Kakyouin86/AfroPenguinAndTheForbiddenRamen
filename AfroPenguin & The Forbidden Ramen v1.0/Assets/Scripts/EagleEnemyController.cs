using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Win32.SafeHandles;
using UnityEngine;

public class EagleEnemyController : MonoBehaviour
{
    [Header("Components")]
    public Transform[] points;
    public Transform player;
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;
    public Animator theAnimator;

    [Header("Movement")] 
    public Vector2 directionDifferencePlayerPosition;
    public bool isAttacking;

    public int currentPoint;
    public float moveSpeed = 5f;
    public float distanceToAttackPlayer = 4f;
    public float chaseSpeed = 4f;
    
    //If I want the eagle NOT to follow me that much:
    public Vector3 attackTarget;

    //This is if we want to just attack once
    public float waitAfterAttack;
    public float attackCounter;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        theSR = GetComponentInChildren<SpriteRenderer>();
        theAnimator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        isAttacking = false;

        for (int i = 0; i < points.Length; i++) // For i which starts at 0, and as long as i is less than the points i arrayed, but i will keep adding one to each i
        {
            points[i].parent = null;
        }
    }

    void Update()
    {
        WhereToLook();

        if (attackTarget == Vector3.zero)
        {
            isAttacking = false;
        }

        else
        {
            isAttacking = true;
        }

        if (attackCounter > 0)
        {
            attackCounter -= Time.deltaTime;
        }

        else
        {
            if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > distanceToAttackPlayer)
            {
                attackTarget = Vector3.zero;
                transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, points[currentPoint].position) < .05f)
                {
                    currentPoint++;
                    //our currentPoint becomes 1
                    if (currentPoint >= points.Length)
                    {
                        currentPoint = 0;
                    }
                }
            }

            else
            {
                //Attacking the Player
                if (attackTarget == Vector3.zero) //this is a check to see if we are attacking the player. Vector3.zero where EVERYTHING XYZ are 0
                {
                    attackTarget = PlayerController.instance.transform.position;
                }
                
                transform.position = Vector3.MoveTowards(transform.position, attackTarget, chaseSpeed * Time.deltaTime);
                //this next one is I want the eagle to just attack once and then stop and go back
                if (Vector3.Distance(transform.position, attackTarget) <= 0.1f) //close to that attack point
                {
                    attackCounter = waitAfterAttack;
                    attackTarget = Vector3.zero;
                    //Wait a length of time and then you can go move towards the player.
                }
            }
        }
    }

    public void WhereToLook()
    {
        directionDifferencePlayerPosition = player.position - transform.position;
        if (isAttacking)
        {
            if (transform.position.x < directionDifferencePlayerPosition.x)
            {
                theSR.flipX = true;
            }
            else if (transform.position.x > directionDifferencePlayerPosition.x)
            {
                theSR.flipX = false;
            }
        }
        
        if (!isAttacking)
        {
            if (transform.position.x < points[currentPoint].position.x)
            {
                theSR.flipX = true;
            }

            else if (transform.position.x > points[currentPoint].position.x)
            {
                theSR.flipX = false;
            }
        }
    }


}

//If I want the eagle to follow me wherever I am:
//        void Update()
//{
//    if (Vector3.Distance(transform.position, PlayerController.instance.transform.position) > distanceToAttackPlayer) // If the player is outside a range, then flap around
//    {

//        transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);
//        if (Vector3.Distance(transform.position, points[currentPoint].position) < 0.5f)
//        {
//            currentPoint++;
//            //our currentPoint becomes 1
//            if (currentPoint >= points.Length)
//            {
//                currentPoint = 0;
//            }
//        }

//        if (transform.position.x < points[currentPoint].position.x)
//        {
//            theSR.flipX = true;
//        }
//        else if (transform.position.x > points[currentPoint].position.x)
//        {
//            theSR.flipX = false;
//        }
//    }
//    else
//    {
//        transform.position = Vector3.MoveTowards(transform.position, PlayerController.instance.transform.position, chaseSpeed * Time.deltaTime);
//    }
//}



