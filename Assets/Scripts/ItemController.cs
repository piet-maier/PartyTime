using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemController : MonoBehaviour
{
    public Item item;
    public Text pickUpText;

    public bool pickUpAllowed;
    public bool isHandUsed;

    public GameObject itemContainer;

    public GameObject player;


    //https://mixkit.co/free-sound-effects/sword/ Sword blade attack in medieval battle

    public void Start()
    {
        //SpawnItem(InitItem());
        gameObject.name = item.name;
        gameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;

        //pickUpText.gameObject.SetActive(false);
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
        //gameObject.GetComponent<Text>().text = item.name;
        //gameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        
        return gameObject;
    }

    public void SpawnItem(GameObject obj)
    {
        //Instantiate(obj).transform.position = gameObject.transform.position;
    }

    //public void PickUpItem() //neues objekt option
    //{
        
    //    DestroyGameObject(gameObject);
    //    GameObject childObject = Instantiate(itemInHand);
    //    childObject.transform.parent = itemContainer.transform;
    //    childObject.transform.position = player.transform.position;
    //    childObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
    //    childObject.name = item.name;
    //    Debug.Log("picked up");
    //    isHandUsed = true;


    //}

    public void PickUpItem()
    {
        gameObject.GetComponent<Floating>().enabled = false;
        gameObject.transform.parent = itemContainer.transform;
        gameObject.transform.position = player.transform.position;
        gameObject.GetComponent<SpriteRenderer>().sprite = null;

        player.GetComponent<PlayerScript>().isHandUsed = true;

        Debug.Log("picked up");
        isHandUsed = true;
        


    }


    public void DropItem()
    {
        gameObject.transform.parent = itemContainer.transform.parent.parent;
        gameObject.transform.position = player.transform.position;
        gameObject.GetComponent<Floating>().enabled = true;
        gameObject.GetComponent<Floating>().posOffset = player.transform.position;
        gameObject.GetComponent<SpriteRenderer>().sprite = item.itemSprite;

        player.GetComponent<PlayerScript>().isHandUsed = false;

        isHandUsed = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //pickUpText.gameObject.SetActive(true);
            pickUpAllowed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("exit");
        if (collision.gameObject.CompareTag("Player"))
        {
            //pickUpText.gameObject.SetActive(false);
            pickUpAllowed = false;
        }
    }

    //public void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log("collision");

    //    Player player = collision.GetComponent<Player>();

    //    Debug.Log(player);

    //    if ((player != null) && (Input.GetKeyDown(KeyCode.E)))
    //    {
    //        pickUpItem();
    //        gameObject.GetComponent<AudioSource>().Play();

    //    }

    //}


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (Input.GetKeyDown(KeyCode.E))
    //    {
    //        pickUpItem();
    //    }
    //}

    void DestroyGameObject(GameObject obj)
    {
        Destroy(obj);
    }

}
