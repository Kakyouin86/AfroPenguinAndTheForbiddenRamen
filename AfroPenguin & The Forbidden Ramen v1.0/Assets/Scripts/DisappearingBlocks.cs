using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DisappearingBlocks : MonoBehaviour
{
    public bool isHidden;
    public float timer;
    public float timeRemaining = 2f;
    public AudioSource audioSource;
    public BoxCollider2D theBC;
    public SpriteRenderer theSR;
    // Start is called before the first frame update
    void Start()
    {
        timer = timeRemaining;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0.1f & !isHidden)
        {
            theBC.enabled = false;
            theSR.enabled = false;
            timeRemaining = timer;
            isHidden = !isHidden;
            audioSource.Play();
        }

        if (timeRemaining <= 0.1f & isHidden)
        {
            theBC.enabled = true;
            theSR.enabled = true;
            timeRemaining = timer;
            isHidden = false;
            audioSource.Play();
        }
    }
}
