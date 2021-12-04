using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CharacterController cc;

    public float moveSpeed;
    public bool canMove;
    public bool finishedMovement;

    public Vector3 targetPosition = Vector3.zero;
    public Vector3 playerMove = Vector3.zero;
    public float playerToPointDistance;
    public float remainingDistance;


    public GameObject currentTarget;
    public EnemyHealth enemyHealth;
    public float damage;
    public bool canAttack;

    public float autoAttackCoolDown;
    public float autoAttackCurrentTime;

    public float doubleClickTimer;
    public bool didDoubleClick;


    public bool isDead;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }











    public NavMeshAgent agent;
    public Camera cam;
    private void Awake()
    {
        agent.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.SetDestination(hit.point);
            }


        }
        if(agent.remainingDistance > 0.5f)
        {
            animator.SetFloat("Walk", 1f);
        }
        else if(agent.isStopped || agent.remainingDistance <= 0.5f)
        {
            animator.SetFloat("Walk", 0f);
        }





        /*
                if (!IsDead)
                {
                    SelectTarget();
                    MoveThePlayer();
                    CheckIfFinishedMovement();
                    cc.Move(playerMove);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if(currentTarget != null && canAttack)
                    {
                        BasicAttack();
                    }
                }
                if (currentTarget != null)
                {
                    float attackDistance = Vector3.Distance(transform.position, currentTarget.transform.position);
                    Vector3 targetDirection = currentTarget.transform.position - transform.position;
                    Vector3 forward = Vector3.forward;
                    float angle = Vector3.Angle(targetDirection, forward);

                    if (angle > 60f)
                    {
                        canAttack = false;
                        autoAttackCurrentTime = 0;
                    }
                    else
                    {
                        if (attackDistance < 60f)
                        {
                            canAttack = true;
                        }
                        else
                        {
                            canAttack = false;
                            autoAttackCurrentTime = 0;
                        }
                    }
                    if (doubleClickTimer > 0)
                    {
                        doubleClickTimer -= Time.deltaTime;
                    }
                    else
                    {
                        didDoubleClick = false;
                    }
                }
                if (currentTarget != null && canAttack)
                {
                    if (autoAttackCurrentTime < autoAttackCoolDown)
                    {
                        autoAttackCurrentTime += Time.deltaTime;
                    }
                    else
                    {
                        BasicAttack();
                        autoAttackCurrentTime = 0;
                    }
                }
            }



            void CheckIfFinishedMovement()
            {
                if (!finishedMovement)
                {
                    if (!animator.IsInTransition(0) && !animator.GetCurrentAnimatorStateInfo(0).IsName("Stand") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.8f)
                    {
                        finishedMovement = true;
                    }
                    else
                    {
                        MoveThePlayer();
                    }

                }

            }
            void MoveThePlayer()
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.CompareTag("Ground"))
                        {
                            playerToPointDistance = Vector3.Distance(transform.position, hit.point);
                            if (playerToPointDistance >= .5f)
                            {
                                canMove = true;
                                targetPosition = hit.point;
                            }
                        }
                    }
                }
                if (canMove)
                {
                    animator.SetFloat("Walk", 1f);

                    Vector3 targetTempPos = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

                    Quaternion quaternion = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetTempPos - transform.position), 30f * Time.deltaTime);
                    transform.rotation = quaternion;

                    playerMove = transform.forward * moveSpeed * Time.deltaTime;
                    if (Vector3.Distance(transform.position, targetPosition) <= remainingDistance)
                    {
                        canMove = false;
                        playerMove.Set(0f, 0f, 0f);
                        animator.SetFloat("Walk", 0f);
                    }
                }
            }*/

        /*   private void SelectTarget()
           {
               if (Input.GetMouseButton(0))
               {
                   Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                   RaycastHit hit;
                   if (Physics.Raycast(ray, out hit, 10000))
                   {
                       if (hit.collider.CompareTag("Enemy"))
                       {
                           currentTarget = hit.collider.gameObject;
                           enemyHealth = currentTarget.transform.gameObject.transform.GetComponent<EnemyHealth>();
                       }
                       else
                       {
                           if (currentTarget != null)
                           {
                               if (!didDoubleClick)
                               {
                                   didDoubleClick = true;
                                   doubleClickTimer = 0.3f;
                               }
                               else
                               {
                                   print("Deselected");
                                   currentTarget = null;
                                   enemyHealth = null;
                                   didDoubleClick = false;
                                   doubleClickTimer = 0;
                                   autoAttackCurrentTime = 0;
                               }
                           }
                       }

                   }
               }
           }   
           private void BasicAttack()
           {
               enemyHealth.TakeDamage(damage);
           }
           public bool FinishedMovement
           {
               get
               {
                   return finishedMovement;
               }
               set
               {
                   finishedMovement = value;
               }
           }
           public Vector3 TargetPosition
           {
               get
               {
                   return targetPosition;
               }
               set
               {
                   targetPosition = value;
               }
           }
        */
    }
}
