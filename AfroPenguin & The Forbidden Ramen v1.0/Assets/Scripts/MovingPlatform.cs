using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public float moveSpeed;
    public int currentPoint;
    public Transform platform;

    void Start()
    {
        platform = transform.GetChild(0);
    }
    void Update()
    {
        platform.position = Vector3.MoveTowards(platform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(platform.position, points[currentPoint].position) < 0.5f)
        {
            currentPoint++;
            //our currentPoint becomes 1
            if (currentPoint >= points.Length)
            {
                currentPoint = 0;
            }
        }
     
    }
}