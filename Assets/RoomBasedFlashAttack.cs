using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBasedFlashAttack : FlashAttack
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        var upDis = distanceToWall(Vector2.up);
        var downDis = distanceToWall(Vector2.down);
        var leftDis = distanceToWall(Vector2.left);
        var rightDis = distanceToWall(Vector2.right);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
