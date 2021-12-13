using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleport : MonoBehaviour
{
    public Slider teleportSlider;
    public float teleportCountDown;

    public bool isTeleporting;

    private HeroCombat heroCombatScript;
    private PlayerController playerControllerScript;
    private Transform startingPoint;
   

    void Start()
    {
        heroCombatScript = GetComponent<HeroCombat>();
        startingPoint = GameObject.FindGameObjectWithTag("Start").transform;
        playerControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        teleportSlider.maxValue = teleportCountDown;
    }

    void Update()
    {
        teleportSlider.value = teleportCountDown;

        if (Input.GetKeyDown(KeyCode.B))
        {
            isTeleporting = true;
        }
        if (isTeleporting)
        {
            if(teleportCountDown <= 0)
            {
                teleportCountDown = 5f;
                isTeleporting = false;
                transform.position = startingPoint.position;
                playerControllerScript.agent.SetDestination(startingPoint.position);
            }
            else
            {
                teleportCountDown -= Time.deltaTime;
                heroCombatScript.targetedEnemy = null;
                teleportSlider.gameObject.SetActive(true);
                playerControllerScript.agent.isStopped = true;

            }
            
            
        }
        if (!isTeleporting)
        {
            playerControllerScript.agent.isStopped = false;
            teleportSlider.gameObject.SetActive(false);
            isTeleporting = false;
            teleportCountDown = 5f;
        }
    }

    
    
}
