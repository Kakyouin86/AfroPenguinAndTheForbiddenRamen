using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeIdle : StateMachineBehaviour
{
    public float timer;
    public float minTime = 0.5f;
    public float maxTime = 1.5f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minTime, maxTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0)
        {
            animator.SetTrigger("jump");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
