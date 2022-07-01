using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBeContinue : MonoBehaviour
{
    public GameObject npcPrefab;

    public GameObject player;

    public GameObject npcspawn;

    public bool pillspawned;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        npcspawn = GameObject.FindGameObjectWithTag("spawnArea");
        pillspawned = false;
    }

    public void Update()
    {
        if(player.GetComponent<Collecting>().collected && npcspawn.GetComponent<NPCSpawn>().bossKilled && !pillspawned)
        {
            
            var item = Instantiate(npcPrefab);
            pillspawned = true;
            player.GetComponent<Collecting>().collectingAmount = 0;

            item.transform.SetParent(gameObject.transform);
            item.transform.position = gameObject.transform.position;
            

            
        }
        else
        {

        }
    }
}
