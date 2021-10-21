using System.Collections;
using System.Globalization;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    #region Variables
    public static PlayerController instance;
    public bool canMove;

    [Header("Ground Check")] 
    public bool isGrounded;
    public bool cameFromTheGround;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;
    public Vector2 bottomOffset;

    [Header("Movement")] 
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;
    public Animator theAnimator;
    public Vector2 direction;
    public bool facingRight;
    public float moveSpeed = 4f;
    public float moveSpeedModifier = 1.5f;

    [Header("Dash")] 
    public bool isdashing;
    public bool dashDown;
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

    [Header("Particle Systems")]
    public ParticleSystem dustParticle;
    public ParticleSystem jumpDustParticle;
    public ParticleSystem dashDustParticle;
    public ParticleSystem dashDownDustParticle;
    #endregion

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
        theAnimator = GetComponent<Animator>();
        //GetComponent<CapsuleCollider2D>().sharedMaterial.friction = 0f;
        //GetComponent<CapsuleCollider2D>().sharedMaterial.bounciness = 0f;
    }

    #endregion

        #region Update
        void Update()
    {

        if (CanMoveOrInteract() == false) 
                return;
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float xRaw = Input.GetAxisRaw("Horizontal");
            float yRaw = Input.GetAxisRaw("Vertical");
            //Vector2 direction = new Vector2(x, y);
            theAnimator.SetFloat("xVelocity", Mathf.Abs(theRB.velocity.x));
            theAnimator.SetFloat("yVelocity", theRB.velocity.y);
            direction = new Vector2(xRaw, yRaw);

            Walk();
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
    }
    #endregion

    #region Can Move Or Interact
    public bool CanMoveOrInteract()
    {
            canMove = true;
        if (isKnockback)
            canMove = false;
        if (PauseMenu.instance.isPaused)
            canMove = false;
        if (LevelManager.instance.isPlayingIntro)
            canMove = false;
        return canMove;
    }
    #endregion

    #region Walk
    public void Walk()
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
    public void GroundCheck()
    {
        cameFromTheGround = isGrounded;
        isGrounded = Physics2D.OverlapCircle((Vector2) transform.position + bottomOffset, groundCheckRadius, whatIsGround);
        theAnimator.SetBool("isGrounded", true);
        theRB.gravityScale = 5;
        if (isGrounded)
        {
            isdashing = false;
            if (!cameFromTheGround)
            {
                availableJumps = totalJumps;
                multipleJumps = false;
                Debug.Log("I came from the sky");
                if (dashDown)
                {
                    CreateDashDownDust();
                    dashDown = false;
                }
                else
                {
                    CreateJumpDust();
                }
            }
        }
        else
        {
            theAnimator.SetBool("isGrounded", false);
            if (cameFromTheGround)
            {
                Debug.Log("I came from the ground");
                theAnimator.SetBool("isGrounded", false);
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
        if (Input.GetButtonDown("Fire1") && !isdashing)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                if (yRaw == -1)
                {
                    dashDown = true;
                }
                else
                {
                    dashDown = false;
                }
                Camera.main.transform.DOComplete();
                FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
                isdashing = true;
                theAnimator.SetBool("isDashing", true);
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
            yield return null;
        }
        FindObjectOfType<GhostTrail>().ShowGhost();
        theRB.velocity = Vector2.zero;
        isdashing = true;
        if (dashDown == false)
        {
            yield return new WaitForSeconds(dashTimeInAir);
        }
        else
        {
            yield return new WaitForSeconds(0);
        }

        theRB.gravityScale = prevGravity;
        Debug.Log("Set Gravity to " +prevGravity);
        dashDustParticle.Stop();
        theAnimator.SetBool("isDashing", false);
    }
    #endregion

    #region Knockback
    public void KnockBack()
    {
        knockbackCounter = knockbackLenght;
        StartCoroutine(KnockBackDelay());
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, knockbackForce);
        theAnimator.SetTrigger("hurt");
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

    public void CreateDashDownDust()
    {
        dashDownDustParticle.Play();
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