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
    public Transform firePoint;
    public GameObject firePrefab;
    public bool facingRight;

    [Header("Movement")]
    public float waitAfterAttackShot = 1.5f;
    public float waitAfterAttackHead = 1f;
    public float distanceToAttackPlayerShot = 3.5f;
    public float distanceToAttackPlayerHead = 3.5f;
    public float maximumDistanceToAttackPlayer = 10f;
    public float attackCounterShot;
    public float attackCounterHead;
    public float shotForce = 0.05f;

    void Start()
    {
        theSR = GetComponentInChildren<SpriteRenderer>();
        theAnimator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        facingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < player.transform.position.x && !facingRight)
        {
            WhereToLook();
        }
        else if (transform.position.x > player.transform.position.x && facingRight)
        {
            WhereToLook();
        }

        if (attackCounterShot > 0)
        {
            attackCounterShot -= Time.deltaTime;
            theAnimator.SetBool("isFar", false);
        }

        if (attackCounterHead > 0)
        {
            attackCounterHead -= Time.deltaTime;
            theAnimator.SetBool("isClose", false);
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
            theAnimator.SetBool("isFar", false);
            theAnimator.SetBool("isClose", false);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < distanceToAttackPlayerHead)
        {
            theAnimator.SetBool("isFar", false);
            theAnimator.SetBool("isClose", true);
            if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Piranha Plant - 01 - Head Attack"))
            {
                attackCounterHead = waitAfterAttackHead;
                theAnimator.SetBool("isClose", false);
            }
        }
        
        else if (Vector3.Distance(transform.position, player.transform.position) >= distanceToAttackPlayerShot && Vector3.Distance(transform.position, player.transform.position) < maximumDistanceToAttackPlayer)
        {
            theAnimator.SetBool("isFar", true);
            if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Piranha Plant - 01 - Attack"))
            {
                attackCounterShot = waitAfterAttackShot;
                theAnimator.SetBool("isFar", false);
            }
        }
    }

    public void Shoot()
    {
        GameObject fire = Instantiate(firePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb2d = fire.GetComponent<Rigidbody2D>();
        rb2d.AddForce(firePoint.up * shotForce);
        //rb2d.velocity = transform.right * shotForce;
    }


    public void WhereToLook()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0);
    }
}
