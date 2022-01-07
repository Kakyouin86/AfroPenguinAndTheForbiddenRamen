using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOffsetOfCameraRevert : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        FindObjectOfType<CameraFollowMegaMan>().timeOffset = 0.25f;
    }
}