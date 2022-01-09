using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeIdle : StateMachineBehaviour
{
    public float timer;
    public float minTime = 2f;
    public float maxTime = 3f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = Random.Range(minTime, maxTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timer <= 0.1f)
        {
            animator.SetTrigger("jump");
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
