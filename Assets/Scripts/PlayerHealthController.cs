using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    private int maxHealth=10, currentHealth;
    public float invicibleLength = 1f;
    private float invincCounter;
    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.HealthSlider.maxValue = maxHealth;
        UIController.instance.HealthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth +"/"+maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;
        }
    }


    public void damagePlayer(int damageAmount)
    {
        if (invincCounter <= 0)
        {
            currentHealth -= damageAmount;

            if (currentHealth <= 0)
            {
                gameObject.SetActive(false);

                currentHealth = 0;
                GameManager.instance.PlayerDie();
            }

            invincCounter = invicibleLength;
            
            
            UIController.instance.HealthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth +"/"+maxHealth;
        }
    }
}
