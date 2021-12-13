using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectile : MonoBehaviour
{

    public float damage;
    public GameObject target;

    public bool targetSet;
    public string targetType;
    public float velocity = 5f;
    public bool stopProjectile;

    private void Update()
    {
        if (target)
        {
            if(target == null)
            {
                Destroy(gameObject);
            }

            transform.position = Vector3.MoveTowards(transform.position, target.transform.transform.position, velocity * Time.deltaTime);

            if (!stopProjectile)
            {
                if(Vector3.Distance(transform.position,target.transform.position) < 0.5f)
                {
                    if(targetType == "Player")
                    {
                        target.GetComponent<Stats>().health -= damage;
                        target.GetComponent<Teleport>().isTeleporting = false;
                        stopProjectile = true;
                        Destroy(gameObject);
                    }
                    else if(targetType == "Minion")
                    {
                        target.GetComponent<Stats>().health -= damage;
                        stopProjectile = true;
                        Destroy(gameObject);
                    }
                }
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
