using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Item", menuName = "Item")]
public class Item : ScriptableObject
{
    public Sprite itemSprite;

    public new string name; //ohne new wird object.name ver�ndert

    public AudioClip pickUpSound;
}
