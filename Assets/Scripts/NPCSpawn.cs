using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public GameObject npcPrefabs;

    public float spawnTime;

    public int npcsAlive;
    public int maxNpcs;

    public bool isSpawning;

    public void Start()
    {
        isSpawning = false;
    }

    public void Update()
    {

        if (!isSpawning && npcsAlive < maxNpcs)
        {
            StartCoroutine(StartCooldown());
            var npc = Instantiate(npcPrefabs);
            npc.transform.SetParent(gameObject.transform);
            npcsAlive++;
        }
    }
    public IEnumerator StartCooldown()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnTime);
        isSpawning = false;
    }

}
