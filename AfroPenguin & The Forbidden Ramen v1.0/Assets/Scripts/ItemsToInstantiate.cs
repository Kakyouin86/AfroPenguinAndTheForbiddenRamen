using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsToInstantiate : MonoBehaviour
{
    public GameObject[] items;
    public Animator theAnimator;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        theAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player") || player.CompareTag("Invulnerable"))
        {
            theAnimator.Play("Chest - 01 - Opening");
            theAnimator.SetBool("stop", true);
            GetComponent<Collider2D>().enabled = false;
            StartCoroutine(InstantiateItems());
            StartCoroutine(SmallWait());
        }
    }

    IEnumerator InstantiateItems()
    {
        yield return new WaitForSeconds(0.9f);
        for (int i = 0; i < items.Length; i++)
        {
            items[i].SetActive(true);
        }
    }

    IEnumerator SmallWait()
    {
        yield return new WaitForSeconds(0.7f);
        GetComponent<AudioSource>().Play();
    }

}
