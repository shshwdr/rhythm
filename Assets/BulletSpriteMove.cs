using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpriteMove : MonoBehaviour
{
    void Start()
    {
        //EventPool.OptIn("Beat", Beat);
    }
    public void Move(Vector3 previous,Vector3 newone)
    {
        //transform.position = previous;
        transform.position += previous - newone;
        transform.DOLocalMove(Vector3.zero,0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
