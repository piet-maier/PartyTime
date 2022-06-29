using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCSpawn : MonoBehaviour
{
    public GameObject npcPrefabs;

    public int npcsAlive;
    public int maxNpcs;
    public float spawnTime;
    public bool isSpawning;

    //Grid
    public Vector3 worldSize;
    public Vector3 worldPosition;
    public Tilemap[] obstacles;

    public void Start()
    {
        isSpawning = false;
    }

    public void Update()
    {
        if (!isSpawning && npcsAlive < maxNpcs)
            StartCoroutine(StartCooldown());
    }

    public IEnumerator StartCooldown()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnTime);
        SpawnNPC();   
        isSpawning = false;
    }

    public void SpawnNPC()
    {
        var npc = Instantiate(npcPrefabs);

        npc.AddComponent<PathFinding>();

        npc.transform.SetParent(gameObject.transform);
        npc.transform.position = gameObject.transform.position;

        npcsAlive++;
    }
}