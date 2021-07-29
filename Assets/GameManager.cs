using Doozy.Engine.Progress;
using Doozy.Engine.UI;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isInGame = true;
    PlayerController player;

    public Progressor bossHPBar;

    public UIView gameoverView;
    public RoomEnemyGenerator currentRoom;
    // Start is called before the first frame update
    void Start()
    {

        player = FindObjectOfType<PlayerController>();
        isInGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)&& !isInGame)
        {
            gameoverView.Hide();
            //ObjectPooler.Instance.all
            EventPool.Trigger(EventPool.startGameEvent);

            isInGame = true;
        }
        //if (player.transform.position.y >= 1.6 && !isInGame)
        //{
        //    EventPool.Trigger(EventPool.startGameEvent);
        //    isInGame = true;

        //}
    }

    public void stopGame()
    {
        if (isInGame)
        {
            EventPool.Trigger(EventPool.stopGameEvent);
            isInGame = false;

            gameoverView.Show();
        }

    }
}
