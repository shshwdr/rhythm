using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZoneController : MonoBehaviour
{
    public GameObject fullZone;
    public GameObject playerClone;
    public GameObject shadow;
    public GameObject fireBoss;
    public GameObject house;

    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();

        EventPool.OptIn(EventPool.stopGameEvent, showPlayerClone);
        EventPool.OptIn(EventPool.startGameEvent, startGame);
        EventPool.OptIn("killedByShadow", showShadow);

        EventPool.OptIn("boss show", showBoss);
        EventPool.OptIn("buyHouse", buyHouse);
        
    }
    void startGame()
    {
        fullZone.SetActive(false);
    }

    void showShadow()
    {
        shadow.SetActive(true);
    }

    void showPlayerClone()
    {
        fullZone.SetActive(true);
        playerClone.SetActive(true);
    }

    void showBoss()
    {
        fireBoss.SetActive(true);
    }

    void buyHouse()
    {
        house.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
