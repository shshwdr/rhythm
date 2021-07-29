using Doozy.Engine.Progress;
using Doozy.Engine.UI;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : HPObjectController
{
    public Vector2 movingDir;
    public bool isBoss;
    public bool ignoreTimeControl = false;
    protected PlayerController player;
    public GameObject coin;
    public GameObject ability;

    public bool chasePlayer;
    public int moveStep = 1;
    int currentMoveStep = 0;
    public bool canMoveDiagnal;
    bool canAttack;

    Progressor progressor;

    public RoomEnemyGenerator room;
    public int attackOrder;//0 one by one // 1 sequence
    int currentAttackId = 0;

    Vector3 originalPosition;
    public virtual void init(Vector3 dir,bool c, bool a ,bool b,int multi)
    {
        movingDir = dir;
        isBoss = b;
        maxHp = maxHp * multi;
        hp = maxHp;

    }

    public void activate()
    {
        isActive = true;
        if (isBoss)
        {
            progressor = GameManager.Instance.bossHPBar;
            progressor.GetComponent<UIView>().Show();
            progressor.SetMax(maxHp);
            progressor.SetValue(maxHp);
        }

        animator.enabled = true;
    }

    public void disactivate()
    {
        isActive = false;

        if (isBoss)
        {
            progressor = FindObjectOfType<Progressor>();
            progressor.GetComponent<UIView>().Hide();
            progressor.SetMax(maxHp);
        }
        animator.enabled = false;
    }

    public override void getDamage(float damage = 1, string element = "")
    {
        base.getDamage(damage, element);
        if (isDead)
        {
            if (isBoss)
            {
                progressor.GetComponent<UIView>().Hide();
            }
        }
        else
        {

            progressor.SetValue(hp);
            animator.SetTrigger("hit");
        }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        player = FindObjectOfType<PlayerController>();
        originalPosition = transform.position;
        animator = GetComponentInChildren<Animator>();
        animator.enabled = false;
        //EventPool.OptIn("Beat", Move);
        //animator.SetFloat("speed", 1);
        moveMode = 1;
        MoveController.Instance.addEnemy(this);
        if (attackOrder == 1)
        {
            currentAttackId = -1;
        }
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


    public override Vector2 Move(Vector3 dir)
    {


        //do extra work for enemy
        bool willColliderWall = checkIfCollideWall(dir);
        GameObject willCollideObject = checkIfCollide(dir);
        bool willCollideEnemy = willCollideObject&& willCollideObject.GetComponent<EnemyController>();
        bool willCollidePlayer = willCollideObject&& willCollideObject.GetComponent<PlayerController>();
        if (willCollidePlayer)
        {
            player.getDamage(1);
            return Utils.positionToGridIndexCenter2d(gridSize, (Vector3)rb.position + dir * gridSize); ;
        }
        else if (!willColliderWall && !willCollideEnemy)
        {

            //detect if collide
            //transform.Translate(dir * gridSize);
            if (moveMode == 0)
            {

                rb.MovePosition(rb.position + (Vector2)dir * gridSize);

                // transform.Translate(dir * gridSize);
                //transform.DOMove(transform.position + dir * gridSize, moveTime);//.SetEase(Ease.OutBack);
            }
            else if (moveMode == 1)
            {

                rb.MovePosition(rb.position + (Vector2)dir * gridSize);

                // transform.Translate(dir * gridSize);
                //transform.DOJump(transform.position + dir * gridSize, 0.4f, 1, moveTime);
            }
            return Utils.positionToGridIndexCenter2d(gridSize, (Vector3)rb.position + dir * gridSize);
            //return true;
        }
        else
        {
            dir = -dir;
            return Vector2.negativeInfinity;
            //return false;
        }

    }

    public void finishAttack()
    {
        if (attackOrder == 0)
        {
            var attacks = GetComponents<EnemyAttack>();
            currentAttackId++;
            currentAttackId = currentAttackId % attacks.Length;
        }
        else
        {
            currentAttackId = -1;
        }
    }

    public Vector2 Move()
    {
        if (!isActive)
        {
            return Vector2.negativeInfinity;
        }

        if (isDead)
        {
            return Vector2.negativeInfinity;

        }
        var attacks = GetComponents<EnemyAttack>();
        var attack = GetComponent<EnemyAttack>();
        if(attacks.Length>1 && !isBoss)
        {
            Debug.LogError("not boss but get multiple attacks " + gameObject.name);
        }
        if (attacks.Length >= 1)
        {
            
            if(attackOrder == 0)
            {
                //one by one
                attack = attacks[currentAttackId]; 
            }
            else
            {

            }
        }
        if (attack)
        {
            if (attack.shouldAttack())
            {
                attack.attack();


            }
            if (attack.isStanding)
            {
                return Vector2.negativeInfinity;
            }
        }

        currentMoveStep++;
        if (currentMoveStep >= moveStep)
        {
            currentMoveStep = 0;

            if (chasePlayer)
            {
                movingDir = Vector3.zero;
                movingDir = Utils.chaseDir2d(transform.position, player.transform.position);
            }

            var res  = Move(movingDir.normalized);
            if (res.Equals(Vector2.negativeInfinity))
            {


                if (chasePlayer)
                {
                    movingDir = Vector3.zero;
                    movingDir = Utils.chaseDir2dSecond(transform.position, player.transform.position);
                    res = Move(movingDir.normalized);
                }

                movingDir = -movingDir;
            }
            return res;
            //transform.Translate(movingDir.normalized * GameMaster.Instance.gridSize);
        }
        else
        {
           // Debug.Log("test");
        }

        return Vector2.negativeInfinity;

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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    var player = collision.GetComponent<PlayerController>();
    //    if (player)
    //    {
    //        player.getDamage(1);
    //    }
    //}


    protected override void Die()
    {
        base.Die();
        var dialog = GetComponent<DialogEnemy>();
        if (dialog)
        {
            dialog.readyDialog();
            room.cleanRoom();
            room.clearRoom();

            animator.SetTrigger("hit");
        }
        else
        {

            animator.SetTrigger("die");

            List<string> explosions = new List<string>() { "explosion", "explosion1", "explosion2", "explosion3", "explosion4", };
            var selectedExplosion = explosions[Random.Range(0, explosions.Count)];
            GameObject exp = ObjectPooler.Instance.GetPooledObject(selectedExplosion);
            exp.GetComponent<PoolObject>().fetch();
            exp.transform.position = transform.position;
            room.enemyDie(this);
            StartCoroutine(remove());
        }

    }

    IEnumerator remove()
    {
        yield return new WaitForSeconds(0.5f);
        MoveController.Instance.removeEnemy(this);
        GetComponent<PoolObject>().returnBack();
    }

    public void reset()
    {
        GetComponent<PoolObject>().fetch();
        MoveController.Instance.addEnemy(this);
        transform.position = originalPosition;
        var attacks = GetComponents<EnemyAttack>();
        foreach(var attack in attacks)
        {
            attack.doFinish();
        }
        getHeal();
        isDead = false;
    }

    public void softReset()
    {
        MoveController.Instance.updateEnemy(this,originalPosition);
        transform.position = originalPosition;
        getHeal();
    }
}
