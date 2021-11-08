using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBlock : MonoBehaviour
{
    public Animator theAnimator;
    public int totalLife;
    public AudioSource audioSource;
    public GameObject child;
    public GameObject objectToInstantiate;
    public Sprite disabled;

    void Start()
    {
        theAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        child = transform.GetChild(0).gameObject;
    }

    void Update()
    {
    }
    void OnCollisionEnter2D(Collision2D player)
    {
        if (!theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Life Block - 01 - Hit") && player.collider.bounds.max.y - 0.5f < transform.position.y
            && player.collider.bounds.min.x < transform.position.x + 1.6f
            && player.collider.bounds.max.x > transform.position.x - 1.2f
            && player.collider.tag == "Player" || player.collider.tag == "Invulnerable")
        {
            if (totalLife > 0)
                {
                    theAnimator.Play("Life Block - 01 - Hit");
                    StartCoroutine(ActivateTrigger());
                    audioSource.Play();
                    totalLife -= 1;
                    if (totalLife == 0)
                    {
                        theAnimator.SetBool("stop", true);
                        objectToInstantiate.SetActive(true);
                        child.GetComponent<SpriteRenderer>().color = Color.white;
                        child.GetComponent<SpriteRenderer>().sprite = disabled;
                    }
                }
        }
    }

    IEnumerator ActivateTrigger()
    {
        yield return new WaitForSeconds(0.2f);
        objectToInstantiate.GetComponent<CircleCollider2D>().isTrigger = true;
    }
}