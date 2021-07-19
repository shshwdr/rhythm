using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepBullet:PoolObject
{

    public virtual Vector2  Move()
    {
        return Vector2.zero;
    }
    public virtual void checkAttack() { }
}