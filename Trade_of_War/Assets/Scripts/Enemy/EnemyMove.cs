using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMove : MonoBehaviour
{
    public enum EnemyAttackType { Melee, Ranged };
    public EnemyAttackType enemyAttackType;

    [Tooltip("Waypoints array.")]
    public GameObject[] waypoints;
    // the current waypoint index
    private int currentWP = 0;
    public GameObject nearestWayPoint;

    [Header("Moving variables")]
    [Tooltip("The moving speed.")]
    public float speed = 10f;
    [Tooltip("The turning speed.")]
    public float rotationSpeed = 10f;
    [Tooltip("The distance before the minion starts turning to an other waypoint direction.")]
    public float turningDistance = 3f;
    [Tooltip("Movement motion smooth time.")]
    public float motionSmoothTime = .1f;

    [Header("Searching Variables")]
    [Tooltip("The range to detect target.")]
    public float searchRadius;
    [Tooltip("How far the minion can attack the target.")]
    public float attackRange;
    [Tooltip("The distance before the minion start to EVADE.")]
    public float evadeDistance;
    public float rotateVelocity;
    public float rotateSpeedForAttack;
    public bool performMeleeAttack = true;

    private Stats enemyStatsScript;
    private Animator anim;
    private NavMeshAgent agent;
    private Transform nearestTarget;
    public LayerMask targetLayer;

    /* [Header("Ranged Varialbes")]
     public bool performRangedAttack = true;
     public GameObject projPrefab;
     public Transform projSpawnPoint;*/




    
    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        enemyStatsScript = GetComponent<Stats>();
    }
    void Update()
    {
        SearchForTarget();
        if (nearestTarget != null)
        {
            float distance = Vector3.Distance(transform.position, nearestTarget.transform.position);
            if (distance < evadeDistance)
            {
                AttackTarget();
            }
            else
            {
                // GetClosestObject();
                nearestTarget = null;

            }

        }
        else
        {
            FollowPath();
        }


        // walking - idle animation
        float speed = agent.velocity.magnitude / agent.speed;
        anim.SetFloat("speed", speed, motionSmoothTime, Time.deltaTime);
    }
   
    private void AttackTarget()
    {
        if (Vector3.Distance(gameObject.transform.position, nearestTarget.transform.position) > attackRange)
        {
            agent.SetDestination(nearestTarget.transform.position);

        }
        else
        {
            // melee character
            if (enemyAttackType == EnemyAttackType.Melee)
            {
                //Rotation
                Quaternion rotationToLookAt = Quaternion.LookRotation(nearestTarget.transform.position - transform.position);
                float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedForAttack * (Time.deltaTime * 5));

                transform.eulerAngles = new Vector3(0, rotationY, 0);

                agent.stoppingDistance = attackRange;
                agent.SetDestination(transform.position);
                if (performMeleeAttack)
                {
                    StartCoroutine(MeleeAttackIntercal());
                }
            }
        }
    }
        IEnumerator MeleeAttackIntercal()
        {
            performMeleeAttack = false;
            anim.SetBool("BasicAttack", true);

            yield return new WaitForSeconds(enemyStatsScript.attackTime / ((100 + enemyStatsScript.attackTime) * 0.01f));

            if (nearestTarget == null)
            {
                anim.SetBool("BasicAttack", false);
                performMeleeAttack = true;
            }
        }

        public void MeleeAttack()
        {
            if (nearestTarget != null)
            {
                if (nearestTarget.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Player)
                {
                    nearestTarget.GetComponent<Stats>().health -= enemyStatsScript.attackDamage;
                    attackRange = 4f;
                }
                else if (nearestTarget.GetComponent<Targetable>().enemyType == Targetable.EnemyType.Base)
                {
                    nearestTarget.GetComponent<Stats>().health -= enemyStatsScript.attackDamage;
                    attackRange = 7f;
                }
            }

            performMeleeAttack = true;
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
                    nearestTarget = collider.transform;
                }
            }
            
        }

        void FollowPath()
        {
            // the distance between the game object and the next waypoint
            // if we reach the waypoint increase the current waypoint
            if (Vector3.Distance(transform.position, waypoints[currentWP].transform.position) < turningDistance)
            {
                currentWP++;
            }
            // if we reach the last waypoint in the array set the next waypoint to the first one so it starts again from the begining
            if (currentWP >= waypoints.Length)
            {
                currentWP = 0;
            }
            // turn the game object to the goal direction in this way the game object will snap straight away to the direction of the goal position
            //transform.LookAt(waypoints[currentWP].transform);

            // the direction of the next goal
            Quaternion lookatWP = Quaternion.LookRotation(waypoints[currentWP].transform.position - transform.position);
            // turn the game object to the direction of the next goal position but smoothly with a given rotation speed;
            transform.rotation = Quaternion.Slerp(transform.rotation, lookatWP, rotationSpeed * Time.deltaTime);
            // push the game object forward/z axis with a given speed

            agent.SetDestination(waypoints[currentWP].transform.position);
            agent.stoppingDistance = 1;
        }

        //TODO: make the minion go to the nearest WP


        /* public GameObject GetClosestObject()
         {

             float closest = 999999; //add your max range here
             for (int i = 0; i < waypoints.Length; i++)  //list of gameObjects to search through
             {
                 float dist = Vector3.Distance(waypoints[i].transform.position, this.transform.position);
                 if (dist < closest)
                 {
                     closest = dist;
                     nearestWayPoint = waypoints[i];
                 }
             }
             return nearestWayPoint;
         }*/
    }
    #region RANGED CHARACTER
    /*//RANGED CHARACTER
    if (enemyAttackType == EnemyAttackType.Ranged)
        {
            //ROTATION
            Quaternion rotationToLookAt = Quaternion.LookRotation(nearestTarget.transform.position - transform.position);
            float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y,
                rotationToLookAt.eulerAngles.y,
                ref rotateVelocity,
                rotateSpeedForAttack * (Time.deltaTime * 5));

            transform.eulerAngles = new Vector3(0, rotationY, 0);

            agent.SetDestination(transform.position);

            if (performRangedAttack)
            {
                Debug.Log("Attack The Minion");

                //Start Coroutine To Attack
                StartCoroutine(RangedAttackInterval());
            }
        }
    }
}

IEnumerator RangedAttackInterval()
{
performRangedAttack = false;
anim.SetBool("BasicAttack", true);

yield return new WaitForSeconds(enemyStatsScript.attackTime / ((100 + enemyStatsScript.attackTime) * 0.01f));

if (nearestTarget == null)
{
    anim.SetBool("BasicAttack", false);
    performRangedAttack = true;
}
}
    void SpawnRangedProj(string typeOfEnemy, GameObject targetedEnemyObj)
{
float dmg = enemyStatsScript.attackDamage;

Instantiate(projPrefab, projSpawnPoint.transform.position, Quaternion.identity);

if (typeOfEnemy == "Minion")
{
    projPrefab.GetComponent<RangedProjectile>().targetType = typeOfEnemy;

    projPrefab.GetComponent<RangedProjectile>().target = targetedEnemyObj;
    projPrefab.GetComponent<RangedProjectile>().targetSet = true;
}
}*/
    #endregion

