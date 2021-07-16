using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBox : MonoBehaviour
{
    public bool isCoin;

    public float moveTime = 1f;
    Vector3 targetPosition;
    // Start is called before the first frame update
    void Start()
    {
        var position = FindObjectOfType<PlaySprites>().transform.position;
        Vector3 screenPoint = position + new Vector3(0, 0, 5);  //the "+ new Vector3(0,0,5)" ensures that the object is so close to the camera you dont see it

        //find out where this is in world space
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(screenPoint);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerInventory>();
        if (player)
        {
            if (isCoin)
            {
                player.addCoin();
            }
            else
            {
                player.addAbility();
            }
            GetComponent<PoolObject>().returnBack();
        }
        //var player = collision.GetComponent<PlayerInventory>();
        //if (player)
        //{
        //    transform.DOMove(targetPosition, moveTime)
        //        .SetEase(Ease.InBack)
        //        .OnComplete(() =>
        //        {

        //        });
        //}
    }
}
