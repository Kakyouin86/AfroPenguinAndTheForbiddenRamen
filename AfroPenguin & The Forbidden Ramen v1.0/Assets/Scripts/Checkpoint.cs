using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpriteRenderer theSR;
    public Sprite checkpointOn, checkpointOff;

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Invulnerable"))
        {
            CheckpointController.instance.HandleCheckpoints(this);
            GetComponent<AudioSource>().Play();
            //CheckpointController.instance.DeactivateCheckpoints();
            //theSR.sprite = checkpointOn;
            //CheckpointController.instance.SetSpawnPoint(transform.position);
        }
    }

    public void ResetCheckpoint()
    {
        theSR.sprite = checkpointOff;
    }

    public void ActivateCheckpoint()
    {
        theSR.sprite = checkpointOn;
    }
}
