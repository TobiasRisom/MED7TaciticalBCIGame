using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{

    public int enemiesKilled = 0;
    public int totalEnemies = 6; // Change this to the total number of enemies in your game.
    public Text objectiveText; // Reference to your UI text element.
    private  EnemyHealth enemy;


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    void UpdateObjectiveText()
{
    objectiveText.text = "Objective: " + enemiesKilled + " / " + totalEnemies + " enemies killed";
}

    // Update is called once per frame
    void Update()
    {
              // Subscribe to the event.
    EnemyHealth.OnEnemyKilled += UpdateObjectiveText;

    // Update the initial objective text.
    UpdateObjectiveText();
    }
}
