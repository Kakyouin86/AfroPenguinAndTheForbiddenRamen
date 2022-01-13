using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    public Transform rotationCenter;
    public float rotationRadius = 2f, angularSpeed = 2f;
    public float posX, posY, angle;
    public bool moveRight;

    void Start()
    {

    }

    void Update()
    {
        if (!moveRight)
        {
            posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
            posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
            transform.position = new Vector2(posX, posY);
            angle = angle + Time.deltaTime * -angularSpeed;

            if (angle >= 360f)
            {
                angle = 0f;
            }
        }

        else
        {
            posX = rotationCenter.position.x + Mathf.Cos(angle) * rotationRadius;
            posY = rotationCenter.position.y + Mathf.Sin(angle) * rotationRadius;
            transform.position = new Vector2(posX, posY);
            angle = angle + Time.deltaTime * angularSpeed;

            if (angle >= 360f)
            {
                angle = 0f;
            }
        }

    }
}