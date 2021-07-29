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

        for (int i = 0; i < 20; i++)
        {

            var blade = Instantiate(flashPrefab, readyParent);
            blade.SetActive(false);
        }
        for (int i = 0; i < 20; i++)
        {

            var blade = Instantiate(attackPrefab, attackParent);
            blade.SetActive(false);
        }
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

    protected int distanceToWall(Vector2 dir)
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
                    int index = i - 1;
                    if (index < readyParent.childCount)
                    {
                        var blade = readyParent.GetChild(index);
                        blade.transform.position = transform.position + (Vector3)position * gridSize;
                        blade.gameObject.SetActive(true);
                    }
                    else
                    {
                        break;
                    }
                    //var blade = Instantiate(flashPrefab, transform.position + (Vector3)position * gridSize, Quaternion.identity, readyParent);
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

        animator.SetTrigger("shoot");
        int index = 0;
        foreach (Transform readyChild in readyParent)
        {

            if (readyChild.gameObject.active && index < readyParent.childCount)
            {
                var blade = attackParent.GetChild(index);
                blade.transform.position = readyChild.position;
                blade.gameObject.SetActive(true);
                index++;
            }
            else
            {
                break;
            }
        }
        Utils.setActiveOfAllChildren(readyParent);
    }
    public override void doFinish()
    {

        Utils.setActiveOfAllChildren(attackParent);
    }
}
