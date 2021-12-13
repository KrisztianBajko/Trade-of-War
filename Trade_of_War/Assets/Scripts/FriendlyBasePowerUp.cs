using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyBasePowerUp : MonoBehaviour
{

    private GameObject player;
    public float healthRegenRate;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        

        if(player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < 8f)
            {
                if(player.GetComponent<Stats>().health > 0 && player.GetComponent<Stats>().health < player.GetComponent<Stats>().maxHealth)
                {
                    player.GetComponent<Stats>().health += healthRegenRate * Time.deltaTime;
                }
                

                

            }
           
        }
    }
   
}
