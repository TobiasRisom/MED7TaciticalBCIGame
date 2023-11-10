using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spinAndSpawn : MonoBehaviour
{
    public GameObject target;
    public GameObject jango; //the object all the clones are cloned from, get it?
    public float decorTimer;
    public float maxTime;
    public float killTime; //these timing objects are assigned in unity
    private float spawnAngle;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("ParticleRefPoint");
        jango = GameObject.Find("Ember");
    }

    // Update is called once per frame
    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        randomizeCloneSpawn();
        spawnEmbers();
    }

    void spawnEmbers()
    {
        if (decorTimer >= maxTime)
        {
            GameObject emberClone = Instantiate(jango);
            emberClone.transform.position = transform.position;
            Destroy(emberClone, killTime);
            decorTimer = 0;
        }
        decorTimer += Time.deltaTime;
    }

    void randomizeCloneSpawn()
    {
        spawnAngle = Random.Range(100f, 300f);
        transform.RotateAround(target.transform.position, Vector3.up, spawnAngle * Time.deltaTime);
    }
}
