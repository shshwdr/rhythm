using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : EnemyAttack
{
    protected int damage = 1;
    public float shootTime = 0.3f;
    float currentShootTimer = 0f;
    public Vector2 dir = new Vector2(1, 0);
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

    public override void attack()
    {
        base.attack();
        Shoot();
    }
    protected virtual void Shoot()
    {
        generateNormalBullet(dir, new Vector3(0, 0, 0));
    }
}
