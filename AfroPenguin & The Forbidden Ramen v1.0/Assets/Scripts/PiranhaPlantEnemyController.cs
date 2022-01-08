using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PiranhaPlantEnemyController : MonoBehaviour
{
    [Header("Components")] 
    public Transform player;
    public SpriteRenderer theSR;
    public Animator theAnimator;
    public GameObject firePrefab;
    public Transform firePoint;

    [Header("Movement")]
    public float waitAfterAttackShot = 1.5f;
    public float waitAfterAttackHead = 1f;
    public float distanceToAttackPlayerShot = 4f;
    public float distanceToAttackPlayerHead = 3f;
    public float maximumDistanceToAttackPlayer = 10f;
    public float attackCounterShot;
    public float attackCounterHead;
    public float shotForce = 7f;

    void Start()
    {
        theSR = GetComponentInChildren<SpriteRenderer>();
        theAnimator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        WhereToLook();

        if (attackCounterShot > 0)
        {
            attackCounterShot -= Time.deltaTime;
            theAnimator.SetBool("is Far", false);
        }

        if (attackCounterHead > 0)
        {
            attackCounterHead -= Time.deltaTime;
            theAnimator.SetBool("is Close", false);
        }

        else
        {
            Attacks();
        }
    }

    public void Attacks()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > maximumDistanceToAttackPlayer)
        {
            theAnimator.SetBool("is Far", false);
            theAnimator.SetBool("is Close", false);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < distanceToAttackPlayerHead)
        {
            theAnimator.SetBool("is Far", false);
            theAnimator.SetBool("is Close", true);
            if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Piranha Plant - 01 - Head Attack"))
            {
                attackCounterHead = waitAfterAttackHead;
                theAnimator.SetBool("is Close", false);
            }
        }
        
        else if (Vector3.Distance(transform.position, player.transform.position) >= distanceToAttackPlayerShot && Vector3.Distance(transform.position, player.transform.position) < maximumDistanceToAttackPlayer)
        {
            theAnimator.SetBool("is Far", true);
            GameObject fire = Instantiate(firePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb2d = fire.GetComponent<Rigidbody2D>();
            rb2d.AddForce(firePoint.up * shotForce, ForceMode2D.Impulse);

            if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Piranha Plant - 01 - Attack"))
            {
                attackCounterShot = waitAfterAttackShot;
                theAnimator.SetBool("is Far", false);
            }
        }
    }

    public void WhereToLook()
    {
        if (transform.position.x < player.transform.position.x)
        {
            theSR.flipX = true; //This is facing right.
        }
        else if (transform.position.x > player.transform.position.x)
        {
            theSR.flipX = false; //This is facing left.
        }
    }
}
