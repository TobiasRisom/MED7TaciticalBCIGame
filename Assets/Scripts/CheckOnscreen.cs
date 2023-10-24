using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnscreen : MonoBehaviour
{
    public GameObject[] targets; //Array for tracking Goblins
    private GameObject[] guides; //Array storing the objects which make up the guide arrow
    private List<GameObject> heads = new List<GameObject>(); //List of the heads of each goblin, used to check if they are onscreen
    public List<float> distancesToEnemy = new List<float>(); //List containing the distance between the player and each enemy as floats
    public float shortestDistance; //Float used to store the distance between the player and the nearest goblin
    private GameObject playerCharacter; //Used to track the playercharacter
    private GameObject nearestTarget;   //Used to mark the head of the nearest enemy, need to track the head because the enemy has no renderer
    public GameObject nearestTargetName; //used to mark the nearest enemy
    private Camera cam; //Used to track the camera
    private Vector3 compassDirection = new Vector3(0, 0, 0); //Used to track and control the position of the guide arrow
    public Vector3 arrowTipScreenPos; //track position of the tip of the guide arrow
    public float screenHeight;
    public float screenWidth;
    // Start is called before the first frame update
    void Start()
    {
        targetLocationSetup();
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        updateDistances();
        locateNearestEnemy();
        pointToNearestEnemy();
        hideCompass();
        checkScreenPosition();
    }

    void targetLocationSetup()//placing objects in the appropriate lists
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");//puts goblins in an array in the order they are "ordered" in unity
        guides = GameObject.FindGameObjectsWithTag("enemyCompass");//places the guide arrow componets in a an array
        playerCharacter = GameObject.FindWithTag("Player");//track player character
        foreach (GameObject e in targets)//find the head of each goblin and add the head to the "heads" list, in order
        {
            heads.Add(e.transform.Find("head").gameObject);
        }
        for (int i = 0; i < targets.Length; i++)//add the distances between the heads and the player to the "distancesToEnemy" list, in order
        {
            distancesToEnemy.Add(Vector3.Distance(playerCharacter.transform.position, heads[i].transform.position));
        }
        shortestDistance = 10;//setting shortestDistance to 10 just as starting point
    }

    void locateNearestEnemy()
    {//the first for loop finds the longest distance and second loop finds the shortest distance
        for (int i = 0; i < targets.Length; i++)
        {//The first loop is necesary to ensure the shortest distance is found after each player turn
            if (distancesToEnemy[i] >= shortestDistance)
            {
                shortestDistance = distancesToEnemy[i];
                nearestTarget = heads[i];
                nearestTargetName = targets[i];
            }
        }
        for (int i = 0; i < targets.Length; i++)
        {//this loop ignores the positions of enemies which are marked as dead so that the guidearrow does not point at corpses
            if (distancesToEnemy[i] <= shortestDistance && targets[i].GetComponent<EnemyHealth>().alive == true)
            {
                shortestDistance = distancesToEnemy[i];
                nearestTarget = heads[i];
                nearestTargetName = targets[i];
            }
        }
    }

    void updateDistances()
    {//whenever the positions of enemies change, the distancesToEnemy list is updated
        for (int i = 0; i < targets.Length; i++)
        {
            distancesToEnemy[i] = Vector3.Distance(playerCharacter.transform.position, heads[i].transform.position);
        }
    }

    void pointToNearestEnemy()
    {//function for controlling the guide arrow's position
        compassDirection = ((playerCharacter.transform.position + nearestTargetName.transform.position) * 0.5f);//find midpoint between player and nearest goblin
        compassDirection = ((playerCharacter.transform.position + compassDirection) * 0.5f);//find halfway point between player and midpoint
        transform.position = compassDirection;// set arrow position to halfway point between player and midpoint
        transform.LookAt(nearestTargetName.transform.position);// make arrow "face"/point to nearest enemy
    }

    void hideCompass()
    {//checks if the head/renderer of the nearest enemy is onscreen
        if (nearestTarget.GetComponent<Renderer>().isVisible == false)
        {//if not onscreen, show guide arrow
            for (int i = 0; i < guides.Length; i++)
            {
                guides[i].GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {//else hide the guide arrow
            for (int i = 0; i < guides.Length; i++)
            {
                guides[i].GetComponent<Renderer>().enabled = false;
            }
        }
    }
    void checkScreenPosition()
    {//this function tracks the screen position of the tip of the guide arrow, it is not really needed it is a remnant of something I replaced
        arrowTipScreenPos = cam.WorldToScreenPoint(guides[0].transform.position);
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }
}
