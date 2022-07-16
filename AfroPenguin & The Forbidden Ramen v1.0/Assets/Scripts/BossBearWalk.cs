using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBearWalk : StateMachineBehaviour
{
    public float timerWalk;
    public float minTimeWalking = 4f;
    public float maxTimeWalking = 5f;
    public float moveSpeed = 5f;
    public Transform playerPosition;
    public Transform bossPosition;
    public Transform bossLeftPoint;
    public Transform bossRightPoint;
    //public Rigidbody2D theRB;
    public Vector3 target;
    public int funcToChoose;
    public bool moveRight;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        bossPosition = GameObject.FindGameObjectWithTag("Boss Hurtbox").GetComponent<Transform>();
        bossLeftPoint = GameObject.FindGameObjectWithTag("Boss Wander Left").GetComponent<Transform>();
        bossRightPoint = GameObject.FindGameObjectWithTag("Boss Wander Right").GetComponent<Transform>();
        //theRB = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Rigidbody2D>();
        timerWalk = Random.Range(minTimeWalking, maxTimeWalking);
        System.Random randomizer = new System.Random();
        funcToChoose = randomizer.Next(2);
        moveRight = (Random.value > 0.5f);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timerWalk <= 0)
        {
            animator.SetTrigger("idle");
        }

        else
        {
            timerWalk -= Time.deltaTime;
            if (moveRight)
            {
                target = new Vector3(bossRightPoint.transform.position.x, animator.transform.position.y);
                animator.transform.position =
                    Vector3.MoveTowards(animator.transform.position, target, moveSpeed * Time.deltaTime);

                if (bossPosition.position.x >= bossRightPoint.position.x)
                {
                    moveRight = false;
                }
            }

            else
            {
                target = new Vector3(bossLeftPoint.transform.position.x, animator.transform.position.y);
                animator.transform.position =
                    Vector3.MoveTowards(animator.transform.position, target, moveSpeed * Time.deltaTime);

                if (bossPosition.position.x <= bossLeftPoint.position.x)
                {
                    moveRight = true;
                }
            }

            //target = new Vector3(bossLeftPoint.transform.position.x, animator.transform.position.y);
            //animator.transform.position = Vector3.MoveTowards(animator.transform.position, target, moveSpeed * Time.deltaTime);

            //target = new Vector3(bossRightPoint.transform.position.x, animator.transform.position.y);
            //animator.transform.position = Vector3.MoveTowards(animator.transform.position, target, moveSpeed * Time.deltaTime);



            //animator.transform.position = Vector3.MoveTowards(animator.transform.position, target, moveSpeed * Time.deltaTime);

            //Vector2 target = new Vector2(playerPosition.position.x, animator.transform.position.y);
            //animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, moveSpeed * Time.deltaTime);


            //if (timerWalk <= 0.1f)
            //{
            //    animator.SetTrigger("isIdle");
            //}
            //else
            //{
            //timerWalk -= Time.deltaTime;
            //    Vector2 target = PlayerController.instance.transform.position;
            //animator.transform.position = Vector3.MoveTowards(animator.transform.position, target, moveSpeed * Time.deltaTime);
            //Vector2 target = new Vector2(playerPosition.position.x, animator.transform.position.y);
            //Vector2 target = new Vector2(playerPosition.position.x, playerPosition.transform.position.y);
            //animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, moveSpeed * Time.deltaTime);

            //}
        }
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
