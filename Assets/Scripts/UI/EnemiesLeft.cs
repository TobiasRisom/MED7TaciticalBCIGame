using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EnemiesLeft : MonoBehaviour
{
    public GameObject[] enemies;
    private TextMeshProUGUI enemiesLeftTMP;
    // Start is called before the first frame update
    void Start()
    {
        enemiesLeftTMP = this.GetComponent<TextMeshProUGUI>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemiesLeftTMP.text = "Enemies Left: " + enemies.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        enemiesLeftTMP.text = "Enemies Left: " + enemiesAlive;
    }
}
