using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Door : EventActivator
{
    string roomId;
    // Start is called before the first frame update
    void Start()
    {
        roomId = GetComponentInParent<RoomEnemyGenerator>().roomId;
        EventPool.OptIn<string>("clearRoom", clearRoom);
    }
    void clearRoom(string id)
    {
        EventPool.OptOut<string>("clearRoom", clearRoom);
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
