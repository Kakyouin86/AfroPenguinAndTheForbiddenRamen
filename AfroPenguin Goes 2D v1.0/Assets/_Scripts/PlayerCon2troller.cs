using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Tilemaps;

public class PlayerCon2troller : MonoBehaviour{
    public ParticleSystem dust;

    [Header("Movement")]
    public float horizontalVelocity = 10f;
    public float maxWalk = 3f;
    public float maxSpeed = 7f;
    public float linearDrag = 7f;
    private Vector2 direction;

    [Header("Jumping")]
    public float jumpVelocity = 8f;
    public float fallMultiplier = 5f;
    public float gravity = 1f;
    public float maxGravity = 10f;
    public float wallJumpLerp = 10f;
    private float jumpPressedDown = -1f;
    private bool wallJumped = false;

    [Header("UI")]
    public TextMeshProUGUI controllerText;

    [Header("Layers")]
    public LayerMask groundLayer;

    public bool onGround;
    public bool canJump;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;
    private float runTrigger;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 groundOffset, rightOffset, leftOffset;
    private Color debugCollisionColor = Color.red;

    [Space]

    [Header("Animation")]
    public GameObject characterSprite;
    public GameObject trail;

    //PLAYER COMPONENTS
    private Rigidbody2D rb;
    private Animator animator;
    private bool facingRight = true;

    // Start is called before the first frame update
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        animator = characterSprite.GetComponentInChildren<Animator>();
    }

    void Update(){
        bool wasOnGround = onGround;
        //collison detection
        onGround = Physics2D.Raycast(new Vector2(transform.position.x + groundOffset.x, transform.position.y - groundOffset.y + 0.1f), Vector2.down, 0.1f, groundLayer) || Physics2D.Raycast(new Vector2(transform.position.x - groundOffset.x, transform.position.y - groundOffset.y + 0.1f), Vector2.down, 0.1f, groundLayer);
        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onWall = onRightWall || onLeftWall;
        wallSide = onRightWall ? -1 : 1;

        //landed
        if(!wasOnGround && onGround){
            StartCoroutine(JumpSqueeze(1.25f, 0.8f, 0.05f));
        }
        if (onGround || !onWall){
            wallJumped = false;
        }

        if(Input.GetButtonDown("Jump")){
            jumpPressedDown = Time.time + 0.25f;
        }
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        runTrigger = Input.GetAxisRaw("WallHold");

        animator.SetBool("onGround", onGround);
    }
    void FixedUpdate(){
        //MOVEMENT
        moveCharacter(direction.x);
        bool jumpTrigger = Time.time < jumpPressedDown;

        //JUMPING
        if (jumpTrigger && onGround) {
            Jump(Vector2.up * jumpVelocity);
        }else if (jumpTrigger && !wallJumped && ((direction.x > 0.5f && onLeftWall) || (direction.x < -0.5f && onRightWall))) { //wallJump
            Jump(new Vector2(maxSpeed * 0.75f * wallSide, jumpVelocity * 0.75f));
            wallJumped = true;
        }
        //GRAVITY & WALL GRABS
        if (onWall){
            rb.gravityScale = gravity * 0.5f;
        } else if (onGround) {
            rb.gravityScale = 0f;
        } else {
            rb.gravityScale = gravity;
        }
        //CONTROLLING VERTICAL VELOCITY
        if (rb.velocity.y < 0){
            rb.gravityScale = gravity * fallMultiplier;
        }else if(rb.velocity.y > 0 && !Input.GetButton("Jump")){
            rb.gravityScale = gravity * fallMultiplier * 0.5f;
        }
        //prevent falling too fast
        if (rb.velocity.y < -maxGravity){
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * maxGravity);
        }

        animator.SetFloat("horizontal", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("vertical", rb.velocity.y);
        animator.SetBool("moving", Mathf.Abs(direction.x) > 0.5f);
        controllerText.SetText("X:" + rb.velocity.x + "\nY: " + rb.velocity.y);
    }
    void moveCharacter(float horizontal){
        bool runButton = runTrigger > 0;
        //control speed
        if ((runButton && horizontal * rb.velocity.x < maxSpeed) || (!runButton && horizontal * rb.velocity.x < maxWalk)){
            rb.AddForce(Vector2.right * horizontal * horizontalVelocity);
        }
        if (runButton && Mathf.Abs(rb.velocity.x) > maxSpeed){
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }
        if(Mathf.Abs(rb.velocity.x) > maxWalk){
            trail.SetActive(true);
        }else{
            trail.SetActive(false);
        }
        bool changingDirections = (horizontal > 0 && rb.velocity.x < 0) || (horizontal < 0 && rb.velocity.x > 0);

        //calculate drag
        if (!onGround){
            if (onWall && Mathf.RoundToInt(horizontal) == wallSide * -1) {
                rb.drag = linearDrag;
            }else{
                rb.drag = linearDrag * 0.15f;
            }
        } else{

            if(Mathf.Abs(horizontal) < 0.4f && !changingDirections){
                rb.drag = linearDrag;
            }else{
                rb.drag = linearDrag * 0.01f;
            }
            if (changingDirections){
                rb.AddForce(Vector2.right * horizontal * (horizontalVelocity * 0.75f));
            }
        }
        if(onWall){
            if((onRightWall && facingRight) || (onLeftWall && !facingRight)){
                Flip(!facingRight);
            }
        }else if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight)){
            Flip(!facingRight);
        }
    }
    void Jump(Vector2 force){
        CreateDust();
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(force, ForceMode2D.Impulse);
        jumpPressedDown = -1f;
        StartCoroutine(JumpSqueeze(0.5f, 1.2f, 0.1f));
    }
    IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float seconds){
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);
        float t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterSprite.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1.0) {
            t += Time.deltaTime / seconds;
            characterSprite.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }

    }
    void Flip(bool right){
        CreateDust();
        facingRight = right;
        transform.rotation = Quaternion.Euler(0, right ? 0 : 180, 0);
    }
    void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(new Vector3(transform.position.x + groundOffset.x, transform.position.y - groundOffset.y + 0.1f, 0), new Vector2(transform.position.x + groundOffset.x, transform.position.y - groundOffset.y));
        Gizmos.DrawLine(new Vector3(transform.position.x - groundOffset.x, transform.position.y - groundOffset.y + 0.1f, 0), new Vector2(transform.position.x - groundOffset.x, transform.position.y - groundOffset.y));
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);

    }
    void CreateDust(){
        dust.Play();
    }
}
