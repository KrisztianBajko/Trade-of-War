using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float currentHealth;
    public float maxHealth;
    public bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            isDead = true;
            currentHealth = 0;
            Destroy(gameObject, 2);
        }
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
    }
}
