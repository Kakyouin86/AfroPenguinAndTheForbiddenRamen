using System.Collections;
using System.Collections.Generic;
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
    public float invincibleLenght = 1;
    public float invincibleCounter;
    private SpriteRenderer spriteRenderer;
    public bool flashing;
    public GameObject deathEffect;
    public GameObject stompbox;


    private void Awake()
    {
        instance = this;
        flashing = false;
        //this means this is the exact components that this script is at this moment.
    }
    void Start()
    {

        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (invincibleCounter > 0)
        {
            invincibleCounter -= Time.deltaTime;
            //we are taking away another value from this from the invincible counter.

            //stompbox.SetActive(false);
            if (invincibleCounter <= 0)
            {
                //stompbox.SetActive(true);
                spriteRenderer.color = new Color(1f, 1f, 1f, 1.0f);
                //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0f);
            }
        }
    }

    public void DealDamage()
    {
        if (invincibleCounter <= 0)
        {
            //currentHealth = currentHealth - 25;
            currentHealth--;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                gameObject.SetActive(false);
                Instantiate(deathEffect, transform.position, transform.rotation);
                LevelManager.instance.RespawnPlayer();
            }


            else
            {
                invincibleCounter = invincibleLenght;
                PlayerController.instance.KnockBack();
                AudioManager.instance.PlaySFX(9);
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = other.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }
    }


    public void FlashWrapper()
    {
        if (!flashing)
            StartCoroutine("Flash");
    }

    IEnumerator Flash()
    {
        while (invincibleCounter > 0)
        {
            // invincibleCounter -= Time.deltaTime;
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
    }
}

