using System.Collections;
using System.Globalization;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public static PlayerController instance;
    public bool stopInput;

    [Header("Ground Check")] 
    public bool isGrounded;
    public bool cameFromTheGround;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;
    public Vector2 bottomOffset;

    [Header("Movement")] 
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;
    public bool facingRight;
    public float moveSpeed = 4f;
    public float moveSpeedModifier = 1.5f;

    [Header("Dash")] 
    public bool hasDashed;
    public bool isDashing;
    public float dashSpeed = 30f;
    public float dashTime = 0.1f;
    public float dashTimeInAir = 0.1f;

    [Header("Knockback")]
    public float knockbackLenght = 0.25f;
    public float knockbackForce = 5.0f;
    public float knockbackCounter;
    public bool isKnockback;

    [Header("Jump")]
    public float jumpForce = 15.0f;
    public int availableJumps;
    public int totalJumps = 2;
    public bool multipleJumps;
    public bool coyoteJump;

    [Header("Bounce After Jump")]
    public float bounceForce = 15.0f;

    [Header("Mario Style Camera")]
    public Transform camTarget;
    public float aheadAmount = 1.5f;
    public float aheadSpeed = 1f;
    #endregion

    [Header("Particle Systems")]
    public ParticleSystem dustParticle;
    public ParticleSystem jumpDustParticle;
    public ParticleSystem dashDustParticle;

    #region Awake
    private void Awake()
    {
        instance = this;
    }
    #endregion

    #region Start
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        theSR = GetComponent<SpriteRenderer>();
    }
    #endregion

    #region Update
    void Update()
    {
        if (!PauseMenu.instance.isPaused && !stopInput)
            //if the pause is false (we are playing) AND, we haven't stopped our input, then... move the player. If one of them true, we can't do ANY of this stuff. If both are true, then it won't move.
        {
            if (CanMoveOrInteract() == false) 
                return;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        float xRaw = Input.GetAxisRaw("Horizontal");
        float yRaw = Input.GetAxisRaw("Vertical");
        //Vector2 direction = new Vector2(x, y);
        Vector2 direction = new Vector2(xRaw, yRaw);

        Walk(direction);
        GroundCheck();
        Jump();
        Dash(xRaw, yRaw);
        CameraTrick();

        if (theRB.velocity.x != 0) //if velocity is 0, then do nothing.
        {
            bool prevFacing = facingRight; //Save previous state so I can compare it to create Dust
            bool isNegativeVelocity = theRB.velocity.x < 0; //Is negative velocity?
            theSR.flipX = isNegativeVelocity; //FlipX is the same as what I wrote previously
            facingRight = !isNegativeVelocity; //facingRight is contrary to FlipX
            if (facingRight != prevFacing && theRB.velocity.y == 0) //Compare bool: if it changed or not
            {
                CreateDust();
            }
        }
    }
    #endregion

    #region Can Move Or Interact
    public bool CanMoveOrInteract()
    {
        bool canMove = true;
        if (isKnockback)
            canMove = false;
        return canMove;
    }
    #endregion

    #region Walk
    void Walk(Vector2 direction)
    {
        float xVal = direction.x * moveSpeed * moveSpeedModifier * 100 * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            theRB.velocity = new Vector2(xVal, theRB.velocity.y);
            //theRB.velocity = new Vector2(direction.x * moveSpeed * moveSpeedModifier, theRB.velocity.y);
        }
        else
        {
            theRB.velocity = new Vector2(direction.x * moveSpeed * 100 * Time.fixedDeltaTime, theRB.velocity.y);
            //theRB.velocity = Vector2.Lerp(theRB.velocity, (new Vector2(direction.x * moveSpeed, theRB.velocity.y)), dashSpeedModifier * Time.deltaTime);
        }
    }
    #endregion

    #region Ground Check
    void GroundCheck()
    {
        cameFromTheGround = isGrounded;
        isGrounded = Physics2D.OverlapCircle((Vector2) transform.position + bottomOffset, groundCheckRadius, whatIsGround);
        if (isGrounded && !isDashing)
        {
            hasDashed = false;
            isDashing = false;
            if (!cameFromTheGround)
            {
                availableJumps = totalJumps;
                multipleJumps = false;
                Debug.Log("I came from the sky");
                CreateJumpDust();
            }
        }
        else
        {
            if (cameFromTheGround)
            {
                Debug.Log("I came from the ground");
                StartCoroutine(CoyoteJumpDelay());
            }
        }
    }
    #endregion

    #region Jump
    public void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                multipleJumps = true;
                availableJumps--;
                theRB.velocity = Vector2.up * jumpForce;
                CreateJumpDust();
            }
            else
            {
                if (coyoteJump)
                {
                    multipleJumps = true;
                    availableJumps--;
                    theRB.velocity = Vector2.up * jumpForce;
                }

                if (multipleJumps && availableJumps > 0)
                {
                    availableJumps--;
                    theRB.velocity = Vector2.up * jumpForce;
                }
            }
        }
        if (Input.GetButtonUp("Jump") && GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, theRB.velocity.y * 0.5f);
        }
    }
    #endregion

    #region Coyote Jump
    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.1f);
        coyoteJump = false;
    }
    #endregion

    #region Dash
    public void Dash(float xRaw, float yRaw)
    {
        if (Input.GetButtonDown("Fire1") && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                Camera.main.transform.DOComplete();
                FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
                hasDashed = true;
                Vector2 dashDirection = new Vector2(xRaw, yRaw);
                theRB.velocity = Vector2.zero;
                float prevGravity = theRB.gravityScale; //Store previous gravity scale
                theRB.gravityScale = 0; //Set gravity scale to 0
                Debug.Log("Set Gravity to 0");
                dashDustParticle.Play();
                FindObjectOfType<GhostTrail>().ShowGhost();
                StartCoroutine(DashCoroutine(dashDirection, prevGravity));
                //theRB.velocity += dashDirection.normalized * dashSpeed;
            }
        }
    }

    IEnumerator DashCoroutine(Vector2 dashDirection, float prevGravity)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / dashTime) //A loop that repeats every frame for a certain amount of seconds
        {
            theRB.velocity = dashDirection * dashSpeed;
            //theRB.velocity = Vector2 * dashSpeed; //Set velocity
            yield return null; //Wait until next frame
        }
        FindObjectOfType<GhostTrail>().ShowGhost();
        theRB.velocity = Vector2.zero;
        hasDashed = true;
        isDashing = false;
        yield return new WaitForSeconds(dashTimeInAir);
        theRB.gravityScale = prevGravity; //Restore previous value of gravity
        Debug.Log("Set Gravity to " +prevGravity);
        dashDustParticle.Stop();
    }
    #endregion

    #region Knockback
    public void KnockBack()
    {
        knockbackCounter = knockbackLenght;
        StartCoroutine(KnockBackDelay());
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, knockbackForce);

        knockbackCounter -= Time.deltaTime;
        //this counts down my time.
        if (!theSR.flipX)
            //if true, we are facing to the left. If false, facing to the right.
        {
            theRB.velocity = new Vector2(-knockbackForce, theRB.velocity.y);
        }
        else
        {
            theRB.velocity = new Vector2(knockbackForce, theRB.velocity.y);
        }
    }
    IEnumerator KnockBackDelay()
    {
        isKnockback = true;
        yield return new WaitForSeconds(knockbackCounter);
        isKnockback = false;
    }
    #endregion

    #region Bounce
    public void Bounce()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, bounceForce);
    }
    #endregion

    #region Camera Mario Bros Style
    public void CameraTrick()
    {
        //Camera Trick: Set a target as a child of the player so it moves either left or right when switching positions
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, aheadAmount * Input.GetAxisRaw("Horizontal"), aheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
        }
    }
#endregion

    #region Dust Particles
    public void CreateDust()
    {
        dustParticle.Play();
    }

    public void CreateJumpDust()
    {
        jumpDustParticle.Play();
    }

    public void CreateDashDust()
    {
        dashDustParticle.Play();
    }
    #endregion

    #region Ground Check Gizmo
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2) transform.position + bottomOffset, groundCheckRadius);
    }
    #endregion    
}