using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentInstructionRow : MonoBehaviour
{
    OneInstruction[] instructions;
    int[] instructionList;
    bool isDisabled = false;
    // Start is called before the first frame update
    void Awake()
    {
        instructions = GetComponentsInChildren<OneInstruction>();
        EventPool.OptIn<int, int>("BeatDown", beatDown);
        EventPool.OptIn("BeatClear", beatClear);
        Init();
        //EventPool.OptIn<int>("BeatDown", beatDown);
    }
    void beatDown(int beatId, int actionId)
    {
        if (isDisabled)
        {
            return;
        }
        //Debug.Log("instruction list "+instructionList+" beat id " + beatId + " actioinId " + actionId+" expected action "+ instructionList[beatId]);
        //if (actionId == instructionList[beatId] || instructionList[beatId] == 0)
        {
            instructions[beatId].gameObject.SetActive(true);
            instructions[beatId].Init(actionId);
            instructions[beatId].beatDown();
        }
        //else
        {
         //   beatDisable();
        }
    }

    void beatClear()
    {
        for (int i = 0; i < instructions.Length; i++)
        {
            instructions[i].gameObject.SetActive(false);
        }
    }
    void beatDisable()
    {
        isDisabled = true;
        foreach (var ins in instructions)
        {
            ins.beatDisable();
        }
    }
    public void Init()
    {
        //instructionList = list;
        for (int i = 0; i < instructions.Length; i++)
        {
            instructions[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
