using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDamageController : MonoBehaviour
{
    public PlayerController thePC;
    public GameObject deathEffect;
    public GameObject collectible;
    public int damageToDealDash = 3;
    [Range(0, 100)] public float chanceToDrop = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        thePC = GetComponentInParent<PlayerController>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
            {
                other.gameObject.GetComponentInChildren<EnemyHP>().TakeDamageDash(damageToDealDash);
                Instantiate(deathEffect, other.transform.position, other.transform.rotation);
                Debug.Log(other);
            }

            if (other.gameObject.tag == "Enemy" && other.gameObject.GetComponentInChildren<EnemyHP>().isDead)
            {
                float dropSelect = Random.Range(0, 100f);

                if (dropSelect <= chanceToDrop)
                {
                    Instantiate(collectible, other.transform.position, other.transform.rotation);
                }
            }
        }
    }
