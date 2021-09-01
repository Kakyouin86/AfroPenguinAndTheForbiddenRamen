using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    public float lifeTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
