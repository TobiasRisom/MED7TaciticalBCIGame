using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AmmoSpawn : MonoBehaviour
{
    // Spawns the ammo graphic above the player's head to signify how many attacks they have
    public GameObject ammo;
    private PlayerFeatures playerFeatures;
    private Transform player; 
    private float manaCount; 
    private float radius = 0.5f; 
    private float height = 1f;

    void Start()
    {
        playerFeatures = GameObject.Find("Player").GetComponent<PlayerFeatures>();
        player = GameObject.Find("Player").transform;
        manaCount = playerFeatures.mana;
        SpawnAmmo();
    }

    public void SpawnAmmo()
    {
        manaCount = playerFeatures.mana;
        for (int i = 0; i < manaCount; i++)
        {
            float angle = i * (2 * Mathf.PI) / manaCount;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;

            Vector3 spawnPosition = player.position + new Vector3(x, height, z);
            Quaternion spawnRotation = Quaternion.Euler(0, angle * Mathf.Rad2Deg, 0);

            GameObject newAmmo = Instantiate(ammo, spawnPosition, spawnRotation);
            newAmmo.transform.parent = transform;
        }
    }

    public void DeleteAmmo()
    {
        foreach (Transform child in transform) 
        {
	        GameObject.Destroy(child.gameObject);
        }   
    }
}
