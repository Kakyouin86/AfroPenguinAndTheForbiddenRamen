using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerPressStart : MonoBehaviour
{//Few values I worked with (backup): Mass: 1 | Linear Drag: 0.6 | Angular: 0.05 | Gravity: 5 | Dynamic & Continuous | MoveSpeed: 10 | Jump Speed: 8 | Max Speed: 7 | Linear Drag: 4 | Gravity: 1 | Fall Multi.: 5 | Ground: 0.6
 //Also, for this to work, we have to set the objects such as this: Player, then child: Character Holder, then child: Chaarcter Nnimation.
 //The "Player" has the Rigidbody, this script, and an Edge Collider making it look like and M (below, the whole line is drawn).
 //Also Animation has to have a "horizonal" and "vertical" float.

    [Header("Horizontal Movement")]
    public float moveSpeed = 10f;
    public Vector2 direction;
    private bool facingRight = true;

    [Header("Vertical Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    private float jumpTimer; // How long since we pressed the Jump button

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask groundLayer; //We create a Ground Layer in the Layer part and assign Ground to every single part of Ground we have in our scenario.
    public GameObject characterHolder;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1f;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false; //We need to check if the Raycast is on the ground or off the ground.
    public float groundLength = 0.6f;
    public Vector3 colliderOffset; //In case the red line in the middle is off but the other leg is on the ground. If we don't have this, we can't jump as he is not on the ground.

    // Update is called once per frame
    void Update()
    {
        bool wasOnGround = onGround;
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);
        //First part is the origin point, directly in the center of the character | direction of the line (down) | lengh of that line | the layer mark (the ground
        //Each leg, each part of the "OR".
        if (!wasOnGround && onGround) //If we are not on the ground, we just landed on the ground.
        {
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }
        animator.SetBool("onGround", onGround);
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    void FixedUpdate()
    {
        moveCharacter(direction.x);
        if (jumpTimer > Time.time && onGround) // Then we are in the jump delay period
        {
            Jump();
        }

        modifyPhysics();
    }
    void moveCharacter(float horizontal)
    {
        rb.AddForce(Vector2.right * horizontal * moveSpeed);

        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("vertical", rb.velocity.y);
    }
    void Jump()
    {
        //First we reset the vertical velocity. It stops all vertical movement.
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse); //First paramether: where we apply the force (up). The second, the RB we want this force to be applied.
        jumpTimer = 0; //We prevent the player from jumping more than once.
        StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    }
    void modifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (onGround) //Only if he's touching the ground.
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0; //No gravity when the player is touching the ground.
        }
        else
        {
            rb.gravityScale = gravity; //We apply gravity, the minute the player is off the ground
            rb.drag = linearDrag * 0.15f; //On experience, 50% of the drag value.
            if (rb.velocity.y < 0) //The first thing we want to check is the downward velocity so we can apply our fall multiplier
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) //If we are moving upwards but we have released our jump button, we want to limit out jump to a smaller height.
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }
    private void OnDrawGizmos()
    {
        //Gizmos: starting point and ending points . It takes the ground point and takes it into the Y axis.
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
        //Each line is a leg, so go ahead and move the X values so each line is on one leg.
    }
}