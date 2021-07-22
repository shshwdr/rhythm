using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack:MonoBehaviour
{

    protected int readyRound = 0;
    int currentReadyRound = 0;
    protected int finishRound = 0;
    int currentFinishRound = 0;
    protected float gridSize;
    protected bool isStandingAttacking;
    public virtual bool isReadyToAttack()
    {
        if (currentReadyRound >= readyRound)
        {
            currentReadyRound = 0;
            return true;
        }
        currentReadyRound++;
        return false;
    }

    protected virtual bool isAttackFinished()
    {
        if (currentFinishRound >= finishRound)
        {
            currentFinishRound = 0;
            return true;
        }
        currentFinishRound++;
        return false;
    }
    protected virtual bool shouldAttack()
    {
        return true;
    }
    protected virtual void doAttack()
    {

    }
    public virtual void attack()
    {
        if (isStandingAttacking)
        {
            if (isAttackFinished())
            {
                isStandingAttacking = false;
                return;
            }
        }
        if (shouldAttack())
        {

            if (isReadyToAttack())
            {
                doAttack();
            }
        }
    }
    private void Start()
    {
        gridSize = GameMaster.Instance.gridSize;
    }
}