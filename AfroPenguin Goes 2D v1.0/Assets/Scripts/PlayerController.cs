using System.Collections;
using DG.Tweening;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    public bool stopInput;

    [Header("Ground Check")] 
    public bool isGrounded;
    public bool wasGrounded;
    public bool groundTouch;
    public LayerMask whatIsGround;
    public float groundCheckRadius = 0.2f;
    public Vector2 bottomOffset;

    [Header("Movement")] 
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;
    public bool facingRight;
    public float moveSpeed = 10f;
    public float moveSpeedModifier = 1.5f;
    public float wallJumpLerp = 10f;

    [Header("Dash")] 
    public bool hasDashed;
    public bool isDashing;
    public float dashSpeed = 50f;

    [Header("Knockback")]
    public float knockbackLenght = 0.25f;
    public float knockbackForce = 5.0f;
    public float knockbackCounter;
    public bool isKnockback;

    [Header("Jump")]
    public float jumpForce = 12.0f;
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


    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        theSR = GetComponent<SpriteRenderer>();
    }

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

        if (theRB.velocity.x != 0) //si la velocidad es 0, no hacer nada
        {
            bool prevFacing = facingRight; //Guardo el estado anterior para poder compararlo luego, para hacer el humito
            bool isNegativeVelocity = theRB.velocity.x < 0; //la velocidad es negativa?
            theSR.flipX = isNegativeVelocity; // filpX es igual a lo anterior
            facingRight = !isNegativeVelocity; //facingRight es lo contrario a flipX
            if (facingRight != prevFacing && theRB.velocity.y == 0) // comparo si el booleano cambió o no para ver si triggerear el humito
            {
                CreateDust();
            }
        }
    }

    #region Can Move Or Interact
    public bool CanMoveOrInteract()
    {
        bool canMove = true;
        if (isKnockback)
            canMove = false;
        return canMove;
    }
    #endregion

    void Walk(Vector2 direction)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            theRB.velocity = new Vector2(direction.x * moveSpeed * moveSpeedModifier, theRB.velocity.y);
        }
        else
        {
            theRB.velocity = Vector2.Lerp(theRB.velocity, (new Vector2(direction.x * moveSpeed, theRB.velocity.y)), wallJumpLerp * Time.deltaTime);
        }
    }

    void GroundCheck()
    {
        wasGrounded = isGrounded;
        isGrounded = false;
        isGrounded = Physics2D.OverlapCircle((Vector2) transform.position + bottomOffset, groundCheckRadius, whatIsGround);
        if (isGrounded && !groundTouch)
        {
            hasDashed = false;
            isDashing = false;
            groundTouch = true;
            if (!wasGrounded)
            {
                availableJumps = totalJumps;
                multipleJumps = false;
            }
        }
        
        if (!isGrounded && groundTouch)
        {
            StartCoroutine(CoyoteJumpDelay());
            groundTouch = false;
        }

        if (wasGrounded != isGrounded) // comparo si el booleano cambió o no para ver si triggerear el humito
        {
            CreateJumpDust();
        }
    }

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
        if (Input.GetButtonUp("Jump") && theRB.velocity.y > 0)
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

    public void Dash(float xRaw, float yRaw)
    {
        if (Input.GetButtonDown("Fire1") && !hasDashed)
        {
            if (xRaw != 0 || yRaw != 0)
            {
                hasDashed = true;
                theRB.velocity = Vector2.zero;
                Vector2 dashDirection = new Vector2(xRaw, yRaw);
                theRB.velocity += dashDirection.normalized * dashSpeed;
                StartCoroutine(DashWait());
            }
        }
    }

    IEnumerator DashWait()
    {

        StartCoroutine(GroundDash());
        DOVirtual.Float(14, 0, .8f, RigidbodyDrag);
        theRB.gravityScale = 0;

        isDashing = true;
        yield return new WaitForSeconds(.3f);
        theRB.gravityScale = 3;

        isDashing = false;
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
        if (isGrounded)
            hasDashed = false;
    }

    public void RigidbodyDrag(float x3)
    {
        theRB.drag = x3;
    }

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

    public void CameraTrick()
    {
        //Camera Trick: Set a target as a child of the player so it moves either left or right when switching positions
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, aheadAmount * Input.GetAxisRaw("Horizontal"), aheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
        }
    }

    public void CreateDust()
    {

    }

    public void CreateJumpDust()
    {

    }

    public void CreateDashDust()
    {

    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2) transform.position + bottomOffset, groundCheckRadius);
    }
}
