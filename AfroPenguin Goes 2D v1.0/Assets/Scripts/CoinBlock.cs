using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBlock : MonoBehaviour 
{
	public Animator theAnimator;
    public int totalStars;
    public AudioSource audioSource;
    public GameObject child;
    public Sprite disabled;

    void Start () 
    {
        theAnimator = GetComponent<Animator>();
        this.audioSource = GetComponent<AudioSource>();
        child = transform.GetChild(0).gameObject;
    }

    void Update () 
    {
		
	}
	void OnCollisionEnter2D(Collision2D player) 
    {
        if (player.collider.tag == "Player" && 
            player.collider.bounds.max.y -0.05f < transform.position.y
            && player.collider.bounds.min.x < transform.position.x + 1.6f
            && player.collider.bounds.max.x > transform.position.x - 1.2f && !theAnimator.GetCurrentAnimatorStateInfo(0).IsName("Star Block - 01 - Hit")) 
            {
                if (totalStars > 0) 
                {
                    theAnimator.Play("Star Block - 01 - Hit");
                    LevelManager.instance.starsCollected++;
                    UIController.instance.UpdateStarsCount();
				    //audioSource.Play();
                    totalStars -= 1;
					if (totalStars == 0) 
                    {
                        theAnimator.SetBool("stop",true);
                        child.GetComponent<SpriteRenderer>().color = Color.white;
                        child.GetComponent<SpriteRenderer>().sprite = disabled;
                    }
				}
            }
    }
}
