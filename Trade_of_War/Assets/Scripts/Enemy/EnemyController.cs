using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    //referencies
    private NavMeshAgent agent;
    private Animator anim;
    private BaseHealth baseHealth;
    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    // current target
    private GameObject target;
    // player reference
    private GameObject player;
    // reference for waypoints
    private GameObject[] waypoints;
    // current waypoint
    private GameObject friendlyBase;
    private int currentWaypoint;
    // distance beetwee player and enemy
    private float distanceToPlayer;
    // distance beetwee the scroll holder and enemy
    private float distanceToScroll;
    // distance from the lane and enemy
    private float distanceToLane;
    // check if the player is evade
    private bool evade;


    public float moveSpeed;
    public float attackSpeed;
    public float searchRadius;
    public float rotationSpeed;
    public float attackRadius;
    public float damage;
    private void Start()
    {
        // find the scroll holder by name
        friendlyBase = GameObject.Find("FriendlyBase");
        // reference for scroll script
        baseHealth = friendlyBase.GetComponent<BaseHealth>();
        // find player by name
        player = GameObject.FindGameObjectWithTag("Player");
        //reference for player health script
        playerHealth = player.GetComponent<PlayerHealth>();
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
        // get the enemy health script
        enemyHealth = GetComponent<EnemyHealth>();
    }
    private void Update()
    {
        if (enemyHealth.isDead)
        {
            enemyHealth.enabled = false;
            agent.enabled = false;
            return;
        }
        // check the distance between targets and enemy
        CheckDistance();
        // if the distance between player and enemy is bigger then the search raduis then evade and go back to the lane otherwise chase the player
        if (distanceToPlayer < searchRadius)
        {
            if (evade)
            {
                target = waypoints[currentWaypoint];
            }
            else
            {
                ChasePlayer();
            }
        }
        // if the distance between the scroll holder and the enemy is less then the search raduis and the scroll is still standning
        // attack the scroll holder otherwise just stop the agent
        else if (distanceToScroll < searchRadius)
        {
            if (!baseHealth.isStanding)
            {
                agent.isStopped = true;
                anim.SetBool("isAttacking", false);
                anim.SetBool("isRunning", false);
            }
            else
            {
                AttackTheScrollHolder();
            }
        }
        // if the targets are not close enough then follow the path
        else
        {
            FollowPath();
        }
        // look at the target the enemy goes 
        LookAt();
        // set the destination to target position
        agent.SetDestination(target.transform.position);
    }
    void CheckDistance()
    {
        // check the distance between the targets and the enemy
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        distanceToScroll = Vector3.Distance(transform.position, friendlyBase.transform.position);
        distanceToLane = Vector3.Distance(transform.position, waypoints[currentWaypoint].transform.position);
        // check if the enemy is too far from the lane so it will goes back 
        if (distanceToLane > 35f)
        {
            evade = true;
        }
    }
    void ChasePlayer()
    {
        // if the player is not dead or the player is too far from enemy then go back to lane
        // otherwise chase the player and attack
        if (playerHealth.isDead || distanceToPlayer > 10f)
        {
            anim.SetBool("isAttacking", false);
            anim.SetBool("isRunning", true);
            target = waypoints[currentWaypoint];
        }
        else
        {
            target = player;
            if (target == player)
            {
                Attack();
            }
        }
    }
    void AttackTheScrollHolder()
    {
        // if the target is the scroll holder then attack 
        target = friendlyBase;
        if (target == friendlyBase)
        {
            Attack();
        }
    }
    void FollowPath()
    {
        // follow the path if the enemy close enough to the next waypoint then target the next waypoint
        if (agent.remainingDistance < 0.5f)
        {
            evade = false;
            currentWaypoint++;
        }
        if (currentWaypoint >= waypoints.Length)
        {
            currentWaypoint = 0;
        }
        target = waypoints[currentWaypoint];
    }
    void LookAt()
    {
        // make the enemy look at the target at all time
        Quaternion lookAt = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookAt, rotationSpeed * Time.deltaTime);
    }
    void Attack()
    {
        // if the enemy is close enough to attack then attack the target otherwise just keep running
        float distance = Vector3.Distance(transform.position, target.transform.position);
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
        if (target == null)
        {
            return;
        }
        if (target == player && !playerHealth.isDead)
        {
            playerHealth.TakeDamage(damage);
        }
        if (target == friendlyBase)
        {
            baseHealth.TakeDamage(damage);
        }
    }
}
