using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpriteMove : MonoBehaviour
{
    bool shouldMove;
    Vector3 previous;
    Vector3 newOne;
    void Start()
    {
        //EventPool.OptIn("Beat", Beat);
    }
    public void Move(Vector3 p,Vector3 n)
    {
        previous = p;
        newOne = n;
        shouldMove = true;
        //transform.position = previous;
    }
    private void OnPostRender()
    {

        if (shouldMove)
        {
            transform.position += previous - newOne;
            transform.DOLocalMove(Vector3.zero, 0.1f);
            shouldMove = false;
        }
    }
    private void LateUpdate()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
