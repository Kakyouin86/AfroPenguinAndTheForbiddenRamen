using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed = 8.0f;
    //public Rigidbody2D theRB;
    public float jumpForce = 15.0f;
    public bool isGrounded;
    public bool canDoubleJump;
    public Transform groundCheckPoint;
    public LayerMask whatIsGround;
    //public Vector2 jumpHeight;
    private Animator anim;
    public SpriteRenderer sr;
    public float knockbackLenght = 0.25f;
    public float knockbackForce = 5.0f;
    public float knockbackCounter;
    public float bounceForce = 15.0f;
    public bool stopInput;

    private void Awake()
    {
        instance = this;
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
            MovePlayer();
            //Debug.Log(transform.position);
        }


    }
    void MovePlayer()
    {
        if (knockbackCounter <= 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), GetComponent<Rigidbody2D>().velocity.y);
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);
            if (isGrounded)
            {
                canDoubleJump = true;
            }
            //rb.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal") , rb.velocity.y);
            //Esto es si quisiera que el movimiento del personaje sea menos "suave".
            //GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal") , GetComponent<Rigidbody2D>().velocity.y);

            //float verticalMovement = Input.GetAxis("Vertical" + playerIndex);
            //GetComponent<Rigidbody2D>().velocity = new Vector2(0, verticalMovement * speed);

            //float xValue = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
            //float zValue = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
            //Debug.Log(xValue);
            //Debug.Log(zValue);
            //transform.Translate(xValue, yValue, zValue);

            //if (Input.GetKey(KeyCode.Q))
            //{
            //    GetComponent<Rigidbody2D>().AddForce(jumpHeight, ForceMode2D.Impulse);
            //}

            if (Input.GetButtonDown("Jump"))
            {
                if (isGrounded)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
                    AudioManager.instance.PlaySFX(10);
                }

                else
                    if (canDoubleJump)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
                    canDoubleJump = false;
                    AudioManager.instance.PlaySFX(10);
                }
            }

            if (GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                sr.flipX = true;
            }
            else if (GetComponent<Rigidbody2D>().velocity.x > 0)
            {
                sr.flipX = false;
            }
        }

        else

        {
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

        anim.SetFloat("moveSpeed", Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));
        anim.SetBool("isGrounded", isGrounded);
    }

    public void KnockBack()
    {
        knockbackCounter = knockbackLenght;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, knockbackForce);
        anim.SetTrigger("hurt");
    }

    public void Bounce()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, bounceForce);
        AudioManager.instance.PlaySFX(10);
    }
}
