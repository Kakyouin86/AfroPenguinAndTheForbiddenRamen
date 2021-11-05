using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
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
    public bool isLife;
    public bool isGem;
    public float timeOfInvulnerability;
    public float timeOfInvulnerabilityTimer;

    [Header("Effects")]
    public GameObject pickupEffectStar;
    public GameObject pickupEffectHeal;
    public GameObject pickupEffectOrb;
    public GameObject pickupEffectBarEffect;
    public GameObject pickupEffectThunderInBarEffect;
    public GameObject pickupEffectFish;
    public ParticleSystem pickupEffectLife;
    public Vector2 placeToInstantiate;

    void Update()
    {
    }

    void Start()
    {
        pickupEffectBarEffect = GameObject.Find("Canvas/Dash Sprites/Bar-Fill");
        pickupEffectThunderInBarEffect = GameObject.Find("UI Camera/Thunder In Bar");
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
                pickupEffectBarEffect.GetComponent<_2dxFX_LightningBolt>().enabled = true;
                pickupEffectBarEffect.GetComponent<_2dxFX_Lightning>().enabled = true;
                PlayerController.instance.GetComponent <_2dxFX_LightningBolt>().enabled = true;
                pickupEffectThunderInBarEffect.GetComponent<Animator>().Play("Thunder In Bar Effect - 01");
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                Instantiate(pickupEffectFish, placeToInstantiate, transform.rotation);
                StartCoroutine(IsInvulnerable());
            }

            if (isLife)
            {
                if (LevelManager.instance.sumLostLife <= 0)
                {

                }
                else
                {
                    LevelManager.instance.sumLostLife--;
                    UIController.instance.SumLostLife();
                    Destroy(gameObject);
                    pickupEffectLife.Play();
                    AudioManager.instance.PlaySFX(7);
                }
            }

            if (isGem)
            {
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                isCollected = true;
                Destroy(gameObject);
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                Instantiate(pickupEffectStar, placeToInstantiate, transform.rotation);
                UIController.instance.UpdateStarsCount();
                AudioManager.instance.PlaySFX(6);
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
        pickupEffectBarEffect.GetComponent<_2dxFX_LightningBolt>().enabled = false;
        pickupEffectBarEffect.GetComponent<_2dxFX_Lightning>().enabled = false;
        PlayerController.instance.GetComponent<_2dxFX_LightningBolt>().enabled = false;
    }
}
