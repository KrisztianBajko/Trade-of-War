using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyHealth : MonoBehaviour
{
    public Slider enemySlider3D;

    public Stats statScript;
    void Start()
    {
        enemySlider3D = GetComponentInChildren<Slider>();


        enemySlider3D.maxValue = statScript.maxHealth;
        statScript.health = statScript.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        enemySlider3D.value = statScript.health;
    }
}
