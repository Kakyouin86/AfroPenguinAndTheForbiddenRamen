using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossBattle : MonoBehaviour
{
    public int health;
    public int damage;
    public float timeBtwDamage = 1.5f;
    public Slider healthBar;
    public Animator theAnimator;
    public bool isDead;

    private void Start()
    {
        theAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (health <= 40)
        {
            theAnimator.SetTrigger("stageTwo");
        }

        if (health <= 0)
        {
            theAnimator.SetTrigger("death");
        }

        // give the player some time to recover before taking more damage !
        if (timeBtwDamage > 0)
        {
            timeBtwDamage -= Time.deltaTime;
        }

        healthBar.value = health;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // deal the player damage ! 
        if (other.CompareTag("Player") && isDead == false)
        {
            if (timeBtwDamage <= 0)
            {
                //camAnim.SetTrigger("shake");
                //other.GetComponent<Player>().health -= damage;
            }
        }
    }
}
