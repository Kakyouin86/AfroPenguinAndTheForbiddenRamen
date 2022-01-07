using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shot01 : MonoBehaviour
{
    public GameObject hitFX;
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject effect = Instantiate(hitFX, transform.position, Quaternion.identity);
        Destroy(effect, 0.3f);
        Destroy(gameObject);
    }
}