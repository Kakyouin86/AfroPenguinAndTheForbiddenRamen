using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            //FindObjectOfType<PlayerHealthController>().DealDamage();
            //Con eso busco al script dentro de todos los objetos que existan en esa escena, y aquel que tenga el script PlayerHealthController, andá a buscarlo y buscá y activá DealDamage()
            PlayerHealthController.instance.DealDamage();
 
            Debug.Log("Hit");
        }
    }
}
