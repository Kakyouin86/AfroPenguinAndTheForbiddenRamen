using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJumping : MonoBehaviour
{
    public Rigidbody2D theRB;
    public float fallMultiplier = 3f;
    public float lowJumpMultiplier = 8f;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(theRB.velocity.y < 0)
        {
            theRB.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(theRB.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            theRB.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
