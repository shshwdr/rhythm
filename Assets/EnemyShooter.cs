using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyAttack
{
    protected int damage = 1;
    public float shootTime = 0.3f;
    float currentShootTimer = 0f;
    public List<Vector2> dirs = new List<Vector2>() { new Vector2(1, 0) };
    //void Update()
    //{
    //    currentShootTimer += Time.deltaTime;
    //    if (currentShootTimer >= shootTime)
    //    {
    //        currentShootTimer = 0;
    //        Shoot();
    //    }
    //}

    protected void generateNormalBullet(Vector3 dir, Vector3 startOffset = new Vector3())
    {
        var bullet = ObjectPooler.Instance.GetPooledObject("enemyBullet");
        bullet.GetComponent<EnemyStepBullet>().fetch();
        bullet.GetComponent<EnemyStepBullet>().Init(transform.position + startOffset, dir, damage);
    }


    protected override void doAttack()
    {
        Shoot();
    }
    protected virtual void Shoot()
    {
        foreach(var dir in dirs)
        {

            generateNormalBullet(dir, dir * gridSize);
        }
    }
}
