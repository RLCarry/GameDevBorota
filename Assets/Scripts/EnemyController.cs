using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class EnemyController : MonoBehaviour
{
    private bool chasing;
    public float distanceToChase=10f, distanceToLose = 15f, distanceToStop=2f;

    private Vector3 targetPoint;

    public NavMeshAgent agent;

    private Vector3 startPoint;

    private float keepChasingTime = 5f;

    private float chaseCounter=5f;

    public GameObject bullet;
    public Transform firePoint;

    public float fireRate, waitBetweenShots=2f,timeToShoot=1f;
    private float fireCount, shotWaitCounter,shootTimeCounter;

    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;

        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;
        if (!chasing)
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                chasing = true;
                shootTimeCounter = timeToShoot;
                shotWaitCounter = waitBetweenShots; 
            }

            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;


                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }

            if (agent.remainingDistance < .1f)
            {
                anim.SetBool("isMoving",false);
            }
            else
            {
                anim.SetBool("isMoving",true);
            }   
        }
        else
        {
            //transform.LookAt(PlayerController.instance.transform.position);
            //rb.velocity = transform.forward * moveSpeed;
            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;
            }
            else
            {
                agent.destination = transform.position; 
            }

            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;
                chaseCounter=keepChasingTime;
            }

            if (shotWaitCounter > 0)
            {
                shotWaitCounter -= Time.deltaTime;
                if (shotWaitCounter <= 0)
                {
                    shootTimeCounter = timeToShoot;
                }
                
                anim.SetBool("isMoving",true);
            }
            else
            {
                if (PlayerHealthController.instance.gameObject.activeInHierarchy)
                {
                    shootTimeCounter -= Time.deltaTime;
                    if (shootTimeCounter > 0)
                    {
                        fireCount -= Time.deltaTime;

                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;

                            firePoint.LookAt(PlayerController.instance.groundCheckPoint.position +
                                             new Vector3(0f, 1.2f, 0f));
                            //check the angle to the player
                            Vector3 targetDir = PlayerController.instance.groundCheckPoint.position -
                                                transform.position;
                            float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);
                            if (Mathf.Abs(angle) < 30f)
                            {
                                Instantiate(bullet, firePoint.position, firePoint.rotation);

                                anim.SetTrigger("fireShot");
                            }
                            else
                            {
                                shotWaitCounter = waitBetweenShots;
                            }
                        }

                        agent.destination = transform.position;
                    }
                    else
                    {
                        shotWaitCounter = waitBetweenShots;
                    }

                    anim.SetBool("isMoving", false);
                }
            }
        }
    }
}
