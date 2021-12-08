using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Stats towerStatScript;
    public GameObject projectilePrefab;
    public GameObject firePoint;
    public LayerMask targetLayer;
    public float searchRadius;
    public GameObject nearestTarget;
    public string typeOfEnemy;

    [Header("Ranged Varialbes")]
    public bool performRangedAttack = true;
    public Animator anim;

    private void Update()
    {
        SearchForTarget();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, searchRadius);
        Gizmos.color = Color.green;
    }
    void SearchForTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, searchRadius, targetLayer);
        float minimumDistance = Mathf.Infinity;
        foreach (Collider collider in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, collider.transform.position);
            if (distance < minimumDistance)
            {
                minimumDistance = distance;
                nearestTarget = collider.transform.gameObject;
            }
            else if(distance> searchRadius)
            {
                nearestTarget = null;
            }
        }
        if (nearestTarget != null)
        {
            if (performRangedAttack)
            {
                StartCoroutine(RangedAttackInterval());
            }
            
        }
        else
        {
            Debug.Log("There is no enemy in the given radius");
        }
    }

    IEnumerator RangedAttackInterval()
    {
        performRangedAttack = false;
        anim.SetBool("BasicAttack", true);

        yield return new WaitForSeconds(towerStatScript.attackTime / ((100 + towerStatScript.attackTime) * 0.01f));

        if (nearestTarget == null)
        {
            anim.SetBool("BasicAttack", false);
            performRangedAttack = true;
        }
    }
    public void RangedAttack()
    {
        if (nearestTarget != null)
        {
            if (nearestTarget.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Player)
            {
                SpawnRangedProj(typeOfEnemy, nearestTarget);
            }
        }

        performRangedAttack = true;
    }
    public void SpawnRangedProj(string typeEnemy, GameObject targetedEnemyObj)
        {
            float dmg = towerStatScript.attackDamage;

            Instantiate(projectilePrefab, firePoint.transform.position, Quaternion.identity);

            if (typeEnemy == typeOfEnemy)
            {
               projectilePrefab.GetComponent<RangedProjectile>().targetType = typeOfEnemy;

               projectilePrefab.GetComponent<RangedProjectile>().target = targetedEnemyObj;
               projectilePrefab.GetComponent<RangedProjectile>().targetSet = true;
            }
        }

}
