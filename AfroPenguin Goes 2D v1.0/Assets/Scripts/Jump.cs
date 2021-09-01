using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    public float jumpForce = 15.0f;
    public bool isGrounded;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // If we have collided with the platform
        if (other.gameObject.tag == "Ground")
        {
            // Then we must be on the ground
            isGrounded = true;
        }
    }

    void Update()
    {
        // If we press space and we are on the ground
        if (Input.GetKeyDown(KeyCode.Q) && isGrounded)
        {
            // Add some force to our Rigidbody
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);

            // We have jumped, we are no longer on the ground
            isGrounded = false;
        }
    }
}
