using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DarkCaveController : MonoBehaviour
{
    public SpriteRenderer theSR;
    public bool shouldFadeToBlack;
    public float fadeSpeed = 0.5;

    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
        shouldFadeToBlack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeToBlack)
        {
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, Mathf.MoveTowards(theSR.color.a, 0.5f, fadeSpeed * Time.deltaTime));
            if (theSR.color.a == 0.5f)
            {
                shouldFadeToBlack = false;
                GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player") || player.CompareTag("Invulnerable"))
        {
            theSR.enabled = true;
            shouldFadeToBlack = true;
        }
    }
}
