using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private float motionSmoothTime = .1f;
    public NavMeshAgent agent;

    Camera cam;
    public float rotateSpeedMovement = 0.1f;
    public float rotateVelocity;

    private HeroCombat heroCombatScript;
    private void Start()
    {
        cam = Camera.main;
        agent.GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        heroCombatScript = GetComponent<HeroCombat>();
    }
    private void Update()
    {
        if (heroCombatScript.targetedEnemy != null)
        {
            if(heroCombatScript.targetedEnemy.GetComponent<HeroCombat>() != null)
            {
                if (!heroCombatScript.targetedEnemy.GetComponent<HeroCombat>().isAlive)
                {
                    heroCombatScript.targetedEnemy = null;
                }
            }
            
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    // move the player to the point we hit
                    agent.SetDestination(hit.point);
                    heroCombatScript.targetedEnemy = null;
                    agent.stoppingDistance = 0;

                    //rotation
                    //TODO: fix rotation bug

                    Quaternion rotationToLookAt = Quaternion.LookRotation(hit.point - transform.position);
                    float rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationToLookAt.eulerAngles.y, ref rotateVelocity, rotateSpeedMovement * (Time.deltaTime * 5));

                    transform.eulerAngles = new Vector3(0, rotationY, 0);
                }
                
            }


        }
        // walking - idle animation
        float speed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Walk", speed, motionSmoothTime,Time.deltaTime);
        
       
      


    }
    
}

