using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBearThrow : StateMachineBehaviour
{
    public GameObject firePrefab;

    public float shotForceMultiplier = 0.015f; // Multiplicador para ajustar a las físicas de unity
    [Range(3.5f, 10.0f)]
    public float v0 = 3.5f; // velocidad inicial del proyectil, no puede bajar de 3.5f

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform firePoint = GameObject.FindGameObjectWithTag("FirePoint").GetComponent<Transform>();
        Transform target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
       
        Rigidbody2D rb = Instantiate(firePrefab, firePoint.position, firePoint.rotation).GetComponent<Rigidbody2D>();
        float x = target.transform.position.x - firePoint.position.x;
        float y = target.transform.position.y - firePoint.position.y;
        float g = rb.gravityScale;

        float angle = Mathf.Atan(
            (v0 * v0 - Mathf.Sqrt(Mathf.Pow(v0, 4) - g * (g * x * x + 2 * y * v0 * v0)))
            /
            (g * x)
            );

        firePoint.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);
        rb.transform.eulerAngles = new Vector3(0, 0, 90 + angle * Mathf.Rad2Deg);
        rb.AddForce(-firePoint.right * v0 * shotForceMultiplier);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}