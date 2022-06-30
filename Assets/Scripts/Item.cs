using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public new string name; //ohne new wird object.name ver�ndert

    public int heathValue;

    public bool collectable;
    public bool isPotion;
    public bool isRedPill;
    public bool isWeapon;

    public Sprite itemSprite;
    
    public AudioClip pickUpSound;
}
