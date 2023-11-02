using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraTutorialController : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 offset;
    private Vector3 tutorialOffset;
    private TutorialManager TM;

    private void Start()
    {
        offset = new Vector3(transform.position.x - target.position.x, 0, transform.position.z - target.position.z);
        TM = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if(TM.currentArea >= 1 || TM.currentArea <= 3)
        {
            transform.position = new Vector3(
            target.position.x + offset.x,
            transform.position.y + offset.y,
            target.position.z + offset.z);
        }

        if(TM.currentArea == 4)
        {
            transform.position = new Vector3(16.7000008f,0,0.300000012f);
        }

        if(TM.currentArea == 5)
        {
            transform.position = new Vector3(34.4000015f,0,1f);
        }
    }
}

/*

Låser cam til en fast stilling efter targets placering og flytter kamera I forhold til target. 
I det her tilfælde er det playeren. 
I stedet for at gøre det til et child af det. 

*/
