using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LookAtPlayer : MonoBehaviour
{
    public Transform player;
    [SerializeField]
    public float rotSpeed = 5f;
    void Start()
    {
        player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
    }
    void Update()
    {
        //Difference between the player's position minus this eye's gameObject position
        Vector2 direction = new Vector3 (player.position.x, player.position.y+2) - transform.position;
        //Atan2 returns the angle in the form of radians relative to the value in here (x and y). The other one converts into degrees that radian.
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        //Quaternion = rotation | Angle Axis: the axis that the object is going to rotate along: angle and axis. Vector 3 uses as well forward and back.
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
        //Slerp: Calculate spherically between the rotations and quaternions using the values we have set.
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotSpeed * Time.deltaTime);
    }
}