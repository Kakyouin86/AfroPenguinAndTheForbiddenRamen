using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public Animator anim;

    public float bounceForce = 20f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerController.instance.theRB.velocity = new Vector2(PlayerController.instance.theRB.velocity.x, bounceForce);
            anim.SetTrigger("Bounce");
        }
    }
}
