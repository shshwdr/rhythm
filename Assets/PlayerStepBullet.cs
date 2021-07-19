using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStepBullet : PoolObject
{
    public int damage = 1;
    Vector3 dir;
    public float speed = 20f;
    Rigidbody2D rb;
    protected float gridSize;
    public void Init(Vector3 pos, Vector3 di, int d = 1)
    {
        transform.position = pos;

        dir = di;



        float angle = Mathf.Atan2(di.y, di.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        damage = d;
        gridSize = GameMaster.Instance.gridSize;

        MoveController.Instance.addBullet(this);
    }
    bool checkIfCollide(Vector3 dir)
    {
        int layerMask = LayerMask.GetMask("wall");

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics2D.Raycast(transform.position, dir, gridSize * 0.9f, layerMask))
        {
            Debug.DrawRay(transform.position, dir * 100 * 0.9f, Color.yellow);
            Debug.Log("Did Hit");
            return true;
        }
        else
        {
            Debug.DrawRay(transform.position, dir * 100 * 0.9f, Color.red);
            Debug.Log("Did not Hit");
            return false;
        }
    }
    public virtual bool Move(Vector3 dir)
    {
        //do extra work for enemy
        bool willCollider = checkIfCollide(dir);
        if (!willCollider)
        {
            rb.MovePosition(rb.position + (Vector2)dir * gridSize);
           // transform.Translate(dir * gridSize);
                //transform.DOMove(transform.position + dir * gridSize, moveTime);//.SetEase(Ease.OutBack);
            
            return true;
        }
        else
        {
            dir = -dir;
            return false;
        }

    }
    public Vector2 Move()
    {
        bool willCollider = checkIfCollide(dir);
        if (!willCollider)
        {
            rb.MovePosition(rb.position + (Vector2)dir * gridSize);
            return  Utils.positionToGridIndexCenter2d(gridSize, (Vector3)rb.position + dir * gridSize);
            // transform.Translate(dir * gridSize);
            //transform.DOMove(transform.position + dir * gridSize, moveTime);//.SetEase(Ease.OutBack);

            //return true;
        }
        else
        {
            returnBack();
            return Vector2.negativeInfinity;
            //dir = -dir;
            //return false;
        }
    }
    // Start is called before the first frame update
    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    rb.MovePosition(rb.position + speed * (Vector2)dir * Time.deltaTime);
    //    //transform.Translate(speed * dir * Time.deltaTime);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    var enemy = collision.GetComponent<EnemyController>();
    //    if (enemy && !enemy.isImmortal)
    //    {
    //        enemy.getDamage(damage);
    //        returnBack();
    //    }

    //    else if (collision.tag == "boundary")
    //    {
    //        returnBack();
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    OnTriggerEnter2D(collision.collider);
    //}
    public override void returnBack()
    {
        base.returnBack();

        MoveController.Instance.removeBullet(this);
    }
}
