using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class MenuButton : MonoBehaviour
{
    public MenuButtonController menuButtonController;
    public Animator theAnimator;
    public AnimatorFunctions animatorFunctions;
    public int thisIndex;
    public Button thisButton;
    public bool canMoveMenuButton;

    void Start()
    {
        theAnimator = GetComponent<Animator>();
        animatorFunctions = GetComponent<AnimatorFunctions>();
        thisButton = GetComponent<Button>();
        canMoveMenuButton = true;
    }
    void Update()
    {
        if (menuButtonController.index == thisIndex)
            {
                theAnimator.SetBool("selected", true);

                if (Input.GetMouseButtonDown(0) || Input.GetAxis("Submit") == 1 && canMoveMenuButton)
                {
                    theAnimator.SetBool("pressed", true);
                    FindObjectOfType<MenuButtonController>().canMoveMenuButtonController = false;
                    canMoveMenuButton = false;
                    StartCoroutine("LittleFade");
                }

                else if (theAnimator.GetBool("pressed"))
                {
                    theAnimator.SetBool("pressed", false);
                    animatorFunctions.disableOnce = true;
                }
            }

        else
        {
            theAnimator.SetBool("selected", false);
        }
    }
    IEnumerator LittleFade()

    {
        yield return new WaitForSeconds(2.0f);
        thisButton.onClick.Invoke();
    }
}
