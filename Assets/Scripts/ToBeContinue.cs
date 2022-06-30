using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToBeContinue : MonoBehaviour
{
    public GameObject npcPrefab;

    public GameObject player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        if(player.GetComponent<Collecting>().collected)
        {
            player.GetComponent<Collecting>().collectingAmount = 0;
            var item = Instantiate(npcPrefab);

            item.transform.SetParent(gameObject.transform);
            item.transform.position = gameObject.transform.position;
            

            
        }
        else
        {

        }
    }
}
