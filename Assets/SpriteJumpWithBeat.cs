using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SpriteJumpWithBeat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("Beat", Beat);
    }
    void Beat()
    {
        transform.DOLocalJump(Vector3.zero, 0.08f, 1, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
