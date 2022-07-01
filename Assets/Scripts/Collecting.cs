using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Collecting : MonoBehaviour
{
    public Item item;

    public int collectingMax;
    public int collectingAmount;

    public bool collected;

    public string itemName;

    public GameObject collectingUI;

    public void Start()
    {
        itemName = item.name;
    }

    public void Update()
    {
        if (gameObject.GetComponent<PlayerScript>().isPsychoCam)
            collectingUI.SetActive(true);
        else
            collectingUI.SetActive(false);

        ItemsCollected();

        if (collectingUI != null)
            collectingUI.GetComponent<TextMeshProUGUI>().SetText(item.name + ": " + collectingAmount.ToString() + "/" + collectingMax);
    }


    public void SetItemAmount(Item item)
    {
        if(this.item.name == item.name)
        {
            collectingAmount += 1;
        }
    }

    public void ItemsCollected()
    {
        if(collectingAmount >= collectingMax)
        {
            collected = true;
        }
        else
        {
            collected = false;
        }
    }

}
