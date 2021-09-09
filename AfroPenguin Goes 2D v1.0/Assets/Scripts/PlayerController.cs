using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Movement")]
    private float horizontalValue;
    //public float moveSpeed = 8.0f;
    public float moveSpeed = 4.0f;
    public float moveSpeedModifier = 1.5f;
    public bool isRunning;
    public LayerMask whatIsGround;
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.2f;
    public bool isGrounded = true;
    public float jumpForce = 15.0f;
    public int availableJumps;
    public int totalJumps = 2;
    public bool multipleJumps;
    public bool coyoteJump;
    public bool stopInput;
    public bool facingRight = true;

    [Header("Animations & Sprite")]
    private Animator anim;
    public SpriteRenderer sr;
    //public Rigidbody2D theRB;

    [Header("Knockback")]
    public float knockbackLenght = 0.25f;
    public float knockbackForce = 5.0f;
    public float knockbackCounter;
    public bool isKnockback;

    [Header("Bounce After Jump")]
    public float bounceForce = 15.0f;


    //NEW tricks:
    //public float hangTime = 0.2f;
    //public float hangCounter;

    //public float jumpBufferLength = 0.5f; //from where we are pressing the Jump button
    //public float jumpBufferCounter;

    //Little trick on seeing ahead:

    [Header("Mario Style Camera")]
    public Transform camTarget;
    public float aheadAmount = 1.5f;
    public float aheadSpeed = 1f;

    //Dust effect

    [Header("Particle System (Dust)")]
    public ParticleSystem dust;

    private void Awake()
    {
        instance = this;
        availableJumps = totalJumps;
    }
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!PauseMenu.instance.isPaused && !stopInput)
        //if the pause is false (we are playing) AND, we haven't stopped our input, then... move the player. If one of them true, we can't do ANY of this stuff. If both are true, then it won't move.
        {
            if (CanMoveOrInteract() == false)
                return;
            GroundCheck();
            MovePlayer(horizontalValue);
            Jump();
        }
    }

    void FixedUpdate()
    {

    }

    #region Can Move Or Interact
    public bool CanMoveOrInteract()
    {
        bool can = true;
        if (isKnockback)
            can = false;
        return can;
    } 
    #endregion

    #region Ground Check
    public void GroundCheck()
    {
        bool wasGrounded = isGrounded;
        isGrounded = false;
        //Check if I'm grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPoint.position, groundCheckRadius, whatIsGround);
        if (colliders.Length > 0)
        {
            isGrounded = true;
            anim.SetBool("isGrounded", true);
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJumps = false;
            }
        }
        else
        {
            //Un-parent the transform
            anim.SetBool("isGrounded", false);
            transform.parent = null;
            if (wasGrounded)
                StartCoroutine(CoyoteJumpDelay());
        }
    }
    #endregion

    #region Move Player
    void MovePlayer(float dir)
    {
        //Set the yVelocity Value
        anim.SetFloat("yVelocity", GetComponent<Rigidbody2D>().velocity.y);

        horizontalValue = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRunning = false;
        }
        float xVal = dir * moveSpeed * 100 * Time.fixedDeltaTime;
        //If we are running mulitply with the running modifier (higher)
        if (isRunning)
            xVal *= moveSpeedModifier;
        //Create Vec2 for the velocity
        Vector2 targetVelocity = new Vector2(xVal, GetComponent<Rigidbody2D>().velocity.y);
        //Set the player's velocity
        GetComponent<Rigidbody2D>().velocity = targetVelocity;
        //Set the float xVelocity according to the x value of the RigidBody2D velocity 
        anim.SetFloat("moveSpeed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));

        //Do the "Dust" effect when switching positions (or jumping)
        if (GetComponent<Rigidbody2D>().velocity.x != 0) //si la velocidad es 0, no hacer nada
        {
            bool prevFacing = facingRight; //Guardo el estado anterior para poder compararlo luego, para hacer el humito
            bool isNegativeVelocity = GetComponent<Rigidbody2D>().velocity.x < 0; //la velocidad es negativa?
            sr.flipX = isNegativeVelocity; // filpX es igual a lo anterior
            facingRight = !isNegativeVelocity; //facingRight es lo contrario a flipX
            if (facingRight != prevFacing) // comparo si el booleano cambió o no para ver si triggerear el humito
            {
                CreateDust();
            }
        }

        //Camera Trick: Set a target as a child of the player so it moves either left or right when switching positions
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, aheadAmount * Input.GetAxisRaw("Horizontal"), aheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
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
                GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpForce;
                CreateDust();
            }
            else
            {
                if (coyoteJump)
                {
                    multipleJumps = true;
                    availableJumps--;
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpForce;
                }

                if (multipleJumps && availableJumps > 0)
                {
                    availableJumps--;
                    GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpForce;
                }
            }
        }
        if (Input.GetButtonUp("Jump") && GetComponent<Rigidbody2D>().velocity.y > 0)
        {
            // isGrounded = false;
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y * 0.5f);
        }
    }
    #endregion

    #region Coyote Jump
    IEnumerator CoyoteJumpDelay()
    {
        coyoteJump = true;
        yield return new WaitForSeconds(0.2f);
        coyoteJump = false;
    }
    #endregion

    #region Knockback
    public void KnockBack()
    {
        knockbackCounter = knockbackLenght;
        StartCoroutine(KnockBackDelay());
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, knockbackForce);
        anim.SetTrigger("hurt");
        knockbackCounter -= Time.deltaTime;
        //this counts down my time.
        if (!sr.flipX)
        //if true, we are facing to the left. If false, facing to the right.
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-knockbackForce, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(knockbackForce, GetComponent<Rigidbody2D>().velocity.y);
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
        AudioManager.instance.PlaySFX(10);
    }
    #endregion

    #region Create Dust
    public void CreateDust()
    {
        dust.Play();
    }
} 
#endregion
