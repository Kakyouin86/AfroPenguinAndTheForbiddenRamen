using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderCollisionDisabler : MonoBehaviour
{
    #region Variables
    // Reference to the hidden platform box collider
    BoxCollider2D boxCollider;
    // How far the player has to press before disabling the collider
    float deadZone = 0.5f;
    // Reference to the player controller script
    PlayerController playerController;
    #endregion

    #region Unity Base Methods
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Invulnerable")
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Invulnerable")
        {
            // Disable the collider
            if (playerController.yRaw < -deadZone)
            {
                boxCollider.enabled = false;
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Invulnerable")
        {
            if (playerController.yRaw > -deadZone)
            {
                boxCollider.enabled = true;
                playerController = null;
            }
        }
    }
    #endregion
}
