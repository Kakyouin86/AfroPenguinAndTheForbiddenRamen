using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Stompbox : MonoBehaviour
{
    public PlayerController thePC;
    public GameObject deathEffect;
    public GameObject collectible;
    public int damageToDeal = 1;
    public int damageToDealDash = 3;
    public Vector2 placeToInstantiate;
    [Range(0, 100)] public float chanceToDrop = 100f;

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
                other.gameObject.GetComponent<EnemyHP>().TakeDamage(damageToDeal);
                placeToInstantiate = new Vector2(other.transform.position.x, other.transform.position.y + 1.00f);
                Instantiate(deathEffect, placeToInstantiate, other.transform.rotation);
                PlayerController.instance.Bounce(1f);
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