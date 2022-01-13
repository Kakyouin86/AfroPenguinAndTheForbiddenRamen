using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Pickup : MonoBehaviour
{
    public bool isCollected;
    public bool hasReinstantiation;

    [Header("Item Types")]
    public bool isStar;
    public bool isHeal;
    public bool isOrb;
    public bool isFish;
    public bool isLife;
    public bool isGem;
    public bool isGemx2;
    public bool isDiamond;
    public float timeOfInvulnerability = 22f;
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
                DestroyOrNot();
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 0.50f);
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
                    DestroyOrNot();
                    placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                    Instantiate(pickupEffectHeal, placeToInstantiate, transform.rotation);
                    AudioManager.instance.PlaySFX(8);
                }
                else
                {
                    GetComponentInChildren<Animator>().Play("Life Item - 01 - Life Is Full");
                    AudioManager.instance.PlaySFX(14);
                }
            }

            if (isOrb)
            {
                PlayerController.instance.BuildDash();
                isCollected = true;
                DestroyOrNot();
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
                AudioManager.instance.PlaySFX(11);
                AudioManager.instance.PlayFishItemMusic();
                StartCoroutine(IsInvulnerable());
            }

            if (isLife)
            {
                if (LevelManager.instance.sumLostLife <= 0)
                {
                    GetComponentInChildren<Animator>().Play("Minus Life Lost Item - 01 - Life Is Full");
                    AudioManager.instance.PlaySFX(14);
                }

                else
                {
                    LevelManager.instance.sumLostLife--;
                    UIController.instance.SumLostLife();
                    DestroyOrNot();
                    pickupEffectLife.Play();
                    AudioManager.instance.PlaySFX(10);
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
                DestroyOrNot();
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                Instantiate(pickupEffectStar, placeToInstantiate, transform.rotation);
                AudioManager.instance.PlaySFX(9);
                UIController.instance.UpdateStarsCount();
            }

            if (isGemx2)
            {
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                isCollected = true;
                DestroyOrNot();
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                Instantiate(pickupEffectStar, placeToInstantiate, transform.rotation);
                AudioManager.instance.PlaySFX(12);
                UIController.instance.UpdateStarsCount();
            }

            if (isDiamond)
            {
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                LevelManager.instance.starsCollected++;
                isCollected = true;
                DestroyOrNot();
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                Instantiate(pickupEffectStar, placeToInstantiate, transform.rotation);
                AudioManager.instance.PlaySFX(13);
                UIController.instance.UpdateStarsCount();

                if (LevelManager.instance.sumLostLife > 0)
                {
                    LevelManager.instance.sumLostLife--;
                    UIController.instance.SumLostLife();
                    pickupEffectLife.Play();
                }
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
        AudioManager.instance.PlayNormalBGM();
    }

    public void DestroyOrNot()
    {
        if (hasReinstantiation)
        {
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(TimeInvisible());
        }

        else
        {
            Destroy(gameObject);
        }
    }
    IEnumerator TimeInvisible()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
