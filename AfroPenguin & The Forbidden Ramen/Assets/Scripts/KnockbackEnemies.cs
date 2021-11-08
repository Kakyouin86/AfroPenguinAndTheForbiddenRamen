using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackEnemies : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;
    public Animator anim;

    [Header("Knockback")]
    public float knockbackLenght = 0.25f;
    public float knockbackForce = 5.0f;
    public float knockbackCounter;
    public bool isKnockback;

    void Start()
    {
        isKnockback = false;
        theRB = GetComponentInParent<Rigidbody2D>();
        anim = GetComponentInParent<Animator>();
        theSR = GetComponentInParent<SpriteRenderer>();
    }

    void Update()
    {
    }

    #region KnockBack
    public void KnockBack()
    {
        StartCoroutine(KnockBackDelay());
        knockbackCounter = knockbackLenght;
        theRB.velocity = new Vector2(knockbackForce, knockbackForce);
        knockbackCounter -= Time.deltaTime;
    }
    IEnumerator KnockBackDelay()
    {
        isKnockback = true;
        yield return new WaitForSeconds(knockbackCounter);
        isKnockback = false;
    }
    #endregion
}
