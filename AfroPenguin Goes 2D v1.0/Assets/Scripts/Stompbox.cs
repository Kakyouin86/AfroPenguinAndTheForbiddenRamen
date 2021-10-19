using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompbox : MonoBehaviour
{
    public GameObject deathEffect;
    public GameObject collectible;
    public int damageToDeal = 1;
    [Range(0, 100)] public float chanceToDrop = 1f;
    
    public void OnTriggerEnter2D(Collider2D other)
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