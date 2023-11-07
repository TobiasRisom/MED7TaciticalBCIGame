using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSpin : MonoBehaviour
{
    private GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // Spin the object around the target at 20 degrees/second.
        transform.RotateAround(target.transform.position, Vector3.up, 180 * Time.deltaTime);
    }
}
