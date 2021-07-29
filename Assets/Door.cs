using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Door : EventActivator
{
    public Sprite closedSprite;
    public Sprite openedSprite;
    public override void clearRoom()
    {
        //gameObject.SetActive(false);
        openDoor();
    }

    public void openDoor()
    {

        GetComponent<SpriteRenderer>().sprite = openedSprite;
        GetComponent<Collider2D>().enabled = false;
    }

    public void closeDoor()
    {

        GetComponent<SpriteRenderer>().sprite = closedSprite;
        GetComponent<Collider2D>().enabled = true;
    }
}
