using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public GameObject startButton;
    public Animator theAnimator;

    // Start is called before the first frame update
    void Start()
    {
        theAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startButton.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime <= 1 && startButton.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Menu Animations - Pressed - 03"))
        {
            theAnimator.Play("Background - Animation - 02");
            StartCoroutine(ActivatePause());
        }
    }

    IEnumerator ActivatePause()
    {
        yield return new WaitForSeconds(1f);
    }
}
