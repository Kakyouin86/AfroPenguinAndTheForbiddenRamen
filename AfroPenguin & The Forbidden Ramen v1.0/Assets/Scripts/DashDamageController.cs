using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDamageController : MonoBehaviour
{
    public PlayerController thePC;
    public GameObject deathEffect;
    public int damageToDealDash = 3;
    public Vector2 placeToInstantiate;
    [Range(0, 100)] public float chanceToDrop = 100f;

    // Start is called before the first frame update
    void Start()
    {
        thePC = GetComponentInParent<PlayerController>();
    }

    void Update()
    {

    }
    public void OnTriggerEnter2D(Collider2D enemy)
    {
        if (enemy.gameObject.tag == "Hurtbox")
        {
            enemy.gameObject.GetComponentInChildren<EnemyHP>().TakeDamageDash(damageToDealDash);
            placeToInstantiate = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 1.00f);
            Instantiate(deathEffect, placeToInstantiate, enemy.transform.rotation);
            PlayerController.instance.hasHit = true;
            enemy.GetComponent<KnockbackEnemies>().KnockBack();
        }

        if (enemy.gameObject.tag == "Boss Hurtbox")
        {
            enemy.gameObject.GetComponentInChildren<EnemyHP>().TakeDamageDashBoss(damageToDealDash);
            placeToInstantiate = new Vector2(enemy.transform.position.x, enemy.transform.position.y + 1.00f);
            Instantiate(deathEffect, placeToInstantiate, enemy.transform.rotation);
            PlayerController.instance.hasHit = true;
            enemy.GetComponent<KnockbackEnemies>().KnockBack();
        }
    }
}
