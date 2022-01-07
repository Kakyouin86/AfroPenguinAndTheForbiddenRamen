using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BallEnemyControllerViolet : MonoBehaviour
{
    public Transform firePoint;
    public GameObject shotPrefab;
    public float shotForce = 7f;
    public float timeBetweenShots = 1f;
    [SerializeField]
    private float shotTimer = 0f;
    //public EnemyHP bossHp;
    public GameObject eyeBall;
    //public GameObject goalPole;
    void Start()
    {
        shotTimer = timeBetweenShots;
     //   bossHp = transform.GetChild(0).GetComponent<EnemyHP>();
        eyeBall = transform.GetChild(1).gameObject;
    }
    void Update()
    {
        shotTimer -= Time.deltaTime;
        if (shotTimer <= 0)
        {
            shotTimer = timeBetweenShots;
            Shoot();
        }
     //   if (bossHp.isDead)
     //   {
     //       Destroy(eyeBall);
     //       //goalPole.SetActive(true);
     //   }
    }
    void Shoot()
    {
        GameObject laser = Instantiate(shotPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb2d = laser.GetComponent<Rigidbody2D>();
        rb2d.AddForce(firePoint.up * shotForce, ForceMode2D.Impulse);
    }
}