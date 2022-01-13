using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SineMovement : MonoBehaviour
{
    public Vector2 movePos;
    public Vector2 startPos;
    public float moveFreq = 1f;
    public float moveDis = 1f;
    public bool moveHori;
    void Start()
    {
        startPos = transform.position;
    }
    void Update()
    {
        if (moveHori)
        {
            movePos.x = startPos.x + Mathf.Sin(Time.time * moveFreq) * moveDis;
            transform.position = new Vector2(movePos.x, transform.position.y);
        }
        else
        {
            movePos.y = startPos.y + Mathf.Sin(Time.time * moveFreq) * moveDis;
            transform.position = new Vector2(transform.position.x, movePos.y);
        }
    }
}