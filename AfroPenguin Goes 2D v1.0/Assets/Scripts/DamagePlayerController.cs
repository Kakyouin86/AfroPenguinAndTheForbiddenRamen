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
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && !PlayerController.instance.isDashing)
        {
            //FindObjectOfType<PlayerHealthController>().DealDamage();
            //Con eso busco al script dentro de todos los objetos que existan en esa escena, y aquel que tenga el script PlayerHealthController, andá a buscarlo y buscá y activá DealDamage()
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = true;
            PlayerHealthController.instance.DealDamage();
            Debug.Log("Hit Player");
        }
    }
}
