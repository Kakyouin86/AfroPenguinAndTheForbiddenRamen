using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    //Singleton: Create a version of this script that only one version can of it can exist. This is what I do below.
    //Instance is a standard convention.
    //What's being done here is called a "singleton" pattern. Which is setting just one version (or instance) of the class
    //object to be active. What the instance = this is saying is the only version of this class which exists is this class.
    //e.g. In your PlayerHealthController script the this means the PlayerHealthController, and being static means any other
    //script or object can get the PlayerHealthController by using PlayerHealthController.instance.
    //The use of the variable instance is just a naming convention and could be anything really, but instance really makes sense.
    //This accesses straight away from any other scripts.

    //instance es, cada objeto tiene sus propiedades con sus valores. Static todos tienen el mismo valor compartido
    //Instance es: dame este objeto en este momento. Static es dame esto que es constante en todas las instancias
    // si vos haces por ejemplo:
    //class Person
    //{
    //    public string name;
    //    public static string lastname = "pepito";
    //    constructor {
    //   this.name = name;
    //}

    // Person.name // no existe, porque no está instanciado
    // Person.lastname // es "pepito" porque es static
    public static PlayerHealthController instance;
    public int currentHealth = 6;
    public int maxHealth = 6;
    public float invisibleLength = 1;
    public float invisibleCounter;
    public SpriteRenderer spriteRenderer;
    public bool flashing;
    public GameObject deathEffect;
    public GameObject stompbox;
    public Vector2 placeToInstantiate;
    public GameObject pickupEffectBarEffect;
    public GameObject pickupEffectThunderInBarEffect;

    private void Awake()
    {
        instance = this;
        flashing = false;
    }
    void Start()
    {
        pickupEffectBarEffect = GameObject.Find("Canvas/Dash Sprites/Bar-Fill");
        pickupEffectThunderInBarEffect = GameObject.Find("UI Camera/Thunder In Bar");
        flashing = false;
        pickupEffectBarEffect.GetComponent<_2dxFX_LightningBolt>().enabled = false;
        pickupEffectThunderInBarEffect.GetComponent<_2dxFX_Lightning>().enabled = false;
        PlayerController.instance.GetComponent<_2dxFX_LightningBolt>().enabled = false;
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invisibleCounter > 0)
        {
            invisibleCounter -= Time.deltaTime;
            //we are taking away another value from this from the invincible counter.
            StartCoroutine(StompboxDeactivated());

            if (invisibleCounter <= 0.1)
            {
                spriteRenderer.color = new Color(1f, 1f, 1f, 1.0f);
                //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f);
            }
        }
    }

    public void DealDamage()
    {
        if (invisibleCounter <= 0)
        {
            currentHealth--;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                gameObject.SetActive(false);
                placeToInstantiate = new Vector2(transform.position.x, transform.position.y + 1.00f);
                Instantiate(deathEffect, placeToInstantiate, transform.rotation);
                AudioManager.instance.PlaySFX(1);
                LevelManager.instance.RespawnPlayer();
                LevelManager.instance.SumLostLife();
                UIController.instance.SumLostLife();
            }

            else
            {
                invisibleCounter = invisibleLength;
                PlayerController.instance.KnockBack();
                AudioManager.instance.PlaySFX(3);
                if (currentHealth < 2)
                {
                    FlashWrapper();
                }
                else
                {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
                }
            }
            UIController.instance.UpdateHealthUpdate();
        }
    }

    public void HealPlayer()
    {
        currentHealth++;

        if (currentHealth > maxHealth)

        {
            currentHealth = maxHealth;
        }

        UIController.instance.UpdateHealthUpdate();
    }
    
    public void FlashWrapper()
    {
        if (!flashing)
            StartCoroutine("Flash");
    }

    IEnumerator Flash()
    {
        while (invisibleCounter > 0.1)
        {
            //invincibleCounter -= Time.deltaTime;
            flashing = true;
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
            yield
            return new WaitForSeconds(0.05f);
            spriteRenderer.color = new Color(0.7264151f, 0.1329476f, 0.1329476f, 1f);
            //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
            yield
            return new WaitForSeconds(0.05f);
            flashing = false;
        }

        if (invisibleCounter <= 0.1)
        {
            flashing = false;
        }
    }

    IEnumerator StompboxDeactivated()
    {
        stompbox.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        //it was 0.2f
        stompbox.SetActive(true);
    }
}

