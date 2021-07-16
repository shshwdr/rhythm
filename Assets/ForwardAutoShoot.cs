using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardAutoShoot : AutoShoot
{
    protected override void Start()
    {
        levelToBulletStartOffset = new List<List<Vector3>>() {
        new List<Vector3>() { new Vector3() },
        new List<Vector3>() { new Vector3(-0.3f,0,0),new Vector3(0.3f,0,0) },
        new List<Vector3>() { new Vector3(-0.4f,0,0),new Vector3(0.4f,0,0),new Vector3(0,0,0) },
    };
        base.Start();
    }
    protected override void Shoot()
    {
        foreach(var offset in allBulletsStartOffset)
        {

            generateNormalBullet(new Vector3(0, 1, 0),offset);
        }
    }
}
