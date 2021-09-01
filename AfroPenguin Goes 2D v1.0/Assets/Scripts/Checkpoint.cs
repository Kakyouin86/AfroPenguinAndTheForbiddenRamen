using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SpriteRenderer theSR;
    public Sprite checkpointOn, checkpointOff;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CheckpointController.instance.HandleCheckpoints(this);
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
