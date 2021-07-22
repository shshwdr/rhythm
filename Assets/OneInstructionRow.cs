using Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OneInstructionRow : MonoBehaviour
{
    OneInstruction[] instructions;
    int[] instructionList;
    bool isDisabled = false;
    public TMP_Text nameLabel;
    // Start is called before the first frame update
    void Awake()
    {
        instructions = GetComponentsInChildren<OneInstruction>();
        EventPool.OptIn<int,int>("BeatDown", beatDown);
        EventPool.OptIn("BeatClear", beatClear);
        //EventPool.OptIn<int>("BeatDown", beatDown);
    }
    void beatDown(int beatId,int actionId) {
        if (isDisabled)
        {
            return;
        }
        //Debug.Log("instruction list "+instructionList+" beat id " + beatId + " actioinId " + actionId+" expected action "+ instructionList[beatId]);
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
        isDisabled = false;
        foreach (var ins in instructions)
        {
            ins.beatClear();
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
    public void Init(Instruction instru)
    {
        instructionList = instru.instructionStep.ToArray();
        for(int i = 0;i< instructionList.Length;i++)
        {
            instructions[i].Init(instructionList[i]);
        }
        nameLabel.text = instru.name;
        if (!instru.isUnlocked)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
