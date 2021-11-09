using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Invulnerable")
        {
            LevelManager.instance.RespawnPlayer();
            LevelManager.instance.SumLostLife();
            UIController.instance.SumLostLife();
        }
    }
}
