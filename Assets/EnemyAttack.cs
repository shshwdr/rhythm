﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyAttack:MonoBehaviour
{

    protected int readyRound = 0;
    int currentReadyRound = 0;
    protected float gridSize;

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
    public virtual void attack()
    {

    }
    private void Start()
    {
        gridSize = GameMaster.Instance.gridSize;
    }
}