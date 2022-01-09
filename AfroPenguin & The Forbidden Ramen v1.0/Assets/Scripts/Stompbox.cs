using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class Stompbox : MonoBehaviour
{
    public PlayerController thePC;
    public GameObject enemyDeathEffect;
    public GameObject starCollectible;
    public int damageToDeal = 1;
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
                Instantiate(enemyDeathEffect, placeToInstantiate, other.transform.rotation);
                AudioManager.instance.PlaySFX(2);
                PlayerController.instance.Bounce(1f);
                GetComponentInParent<CapsuleCollider2D>().enabled = false;
                GetComponentInParent<CapsuleCollider2D>().enabled = true;
            }

            if (other.gameObject.tag == "Boss Hurtbox")
            {
                other.gameObject.GetComponent<EnemyHP>().TakeDamageBoss(damageToDeal);
                placeToInstantiate = new Vector2(other.transform.position.x, other.transform.position.y + 1.00f);
                Instantiate(enemyDeathEffect, placeToInstantiate, other.transform.rotation);
                AudioManager.instance.PlaySFX(2);
                PlayerController.instance.Bounce(1f);
                GetComponentInParent<CapsuleCollider2D>().enabled = false;
                GetComponentInParent<CapsuleCollider2D>().enabled = true;
            }


            if (other.gameObject.tag == "Hurtbox" && other.gameObject.GetComponent<EnemyHP>().isDead)
            {
                float dropSelect = Random.Range(0, 100f);

                if (dropSelect <= chanceToDrop)
                {
                    placeToInstantiate = new Vector2(other.transform.position.x + 1.00f, other.transform.position.y + 1.00f);
                    Instantiate(starCollectible, placeToInstantiate, other.transform.rotation);
                }
            }

            if (other.gameObject.tag == "Boss Hurtbox" && other.gameObject.GetComponent<EnemyHP>().isDead)
            {
                float dropSelect = Random.Range(0, 100f);

                if (dropSelect <= chanceToDrop)
                {
                    placeToInstantiate = new Vector2(other.transform.position.x + 1.00f, other.transform.position.y + 1.00f);
                }
            }
        }
    }
}