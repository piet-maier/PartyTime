using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collecting : MonoBehaviour
{
    public Item item;

    public int collectingMax;
    public int collectingAmount;

    public string itemName;

    public GameObject collectingUI;

    public void Start()
    {
        itemName = item.name;
    }

    public void Update()
    {
        collectingUI.GetComponent<TextMeshProUGUI>().SetText(item.name + ": " + collectingAmount.ToString() + "/" + collectingMax);
    }


    public void SetItemAmount(Item item)
    {
        if(this.item.name == item.name)
        {
            collectingAmount += 1;
        }
    }
}
