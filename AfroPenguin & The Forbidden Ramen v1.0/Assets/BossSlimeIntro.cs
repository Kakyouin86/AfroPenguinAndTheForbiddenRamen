using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeIntro : StateMachineBehaviour
{
    public int random;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        random = Random.Range(0, 2);
        if (random == 0)
        {
            animator.SetTrigger("idle");
        }
        else
        {
            animator.SetTrigger("jump");
        }
    }
}
