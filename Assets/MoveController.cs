using Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : Singleton<MoveController>
{
    PlayerController player;
    List<EnemyController> enemies;
    List<PlayerStepBullet> playerBullets;

    Dictionary<Vector2, GameObject> positionToObject;

    Dictionary<GameObject, Vector2> objectToPosition;

    // Start is called before the first frame update
    void Awake()
    {
        enemies = new List<EnemyController>();
        playerBullets = new List<PlayerStepBullet>();
    }
    private void Start()
    {

        player = FindObjectOfType<PlayerController>();
        EventPool.OptIn("Beat", Move);
    }

    public void Move()
    {
        //move bullet 

        foreach (var enemy in playerBullets)
        {
            Vector2 oldPosition = objectToPosition[enemy.gameObject];
            positionToObject[oldPosition] = null;
            Vector2 newPosition =  enemy.Move();
            positionToObject[newPosition] = enemy.gameObject;
        }
        //move enemy 
        foreach (var enemy in enemies)
        {
            enemy.Move();
        }
        //move player
    }

    public void addEnemy(EnemyController enemy)
    {
        enemies.Add(enemy);
    }

    public void removeEnemy(EnemyController enemy)
    {
        enemies.Remove(enemy);
    }

    public void addBullet(PlayerStepBullet bullet)
    {
        playerBullets.Add(bullet);
    }

    public void removeBullet(PlayerStepBullet bullet)
    {
        playerBullets.Remove(bullet);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
