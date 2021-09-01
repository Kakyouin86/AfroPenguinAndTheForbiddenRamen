using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerPad : MonoBehaviour
{
    private Animator anim;
    public float bounceForce = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerController.instance.GetComponent<Rigidbody2D>().velocity = new Vector2(PlayerController.instance.GetComponent<Rigidbody2D>().velocity.x, bounceForce);
            anim.SetTrigger("Bounce");
        }

    }
}
