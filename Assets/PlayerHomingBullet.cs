using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHomingBullet : PoolObject
{
    public int damage = 1;
    Vector3 dir;
    public float speed = 20f;
    Transform target;
    Rigidbody2D rb;
    public float rotateSpeed = 200f;
    Vector2 direction ;
    public float homingTime = 5f;
    float currentHomingTimer;
    public float homingRadius = 5f;

    Transform GetClosestEnemy(Collider2D[] enemies, Transform fromThis)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = fromThis.position;
        foreach (Collider2D potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }
        return bestTarget;
    }

    void findTarget()
    {
        LayerMask mask = LayerMask.GetMask("enemy");
        var colliders = Physics2D.OverlapCircleAll(transform.position, homingRadius, mask);
        if (colliders.Length > 0)
        {
            var selected = GetClosestEnemy(colliders, transform);
            target = selected;
        }
        else
        {
            target = null;
        }
    }
    public void Init(Vector3 pos, int d = 1)
    {
        transform.position = pos;

        dir = new Vector3(0, 1, 0);
        currentHomingTimer = 0;
        findTarget();
        direction = new Vector2(0, 1);
        //float angle = Mathf.Atan2(di.y, di.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        damage = d;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerController>().transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentHomingTimer += Time.deltaTime;
        if(target && target.gameObject.active)
        {

            direction = (Vector2)target.position - rb.position;
        }
        else
        {
            if (currentHomingTimer < homingTime)
            {
                findTarget();
            }
            //direction = new Vector2(0, 1);
        }

        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.up * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<EnemyController>();
        if (enemy && !enemy.isImmortal)
        {
            enemy.getDamage(damage);
            returnBack();
        }

        else if (collision.tag == "boundary")
        {
            returnBack();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnTriggerEnter2D(collision.collider);
    }
}