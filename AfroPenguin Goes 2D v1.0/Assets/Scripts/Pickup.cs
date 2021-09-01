using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool isGem;
    public bool isHeal;
    private bool isCollected;
    public GameObject pickupEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            if (isGem)
            {
                LevelManager.instance.gemsCollected++;
                isCollected = true;
                Destroy(gameObject);
                Instantiate(pickupEffect, transform.position, transform.rotation);
                UIController.instance.UpdateGemCount();
                AudioManager.instance.PlaySFX(6);
            }
            
            if (isHeal)
            {

                if (PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth)

                {
                    PlayerHealthController.instance.HealPlayer();
                    isCollected = true;
                    Destroy(gameObject);
                    Instantiate(pickupEffect, transform.position, transform.rotation);
                    AudioManager.instance.PlaySFX(7);
                }
            }
        }
    }
}
