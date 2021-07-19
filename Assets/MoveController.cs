using Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : Singleton<MoveController>
{
    PlayerController player;
    List<EnemyController> enemies;
    List<StepBullet> playerBullets;

    Dictionary<Vector2, GameObject> positionToObject = new Dictionary<Vector2, GameObject>();

    Dictionary<GameObject, Vector2> objectToPosition = new Dictionary<GameObject, Vector2>();
    float gridSize;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = new List<EnemyController>();
        playerBullets = new List<StepBullet>();
    }
    private void Start()
    {

        gridSize = GameMaster.Instance.gridSize;
        player = FindObjectOfType<PlayerController>();
        EventPool.OptIn("Beat", Move);
    }

    public void Move()
    {
        //move bullet 

        foreach (var enemy in playerBullets)
        {
            //Vector2 oldPosition = objectToPosition[enemy.gameObject];
            Vector2 newPosition = enemy.Move();
            //if (!newPosition.Equals(Vector2.negativeInfinity))
            {
                // setPosition(oldPosition, null);
                //  setPosition(newPosition, enemy.gameObject);
            }
        }
        //move enemy 
        foreach (var enemy in enemies)
        {

            Vector2 newPosition = enemy.Move();
            if (!newPosition.Equals(Vector2.negativeInfinity))
            {
                Vector2 oldPosition = objectToPosition[enemy.gameObject];

                setPosition(oldPosition, null);
                setPosition(newPosition, enemy.gameObject);
            }
        }

        foreach (var enemy in playerBullets)
        {
            enemy.checkAttack();
        }
        //move player
    }

    public void addPlayer()
    {

        Vector2 position = Utils.positionToGridIndexCenter2d(gridSize, player.transform.position);
        setPosition(position, player.gameObject);
    }

    public void updatePlayer(Vector2 newPosition)
    {
        if (!newPosition.Equals(Vector2.negativeInfinity))
        {

            Vector2 oldPosition = objectToPosition[player.gameObject];
            setPosition(oldPosition, null);
            setPosition(newPosition, player.gameObject);
        }
    }

    public void addEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);
        Vector2 position = Utils.positionToGridIndexCenter2d(gridSize, enemy.transform.position);
        setPosition(position, enemy.gameObject);
    }

    public void removeEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
        Vector2 position = objectToPosition[enemy.gameObject];
        setPosition(position, null);
    }

    void setPosition(Vector2 position, GameObject obj)
    {
        positionToObject[position] = obj;
        if (obj != null)
        {

            objectToPosition[obj] = position;
        }
        int index = 0;
        if (obj == null)
        {

        }
        else if (obj.GetComponent<EnemyController>())
        {
            index = 1;
        }
        else if (obj.GetComponent<PlayerStepBullet>())
        {

            index = 2;
        }
        updateDebugUI(position, index);
    }
    public void addBullet(StepBullet bullet)
    {
        playerBullets.Add(bullet);
        //Vector2 position = Utils.positionToGridIndexCenter2d(gridSize, bullet.transform.position);
        //setPosition(position, bullet.gameObject);
    }

    public void removeBullet(StepBullet bullet)
    {
        StartCoroutine(delayRemoveBullet(bullet));
    }

    IEnumerator delayRemoveBullet(StepBullet bullet)
    {
        yield return new WaitForEndOfFrame();
        playerBullets.Remove(bullet);
        //Vector2 position = objectToPosition[bullet.gameObject];
        //setPosition(position, null);
    }

    public void updateDebugUI(Vector2 index, int item)
    {
        switch (item)
        {
            case 0:

                break;
        }
    }

    public GameObject checkPositionItem(Vector2 position)
    {
        if (positionToObject.ContainsKey(position))
        {
            return positionToObject[position];
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
