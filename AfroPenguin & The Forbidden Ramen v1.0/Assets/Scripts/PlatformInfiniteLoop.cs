using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInfiniteLoop : MonoBehaviour
{
    public Transform platform;
    public Transform targetPosition;
    public float moveSpeed;
    public Vector2 startPos;
    public float Timer;
    void Start()
    {
        startPos = transform.position;
        platform = transform.GetChild(0);
        targetPosition = transform.GetChild(1);
        targetPosition.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer <= 0.1f)
        {
            Destroy(gameObject);
        }

        else
        {
            platform.position =
                Vector3.MoveTowards(platform.position, targetPosition.position, moveSpeed * Time.deltaTime);
            //if (Vector3.Distance(platform.position, targetPosition.position) < 0.5f)
            //{
            //    platform.GetComponent<BoxCollider2D>().enabled = false;
            //    platform.GetComponent<SpriteRenderer>().enabled = false;
            //}
        }
    }
}
