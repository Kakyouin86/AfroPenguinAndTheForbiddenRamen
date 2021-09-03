using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //Door with Box Collider, and trigger.
    //Create a second door so the character teleports to the next one.
    public GameObject connectedDoor;
    public bool teleported = false;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (teleported && Input.GetAxisRaw("Vertical") < 1) //After we teletransported, we let go of the "up" direction.
        {
            teleported = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetAxisRaw("Vertical") == 1) //1 is UP, -1 is DOWN.
        {
            if (Input.GetAxisRaw("Vertical") == 1 && !teleported) //Only teleports when the value is "false".
            {
                player.transform.position = connectedDoor.transform.position;
                connectedDoor.GetComponent<Door>().teleported = true; //This takes our connected door reference, finds the script attached to it, and sets
                //the value to true.
            }
        }
    }
}