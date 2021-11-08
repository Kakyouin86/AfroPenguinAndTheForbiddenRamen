using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMegaMan : MonoBehaviour
{
    public Transform player;
    public float timeOffset;
    public Vector3 offsetPos;
    public Vector3 boundsMin;
    public Vector3 boundsMax;
    public Vector2 lastPos;
    public Transform farBackground, middleBackground;


    void Start()
    {
        //lastXPos = transform.position.x;
        lastPos = transform.position;
    }
    void Update()
    {
        Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);
        farBackground.position = farBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0f);
        middleBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f;
        lastPos = transform.position;
    }

    private void LateUpdate()
    {
        if (player != null)
        {
            Vector3 startPos = transform.position;
            Vector3 targetPos = player.position;

            targetPos.x += offsetPos.x;
            targetPos.y += offsetPos.y;
            targetPos.z = transform.position.z;

            targetPos.x = Mathf.Clamp(targetPos.x, boundsMin.x, boundsMax.x);
            targetPos.y = Mathf.Clamp(targetPos.y, boundsMin.y, boundsMax.y);

            float t = 1f - Mathf.Pow(1f - timeOffset, Time.deltaTime * 30);
            transform.position = Vector3.Lerp(startPos, targetPos, t);
        }
    }
}
