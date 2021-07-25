using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventActivator:MonoBehaviour
{
    string roomId;
    protected virtual void Start()
    {
        roomId = GetComponentInParent<RoomEnemyGenerator>().roomId;
        EventPool.OptIn<string>("clearRoom", clearRoom_internal);
    }


    void clearRoom_internal(string id)
    {
        if (id == roomId)
        {
            EventPool.OptOut<string>("clearRoom", clearRoom_internal);
            clearRoom();
        }
    }

    protected virtual void clearRoom()
    {

    }
}