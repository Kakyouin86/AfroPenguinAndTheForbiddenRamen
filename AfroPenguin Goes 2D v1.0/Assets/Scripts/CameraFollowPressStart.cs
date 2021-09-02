using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Basics: main camera to ortographic.
    public GameObject followObject; //Our character.
    public Vector2 followOffset; //The amount of space we want our character to move freely |<----°---->| before our camera starts moving.
    public float speed = 3f; //How fast we want our camera to move.
    private Vector2 threshold; //Our boundary box (inside the screen, we have a "box" in which we have our character. The smaller our x and y values for our follow offset, the larger our boundary box becomes.
    private Rigidbody2D rb; //Our character's RB, because if the character moves faster, then we use this to know how fast.

    // Start is called before the first frame update
    void Start()
    {
        threshold = calculateThreshold(); //Define the threshold
        rb = followObject.GetComponent<Rigidbody2D>(); //RB explanation
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 follow = followObject.transform.position; //Calculate the distance our object (character) is from the center of our camera. We create the X below.
        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x); //Vector2.Distance measures distance from point A to B. We want it for the X. Vector2.right (value 1,0,0) and put a cero for the Y and Z values.
        float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

        Vector3 newPosition = transform.position; //By default it will be where our camera currently is.
        if (Mathf.Abs(xDifference) >= threshold.x) //Always positive number-->ABS: absolute.
        {
            newPosition.x = follow.x; //If this is true, we want our camera to follow on the direction of our follow object.
        }
        if (Mathf.Abs(yDifference) >= threshold.y)
        {
            newPosition.y = follow.y;
        }
        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed; //The default speed is used as backup in case we move slower or faster. So we want the camer to continue moving even if we stopped.
        //? means the true value which is our RB velocity.magnitude and the : which is speed. RB.Vel.Magn is the highest velocity value. Regardless the direction of the character, it will display the maximum speed.
        //If it's slower, we use our "speed" value.
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime); //Incrementally move the camera towards a position.
    }
    private Vector3 calculateThreshold() //We use this because we want to return a Vector3 value (we want to create a threashold which defines our boundary box).
        //The smaller our x and y values for our follow offset, the larger our boundary box becomes.
    {
        Rect aspect = Camera.main.pixelRect; // aspect ratio of our camera
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize); 
        //Dimensions of the camera in a Vector2 "t".
        //The second part (y) we use the ortographic size of the camera to figure out the width and height of our screen boundary box.
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
        //t: we are calculating the threshold
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1)); //First: we set the camera's position | Our border value | Z parameter = 1 (last)
    }
}