using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private bool isCollected;

    [Header("Item Types")]
    public bool isStar;
    public bool isHeal;
    public bool isOrb;

    [Header("Effects")]
    public GameObject pickupEffectStar;
    public GameObject pickupEffectHeal;
    public GameObject pickupEffectOrb;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {
            if (isStar)
            {
                LevelManager.instance.starsCollected++;
                isCollected = true;
                Destroy(gameObject);
                Instantiate(pickupEffectStar, transform.position, transform.rotation);
                UIController.instance.UpdateStarsCount();
                AudioManager.instance.PlaySFX(6);
            }
            
            if (isHeal)
            {

                if (PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth)

                {
                    PlayerHealthController.instance.HealPlayer();
                    isCollected = true;
                    Destroy(gameObject);
                    Instantiate(pickupEffectHeal, transform.position, transform.rotation);
                    AudioManager.instance.PlaySFX(7);
                }
            }

            if (isOrb)
            {
                PlayerController.instance.BuildDash();
                isCollected = true;
                Destroy(gameObject);
                Instantiate(pickupEffectOrb, transform.position, transform.rotation);
                AudioManager.instance.PlaySFX(7);
            }
        }
    }
}
