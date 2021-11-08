using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    public Animator theAnimator;
    public AudioSource audioSource;
    public PlayerController thePC;

    void Start()
    {
        theAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        thePC = FindObjectOfType<PlayerController>();
    }

    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D player)
    {
        if (player.collider.tag == "Invulnerable")
        {
            StartCoroutine(Destroy());
            theAnimator.Play("Breakable Block - 01 - Hit");
            theAnimator.SetBool("stop", true);
            audioSource.Play();
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}