using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkCaveControllerDisabler : MonoBehaviour
{
    public SpriteRenderer theSR;
    public bool shouldFadeFromBlack;
    public float fadeSpeed = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        shouldFadeFromBlack = false;
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
            theSR.enabled = true;
            shouldFadeFromBlack = true;
            FindObjectOfType<DarkCaveController>().GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
