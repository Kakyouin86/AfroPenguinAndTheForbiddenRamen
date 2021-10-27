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
        theRB.velocity = new Vector2(0f, knockbackForce);
        knockbackCounter -= Time.deltaTime;
        //this counts down my time.
 
            if (theRB.velocity.x > 0 && PlayerController.instance.theRB.velocity.x > 0)
            { 
                theRB.velocity = new Vector2(knockbackForce, theRB.velocity.y);
                Debug.Log(PlayerController.instance.theRB.velocity.x);
                Debug.Log(theRB.velocity.x);
                Debug.Log("1");
            }
            else if (theRB.velocity.x > 0 && PlayerController.instance.theRB.velocity.x < 0)
            {
                theRB.velocity = new Vector2(-knockbackForce, theRB.velocity.y);
                Debug.Log(PlayerController.instance.theRB.velocity.x);
                Debug.Log(theRB.velocity.x);
                Debug.Log("2");
        }
            else if (theRB.velocity.x < 0 && PlayerController.instance.theRB.velocity.x > 0)
            {
                theRB.velocity = new Vector2(knockbackForce, theRB.velocity.y);
                Debug.Log(PlayerController.instance.theRB.velocity.x);
                Debug.Log(theRB.velocity.x);
                Debug.Log("3");
        }
            else if (theRB.velocity.x < 0 && PlayerController.instance.theRB.velocity.x < 0)
            {
                theRB.velocity = new Vector2(-knockbackForce, theRB.velocity.y);
                Debug.Log(PlayerController.instance.theRB.velocity.x);
                Debug.Log(theRB.velocity.x);
                Debug.Log("4");
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
