using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoring : MonoBehaviour
{
    public GameObject highscoreDisplay;
    public GameObject nameDisplay;
    public GameObject player;

    public void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if(player != null)
        {

        }
  
    }
}
