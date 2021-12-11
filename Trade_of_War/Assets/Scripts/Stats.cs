using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public float attackDamage;
    public float attackSpeed;
    public float attackTime;
    public float experienceValue;

    public bool isAlive;

    private HeroCombat heroCombatScript;
    private GameObject player;
    
    private void Start()
    {
        heroCombatScript = GameObject.FindGameObjectWithTag("Player").GetComponent<HeroCombat>();
        player = GameObject.FindGameObjectWithTag("Player");
        health = maxHealth;

    }

    private void Update()
    {
        if(health <= 0)
        {
            isAlive = false;
            heroCombatScript.isAlive = false;
            Destroy(gameObject);
            heroCombatScript.targetedEnemy = null;
            heroCombatScript.performMeleeAttack = false;
            if(player != null)
            {
                //give exp
                player.GetComponent<LevelUpStats>().SetExperience(experienceValue);
            }
            
        }
    }
}
