using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLifeBlock : MonoBehaviour
{
    public Animator theAnimator;
    public int totalStars;
    public AudioSource audioSource;
    public GameObject child;
    public GameObject starChild;
    public GameObject objectToInstantiate;
    public Sprite disabled;

    void Start()
    {
        theAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        child = transform.GetChild(0).gameObject;
        starChild = transform.GetChild(1).gameObject;
        objectToInstantiate = transform.GetChild(2).gameObject;
    }

    void Update()
    {
    }
    void OnCollisionEnter2D(Collision2D player)
    {
        if (!theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Star & Life Block - 01 - Hit Star") && player.collider.bounds.max.y - 0.5f < transform.position.y
            && player.collider.bounds.min.x < transform.position.x + 1.6f
            && player.collider.bounds.max.x > transform.position.x - 1.2f
            && player.collider.tag == "Player" || player.collider.tag == "Invulnerable")
        {
            if (totalStars > 0)
            {
                theAnimator.Play("Star & Life Block - 01 - Hit Star");
                LevelManager.instance.starsCollected++;
                UIController.instance.UpdateStarsCount();
                audioSource.Play();
                totalStars -= 1;
                if (totalStars == 1)
                {
                    starChild.GetComponent<SpriteRenderer>().enabled = false;
                    theAnimator.Play("Star & Life Block - 01 - Hit Life");
                    StartCoroutine(ActivateTrigger());
                    audioSource.Play();
                    totalStars -= 1;
                }
                if (totalStars == 0)
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
