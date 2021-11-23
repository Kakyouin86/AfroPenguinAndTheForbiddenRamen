using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCaveController : MonoBehaviour
{
    public SpriteRenderer theSR;
    public bool shouldFadeToBlack;
    public float fadeSpeed = 3.0f;

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

    public void OnTriggerExit2D(Collider2D player)
    {
        if (player.CompareTag("Player") || player.CompareTag("Invulnerable"))
        {
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 0.0f);
            theSR.enabled = false;
        }
    }
}
