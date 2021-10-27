using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioStyleCamera : MonoBehaviour
{

    [Header("Mario Style Camera")]
    public Transform camTarget;
    public float aheadAmount = 1.5f;
    public float aheadSpeed = 1f;

    public void Update()
    {
        //Camera Trick: Set a target as a child of the player so it moves either left or right when switching positions
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, aheadAmount * Input.GetAxisRaw("Horizontal"), aheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
        }
    }
}
