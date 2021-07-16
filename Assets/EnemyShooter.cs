using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    protected int damage = 1;
    public float shootTime = 0.3f;
    float currentShootTimer = 0f;
    public bool isHorizontal = true;
    void Update()
    {
        currentShootTimer += Time.deltaTime;
        if (currentShootTimer >= shootTime)
        {
            currentShootTimer = 0;
            Shoot();
        }
    }

    protected void generateNormalBullet(Vector3 dir, Vector3 startOffset = new Vector3())
    {
        var bullet = ObjectPooler.Instance.GetPooledObject("enemyBullet");
        bullet.GetComponent<EnemyBullet>().fetch();
        bullet.GetComponent<EnemyBullet>().Init(transform.position + startOffset, dir, damage);
    }
    protected virtual void Shoot() {
        if (isHorizontal)
        {

            generateNormalBullet(new Vector3(1, 0, 0), new Vector3(0, 0, 0));
            generateNormalBullet(new Vector3(-1, 0, 0), new Vector3(0, 0, 0));
        }
        else
        {

            generateNormalBullet(new Vector3(0, -1, 0), new Vector3(0, 0, 0));
        }
    }
}
