using System;
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
    public bool isDashing;
    public bool dashDown;
    public bool hasntHit;
    public float dashSpeed = 30f;
    public float dashTime = 0.1f;
    public float dashTimeInAir = 0.1f;
    public GameObject dashRightCollider;
    public GameObject dashLeftCollider;
    public GameObject dashUpCollider;
    public GameObject dashDownCollider;

    [Header("Dash Controller")] 
    public bool canDash;
    public float currentDashGauge;
    public float maxDashGauge = 100f;
    public float collectDashGauge = 20f;

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
        currentDashGauge = 0f;
        isKnockback = false;
        canDash = false;
        hasntHit = true;
        dashRightCollider.SetActive(false);
        dashLeftCollider.SetActive(false);
        dashUpCollider.SetActive(false);
        dashDownCollider.SetActive(false);
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
        }
    }
    #endregion
    
    #region Dash
    public void Dash(float xRaw, float yRaw)
    {
        if (Input.GetButtonDown("Fire1") && !isDashing && canDash)
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

                if (xRaw == 1)
                {
                    dashRightCollider.SetActive(true);
                }

                if (xRaw == -1)
                {
                    dashLeftCollider.SetActive(true);
                }

                if (yRaw == 1)
                {
                    dashUpCollider.SetActive(true);
                }

                if (yRaw == -1)
                {
                    dashDownCollider.SetActive(true);
                }

                Camera.main.transform.DOComplete();
                FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
                isDashing = true;
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
        isDashing = true;
        
        if (dashDown == false)
        {
            yield return new WaitForSeconds(dashTimeInAir);
        }
        else
        {
            yield return new WaitForSeconds(0);
        }

        hasntHit = true;
        dashRightCollider.SetActive(false);
        dashLeftCollider.SetActive(false);
        dashUpCollider.SetActive(false);
        dashDownCollider.SetActive(false);
        theRB.gravityScale = prevGravity;
        Debug.Log("Set Gravity to " +prevGravity);
        dashDustParticle.Stop();
        theAnimator.SetBool("isDashing", false);
        isDashing = false;
        canDash = false;
        currentDashGauge = 0f;
        UIController.instance.barAnimator.SetBool("isFilled",false);
        UIController.instance.iconAnimator.SetBool("isFilled",false);
        UIController.instance.dashIndicatorSlider.value = currentDashGauge;
    }
    #endregion

    #region KnockBack
    public void KnockBack()
    {
        StartCoroutine(KnockBackDelay());
        knockbackCounter = knockbackLenght;
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
    public void KnockBackDash(float multiplier)
    {
        StartCoroutine(KnockBackDashDelay());
        knockbackCounter = knockbackLenght;
        theRB.velocity = new Vector2(0f, knockbackForce * multiplier);
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
        GetComponent<PlayerHealthController>().enabled = true;
    }
    IEnumerator KnockBackDashDelay()
    {
        isKnockback = true;
        yield return new WaitForSeconds(knockbackCounter*3);
        isKnockback = false;
    }
    #endregion



    #region Bounce
    public void Bounce()
    {
        theRB.velocity = new Vector2(theRB.velocity.x, bounceForce);
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