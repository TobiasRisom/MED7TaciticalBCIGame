using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOnscreen : MonoBehaviour
{
    public GameObject[] targets;
    public List<GameObject> heads = new List<GameObject>();
    public List<float> distancesToEnemy = new List<float>();
    public float shortestDistance;
    public GameObject playerCharacter;
    private GameObject nearestTarget;
    public GameObject nearestTargetName;
    float distance;

    public Vector3 compassDirection = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        targetLocationSetup();
    }

    // Update is called once per frame
    void Update()
    {
        updateDistances();
        locateNearestEnemy();
        pointToNearestEnemy();
        //if (target1.GetComponent<Renderer>().isVisible == false)
        //{
        //    print("Gobling1(1) is not currently visable");
        //}
    }

    void targetLocationSetup()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        playerCharacter = GameObject.FindWithTag("Player");
        foreach (GameObject e in targets)
        {
            //print(e.transform.FindChild("head"));
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
            if (distancesToEnemy[i] <= shortestDistance)
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
        transform.position = compassDirection;
        transform.rotation = Quaternion.LookRotation(compassDirection);
    }
}
