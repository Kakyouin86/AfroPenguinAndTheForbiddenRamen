using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlatform : MonoBehaviour
{
    public Animator theAnimator;
    public AudioSource audioSource;
    public float disappearTime = 1f;
    public bool canReset;
    public float resetTime = 6f;
    public ParticleSystem dustParticle;

    void Start()
    {
        theAnimator = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        theAnimator.SetFloat("DisappearTime", 1 / disappearTime);
        dustParticle = GetComponentInChildren<ParticleSystem>();
        canReset = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Disappearing Platform - 01 - Disappear"))
        {
            theAnimator.SetBool("Trigger", true);
        }
    }

    void OnCollisionEnter2D(Collision2D player)
    {
        if (player.collider.tag == "Player" || player.collider.tag == "Invulnerable")
        {
            theAnimator.Play("Disappearing Platform - 01 - Disappear");
        }
    }

    public void TriggerReset()
    {
        if (canReset)
        {
            dustParticle.Play();
            StartCoroutine(Reset());
        }
    }

    public void Sound()
    {
        if (canReset)
        {
            audioSource.Play();
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetTime);
        theAnimator.SetBool("Trigger", false);
    }
}