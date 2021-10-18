using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyHP : MonoBehaviour
{
    public Animator theAnim;
    public SpriteRenderer theSR;
    public Collider2D parentCol;
    public Collider2D hurtboxCol;
    public int enemyHP;
    public int currentHP;
    public bool flashing;
    public bool isDead;
    public float invisibleLength = 0.03f;
    public float invisibleCounter;

    void Start()
    {
        currentHP = enemyHP;
        //theAnim = transform.parent.GetComponent<Animator>();
        parentCol = transform.parent.GetComponent<Collider2D>();
        hurtboxCol = GetComponent<Collider2D>();
        theSR = transform.parent.GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP > 0)
        {
            StartCoroutine("HitConfirm");
        }
        else
        {
            isDead = true;
            parentCol.enabled = false;
            hurtboxCol.enabled = false;
            StartCoroutine("KillSwitch");
        }
    }

    IEnumerator KillSwitch()
    {
        yield return new WaitForSeconds(0f);
        Destroy(transform.parent.gameObject);
    }

    IEnumerator HitConfirm()
    {
            theSR.enabled = false;
            StartCoroutine("Flash");
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
            theSR.color = new Color(0.7264151f, 0.1329476f, 0.1329476f, 1f);
            yield
                return new WaitForSeconds(0.05f);
        }
        theSR.color = new Color(1f, 1f, 1f, 1.0f);
    }
}