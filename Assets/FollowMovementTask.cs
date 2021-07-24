using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovementTask : MonoBehaviour
{

    public int[] instructionList;
    public int currentInstructionId = 0;
    public bool finishedTask;
    float gridSize;
    // Start is called before the first frame update
    void Start()
    {
        EventPool.OptIn("Beat", beat);
        gridSize = GameMaster.Instance.gridSize;
    }

    void beat()
    {
        Vector2 dir = Vector2.zero; ;
        switch (instructionList[currentInstructionId])
        {
            case 1:
                dir = new Vector2(-1, 0);
                break;
            case 2:
                dir = new Vector2(1, 0);
                break;
            case 3:
                dir = new Vector2(0,1);
                break;
            case 4:
                dir = new Vector2(0,-1);
                break;
            case -1:
                dir = Vector2.zero;
                break;
        }
        dir *= gridSize;
        transform.Translate(dir);
        currentInstructionId++;
        if (currentInstructionId >= instructionList.Length)
        {
            currentInstructionId = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
