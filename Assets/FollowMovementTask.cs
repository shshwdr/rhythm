using PixelCrushers.DialogueSystem;
using Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMovementTask : MonoBehaviour
{

    public int[] instructionList;
    public int currentInstructionId = 0;
    public bool finishedTask;
    float gridSize;
    bool isActive = false;
    GameMaster player;
    //Collider2D collider;
    DialogueSystemTrigger dialogue;
    Usable dialogUse;
    public GameObject followText;
    // Start is called before the first frame update
    void Start()
    {
        gridSize = GameMaster.Instance.gridSize;
        player = FindObjectOfType<GameMaster>();
        //dialogue = GetComponent<DialogueSystemTrigger>();
        dialogUse = GetComponent<Usable>();
        dialogUse.enabled = false;
        //dialogue.enabled = false;
    }

    public void active()
    {
        isActive = true;
        GameMaster.Instance.startMoveInput();
        EventPool.OptIn("Beat", beat);
        EventPool.OptIn("player move input", checkMatch);
    }

    public void disactive()
    {
        isActive = false;
        GameMaster.Instance.stopMoveInput();
        EventPool.OptOut("player move input", checkMatch);
        EventPool.OptOut("Beat", beat);
    }

    void stop()
    {
        finishedTask = true;
        dialogUse.enabled = true;
        followText.SetActive(false);
        GetComponentInChildren<ShowLocalPopup>().show("Good Dance!");
        EventPool.OptOut("player move input", checkMatch);
        EventPool.OptOut("Beat", beat);
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
        checkMatch();
        currentInstructionId++;
        if (currentInstructionId >= instructionList.Length)
        {
            currentInstructionId = 0;
        }
    }

    public void checkMatch()
    {
        if (finishedTask || !isActive)
        {
            return;
        }
        var playerInput = player.moveInput;
        int i = currentInstructionId;
        //for (int i = currentInstructionId;i<= currentInstructionId;i++) 
        //for (int i = instructionList.Length - 1; i > 0; i--)
        if(i == 0)
        {
            i = 0;
        }
        for(int k = -1;k<=1;k++)
        {
            bool match = true;
            var adjustPlayerInput = 0;
            var adjustInstruction = 0;
            var adjustedI = i;
            if(k == 1)
            {
                adjustInstruction = -1;
                if (adjustedI + adjustInstruction < 0)
                {
                    adjustedI += instructionList.Length;
                }
                if(adjustedI+adjustInstruction>= instructionList.Length)
                {
                    adjustedI -= instructionList.Length;
                }
            }
            for(int j = 1; j <= adjustedI + 1; j++)
            {
                int playerInputIndex = playerInput.Count - j+ adjustPlayerInput;
                int instructionIndex = adjustedI - j+1 + adjustInstruction;
                if (playerInputIndex < 0 || instructionIndex<0)
                {
                    match = false;
                    break;
                }
                if(playerInputIndex>= playerInput.Count || instructionIndex>= instructionList.Length)
                {
                    match = false;
                    break;
                }
                if ( playerInput[playerInputIndex] != instructionList[instructionIndex])
                {
                    match = false;
                    break;
                }
            }
            if (match)
            {
                Debug.Log("match " + adjustedI);
                if(adjustedI == instructionList.Length-1)
                {
                    stop();
                }
                //return true;
                return;
            }
        }
        Debug.Log("not match");
        //player.clearMoveInput();
        //return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
