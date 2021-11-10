using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlammingBlock : MonoBehaviour
{
    public Transform theSlammer;
    public Transform slammerTarget;
    public Vector3 startPoint;
    public float slamSpeed = 10f;
    public float waitAfterSlam = 1f;
    public float resetSpeed = 2f;
    public float waitCounter;
    public bool slamming, resetting;

    // Start is called before the first frame update
    void Start()
    {
        theSlammer = transform.GetChild(0);
        slammerTarget = transform.GetChild(1);
        startPoint = theSlammer.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!slamming && !resetting)
        {
            if (Vector3.Distance(slammerTarget.position, PlayerController.instance.transform.position) < 2f)
            {
                slamming = true;
                waitCounter = waitAfterSlam;
            }
        }

        if (slamming)
        {
            theSlammer.position = Vector3.MoveTowards(theSlammer.position, slammerTarget.position, slamSpeed * Time.deltaTime);



            if (theSlammer.position == slammerTarget.position)
            {
                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    slamming = false;
                    resetting = true;
                }

            }
        }

        if (resetting)
        {
            theSlammer.position = Vector3.MoveTowards(theSlammer.position, startPoint, resetSpeed * Time.deltaTime);

            if (theSlammer.position == startPoint)
            {
                resetting = false;
            }
        }
    }
}
