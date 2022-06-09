using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BossSlimeJump : StateMachineBehaviour
{
    public float stateTimer;
    public float jumpTimer;
    public float minTime = 1f;
    public float maxTime = 1.5f;
    Vector2 target;
    public float speed = 4f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        stateTimer = Random.Range(minTime, maxTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateTimer <= 0.1f)
        {
            animator.SetTrigger("idle");
        }
        else
        {
            stateTimer -= Time.deltaTime;
        }
        Vector3 playerPosition = animator.gameObject.GetComponent<BossBattleSlime>().targetPosition;
        target = new Vector2(playerPosition.x, animator.transform.position.y);
        animator.transform.position = Vector2.MoveTowards(animator.transform.position, target, speed * Time.deltaTime);
    }
}