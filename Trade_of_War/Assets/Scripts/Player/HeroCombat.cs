using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCombat : MonoBehaviour
{
    public enum HeroAttackType { Melee, Ranged};
    public HeroAttackType heroAttackType;

    public GameObject targetedEnemy;
    public float attackRange;
    public float rotateSpeedForAttack;

    private PlayerController moveScript;
    private Stats statsScript;
    private Animator anim;

    public bool basicAttackIdle = false;
    public bool isAlive;
    public bool performMeleeAttack = true;

    [Header("Ranged Varialbes")]
    public bool performRangedAttack = true;
    public GameObject projPrefab;
    public Transform projSpawnPoint;

    private void Start()
    {
        statsScript = GetComponent<Stats>();
        anim = GetComponent<Animator>();
        moveScript = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (targetedEnemy != null)
        {
            if (Vector3.Distance(gameObject.transform.position, targetedEnemy.transform.position) > attackRange)
            {
                moveScript.agent.SetDestination(targetedEnemy.transform.position);
                moveScript.agent.stoppingDistance = attackRange;
            }
            else
            {
                // melee character
                if (heroAttackType == HeroAttackType.Melee)
                {
                    //Rotation
                    Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref moveScript.rotateVelocity, rotateSpeedForAttack * (Time.deltaTime * 5));

                    transform.eulerAngles = new Vector3(0, rotationY, 0);

                    moveScript.agent.SetDestination(transform.position);
                    if (performMeleeAttack)
                    {
                        Debug.Log("Attack");
                        StartCoroutine(MeleeAttackIntercal());
                    }
                }

                //RANGED CHARACTER
                if (heroAttackType == HeroAttackType.Ranged)
                {
                    //ROTATION
                    Quaternion rotationToLookAt = Quaternion.LookRotation(targetedEnemy.transform.position - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                        rotationToLookAt.eulerAngles.y,
                        ref moveScript.rotateVelocity,
                        rotateSpeedForAttack * (Time.deltaTime * 5));

                    transform.eulerAngles = new Vector3(0, rotationY, 0);

                    moveScript.agent.SetDestination(transform.position);

                    if (performRangedAttack)
                    {
                        Debug.Log("Attack The Minion");

                        //Start Coroutine To Attack
                        StartCoroutine(RangedAttackInterval());
                    }
                }
            }
        }

    }
    IEnumerator RangedAttackInterval()
    {
        performRangedAttack = false;
        anim.SetBool("BasicAttack", true);

        yield return new WaitForSeconds(statsScript.attackTime / ((100 + statsScript.attackTime) * 0.01f));

        if (targetedEnemy == null)
        {
            anim.SetBool("BasicAttack", false);
            performRangedAttack = true;
        }
    }
    IEnumerator MeleeAttackIntercal()
    {
        performMeleeAttack = false;
        anim.SetBool("BasicAttack", true);

        yield return new WaitForSeconds(statsScript.attackTime / ((100 + statsScript.attackTime) * 0.01f));

        if(targetedEnemy == null)
        {
            anim.SetBool("BasicAttack", false);
            performMeleeAttack = true;
        }
    }
    public void RangedAttack()
    {
        if (targetedEnemy != null)
        {
            if (targetedEnemy.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Minion)
            {
                SpawnRangedProj("Minion", targetedEnemy);
            }
        }

        performRangedAttack = true;
    }

    void SpawnRangedProj(string typeOfEnemy, GameObject targetedEnemyObj)
    {
        float dmg = statsScript.attackDamage;

        Instantiate(projPrefab, projSpawnPoint.transform.position, Quaternion.identity);

        if (typeOfEnemy == "Minion")
        {
            projPrefab.GetComponent<RangedProjectile>().targetType = typeOfEnemy;

            projPrefab.GetComponent<RangedProjectile>().target = targetedEnemyObj;
            projPrefab.GetComponent<RangedProjectile>().targetSet = true;
        }
    }
    public void MeleeAttack()
    {
        if(targetedEnemy != null)
        {
            if(targetedEnemy.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Minion)
            {
                targetedEnemy.GetComponent<Stats>().health -= statsScript.attackDamage;
            }
        }

        performMeleeAttack = true;
    }
}