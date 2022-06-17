using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new NPC", menuName = "NPC")]
public class NPC : ScriptableObject
{
    public Transform transform;
    public new string name;
    public string category;
    public int health;
    public int damage;
    public int level;
    public int scoreValue;
    public float speed;
    public float spawnTime;
    public float hitCD;
    public float aggroRange;
    public float attackRange;
    

    public int counter;

    public Sprite sprite;


    public void Awake()
    {
        Debug.Log(name + " ist erschienen");
    }


    public void OnDestroy()
    {
        Debug.Log(name + " ist gestorben");
    }
    
   
    

}
