using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using System;

[Serializable]
public class Instruction
{
    public List<int> instructionStep;
    public bool isUnlocked;
    public string name;
    public Instruction(List<int> steps,bool unlock,string n)
    {
        instructionStep = steps;
        isUnlocked = unlock;
        name = n;
    }
}
public class TeamController : MonoBehaviour
{

    AudioSource unmatchedCommand;
    Animator sprite1Animator;
    Animator sprite2Animator;
    Animator[] spriteAnimators;
    Rigidbody2D thisRigidbody;

    public Transform instructionsUI;

    public List<Instruction> instructions = new List<Instruction>()
    {
        //new Instruction new List<int>(){3,4,0,0},
        //new List<int>(){1,2,0,0},

    };


    [HideInInspector] public int secondsToBeats;

    //movement variables
    [Header("Movement Variables")]
    public float jumpHeight = 2f;
    public float moveDistance = 3f;

    [Header("Image references")]
    public Image unmatchedCommandSprite;
    float spriteFlashSpeed = 0.5f;
    Color flashColor = new Color(255f, 255f, 255f, 1);

    List<Transform> instructionChildren = new List<Transform>();

    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody2D>();

        unmatchedCommand = GetComponent<AudioSource>();
        InitInstructionUI();
        //spriteAnimators = GetComponentsInChildren<Animator>();
        //sprite1Animator = spriteAnimators[0];
        //sprite2Animator = spriteAnimators[1];
    }

    void InitInstructionUI()
    {
        for(int i = 0;i< instructionsUI.childCount-1;i++) 
        {
            Transform child = instructionsUI.GetChild(i);
            child.GetComponent<OneInstructionRow>().Init(instructions[i]);
            instructionChildren.Add(child);
        }
    }

    public void getInstruction(int i)
    {
        Transform child = instructionChildren[i];
        if (child.gameObject.activeInHierarchy)
        {
            Debug.Log("instruction " + i + " was actived");
            return;
        }
        instructions[i].isUnlocked = true;
        child.gameObject.SetActive(true);

        Debug.Log("instruction " + i + " active!");
    }

    private void Update()
    {
        if (unmatchedCommandSprite.color != Color.clear)
            unmatchedCommandSprite.color = Color.Lerp(unmatchedCommandSprite.color, Color.clear, spriteFlashSpeed);
    }


    void doAttack(int i)
    {
        switch (i)
        {
            case 0:
                Debug.Log("up");
                GetComponent<ForwardAutoShoot>().startShoot();
                break;
            case 1:
                Debug.Log("side");
                GetComponent<SideAutoShoot>().startShoot();
                break;
        }
    }

    public bool GetInput(int[] commandType)
    {
        bool found = false;
        for(int i = 0; i < instructions.Count; i++)
        {
            if (instructions[i].isUnlocked)
            {
                var expectedCommand = instructions[i].instructionStep.ToArray();
                if (ArrayCompare(commandType, expectedCommand))
                {
                    doAttack(i);
                    found = true;
                }
            }
        }

        //Physics2D.BoxCast(transform.position,GameMaster.Instance.gridSize,)

        if (!found)
        {
            unmatchedCommand.Play();
            unmatchedCommandSprite.color = flashColor;
            return false;
        }
        return true;
    }

    bool ArrayCompare(int[] array1, int[] array2)
    {
        //if (array1.Length != array2.Length)
        //    return false;
        for (var i = 0; i < array2.Length; i++)
        {
            if (array1[i] != array2[i] && array2[i]!=0)
                return false;
        }
        return true;
    }

    //public void resetSpritesToIdle()
    //{
    //    sprite1Animator.SetBool("Walking", false);
    //    sprite2Animator.SetBool("Walking", false);

    //    sprite1Animator.SetBool("Jumping", false);
    //    sprite2Animator.SetBool("Jumping", false);
    //}


}

