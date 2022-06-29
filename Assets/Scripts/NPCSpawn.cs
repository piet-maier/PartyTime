using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCSpawn : MonoBehaviour
{
    public GameObject npcPrefabs;

    public float spawnTime;

    public int npcsAlive;
    public int maxNpcs;

    public bool isSpawning;

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
        {
            StartCoroutine(StartCooldown());
            var npc = Instantiate(npcPrefabs);

            npc.AddComponent<PathFinding>();

            npc.transform.SetParent(gameObject.transform);
            npc.transform.position = gameObject.transform.position;

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