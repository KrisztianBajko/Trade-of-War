using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public bool isStanding = true;
    public float currentHealth;
    public float maxHealth;

    
    void Update()
    {
        if(currentHealth <= 0)
        {
            currentHealth = 0;
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
    }
}
