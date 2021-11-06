
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBlock : MonoBehaviour
{
    public Animator theAnimator;
    public int totalFish;
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
        if (player.collider.tag == "Player" || player.collider.tag == "Invulnerable" &&
            player.collider.bounds.max.y - 0.5f < transform.position.y
            && player.collider.bounds.min.x < transform.position.x + 1.6f
            && player.collider.bounds.max.x > transform.position.x - 1.2f && !theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Fish Block - 01 - Hit"))
        {
            if (totalFish > 0)
            {
                theAnimator.Play("Fish Block - 01 - Hit");
                StartCoroutine(ActivateTrigger());
                audioSource.Play();
                totalFish -= 1;
                if (totalFish == 0)
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
        yield return new WaitForSeconds(0.5f);
        objectToInstantiate.GetComponent<BoxCollider2D>().isTrigger = true;
    }
}
