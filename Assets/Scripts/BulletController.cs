using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BulletController : MonoBehaviour
{
    public float bulletSpeed;
    public float lifetime=5;
    public Rigidbody TheRB;

    public GameObject impactEffect;

    private int damage=10;

    public bool damageEnemy, damagePlayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TheRB.velocity = transform.forward* bulletSpeed;
        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && damageEnemy)
        {
            //Destroy(other.gameObject);
            other.gameObject.GetComponent<EnemyHealthController>().DamageEnemy(damage);
        }
        if (other.gameObject.tag == "HeadShot" && damageEnemy)
        {
            other.transform.parent.GetComponent<EnemyHealthController>().DamageEnemy(damage*2);
            Debug.Log("Headshot");
        }

        if (other.gameObject.tag == "Player" && damagePlayer)
        {
            PlayerHealthController.instance.damagePlayer(damage);
        }
            Destroy(gameObject);
        Instantiate(impactEffect, transform.position +(transform.forward*(-bulletSpeed*Time.deltaTime)), transform.rotation);
    }
}
