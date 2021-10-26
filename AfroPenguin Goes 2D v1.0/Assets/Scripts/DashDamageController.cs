using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
        if (other.gameObject.tag == "Hurtbox")
        {
            if (other.gameObject.GetComponentInChildren<EnemyHP>().enemyHP <= damageToDealDash)
            {
                other.gameObject.GetComponentInChildren<EnemyHP>().TakeDamageDash(damageToDealDash);
                Instantiate(deathEffect, other.transform.position, other.transform.rotation);
                float dropSelect = Random.Range(0, 100f);

                if (dropSelect <= chanceToDrop)
                {
                    Instantiate(collectible, other.transform.position, other.transform.rotation);
                }
            }
            else
            {
                other.gameObject.GetComponentInChildren<EnemyHP>().TakeDamageDash(damageToDealDash);
                Instantiate(deathEffect, other.transform.position, other.transform.rotation);
                PlayerController.instance.hasHit = true;
                other.GetComponent<KnockbackEnemies>().KnockBack();
            }
        }
    }
}
