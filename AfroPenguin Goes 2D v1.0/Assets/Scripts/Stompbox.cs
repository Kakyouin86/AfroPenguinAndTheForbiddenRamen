using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompbox : MonoBehaviour
{
    public static Stompbox instance;
    public PlayerController thePC;
    public GameObject deathEffect;
    public GameObject collectible;
    public int damageToDeal = 1;
    [Range(0, 100)] public float chanceToDrop = 1f;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        thePC = FindObjectOfType<PlayerController>();
    }

    public void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (thePC.theRB.velocity.y <= 0.1f && (thePC.isGrounded == false))
        {
            if (other.gameObject.tag == "Hurtbox")
            {
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