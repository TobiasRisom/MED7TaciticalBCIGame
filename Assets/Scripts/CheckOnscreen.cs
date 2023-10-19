using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnscreen : MonoBehaviour
{
    public GameObject[] targets;
    private GameObject[] guides;
    private List<GameObject> heads = new List<GameObject>();
    public List<float> distancesToEnemy = new List<float>();
    public float shortestDistance;
    private GameObject playerCharacter;
    private GameObject nearestTarget;
    public GameObject nearestTargetName;
    private Camera cam;
    private Vector3 compassDirection = new Vector3(0, 0, 0);
    public Vector3 arrowTipScreenPos;
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

    void targetLocationSetup()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        guides = GameObject.FindGameObjectsWithTag("enemyCompass");
        playerCharacter = GameObject.FindWithTag("Player");
        foreach (GameObject e in targets)
        {
            heads.Add(e.transform.Find("head").gameObject);
        }
        for (int i = 0; i < targets.Length; i++)
        {
            distancesToEnemy.Add(Vector3.Distance(playerCharacter.transform.position, heads[i].transform.position));
        }
        shortestDistance = 10;
    }

    void locateNearestEnemy()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (distancesToEnemy[i] >= shortestDistance)
            {
                shortestDistance = distancesToEnemy[i];
                nearestTarget = heads[i];
                nearestTargetName = targets[i];
            }
        }
        for (int i = 0; i < targets.Length; i++)
        {
            if (distancesToEnemy[i] <= shortestDistance && targets[i].GetComponent<EnemyHealth>().alive == true)
            {
                shortestDistance = distancesToEnemy[i];
                nearestTarget = heads[i];
                nearestTargetName = targets[i];
            }
        }
    }

    void updateDistances()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            distancesToEnemy[i] = Vector3.Distance(playerCharacter.transform.position, heads[i].transform.position);
        }
    }

    void pointToNearestEnemy()
    {
        compassDirection = ((playerCharacter.transform.position + nearestTargetName.transform.position) * 0.5f);
        compassDirection = ((playerCharacter.transform.position + compassDirection) * 0.5f);
        transform.position = compassDirection;
        transform.LookAt(nearestTargetName.transform.position);
    }

    void hideCompass()
    {
        if (nearestTarget.GetComponent<Renderer>().isVisible == false)
        {
            for (int i = 0; i < guides.Length; i++)
            {
                guides[i].GetComponent<Renderer>().enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < guides.Length; i++)
            {
                guides[i].GetComponent<Renderer>().enabled = false;
            }
        }
    }
    void checkScreenPosition()
    {
        arrowTipScreenPos = cam.WorldToScreenPoint(guides[0].transform.position);
        screenHeight = Screen.height;
        screenWidth = Screen.width;
    }
}
