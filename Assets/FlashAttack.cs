using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlashType { direction}
public class FlashAttack : EnemyAttack
{
    public GameObject flashPrefab;
    public GameObject attackPrefab;
    public FlashType flashType;

    public Transform readyParent;
    public Transform attackParent;

    protected override void Start()
    {
        base.Start();
    }

    // Start is called before the first frame update
    public override bool shouldAttack()
    {

        if (isReadying())
        {
            return true;
        }
        if (GetComponent<EnemyController>().isDead)
        {
            return false;

        }
        if (currentRepeatAttack > 0)
        {

            return true;
        }
        var playerIndexPosition = MoveController.Instance.positionOfPlayer();
        var enemyIndexPosition = MoveController.Instance.positionOfObject(gameObject);
        if (playerIndexPosition.x == enemyIndexPosition.x || playerIndexPosition.y == enemyIndexPosition.y)
        {

            return true;
        }
        return false;
    }

    int distanceToWall(Vector2 dir)
    {
        int layerMask = LayerMask.GetMask("wall");

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 20, layerMask);
        // Does the ray intersect any objects excluding the player layer
        if (hit)
        {
            //Debug.DrawRay(transform.position, dir * 100 * 0.9f, Color.yellow);
            //Debug.Log("Did Hit");
            return Utils.distanceCenterToIndex(gridSize, (hit.point - (Vector2)transform.position).magnitude);
        }
        else
        {
            //Debug.DrawRay(transform.position, dir * 100 * 0.9f, Color.red);
            //Debug.Log("Did not Hit");
            //Debug.LogError("does not hit wall?");
            return 20;
        }
    }

    public override void doReady()
    {
        base.doReady();
        var playerIndexPosition = MoveController.Instance.positionOfPlayer();
        var enemyIndexPosition = MoveController.Instance.positionOfObject(gameObject);
        if (playerIndexPosition.x == enemyIndexPosition.x || playerIndexPosition.y == enemyIndexPosition.y)
        {
            //verticle flash
            var dir = (playerIndexPosition - enemyIndexPosition).normalized;
            var distance = distanceToWall(dir);
            if (distance > 0)
            {

                for (int i = 1; i <= distance; i++)
                {
                    var position = dir * i;
                    var blade = Instantiate(flashPrefab, transform.position + (Vector3)position * gridSize, Quaternion.identity, readyParent);
                }
            }
            else
            {

                Debug.LogError("distance to wall is too small " + distance);
            }
        }
    }

    protected override void doAttack()
    {
        foreach (Transform readyChild in readyParent)
        {

            var blade = Instantiate(attackPrefab, readyChild.position, Quaternion.identity, attackParent);
        }
        Utils.destroyAllChildren(readyParent);
    }
    protected override void doFinish()
    {

        Utils.destroyAllChildren(attackParent);
    }
}
