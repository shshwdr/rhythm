using PixelCrushers.DialogueSystem;
using Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public bool isDialog;
    float startTime;
    PlayerInventory player;
    int coffeeId = 0;
    int coffeeCost = 10;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.unscaledTime;
        player = FindObjectOfType<PlayerInventory>();
    }

    public void buyOneCoffee()
    {
        player.spendCoin();
        coffeeId += 1;
        DialogueLua.SetVariable("coffeeId", coffeeId);
        coffeeCost *= 2;
        DialogueLua.SetVariable("coffeeCost", coffeeCost);
        player.addAbility(true);

        if(coffeeId == 2)
        {
            EventPool.Trigger("buyHouse");
        }
    }


    // Update is called once per frame
    void Update()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(Time.unscaledTime - startTime);
        string timeText = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        DialogueLua.SetVariable("timeFromStart", timeText);
    }
}
