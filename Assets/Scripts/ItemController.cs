using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public Item item;
    public TextMeshProUGUI pickUpText;

    public bool pickUpAllowed;
    public bool isHandUsed;

    public GameObject itemContainer;

    public GameObject player;


    //https://mixkit.co/free-sound-effects/sword/ Sword blade attack in medieval battle

    public void Start()
    {
        InitItem();
    }

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E)) && pickUpAllowed && !isHandUsed)
        {
            PickUpItem();

           

        }
        if(Input.GetKeyDown(KeyCode.G) && isHandUsed)
        {
            DropItem(); 
        }
    }

    public GameObject InitItem()
    {
        gameObject.name = item.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;

        pickUpText.gameObject.SetActive(false);

        return gameObject;
    }

    public void PickUpItem()
    {
        gameObject.GetComponent<Floating>().enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.transform.parent = itemContainer.transform;
        gameObject.transform.position = player.transform.position;
        gameObject.GetComponent<SpriteRenderer>().sprite = null;

        player.GetComponent<PlayerScript>().isHandUsed = true;
        isHandUsed = true;
    }


    public void DropItem()
    {
        gameObject.transform.parent = itemContainer.transform.parent.parent;
        gameObject.transform.position = player.transform.position;
        gameObject.GetComponent<Floating>().enabled = true;
        gameObject.GetComponent<Floating>().posOffset = player.transform.position;
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;

        player.GetComponent<PlayerScript>().isHandUsed = false;
        isHandUsed = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pickUpText.gameObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pickUpText.gameObject.SetActive(false);
            pickUpAllowed = false;
        }
    }
}
