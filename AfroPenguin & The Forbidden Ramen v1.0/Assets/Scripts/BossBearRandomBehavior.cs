using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBearRandomBehavior : StateMachineBehaviour
{
    public float timerIdle; 
    public float minTimeWaiting = 1f;
    public float maxTimeWaiting = 3f;
    public int randomBehavior;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timerIdle = Random.Range(minTimeWaiting, maxTimeWaiting);
        randomBehavior = Random.Range(0, 0);
        switch (randomBehavior)
        {
            case 0:
                animator.SetTrigger("walk");
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timerIdle <= 0.1f)
        {
            animator.SetTrigger("walk");
        }
        else
        {
            timerIdle -= Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //    // Implement code that processes and affects root motion
    }

    // OnStateIK is called right after Animator.OnAnimatorIK()
    override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Implement code that sets up animation IK (inverse kinematics)
    }
}
