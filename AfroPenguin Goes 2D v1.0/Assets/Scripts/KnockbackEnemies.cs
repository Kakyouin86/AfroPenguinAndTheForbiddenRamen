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
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        theSR = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (isKnockback)
        {
            KnockBack();
            Debug.Log("True");
        }
        else
        {
            isKnockback = false;
            Debug.Log("False");
        }
    }

    #region KnockBack
    public void KnockBack()
    {
        StartCoroutine(KnockBackDelay());
        knockbackCounter = knockbackLenght;
        theRB.velocity = new Vector2(0f, knockbackForce);
        knockbackCounter -= Time.deltaTime;
        //this counts down my time.
 
            if (PlayerController.instance.theRB.velocity.x >= 0)
            { 
                theRB.velocity = new Vector2(-knockbackForce, theRB.velocity.y);
            }
            else
            {
            theRB.velocity = new Vector2(knockbackForce, theRB.velocity.y);
            }
    }
    IEnumerator KnockBackDelay()
    {
        isKnockback = true;
        yield return new WaitForSeconds(knockbackCounter);
        isKnockback = false;
    }
    #endregion
}
