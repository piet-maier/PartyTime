using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NPCSpawn : MonoBehaviour
{
    public GameObject npcPrefab;
    public GameObject bossPrefab;

    public int bossesAlive;
    public int maxBosses;

    public int npcsAlive;
    public int maxNpcs;
    public float spawnTime;
    public bool isSpawning;
    public bool spawnAllowed;

    public bool bossAlive;
    public bool collected;
    public bool bossKilled;

    public GameObject player;



    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        
        maxNpcs = player.GetComponent<PlayerScript>().level;

        isSpawning = false;
        spawnAllowed = true;
        bossAlive = false;
        bossKilled = false;
    }



    public void Update()
    {
        collected = player.GetComponent<Collecting>().collected;

        if (!bossAlive && collected && !bossKilled && bossPrefab != null)
        {
            SpawnBoss();
            
        }
        

        if (!isSpawning && npcsAlive < maxNpcs && spawnAllowed && npcPrefab != null)
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
        var npc = Instantiate(npcPrefab);

        npc.transform.SetParent(gameObject.transform);
        npc.transform.position = gameObject.transform.position;

        npcsAlive++;
    }

    public void SpawnBoss()
    {

        var boss = Instantiate(bossPrefab);
        bossAlive = true;

        boss.transform.SetParent(gameObject.transform);
        boss.transform.position = gameObject.transform.position;

        bossesAlive++;
    }
}