using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    public SpriteRenderer theSR;
    public Animator theAnimator;
    public Collider2D parentCol;
    public Collider2D hurtboxCol;
    public int enemyHP = 1;
    public int currentHP;
    public bool isDead;
    public float invisibleLength = 0.05f;
    public float invisibleCounter;
    public float knockbackDashMultiplier = 2f;
    public Vector2 placeToInstantiate;
    public GameObject collectible;
    public Slider theBossSlider;
    public GameObject theBossHealthBar;

    void Start()
    {
        currentHP = enemyHP;
        theAnimator = transform.parent.GetComponent<Animator>();
        parentCol = transform.parent.GetComponent<Collider2D>();
        hurtboxCol = GetComponent<Collider2D>();
        theSR = transform.GetComponent<SpriteRenderer>();
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP >= 2)
        {
            StartCoroutine("HitConfirm");
        }

        else if (currentHP == 1)
        {
            StartCoroutine("HitConfirmAlmostDead");
        }

        else if (currentHP <= 0)
        {
            isDead = true;
            theAnimator.SetBool("isDead", isDead);
            parentCol.enabled = false;
            hurtboxCol.enabled = false;
            StartCoroutine("KillSwitch");
        }
    }

    public void TakeDamageBoss(int damage)
    {
        currentHP -= damage;
        theBossSlider.value = currentHP;
        if (currentHP >= 2)
        {
            StartCoroutine("HitConfirm");
        }

        else if (currentHP == 1)
        {
            StartCoroutine("HitConfirmAlmostDead");
        }

        else if (currentHP <= 0)
        {
            isDead = true;
            theBossHealthBar.SetActive(false);
        }
    }

    public void TakeDamageDash(int damageDash)
    {
        currentHP -= damageDash;

        if (currentHP >= 4)
        {
            if (PlayerController.instance.dashDown)
            {
                PlayerController.instance.Bounce(1.5f);
                StartCoroutine("HitConfirm");
            }
            else
            {
                PlayerController.instance.KnockBackDash(knockbackDashMultiplier);
                StartCoroutine("HitConfirm");
            }
        }

        else if (currentHP == 1)
        {
            PlayerController.instance.KnockBackDash(knockbackDashMultiplier);
            StartCoroutine("HitConfirmAlmostDead");
        }

        else if (currentHP <= 0)
        {
            isDead = true;
            theAnimator.SetBool("isDead", isDead);
            parentCol.enabled = false;
            hurtboxCol.enabled = false;
            StartCoroutine("KillSwitch");
            placeToInstantiate = new Vector2(transform.position.x + 1.00f, transform.position.y);
            Instantiate(collectible, placeToInstantiate, transform.rotation);
        }
    }

    public void TakeDamageDashBoss(int damageDash)
    {
        if (currentHP <= 3)
        {
            currentHP = 0;
            theBossSlider.value = currentHP;
            isDead = true;
            theBossHealthBar.SetActive(false);
        }

        if (currentHP >= 4)
        {

            currentHP -= damageDash;
            theBossSlider.value = currentHP;
            if (PlayerController.instance.dashDown)
            {
                PlayerController.instance.Bounce(1.5f);
                StartCoroutine("HitConfirm");
            }
            else
            {
                PlayerController.instance.KnockBackDash(knockbackDashMultiplier);
                StartCoroutine("HitConfirm");
            }
        }
    }

    IEnumerator KillSwitch()
    {
        yield return new WaitForSeconds(0f);
        //Destroy(transform.parent.gameObject);
        theSR.color = new Color(1f, 1f, 1f, 1.0f);
        theSR.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInChildren<EnemyHP>().currentHP = GetComponentInChildren<EnemyHP>().enemyHP;
        transform.parent.gameObject.SetActive(false);
    }

    public void KillInstantly()
    {
        theSR.color = new Color(1f, 1f, 1f, 1.0f);
        theSR.enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        transform.parent.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInChildren<EnemyHP>().currentHP = GetComponentInChildren<EnemyHP>().enemyHP;
        transform.parent.gameObject.SetActive(false);
    }

    IEnumerator HitConfirm()
    {
        theSR.enabled = false;
        StartCoroutine("Flash");
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInParent<BoxCollider2D>().enabled = false;
        GetComponentInParent<BoxCollider2D>().enabled = true;
        theSR.enabled = true;
    }

    IEnumerator HitConfirmAlmostDead()
    {
        theSR.enabled = false;
        StartCoroutine("FlashAlmostDead");
        yield return new WaitForSeconds(0.1f);
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponentInParent<BoxCollider2D>().enabled = false;
        GetComponentInParent<BoxCollider2D>().enabled = true;
        theSR.enabled = true;
    }

    IEnumerator Flash()
    {
        invisibleCounter = invisibleLength;
        while (invisibleCounter > 0)
        {
            invisibleCounter -= Time.deltaTime;
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.5f);
            yield return new WaitForSeconds(0.05f);
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
            yield return new WaitForSeconds(0.05f);
        }
        theSR.color = new Color(1f, 1f, 1f, 1.0f);
    }

    IEnumerator FlashAlmostDead()
    {
        invisibleCounter = invisibleLength;
        while (invisibleCounter > 0)
        {
            invisibleCounter -= Time.deltaTime;
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.5f);
            yield return new WaitForSeconds(0.05f);
            theSR.color = new Color(0.7264151f, 0.1329476f, 0.1329476f, 1f);
            yield return new WaitForSeconds(0.05f);
        }
        theSR.color = new Color(1f, 1f, 1f, 1.0f);
    }
}