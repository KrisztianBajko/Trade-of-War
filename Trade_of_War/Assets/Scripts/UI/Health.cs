using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Health : MonoBehaviour
{
    public Slider playerSlider3D;
    public Slider playerSlider2D;
    public TextMeshProUGUI healthText;
    private Stats playerStatScript;

    void Start()
    {
        playerStatScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>();
        playerSlider2D.maxValue = playerStatScript.maxHealth;
        playerSlider3D.maxValue = playerStatScript.maxHealth;
        playerStatScript.health = playerStatScript.maxHealth;
    }

    void Update()
    {
        healthText.text = playerStatScript.health + " / " + playerStatScript.maxHealth;
        playerSlider2D.value = playerStatScript.health;
        playerSlider3D.value = playerSlider2D.value;
    }
}
