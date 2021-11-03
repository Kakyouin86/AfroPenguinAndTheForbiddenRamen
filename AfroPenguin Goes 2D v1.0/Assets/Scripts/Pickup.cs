using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickup : MonoBehaviour
{
    private bool isCollected;

    [Header("Item Types")]
    public bool isStar;
    public bool isHeal;
    public bool isOrb;
    public bool isFish;
    public float timeOfInvulnerability;
    public float timeOfInvulnerabilityTimer;

    [Header("Effects")]
    public GameObject pickupEffectStar;
    public GameObject pickupEffectHeal;
    public GameObject pickupEffectOrb;
    public GameObject pickupEffectFish;
    public Vector2 placeToInstantiate;

    void Update()
    {
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Invulnerable") && !isCollected)
        {
            if (isStar)
            {
                LevelManager.instance.starsCollected++;
                isCollected = true;
                Destroy(gameObject);
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                Instantiate(pickupEffectStar, placeToInstantiate, transform.rotation);
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
                    placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                    Instantiate(pickupEffectHeal, placeToInstantiate, transform.rotation);
                    AudioManager.instance.PlaySFX(7);
                }
            }

            if (isOrb)
            {
                PlayerController.instance.BuildDash();
                isCollected = true;
                Destroy(gameObject);
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 0.50f);
                Instantiate(pickupEffectOrb, placeToInstantiate, transform.rotation);
                AudioManager.instance.PlaySFX(7);
            }

            if (isFish)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                //Instantiate(pickupEffectFish, placeToInstantiate, transform.rotation);
                StartCoroutine(IsInvulnerable());
            }
        }
    }

    IEnumerator IsInvulnerable()
    {
        while (timeOfInvulnerabilityTimer < timeOfInvulnerability)
        {
            PlayerController.instance.canDash = true;
            PlayerController.instance.currentDashGauge = 100;
            PlayerController.instance.tag = "Invulnerable";
            PlayerController.instance.isInvulnerable = true;
            UIController.instance.barAnimator.SetBool("isFilled", true);
            UIController.instance.iconAnimator.SetBool("isFilled", true);
            UIController.instance.dashIndicatorSlider.value = PlayerController.instance.currentDashGauge;
            timeOfInvulnerabilityTimer += Time.deltaTime;
            yield return null;
            PlayerController.instance.tag = "Player";
            PlayerController.instance.isInvulnerable = false;
        }
    }
}
