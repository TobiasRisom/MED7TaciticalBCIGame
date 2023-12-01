using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SharedDatastructures;
using TMPro;
using UnityEngine;

public class EnemiesLeft : MonoBehaviour
{
    private GameMode gameMode;
    public GameObject[] enemies;
    private TextMeshProUGUI enemiesLeftTMP;
    // Start is called before the first frame update

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        gameMode = gameManager.GetComponent<GameMode>();
        enemiesLeftTMP = this.GetComponent<TextMeshProUGUI>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(gameMode.danish == false)
        {
            enemiesLeftTMP.text = "Enemies Left: " + enemies.Length;
        }
        else
        {
            enemiesLeftTMP.text = "Goblins Tilbage: " + enemies.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckForRemainingEnemies();
    }

    public void CheckForRemainingEnemies()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        int enemiesAlive = 0;
        foreach(GameObject e in enemies)
        {
            if(e.GetComponent<EnemyHealth>().health == 0)
            {
                continue;
            }
            enemiesAlive++;
        }
        if(gameMode.danish == false)
        {
            enemiesLeftTMP.text = "Enemies Left: " + enemies.Length;
        }
        else
        {
            enemiesLeftTMP.text = "Goblins Tilbage: " + enemies.Length;
        }
    }
}
