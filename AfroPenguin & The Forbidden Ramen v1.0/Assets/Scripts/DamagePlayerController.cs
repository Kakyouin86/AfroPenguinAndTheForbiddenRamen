using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerController : MonoBehaviour
{
    public void Start()
    {

    }

    public void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D player)
    {
        if (player.tag == "Player") //&& !PlayerController.instance.isDashing
        {
            //FindObjectOfType<PlayerHealthController>().DealDamage();
            //Con eso busco al script dentro de todos los objetos que existan en esa escena, y aquel que tenga el script PlayerHealthController, and? a buscarlo y busc? y activ? DealDamage()
            player.GetComponent<CapsuleCollider2D>().enabled = false;
            player.GetComponent<CapsuleCollider2D>().enabled = true;
            PlayerHealthController.instance.DealDamage();
            Debug.Log("Hit Player");
        }
    }
}
