using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class JumpyItem : MonoBehaviour
{
    public float jumpSpeed = 15;
    public Rigidbody2D theRB;
    public Transform target;
    public Vector2 origin;
    public bool returnToOrigin;

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        origin = transform.position;
        target.parent = null;
        returnToOrigin = false;
        theRB.velocity = new Vector2(0, jumpSpeed);
    }

    void Update()
    {
        if (transform.position.y >= target.position.y)
        {
            returnToOrigin = true;
        }

        if (returnToOrigin)
        {
            theRB.transform.position = Vector2.MoveTowards(transform.position, PlayerController.instance.theRB.position, 100f * Time.deltaTime);
        }
    }
}
