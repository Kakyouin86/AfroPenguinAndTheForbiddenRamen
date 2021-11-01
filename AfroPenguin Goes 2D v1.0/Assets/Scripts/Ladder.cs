using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    public BoxCollider2D platformBoxCollider;
    public PlatformEffector2D platformEffector;
    public PlayerController playerController;

    void Start()
    {
        platformEffector = GetComponentInChildren<PlatformEffector2D>();
        platformBoxCollider = platformEffector.GetComponent<BoxCollider2D>();
        playerController = FindObjectOfType<PlayerController>();
    }
    void FixedUpdate()
    {
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // Get the player by tag
        if (other.tag == "Player")
        {
            if (playerController != null)
            {
                // Check if object is a ladder
                if (platformBoxCollider != null)
                {
                    // Disable the platform colliders
                    platformBoxCollider.enabled = false;
                    platformEffector.enabled = false;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (playerController != null)
            {
                if (platformBoxCollider != null)
                {
                    platformBoxCollider.enabled = true;
                    platformEffector.enabled = true;
                }
            }
        }
    }
}