using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifeTime = 0.75f;

    void Update()
    {
        Destroy(gameObject, lifeTime);
        //lifeTime -= Time.deltaTime;
        //if (lifeTime <0)
        //{
        //    Destroy(gameObject);
        //}
    }
}
