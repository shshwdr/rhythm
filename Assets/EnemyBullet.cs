using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : PoolObject
{
    public int damage = 1;
    Vector3 dir;
    public float speed = 20f;
    Rigidbody2D rb;
    public void Init(Vector3 pos, Vector3 di, int d = 1)
    {
        transform.position = pos;

        dir = di;



        float angle = Mathf.Atan2(di.y, di.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        damage = d;
    }

    // Start is called before the first frame update
    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + speed * (Vector2)dir * Time.deltaTime);
        //transform.Translate(speed * dir * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var enemy = collision.GetComponent<PlayerController>();
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