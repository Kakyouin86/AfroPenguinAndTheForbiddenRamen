using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static CheckpointController instance;
    private Checkpoint[] checkpoints;
    //this is an array so I can have one variable to keep track of multiple objects
    private Checkpoint lastCheckpoint;
    public Vector3 spawnPoint;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        checkpoints = FindObjectsOfType<Checkpoint>();
        //I'm finding OBJECTS, all objects which have the Checkpoint script attached.
        spawnPoint = PlayerController.instance.transform.position;
    }

    //public void DeactivateCheckpoints()
    //{
    //for (int i = 0; i < checkpoints.Length; i++)
    //I create a value of i which equals to 0. We want to know how long our checkpoint array is. The third part of the "For" is the
    //update or incremental section of our loop. It will do the code as long as i is less than 3. When it comes to 3 or the Lengh
    //it will do other things
    //{
    //checkpoints[i].ResetCheckpoint();
    //this means: go to position. With this, we will access the checkpoint script that is at position 0.
    //This is for setting the CP off when it hits a new CP.
    //}

    //}

    public void HandleCheckpoints(Checkpoint newCheckpoint)
    {
        if (!lastCheckpoint || newCheckpoint.transform.position.x >= lastCheckpoint.transform.position.x)
        {
            lastCheckpoint?.ResetCheckpoint();
            newCheckpoint.ActivateCheckpoint();
            spawnPoint = newCheckpoint.transform.position;
            lastCheckpoint = newCheckpoint;
        }
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    //Only use this inside this function
    {
        if (spawnPoint.x < newSpawnPoint.x)
        {
            spawnPoint = newSpawnPoint;
        }
    }
}
