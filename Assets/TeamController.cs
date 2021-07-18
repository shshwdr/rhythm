using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class TeamController : MonoBehaviour
{

    AudioSource unmatchedCommand;
    Animator sprite1Animator;
    Animator sprite2Animator;
    Animator[] spriteAnimators;
    Rigidbody2D thisRigidbody;

    public Transform instructionsUI;

    public List<List<int>> instructions = new List<List<int>>()
    {
        new List<int>(){3,4,0,0},
        new List<int>(){1,2,0,0},

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
            child.GetComponent<OneInstructionRow>().Init(instructions[i].ToArray());
        }
    }

    private void Update()
    {
        if (unmatchedCommandSprite.color != Color.clear)
            unmatchedCommandSprite.color = Color.Lerp(unmatchedCommandSprite.color, Color.clear, spriteFlashSpeed);
    }



    public bool GetInput(int[] commandType)
    {

        //walk
        if (ArrayCompare(commandType, new int[] { 3,4 }))
        {
            Debug.Log("up");
            GetComponent<ForwardAutoShoot>().startShoot();
            //Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
            //thisRigidbody.DOMove(currentPosition + (Vector2.left * moveDistance), secondsToBeats, false);

            //sprite1Animator.SetBool("Walking", true);
            //sprite2Animator.SetBool("Walking", true);

            return true;
        }
        //jump
        else if (ArrayCompare(commandType, new int[] { 1, 2 }))
        {
            Debug.Log("side");
            GetComponent<SideAutoShoot>().startShoot();
            //Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
            //thisRigidbody.DOJump(currentPosition + (Vector2.up * jumpHeight), 0.5f, 0, secondsToBeats, false);

            //sprite1Animator.SetBool("Jumping", true);
            //sprite2Animator.SetBool("Jumping", true);

            return true;
        }
        else
        {
            unmatchedCommand.Play();
            unmatchedCommandSprite.color = flashColor;
            return false;
        }
    }

    bool ArrayCompare(int[] array1, int[] array2)
    {
        //if (array1.Length != array2.Length)
        //    return false;
        for (var i = 0; i < array2.Length; i++)
        {
            if (array1[i] != array2[i])
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

