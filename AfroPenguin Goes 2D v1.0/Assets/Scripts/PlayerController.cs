using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

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
    public float previousGravity = 5f;

    [Header("Movement")]
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;
    public Animator theAnimator;
    public float x;
    public float y;
    public float xRaw;
    public float yRaw;
    public bool facingRight;
    public float moveSpeed = 4f;
    public float moveSpeedModifier = 1.5f;

    [Header("Climb")]
    //public Collider2D platformGround;
    public float climbSpeed = 6;
    public bool isClimbing;
    public RaycastHit2D climbRaycast;
    public float climbRaycastDistance = 3;
    public LayerMask whatIsLadder;

    [Header("Dash")]
    public bool isDashing;
    public bool dashDown;
    public bool dashUp;
    public bool hasHit;
    public float dashSpeed = 30f;
    public float dashTime = 0.1f;
    public float dashTimeInAir = 0.1f;

    [Header("Dash Controller")]
    public bool canDash;
    public float currentDashGauge;
    public float maxDashGauge = 100f;
    public float collectDashGauge = 20f;
    public GameObject theDashHurtbox;

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
        previousGravity = theRB.gravityScale;
        currentDashGauge = 0f;
        isKnockback = false;
        canDash = false;
        hasHit = false;
        theDashHurtbox.SetActive(false);
    }
    #endregion

    #region Update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            canDash = true;
            currentDashGauge = 100f;
            UIController.instance.barAnimator.SetBool("isFilled", true);
            UIController.instance.iconAnimator.SetBool("isFilled", true);
            UIController.instance.dashIndicatorSlider.value = currentDashGauge;
        }

        if (CanMoveOrInteract() == false)
            return;
        {
            theAnimator.SetFloat("xVelocity", Mathf.Abs(theRB.velocity.x));
            theAnimator.SetFloat("yVelocity", theRB.velocity.y);
            x = Input.GetAxis("Horizontal");
            y = Input.GetAxis("Vertical");
            xRaw = Input.GetAxisRaw("Horizontal");
            yRaw = Input.GetAxisRaw("Vertical");

            GroundCheck();
            Walk();
            Jump();
            Climb();
            Dash();
            //CameraTrick(); If I want the Super Mario Style camera, Here is the "where"

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
        //if (LevelManager.instance.isPlayingIntro)
        //    canMove = false;
        return canMove;
    }
    #endregion

    #region Ground Check
    public void GroundCheck()
    {
        cameFromTheGround = isGrounded;
        isGrounded = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, groundCheckRadius,
            whatIsGround);
        theAnimator.SetBool("isGrounded", true);
        theRB.gravityScale = previousGravity;

        if (isGrounded)
        {
            //isDashing = false;
            if (!cameFromTheGround)
            {
                availableJumps = totalJumps;
                multipleJumps = false;
                Debug.Log("I came from the sky");
                if (dashDown)
                {
                    CreateDashDownDust();
                    dashDown = false;
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

    #region Walk
    public void Walk()
    {
        if (Input.GetKey(KeyCode.LeftShift))

        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed * moveSpeedModifier, theRB.velocity.y);
        }

        else
        {
            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, theRB.velocity.y);
        }
    }
    #endregion

    #region Climb
    public void Climb()
    {
        climbRaycast = Physics2D.Raycast(transform.position, Vector2.up, climbRaycastDistance, whatIsLadder);
        if (climbRaycast.collider != null)
        {
            if (Input.GetKey(KeyCode.W))
            {
                isClimbing = true;
                theAnimator.SetBool("isClimbing", isClimbing);
            }
            if (Input.GetKey(KeyCode.S))
            {
                isClimbing = true;
                theAnimator.SetBool("isClimbing", isClimbing);
            }
        }

        if (isClimbing && climbRaycast.collider != null)
        {
            theRB.velocity = new Vector2(theRB.velocity.x, yRaw * climbSpeed);
            theRB.gravityScale = 0;
            {
                if (yRaw == 0 && xRaw == 0)
                {
                    theAnimator.speed = 0f;
                }
                else
                {
                    theAnimator.speed = 1f;
                }
            }
        }
        else
        {
            theRB.gravityScale = 5;
            isClimbing = false;
            theAnimator.SetBool("isClimbing", isClimbing);
            theAnimator.speed = 1f;
        }

        if (isClimbing && isGrounded)
        {
            isClimbing = false;
            theAnimator.SetBool("isClimbing", isClimbing);
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

    #region Build Dash
    public void BuildDash()
    {
        currentDashGauge += collectDashGauge;
        UIController.instance.DashSlider();
        if (currentDashGauge >= maxDashGauge)
        {
            currentDashGauge = 100f;
            canDash = true;
            UIController.instance.barAnimator.SetBool("isFilled", true);
            UIController.instance.iconAnimator.SetBool("isFilled", true);
            UIController.instance.dashIndicatorSlider.value = currentDashGauge;
        }
    }
    #endregion

    #region Dash
    public void Dash()
    {
        if (Input.GetButtonDown("Fire1") && !isDashing && canDash && !isClimbing)
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

                if (yRaw == 1)
                {
                    dashUp = true;
                }
                else
                {
                    dashUp = false;
                }
                Camera.main.transform.DOComplete();
                FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
                theDashHurtbox.SetActive(true);
                this.tag="Invulnerable";
                isDashing = true;
                theAnimator.SetBool("isDashing", true);
                Vector2 dashDirection = new Vector2(xRaw, yRaw);
                theRB.velocity = Vector2.zero;
                float prevGravity = theRB.gravityScale; //Store previous gravity scale
                theRB.gravityScale = 0;
                dashDustParticle.Play();
                FindObjectOfType<GhostTrail>().ShowGhost();
                StartCoroutine(DashCoroutine(dashDirection));
                //theRB.velocity += dashDirection.normalized * dashSpeed;
            }
        }
    }

    IEnumerator DashCoroutine(Vector2 dashDirection)
    {
        for (float t = 0;
            t < 1;
            t += Time.deltaTime / dashTime) //A loop that repeats every frame for a certain amount of seconds
        {
            if (!hasHit)
            {
                theRB.velocity = dashDirection * dashSpeed;
                //theRB.velocity = Vector2 * dashSpeed; //Set velocity
                yield return null;
                if (dashUp)
                {
                    theRB.velocity = Vector2.zero;
                }
            }
        }
 
        if (dashDown == false)
        {
            yield return new WaitForSeconds(dashTimeInAir);
        }
        else
        {
            yield return new WaitForSeconds(0);
        }
        theDashHurtbox.SetActive(false);
        this.tag = "Player";
        FindObjectOfType<GhostTrail>().ShowGhost();
        //theRB.velocity = Vector2.zero;
        theRB.gravityScale = previousGravity;
        dashDustParticle.Stop();
        isDashing = false;
        hasHit = false;
        canDash = false;
        theAnimator.SetBool("isDashing", false);
        currentDashGauge = 0f;
        UIController.instance.barAnimator.SetBool("isFilled", false);
        UIController.instance.iconAnimator.SetBool("isFilled", false);
        UIController.instance.dashIndicatorSlider.value = currentDashGauge;
    }
    #endregion

    #region Knockback
    public void KnockBack()
    {
        knockbackCounter = knockbackLenght;
        StartCoroutine(KnockBackDelay());
        theRB.velocity = new Vector2(0f, knockbackForce);
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

    #region KnockbackDash
    public void KnockBackDash(float knockbackDashMultiplier)
    {
        isGrounded = false;
        theAnimator.SetBool("isGrounded", false);
        StartCoroutine(KnockBackDashDelay());
        knockbackCounter = knockbackLenght;
        theRB.velocity = new Vector2(0f, knockbackForce * knockbackDashMultiplier);
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
    IEnumerator KnockBackDashDelay()
    {
        isKnockback = true;
        yield return new WaitForSeconds(knockbackCounter * 3);
        isKnockback = false;
    }
    #endregion

    #region Bounce
    public void Bounce(float bounceDashDownMultiplier)
    {
        theRB.velocity = new Vector2(theRB.velocity.x, bounceForce * bounceDashDownMultiplier);
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
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, groundCheckRadius);
    }
    #endregion   
}
