using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SpriteJumpWithBeat : MonoBehaviour
{
    EnemyController enemyController;
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("Beat", Beat);
        enemyController = GetComponentInParent<EnemyController>();
    }
    void Beat()
    {
        if (!enemyController || enemyController.isActive)
        {
            transform.DOLocalJump(Vector3.zero, 0.15f, 1, 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
