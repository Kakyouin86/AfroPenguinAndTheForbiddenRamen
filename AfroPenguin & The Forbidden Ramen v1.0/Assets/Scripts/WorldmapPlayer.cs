using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WorldmapPlayer : MonoBehaviour
{
    public WorldmapPoint currentPoint;
    public float moveSpeed = 5.0f;
    public bool levelLoading;
    public WorldmapLevelManager theManager;
    public Animator theAnimator;
    public Vector3 lastPosition;

    void Awake()
    {
        //transform.position = new Vector3(lastPosition.x, lastPosition.y);
    }

    void Start()
    {
        theAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentPoint.transform.position) < 0.01f && !levelLoading)
        {
            //the distance between two objects, out current position, and our current POINT position. Since it is really really low, then, we can move since we need to be AT the point.
            theAnimator.SetBool("walkingRight", false);
            theAnimator.SetBool("walkingLeft", false);
            theAnimator.SetBool("walkingUp", false);
            theAnimator.SetBool("walkingDown", false);


            //first: move, to where?, how fast?
            if (Input.GetAxisRaw("Horizontal") > .5f)
            {
                if (currentPoint.right != null)
                {
                    SetNextPoint(currentPoint.right);
                    theAnimator.SetBool("walkingRight",true);
                    theAnimator.SetBool("walkingLeft", false);
                    theAnimator.SetBool("walkingUp", false);
                    theAnimator.SetBool("walkingDown", false);
                }
            }

            if (Input.GetAxisRaw("Horizontal") < -.5f)
            {
                if (currentPoint.left != null)
                {
                    SetNextPoint(currentPoint.left);
                    theAnimator.SetBool("walkingRight", false);
                    theAnimator.SetBool("walkingLeft", true);
                    theAnimator.SetBool("walkingUp", false);
                    theAnimator.SetBool("walkingDown", false);
                }
            }

            if (Input.GetAxisRaw("Vertical") > .5f)
            {
                if (currentPoint.up != null)
                {
                    SetNextPoint(currentPoint.up);
                    theAnimator.SetBool("walkingRight", false);
                    theAnimator.SetBool("walkingLeft", false);
                    theAnimator.SetBool("walkingUp", true);
                    theAnimator.SetBool("walkingDown", false);
                }
            }

            if (Input.GetAxisRaw("Vertical") < -.5f)
            {
                if (currentPoint.down != null)
                {
                    SetNextPoint(currentPoint.down);
                    theAnimator.SetBool("walkingRight", false);
                    theAnimator.SetBool("walkingLeft", false);
                    theAnimator.SetBool("walkingUp", false);
                    theAnimator.SetBool("walkingDown", true);
                }
            }

            if(currentPoint.isLevel && currentPoint.levelToLoad != "" && !currentPoint.isLocked)
            {
                WorldmapUIController.instance.ShowLevelInfo(currentPoint);
                if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Submit"))
                {
                    //lastPosition.x = currentPoint.transform.position.x;
                    //lastPosition.y = currentPoint.transform.position.x;
                    levelLoading = true;
                    theManager.LoadLevel();
                }
            }
        }
    }

    public void SetNextPoint(WorldmapPoint nextPoint)
    {
        currentPoint = nextPoint;
        WorldmapUIController.instance.HideLevelInfo();
        AudioManager.instance.PlaySFX(0);
    }
}
