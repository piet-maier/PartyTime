using System.Collections;
using UnityEngine;

public class NPCSpawn : MonoBehaviour
{
    public GameObject[] npcPrefabs;

    public float spawnTime;

    public int npcsAlive;

    public bool isSpawning;

    public void Start()
    {
        for (var i = 0; i < npcPrefabs.Length; i++)
        {
            var npc = Instantiate(npcPrefabs[i]);
            npc.transform.SetParent(gameObject.transform);
            npcsAlive++;
        }
    }

    public void Update()
    {
        if (npcsAlive < npcPrefabs.Length && !isSpawning)
            for (var i = 0; i < npcPrefabs.Length; i++)
            {
                var npc = Instantiate(npcPrefabs[i]);
                npc.transform.SetParent(gameObject.transform);
                npcsAlive++;
                StartCoroutine(StartCooldown());
            }
    }

    public IEnumerator StartCooldown()
    {
        isSpawning = true;
        yield return new WaitForSeconds(spawnTime);
        isSpawning = false;
    }
}