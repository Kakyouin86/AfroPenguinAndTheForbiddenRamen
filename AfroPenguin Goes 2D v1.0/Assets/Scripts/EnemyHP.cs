using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class EnemyHP : MonoBehaviour
{
    public SpriteRenderer theSR;
    public Animator theAnimator;
    public Collider2D parentCol;
    public Collider2D hurtboxCol;
    public int enemyHP;
    public int currentHP;
    public bool isDead;
    public float invisibleLength = 0.05f;
    public float invisibleCounter;
    public float multiplier = 2f;

    void Start()
    {
        currentHP = enemyHP;
        theAnimator = transform.parent.GetComponent<Animator>();
        parentCol = transform.parent.GetComponent<Collider2D>();
        hurtboxCol = GetComponent<Collider2D>();
        theSR = transform.parent.GetComponent<SpriteRenderer>();
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

    public void TakeDamageDash(int damageDash)
    {
        currentHP -= damageDash;
        if (currentHP >= 2)
        {
            PlayerController.instance.KnockBackDash(multiplier);
            StartCoroutine("HitConfirm");
        }

        else if (currentHP == 1)
        {
            PlayerController.instance.KnockBackDash(multiplier);
            StartCoroutine("HitConfirmAlmostDead");
        }

        else if (currentHP <= 0)
        {
            PlayerController.instance.KnockBackDash(multiplier);
            isDead = true;
            theAnimator.SetBool("isDead", isDead);
            parentCol.enabled = false;
            hurtboxCol.enabled = false;
            StartCoroutine("KillSwitch");
        }



            //PlayerController.instance.KnockBackDash(multiplier);
            //currentHP -= damageDash;
            //StartCoroutine("HitConfirm");
            //isDead = true;
            //theAnimator.SetBool("isDead", isDead);
            //parentCol.enabled = false;
            //hurtboxCol.enabled = false;
            //KillInstantly();
    }

    IEnumerator KillSwitch()
    {
        yield return new WaitForSeconds(0f);
        Destroy(transform.parent.gameObject);
    }
    
    public void KillInstantly()
    {
        Destroy(transform.parent.gameObject);
    }

    IEnumerator HitConfirm()
    {
        theSR.enabled = false;
        StartCoroutine("Flash");
        yield return new WaitForSeconds(0.1f);
        theSR.enabled = true;
    }

    IEnumerator HitConfirmAlmostDead()
    {
            theSR.enabled = false;
            StartCoroutine("FlashAlmostDead");
            yield return new WaitForSeconds(0.1f);
            theSR.enabled = true;
    }
    IEnumerator Flash()
    {
        invisibleCounter = invisibleLength;
        while (invisibleCounter > 0)
        {
            invisibleCounter -= Time.deltaTime;
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.5f);
            yield
                return new WaitForSeconds(0.05f);
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
            yield
                return new WaitForSeconds(0.05f);
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
            yield
                return new WaitForSeconds(0.05f);
            theSR.color = new Color(0.7264151f, 0.1329476f, 0.1329476f, 1f);
            yield
                return new WaitForSeconds(0.05f);
        }
        theSR.color = new Color(1f, 1f, 1f, 1.0f);
    }
}