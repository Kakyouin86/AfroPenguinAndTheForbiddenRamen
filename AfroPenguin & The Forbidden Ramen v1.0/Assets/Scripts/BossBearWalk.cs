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

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        timerWalk = Random.Range(minTimeWalking, maxTimeWalking);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (timerWalk <= 0)
        {
            animator.SetTrigger("isIdle");
        }
        else
        {
            timerWalk -= Time.deltaTime;
        }

        Vector2 target = new Vector2(playerPosition.position.x, animator.transform.position.y);
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, moveSpeed * Time.deltaTime);


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
        if (Vector2.Distance(target, animator.transform.position) <= 1.5f)
            {
                animator.SetTrigger("goAway");
            }
        //}
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
