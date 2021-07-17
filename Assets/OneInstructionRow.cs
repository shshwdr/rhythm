using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneInstructionRow : MonoBehaviour
{
    OneInstruction[] instructions;
    int[] instructionList;
    // Start is called before the first frame update
    void Awake()
    {
        instructions = GetComponentsInChildren<OneInstruction>();
        EventPool.OptIn<int,int>("BeatDown", beatDown);
        EventPool.OptIn("BeatClear", beatClear);
        //EventPool.OptIn<int>("BeatDown", beatDown);
    }
    void beatDown(int beatId,int actionId) {
        Debug.Log("instruction list "+instructionList+" beat id " + beatId + " actioinId " + actionId+" expected action "+ instructionList[beatId]);
        if(actionId == instructionList[beatId] || instructionList[beatId] == 0)
        {
            instructions[beatId].beatDown();
        }
        else
        {
            beatDisable();
        }
    }

    void beatClear()
    {
        foreach (var ins in instructions)
        {
            ins.beatClear();
        }
    }
    void beatDisable()
    {
        foreach (var ins in instructions)
        {
            ins.beatDisable();
        }
    }
    public void Init(int[] list)
    {
        instructionList = list;
        for(int i = 0;i< instructionList.Length;i++)
        {
            instructions[i].Init(instructionList[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
