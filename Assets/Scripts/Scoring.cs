using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public GameObject player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(player.GetComponent<PlayerScript>().highscore.ToString());
        player.GetComponent<SpriteRenderer>().sprite = null;
        Destroy(player);
    }


}
