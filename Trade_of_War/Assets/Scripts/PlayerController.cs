using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public CharacterController cc;

    public float moveSpeed;
    public bool canMove;
    public bool finishedMovement;
    public bool isGrounded = true;
    public Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundLayer;

    public Vector3 targetPosition = Vector3.zero;
    public Vector3 playerMove = Vector3.zero;
    public float playerToPointDistance;

    public float gravity;
    public float height;
    public bool isDead;
    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (!IsDead)
        {
            MoveThePlayer();
            velocity.y += gravity * Time.deltaTime;
            cc.Move(velocity * Time.deltaTime);
            CalculateHeight();
            CheckIfFinishedMovement();
            cc.Move(playerMove);


        }



    }

    void CalculateHeight()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
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
                playerMove.y = height * Time.deltaTime;
            }

        }

    }
    void MoveThePlayer()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider is TerrainCollider)
                {
                    playerToPointDistance = Vector3.Distance(transform.position, hit.point);
                    if (playerToPointDistance >= .2f)
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
            if (Vector3.Distance(transform.position, targetPosition) <= .15f)
            {
                canMove = false;
                playerMove.Set(0f, 0f, 0f);
                animator.SetFloat("Walk", 0f);
            }



        }
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

}
