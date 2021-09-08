using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Transform target;
    public Transform farBackground, middleBackground;
    private Vector2 lastPos;
    public float minHeight = -1.5f, maxHeight = 2.5f;
    public bool stopFollow;
    public float minX = 0, maxX = 100;

    //[Range(1, 10)]
    //public float smoothFactor;
    //public Vector3 offset;
    //Vector3 targetPosition = target.position + offset;
    //Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothFactor*Time.fixedDeltaTime);

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        lastPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (stopFollow == false)
        {
            transform.position = new Vector3(Mathf.Clamp(target.position.x, minX, maxX), Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);
            Vector2 amountToMove = new Vector3(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

            farBackground.position = farBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0f);
            middleBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f;

            lastPos = transform.position;
        }
    }
}
