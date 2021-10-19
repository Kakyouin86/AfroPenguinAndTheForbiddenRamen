using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompbox : MonoBehaviour
{
    public PlayerController thePC;
    public GameObject deathEffect;
    public GameObject collectible;
    public int damageToDeal = 1;
    public int damageToDealDash = 3;
    [Range(0, 100)] public float chanceToDrop = 1f;

    void Start()
    {
        thePC = FindObjectOfType<PlayerController>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (thePC.theRB.velocity.y <= 0.1f && (thePC.isGrounded == false))
        {
            if (other.gameObject.tag == "Hurtbox")
            {
                //other.transform.gameObject.SetActive(false);
                other.gameObject.GetComponent<EnemyHP>().TakeDamage(damageToDeal);
                Instantiate(deathEffect, other.transform.position, other.transform.rotation);
                PlayerController.instance.Bounce();
            }

            if (other.gameObject.tag == "Hurtbox" && other.gameObject.GetComponent<EnemyHP>().isDead)
            {
                float dropSelect = Random.Range(0, 100f);

                if (dropSelect <= chanceToDrop)
                {
                    Instantiate(collectible, other.transform.position, other.transform.rotation);
                }
            }
        }
    }
}