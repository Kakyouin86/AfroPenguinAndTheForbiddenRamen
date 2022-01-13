using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTimePlatforms : MonoBehaviour
{
    public float timer;
    public float timeRemaining;
    // Start is called before the first frame update
    void Awake()
    {
        timeRemaining = GetComponentInParent<PlatformInfiniteLoop>().Timer;
    }

    // Update is called once per frame
    void Start()
    {
        timer = timeRemaining;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0.1f)
        {
            Destroy(gameObject);
        }
    }
}
