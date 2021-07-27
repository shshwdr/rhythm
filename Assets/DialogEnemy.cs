using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEnemy : MonoBehaviour
{

    public void readyDialog()
    {
        foreach(var attack in GetComponents<EnemyAttack>())
        {
            attack.enabled = false;
        }
        GetComponent<EnemyController>().enabled = false;
        GetComponent<Usable>().enabled = true;


    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Usable>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
