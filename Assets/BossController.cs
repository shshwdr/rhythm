using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : EnemyController
{
    enum BossState { chase, prepare, dash, wait }
    BossState state;

    public float finishChasingDistance = 4.5f;
    public float finishDashingDistance = 5f;

    public float prepareTime = 5f;
    public float prepareAllowDistanceOff = 0.5f;
    public float prepareSpeedAdjust = 3f;
    float currentPrepareTime = 0f;
    public float waitTime = 3f;

    public float chaseSpeedAdd = 3f;
    public float dashSpeedAdd = 3f;

    public Vector3 rotateCenter;
    public float waitSpeedMinus = 2f;

    public override void init(Vector3 dir, bool c, bool a, bool b,int multi)
    {
        base.init(dir, c, a, b,multi);
        state = BossState.chase;
        currentPrepareTime = 0;
        EventPool.Trigger("boss show");
    }

    protected override void Die()
    {
        for (int i = 0; i < 6; i++)
        {
            GameObject go = ObjectPooler.Instance.GetPooledObject("coin");
            go.GetComponent<PoolObject>().fetch();
            go.transform.position = Utils.randomVector3_2d(transform.position, 1f);
        }
        GetComponent<PoolObject>().returnBack();
        player.GetComponent<EnemyGenerator>().returnBoss();
    }
    protected override void Start()
    {
        base.Start();
            player = FindObjectOfType<PlayerController>();
        //animator.SetFloat("speed", 1);
    }
    protected override void Update()
    {
        if (GameManager.Instance.isInGame)
        {
            base.Update();
            if((transform.position - player.transform.position).magnitude< finishChasingDistance && state == BossState.chase)
            {
                state = BossState.prepare;

                currentPrepareTime = 0;
            }
            else if(state == BossState.prepare)
            {
                currentPrepareTime += Time.deltaTime;
                if (currentPrepareTime > prepareTime)
                {
                    movingDir = (player.transform.position - transform.position).normalized; 
                    state = BossState.dash;
                }
            }else if(state == BossState.dash && (transform.position - player.transform.position).magnitude > finishDashingDistance)
            {

                state = BossState.wait;
                currentPrepareTime = 0;
                rotateCenter = player.transform.position;
            }
            else if (state == BossState.wait)
            {
                currentPrepareTime += Time.deltaTime;
                if (currentPrepareTime > waitTime)
                {
                    state = BossState.chase;
                }
            }
        }
    }
    float distanceToPlayer()
    {
        return (transform.position - player.transform.position).magnitude;
    }
    private void LateUpdate()
    {
        if (GameManager.Instance.isInGame)
        {
            var speed = moveSpeed;
            switch (state)
            {
                case BossState.chase:
                    speed = player.moveSpeed + chaseSpeedAdd;
                    movingDir = (player.transform.position - transform.position).normalized;
                    break;
                case BossState.prepare:
                    speed = player.moveSpeed;
                    float distance = distanceToPlayer();
                    if (distance <= finishChasingDistance - prepareAllowDistanceOff)
                    {
                        speed -= prepareSpeedAdjust;
                    }else if(distance>= finishChasingDistance + prepareAllowDistanceOff)
                    {

                        speed += prepareSpeedAdjust;
                    }
                    movingDir = (player.transform.position - transform.position).normalized;
                    break;
                case BossState.dash:
                    speed = player.moveSpeed + dashSpeedAdd;
                    //movingDir = (player.transform.position - transform.position).normalized;
                    break;
                case BossState.wait:
                    speed = player.moveSpeed - waitSpeedMinus;
                    movingDir = new Vector3(0,1, 0);
                    //rotateCenter = player.transform.position;
                    //Quaternion q = Quaternion.AngleAxis(rotateDegree*Time.deltaTime, Vector3.up);
                    //rb.MovePosition(q * (rb.transform.position - rotateCenter) + rotateCenter);
                    //return;
                    //rb.MoveRotation(rb.transform.rotation * q);
                    //movingDir = (player.transform.position - transform.position).normalized;
                    break;
            }

            rb.MovePosition(rb.position + movingDir * speed * Time.fixedDeltaTime);

            testFlip(movingDir);
        }
        // rb.velocity = new Vector2(movement.x * moveSpeed, movement.y * moveSpeed);
    }

}
