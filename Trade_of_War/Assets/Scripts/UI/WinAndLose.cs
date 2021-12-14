using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinAndLose : MonoBehaviour
{
    public Image textBackGround;
    public TextMeshProUGUI text;
    public GameObject winAndLoseScreen;

    private void OnDisable()
    {
        if(transform.gameObject.name == "EnemyBase")
        {
            winAndLoseScreen.SetActive(true);
            text.text = "Victory";
            textBackGround.color = Color.green;
            Time.timeScale = 0f;
           
        }
        else if(transform.gameObject.name == "FriendlyBase")
        {
            winAndLoseScreen.SetActive(true);
            text.text = "You Lost";
            textBackGround.color = Color.red;
            Time.timeScale = 0f;
        }
    }
}
