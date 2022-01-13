using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelEnemyController : MonoBehaviour
{
    [Header("Components")]
    public Transform player;
    public SpriteRenderer theSR;
    public bool facingRight;
    public GameObject squirrelChild;
    public Animator theAnimator;
    public AudioSource audioSource;

    void Start()
    {
        theSR = GetComponentInChildren<SpriteRenderer>();
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        theAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        facingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < player.transform.position.x && !facingRight)
        {
            WhereToLook();
        }
        else if (transform.position.x > player.transform.position.x && facingRight)
        {
            WhereToLook();
        }

        if (theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Squirrel - Launch"))
        {
            squirrelChild.tag = "Enemy";
        }
    }

    public void WhereToLook()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0);
    }
    public void soundSFX()
    {
        audioSource.Play();
    }
}
