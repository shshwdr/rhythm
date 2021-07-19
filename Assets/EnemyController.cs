using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : HPObjectController
{
    public Vector2 movingDir;
    bool isCoin;
    bool isAbility;
    bool isBoss;
    public bool ignoreTimeControl = false;
    protected PlayerController player;
    public GameObject coin;
    public GameObject ability;

    public bool chasePlayer;
    public int moveStep = 1;
    int currentMoveStep = 0;
    public bool canMoveDiagnal;
    bool canAttack;
    


    Vector3 originalPosition;
    public virtual void init(Vector3 dir,bool c, bool a ,bool b,int multi)
    {
        movingDir = dir;
        isCoin = c;
        isAbility = a;
        isBoss = b;
        maxHp = maxHp * multi;
        hp = maxHp;

        if (coin && ability)
        {
            if (isCoin)
            {
                coin.SetActive(true);
                ability.SetActive(false);
            }
            else if (isAbility)
            {
                ability.SetActive(true);
                coin.SetActive(false);
            }
            else
            {

                coin.SetActive(false);

                ability.SetActive(false);
            }
        }
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        player = FindObjectOfType<PlayerController>();
        if (ignoreTimeControl)
        {
            originalPosition = transform.position;
            EventPool.OptIn(EventPool.stopGameEvent, restartGame);
        }
        animator = GetComponentInChildren<Animator>();
        //EventPool.OptIn("Beat", Move);
        //animator.SetFloat("speed", 1);
        moveMode = 1;
        MoveController.Instance. addEnemy(this);
    }

    void restartGame()
    {
        transform.position = originalPosition;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (GameManager.Instance.isInGame)
        {

            base.Update();
        }
    }
    public void Move()
    {
        currentMoveStep++;
        if (currentMoveStep >= moveStep)
        {
            currentMoveStep = 0;

            if (chasePlayer)
            {
                movingDir = Utils.chaseDir2d(transform.position, player.transform.position);
            }

            bool succeed = base.Move(movingDir.normalized);
            if (!succeed)
            {
                movingDir = -movingDir;
            }
            //transform.Translate(movingDir.normalized * GameMaster.Instance.gridSize);
        }
        else
        {
            Debug.Log("test");
        }

    }
    private void LateUpdate()
    {
        //if (GameManager.Instance.isInGame)
        //{
        //    var speed = moveSpeed;
        //    if (ignoreTimeControl)
        //    {
        //        if(player.GetComponent<EnemyGenerator>().showMoveLevel <= player.GetComponent<EnemyGenerator>().currentDifficulty)
        //        {

        //            // Vector3 position = new Vector3(player.transform.position.x, rb.position.y + movingDir.y * speed * 0.02f, transform.position.z);
        //            transform.Translate(movingDir * speed * 0.02f);
        //            transform.position = new Vector3(player.transform.position.x, transform.position.y, transform.position.z);
        //            if (Mathf.Abs(transform.position.y - player.transform.position.y) <= 10)
        //            {
        //                player.getDamage(1);
        //                EventPool.Trigger("killedByShadow");
        //                if (player.isDead)
        //                {

        //                }
        //            }
        //            // rb.MovePosition(position);
        //        }
        //    }
        //    else
        //    {
        //        //rb.MovePosition(rb.position + movingDir * speed * Time.fixedDeltaTime);

        //    }
        //    testFlip(movingDir);
        //    // rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<PlayerController>();
        if (player)
        {
            player.getDamage(1);
        }
    }


    protected override void Die()
    {
        if (isCoin)
        {
            GameObject go = ObjectPooler.Instance.GetPooledObject("coin");
            go.GetComponent<PoolObject>().fetch();
            go.transform.position = transform.position;
        }else if (isAbility)
        {
            GameObject go = ObjectPooler.Instance.GetPooledObject("ability");
            go.GetComponent<PoolObject>().fetch();
            go.transform.position = transform.position;
        }
        GetComponent<PoolObject>().returnBack();

        List<string> explosions = new List<string>() { "explosion", "explosion1", "explosion2", "explosion3", "explosion4", };
        var selectedExplosion = explosions[Random.Range(0,explosions.Count)];
        GameObject exp = ObjectPooler.Instance.GetPooledObject(selectedExplosion);
        exp.GetComponent<PoolObject>().fetch();
        exp.transform.position = transform.position;
    }
}
