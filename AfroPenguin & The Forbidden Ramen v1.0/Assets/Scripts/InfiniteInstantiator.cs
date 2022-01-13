using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteInstantiator : MonoBehaviour
{
    public float timer;
    public float timeRemaining = 2f;
    public GameObject toInstantiate;

    // Start is called before the first frame update
    void Start()
    {
        timer = timeRemaining;
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0.1f)
        {
            Instantiate(toInstantiate,transform.position,transform.rotation);
            timeRemaining = timer;
        }
    }
}
