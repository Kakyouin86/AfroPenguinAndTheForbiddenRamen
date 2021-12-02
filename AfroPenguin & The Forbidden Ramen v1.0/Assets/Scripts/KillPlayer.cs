using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameObject deathEffectEnemy;
    public GameObject deathEffectPlayer;
    public Vector2 placeToInstantiate;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" || other.tag == "Invulnerable")
        {
            placeToInstantiate = new Vector2(other.transform.position.x, other.transform.position.y + 1.00f);
            Instantiate(deathEffectPlayer, placeToInstantiate, other.transform.rotation);
            AudioManager.instance.PlaySFX(1);
            LevelManager.instance.RespawnPlayer();
            LevelManager.instance.SumLostLife();
            UIController.instance.SumLostLife();
        }

        if (other.tag == "Enemy")
        {
            placeToInstantiate = new Vector2(other.transform.position.x, other.transform.position.y + 2.00f);
            Instantiate(deathEffectEnemy, placeToInstantiate, other.transform.rotation);
            Destroy(other.gameObject);
        }
    }
}
