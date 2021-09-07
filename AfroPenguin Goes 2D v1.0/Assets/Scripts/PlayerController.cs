using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    [Header("Movement")]
    public float moveSpeed = 8.0f;
    //public Rigidbody2D theRB;
    public float jumpForce = 15.0f;
    public float doubleJumpForce = 15.0f;
    public LayerMask whatIsGround;
    public bool isGrounded;
    public bool wasDoubleJump;
    public bool canDoubleJump;
    public bool stopInput;
    public bool facingRight;
    public Transform groundCheckPoint;
    //public Vector2 jumpHeight;

    [Header("Animations & Sprite")]
    private Animator anim;
    public SpriteRenderer sr;

    [Header("Knockback")]
    public float knockbackLenght = 0.25f;
    public float knockbackForce = 5.0f;
    public float knockbackCounter;

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
            GetComponent<Rigidbody2D>().velocity = new Vector2(Input.GetAxisRaw("Horizontal") * moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, 0.2f, whatIsGround);

            if (isGrounded)
            {
                canDoubleJump = true;
                wasDoubleJump = false;
                //NEW
                //hangCounter = hangTime;
                //Stop NEW
            }

            //NEW
            //else
            //{
            //hangCounter -= Time.deltaTime;
            //}
            //Stop NEW

            if (Input.GetButtonDown("Jump")) //&& hangCounter > 0f)  //Second part is NEW and it's for the hangCounter in case double jump is not an option.
            {
                if (isGrounded)
                {
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jumpForce);
                    AudioManager.instance.PlaySFX(10);
                    isGrounded = false;
                    wasDoubleJump = false;
                    CreateDust();
                }

                else if (canDoubleJump)
                {
                    wasDoubleJump = true;
                    GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, doubleJumpForce);
                    canDoubleJump = false;
                    AudioManager.instance.PlaySFX(10);
                }
            }

            //NEW:
            if (Input.GetButtonUp("Jump") && GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, GetComponent<Rigidbody2D>().velocity.y * 0.5f);
            }
            //Stop NEW

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

            //if (GetComponent<Rigidbody2D>().velocity.x < 0)
            //{
            //    sr.flipX = true;
            //    facingRight = false;
            //}
            //else if (GetComponent<Rigidbody2D>().velocity.x > 0)
            //{
            //    sr.flipX = false;
            //    facingRight = true;
            //}
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

        //Camera Trick: Set a target as a child of the player so it moves either left or right when switching positions
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            camTarget.localPosition = new Vector3(Mathf.Lerp(camTarget.localPosition.x, aheadAmount * Input.GetAxisRaw("Horizontal"), aheadSpeed * Time.deltaTime), camTarget.localPosition.y, camTarget.localPosition.z);
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

    public void CreateDust()
    {
        dust.Play();
    }
}