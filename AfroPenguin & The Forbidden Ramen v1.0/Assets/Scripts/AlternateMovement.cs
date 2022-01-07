using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateMovement : MonoBehaviour
{
    public Transform[] routes;
    public int routeToGo;
    public float tParam;
    public Vector2 objectPosition;
    public float speedModifier;
    public bool returnThePlatform = false;
    private bool coroutineAllowed;

    void Start()
    {
        routeToGo = 0;
        tParam = 0f;
        speedModifier = 0.5f;
        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (coroutineAllowed & !returnThePlatform)
        {
            StartCoroutine(GoByTheRoute(routeToGo));
        }
        if (coroutineAllowed & returnThePlatform)
        {
            StartCoroutine(GoBackTheRoute(routeToGo));
        }
    }

    private IEnumerator GoByTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector2 p0 = routes[routeNum].GetChild(0).position;
        Vector2 p1 = routes[routeNum].GetChild(1).position;
        Vector2 p2 = routes[routeNum].GetChild(2).position;
        Vector2 p3 = routes[routeNum].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;
        returnThePlatform = true;
    }

    private IEnumerator GoBackTheRoute(int routeNum)
    {
        coroutineAllowed = false;

        Vector2 p0 = routes[routeNum].GetChild(3).position;
        Vector2 p1 = routes[routeNum].GetChild(2).position;
        Vector2 p2 = routes[routeNum].GetChild(1).position;
        Vector2 p3 = routes[routeNum].GetChild(0).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;
            transform.position = objectPosition;
            yield return new WaitForEndOfFrame();
        }

        tParam = 0f;
        routeToGo += 1;

        if (routeToGo > routes.Length - 1)
        {
            routeToGo = 0;
        }

        coroutineAllowed = true;
        returnThePlatform = false;
    }
}
