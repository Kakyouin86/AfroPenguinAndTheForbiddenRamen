using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCaveControllerSafeguard : MonoBehaviour
{
    public SpriteRenderer theSR;
    public bool shouldFadeToBlack, shouldFadeFromBlack;
    public float fadeSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
        shouldFadeFromBlack = true;
        shouldFadeToBlack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldFadeFromBlack)
        {
            theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, Mathf.MoveTowards(theSR.color.a, 0.0f, fadeSpeed * Time.deltaTime));
            if (theSR.color.a == 0.0f)
            {
                shouldFadeFromBlack = false;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player") || player.CompareTag("Invulnerable"))
        {
            shouldFadeFromBlack = true;
        }
    }
}
