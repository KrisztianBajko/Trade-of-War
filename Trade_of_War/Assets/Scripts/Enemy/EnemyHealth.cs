using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealth : MonoBehaviour
{
    private Slider enemySlider3D;

    public Stats enemyStatScript;
    void Start()
    {
        enemySlider3D = GetComponentInChildren<Slider>();

        enemySlider3D.maxValue = enemyStatScript.maxHealth;
        enemyStatScript.health = enemyStatScript.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        enemySlider3D.value = enemyStatScript.health;
    }
}
