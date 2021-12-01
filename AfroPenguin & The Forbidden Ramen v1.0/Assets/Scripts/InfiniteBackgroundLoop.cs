using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteBackgroundLoop : MonoBehaviour
{
    public float speed;

    public Vector2 startPosition;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
        if (transform.position.y <= -120)
        {
            transform.position = startPosition;
        }
    }
}
