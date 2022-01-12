using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelEnemyControllerLaunch : MonoBehaviour
{
    public float t;
    public Vector3 startPosition;
    public float timeToReachTarget;
    public Animator theAnimator;
    public float launchSpeed = 3f;
    public GameObject squirrel;
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;
        startPosition = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Invulnerable")
        {
            theAnimator.SetBool("isRunning",true);
            StartCoroutine(MoveABit());
            StartCoroutine(MoveToPosition(transform,launchSpeed));
        }
    }

    IEnumerator MoveABit()
    {
        yield return new WaitForSeconds(0.1f);
        theAnimator.SetTrigger("isLaunching");
    }


    IEnumerator MoveToPosition(Transform transform, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            squirrel.transform.position = Vector3.Lerp(currentPos, FindObjectOfType<PlayerController>().transform.position, t);
            yield return null;
        }
    }

}
