using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack:MonoBehaviour
{
    public int intervalRound;
    public int repeatAttack = 1;
    protected int currentRepeatAttack = 0;
    int currentIntervalRound = 0;
    public int readyRound = 0;
    int currentReadyRound = 0;
    public int finishRound = 0;
    int currentFinishRound = 0;
    protected float gridSize;
    
    public bool isStandingAttacking = false;
    public bool isStanding = false;
    public virtual bool isReadyToAttack()
    {
        if (currentReadyRound == readyRound)
        {
            return true;
        }
        if (currentReadyRound == 0)
        {
            doReady();
        }
        currentReadyRound++;
        return false;
    }

    protected bool isReadying()
    {
        return currentReadyRound > 0;
    }


    public virtual void doReady() { }
    protected virtual void doFinish() { }
    protected virtual bool isAttackFinished()
    {
        if (currentFinishRound >= finishRound)
        {

            currentReadyRound = 0;
            doFinish();
            currentFinishRound = 0;
            return true;
        }
        if (currentFinishRound == 0)
        {

            doFinish();
        }
        currentFinishRound++;
        return false;
    }
    public virtual bool shouldAttack()
    {

        if (GetComponent<EnemyController>().isDead)
        {
            return false;

        }
        if (currentRepeatAttack > 0)
        {
            
            return true;
        }
        if (currentIntervalRound >= intervalRound)
        {
            currentIntervalRound = 0;
            return true;
        }
        currentIntervalRound++;
        return false;
    }
    protected virtual void doAttack()
    {

    }
    public virtual void attack()
    {
        if (isReadyToAttack())
        {
            if (isStandingAttacking)
            {
                isStanding = true;
            }
            doAttack();
            currentRepeatAttack++;
            if (currentRepeatAttack >= repeatAttack)
            {
                currentRepeatAttack = 0;

                if (isAttackFinished())
                {
                    if (isStandingAttacking)
                    {
                        isStanding = false;
                    }
                }
            }
        }
        else
        {
            if (isStandingAttacking)
            {
                isStanding = true;
            }
        }
    }
    protected virtual void Start()
    {
        gridSize = GameMaster.Instance.gridSize;
    }
}