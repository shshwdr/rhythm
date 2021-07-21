using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isInGame = false;
    PlayerController player;

    public RoomEnemyGenerator currentRoom;
    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        if (player.transform.position.y >= 1.6 && !isInGame)
        {
            EventPool.Trigger(EventPool.startGameEvent);
            isInGame = true;

        }
    }

    public void stopGame()
    {
        if (isInGame)
        {
            EventPool.Trigger(EventPool.stopGameEvent);
            isInGame = false;

        }

    }
}
