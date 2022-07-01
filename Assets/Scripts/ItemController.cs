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
    public bool collectable;
    public bool isPotion;
    public bool isRedPill;
    public bool isWeapon;

    public int healthValue;

    public GameObject itemContainer;

    public GameObject player;


    //https://mixkit.co/free-sound-effects/sword/ Sword blade attack in medieval battle

    public void Start()
    {
        InitItem();
    }

    public void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) && pickUpAllowed && !isHandUsed))
        {
            PickUpItem();
        }
        if(Input.GetKeyDown(KeyCode.G) && isHandUsed)
        {
            DropItem(); 
        }
    }

    public void InitItem()
    {
        isWeapon = item.isWeapon;
        isRedPill = item.isRedPill;
        isPotion = item.isPotion;
        collectable = item.collectable;
        healthValue = item.heathValue;
        gameObject.name = item.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;

        pickUpText.gameObject.SetActive(false);
    }

    public void PickUpItem()
    {
        if(isWeapon)
        {
            gameObject.GetComponent<Floating>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.transform.parent = itemContainer.transform;
            gameObject.transform.position = player.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sprite = null;

            player.GetComponent<PlayerScript>().damage += player.GetComponent<PlayerScript>().level;

            player.GetComponent<PlayerScript>().isHandUsed = true;
            isHandUsed = true;
        }
        else if(collectable)
        {
            player.GetComponent<Collecting>().SetItemAmount(item);
            Destroy(gameObject);
        }
        else if(isPotion)
        {
            player.GetComponent<PlayerScript>().maxHealth += 2;
            player.GetComponent<PlayerScript>().health += healthValue;
            Destroy(gameObject);
        }
        else if(isRedPill)
        {
            Destroy(gameObject);
            player.GetComponent<PlayerScript>().isPsychoCam = true;
            player.GetComponent<PlayerScript>().level++;
            ScenenManager.GoToGameScene();
        }

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
        player = collision.gameObject;
        itemContainer = player.transform.GetChild(0).gameObject;

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
