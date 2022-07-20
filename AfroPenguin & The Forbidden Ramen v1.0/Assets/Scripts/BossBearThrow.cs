using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBearThrow : StateMachineBehaviour
{
    public Transform target;
    public float firingAngle = 45.0f;
    public float gravity = 9.8f;
    public Transform projectile;
    public GameObject firePrefab;
    public GameObject fire;
    public Transform firePoint; 
    public float elapse_time = 0;
    public float target_Distance;
    public float projectile_Velocity;
    public float flightDuration;
    public float vx;
    public float vy;
    public float shotForce;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        elapse_time = 0f;
        flightDuration = 0f;
        firePoint = GameObject.FindGameObjectWithTag("FirePoint").GetComponent<Transform>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        target_Distance = Vector3.Distance(firePoint.position, target.position);
        fire = Instantiate(firePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb2d = fire.GetComponent<Rigidbody2D>();
        rb2d.AddForce(firePoint.up * shotForce);

        // Move projectile to the position of throwing object + add some offset if needed.
        // projectile.position = firePoint.position + new Vector3(0, 0.0f, 0);

        // Calculate distance to target
        //target_Distance = Vector3.Distance(firePoint.position, target.position);

        // Calculate the velocity needed to throw the object to the target at specified angle.
        //projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // Extract the X  Y componenent of the velocity
        //vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        //vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Calculate flight time.
        //flightDuration = target_Distance / vx;

        // Rotate projectile to face the target.
        //firePoint.rotation = Quaternion.LookRotation(target.position - firePoint.position);
        //fire.transform.Translate(0, (vy - (gravity * elapse_time)) * Time.deltaTime, vx * Time.deltaTime);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //if (elapse_time < flightDuration)
        {
        //    fire.transform.Translate(0, (vy - (gravity * elapse_time)) * Time.deltaTime, vx * Time.deltaTime);
        //    elapse_time += Time.deltaTime;
        }
    }
    //GameObject fire = Instantiate(firePrefab, firePoint.position, firePoint.rotation);
    //Rigidbody2D rb2d = fire.GetComponent<Rigidbody2D>();
    //rb2d.AddForce(firePoint.up* shotForce);
}