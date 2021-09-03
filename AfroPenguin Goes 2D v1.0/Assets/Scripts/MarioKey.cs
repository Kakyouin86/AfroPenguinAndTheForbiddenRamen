using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private SpringJoint2D spring;
    //Add "Spring Joint 2D" component and Gravity (RB) to 0. Uncheck "Auto Configure Distance", so the objects can snap to each other.
    //Also assign the Spring Joint to our character's RB component.
    //Create under character, a GO called "backpack" slightly below the character's waist. Add RB to this. Kinematic.
    //Distance in the Spring: 0.001 or below that value.
    //Key's linear drag: 4
    //Circle collider and Is Trigger.
    //Add tag "Backpack" to the Backpack under the character.

    // Start is called before the first frame update
    void Start()
    {
        spring = GetComponent<SpringJoint2D>();
        GameObject backpack = GameObject.FindWithTag("Backpack");
        spring.connectedBody = backpack.GetComponent<Rigidbody2D>();
        spring.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !spring.enabled)
        {
            spring.enabled = true;
        }
    }

}