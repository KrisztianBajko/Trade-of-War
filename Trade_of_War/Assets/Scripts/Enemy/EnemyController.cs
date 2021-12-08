using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    //referencies
    private NavMeshAgent agent;
    private Animator anim;
    private BaseHealth enemyBaseHealth;
    private Stats playerStatScript;
    // reference for enemy base
    private GameObject enemyBase;
    // current target
    private GameObject currentTarget;
    // player reference
    private GameObject player;
    // reference for waypoints
    private GameObject[] waypoints;
    // current waypoint
    private int currentWaypoint;
    // distance beetwee player and enemy
    private float distanceToPlayer;
    // distance beetwee the enemy base and the minion
    private float distanceToBase;
    // distance from the lane and the minion
    private float distanceToLane;
    // check if the player is evade
    private bool evade;
    [Tooltip("Run animation speed multiplier.")]
    public float moveSpeed;
    [Tooltip("Attack animation speed multiplier.")]
    public float attackSpeed;
    [Tooltip("Raduis for searching for target.")]
    public float searchRadius;
    [Tooltip("How fast the minion can turn to the target direction.")]
    public float rotationSpeed;
    [Tooltip("The attack range.")]
    public float attackRadius;
    [Tooltip("Damage amount the minion can cause.")]
    public float damage;
    [Tooltip("Defind how far the minion can go from lane befor it turns back.")]
    public float acceptableDistanceFromLane;
    [Tooltip("Defind how far the playe has to be in order for the minion to continue its path.")]
    public float acceptableDistanceFromPlayer;
    private void Start()
    {
        enemyBase = GameObject.Find("FriendlyBase");
        playerStatScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        enemyBaseHealth = enemyBase.GetComponent<BaseHealth>();
        // find player by name
        player = GameObject.FindGameObjectWithTag("Player");
        // find all the waypoints
        waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        // get the animator componenet
        anim = GetComponent<Animator>();
        // get the nave mesh agent component
        agent = GetComponent<NavMeshAgent>();
        // set the enemy to run when its spawns
        anim.SetBool("isRunning", true);
        // set the enemy movespeed
        anim.SetFloat("speed", moveSpeed);
        // set the enemy attack speed
        anim.SetFloat("attackSpeed", attackSpeed);
    }
    private void Update()
    {
      /*  if (enemyHealth.isDead)
        {
            enemyHealth.enabled = false;
            agent.enabled = false;
            return;
        }*/

        DetectTarget();
        // check the distance between targets and enemy
        CheckDistance();
        // look at the target the enemy goes 
        LookAt();
        // set the destination to target position
        agent.SetDestination(currentTarget.transform.position);
    }

   /* private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, searchRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, acceptableDistanceFromPlayer);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, acceptableDistanceFromLane);
        Gizmos.color = Color.blue;
        
    }*/
    void CheckDistance()
    {
        // check the distance between the targets and the enemy
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        distanceToBase = Vector3.Distance(transform.position, enemyBase.transform.position);
        //TODO : if the minion gets too far from the lane, evade and go back
    }
    void ChasePlayer()
    {
        // if the player is not dead or the player is too far from enemy then go back to lane
        // otherwise chase the player and attack
        if (distanceToPlayer > acceptableDistanceFromPlayer)
        {
            anim.SetBool("isAttacking", false);
            anim.SetBool("isRunning", true);
            currentTarget = waypoints[currentWaypoint];
        }
        else
        {
            if (currentTarget == player)
            {
                Attack();
            }
        }
    }
    
 
    
    void DetectTarget()
    {
        //TODO: if the minion sees the player start running towards the player in an higher speed
        if (distanceToPlayer < searchRadius)
        {

            currentTarget = player;

            ChasePlayer();
            
        }
        // if the distance between the enemy base and the enemy is less then the search raduis and the base is still standning
        // attack the enemy base, otherwise just stop the agent
        else if (distanceToBase < searchRadius)
        {
            currentTarget = enemyBase;
            if (!enemyBaseHealth.isStanding)
            {
                agent.isStopped = true;
                anim.SetBool("isAttacking", false);
                anim.SetBool("isRunning", false);
            }
            else
            {
                AttackTheEnemyBase();
            }
        }
        // if the targets are not close enough then follow the path
        else
        {
            FollowPath();
        }
    }
    void FollowPath()
    {
        // follow the path if the enemy close enough to the next waypoint then target is the next waypoint
        if (agent.remainingDistance < .5f)
        {
            evade = false;
            currentWaypoint++;
        }
        if (currentWaypoint >= waypoints.Length)
        {
            currentWaypoint = 0;
        }
        currentTarget = waypoints[currentWaypoint];
    }
    void LookAt()
    {
        // make the enemy look at the target at all time
        Quaternion lookAt = Quaternion.LookRotation(currentTarget.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, rotationSpeed * Time.deltaTime);
    }
    void Attack()
    {
        // if the enemy is close enough to attack then attack the target otherwise just keep running
        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        if (distance < attackRadius)
        {
            agent.isStopped = true;
            anim.SetBool("isRunning", false);
            anim.SetBool("isAttacking", true);
        }
        else
        {
            agent.isStopped = false;
            anim.SetBool("isAttacking", false);
            anim.SetBool("isRunning", true);
        }
    }
    public void AttackPlayer()
    {
        // attack and cause damage only if there is a target
        if (currentTarget == null)
        {
            return;
        }
        if (currentTarget == player)
        {
          //  playerHealth.TakeDamage(damage);
        }
        if (currentTarget == enemyBase)
        {
            enemyBaseHealth.TakeDamage(damage);
        }
    }
    void AttackTheEnemyBase()
    {
        // if the target is the enemy base then attack 
        if (currentTarget == enemyBase)
        {
            Attack();
        }
    }
}
