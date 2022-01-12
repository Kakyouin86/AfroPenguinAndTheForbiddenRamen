using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelEnemyControllerLaunch : MonoBehaviour
{
    public float t;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    public float timeToReachTarget;
    public Animator theAnimator;
    public float launchSpeed = 3f;
    public GameObject squirrel;
    public GameObject effectToInsantiate;
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
            StartCoroutine(MoveToPosition(transform,launchSpeed));
            targetPosition = FindObjectOfType<PlayerController>().transform.position;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }


    IEnumerator MoveToPosition(Transform transform, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            squirrel.transform.position = Vector3.Lerp(currentPos, targetPosition, t);
            yield return null;

        }
        gameObject.SetActive(false);
        Instantiate(effectToInsantiate, new Vector2(squirrel.transform.position.x, squirrel.transform.position.y + 2f), squirrel.transform.rotation);
        squirrel.SetActive(false);
    }

}
