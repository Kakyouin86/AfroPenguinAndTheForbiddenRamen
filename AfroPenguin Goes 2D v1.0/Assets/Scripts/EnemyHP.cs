using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyHP : MonoBehaviour
{
    public int enemyHP;
    private int currentHP;
    private Animator theAnim;
    public bool isDead;
    private Collider2D parentCol;
    private Collider2D hurtboxCol;
    private SpriteRenderer spriteRend;
    void Start()
    {
        currentHP = enemyHP;
        theAnim = transform.parent.GetComponent<Animator>();
        parentCol = transform.parent.GetComponent<Collider2D>();
        hurtboxCol = GetComponent<Collider2D>();
        spriteRend = transform.parent.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (currentHP <= 0)
        {
            isDead = true;
            theAnim.SetBool("Dead", isDead);
            parentCol.enabled = false;
            hurtboxCol.enabled = false;
            StartCoroutine("KillSwitch");
        }
    }
    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        StartCoroutine("HitConfirm");
    }
    IEnumerator KillSwitch()
    {
        yield return new WaitForSeconds(2f);
        Destroy(transform.parent.gameObject);
    }
    IEnumerator HitConfirm()
    {
        if (currentHP > 0)
        {
            spriteRend.enabled = false;
            yield return new WaitForSeconds(0.1f);
            spriteRend.enabled = true;
        }
    }
}