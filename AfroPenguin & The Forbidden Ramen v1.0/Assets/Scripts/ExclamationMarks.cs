using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExclamationMarks : MonoBehaviour
{
    public Animator theAnimator;

    // Start is called before the first frame update
    void Start()
    {
        theAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (PlayerController.instance.canMove)
        {
            if (player.CompareTag("Player") || player.CompareTag("Invulnerable"))
            {
                theAnimator.Play("Exclamation Mark - 01 - Opening Action");
                theAnimator.SetBool("isOpened", true);
                GetComponent<AudioSource>().Play();
            }
        }
    }

    public void OnTriggerExit2D(Collider2D player)
    {
        if (PlayerController.instance.canMove)
        {
            if (player.CompareTag("Player") || player.CompareTag("Invulnerable"))
            {
                theAnimator.Play("Exclamation Mark - 01 - Closing Action");
                theAnimator.SetBool("isOpened", false);
            }
        }
    }
}
