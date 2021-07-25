using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Door : EventActivator
{
    protected override void clearRoom()
    {
        gameObject.SetActive(false);
    }
}
