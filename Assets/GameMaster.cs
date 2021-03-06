using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pool;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : Singleton<GameMaster>
{
    bool shouldGetMoveInput = false;

    public AudioClip masterBeat;
    public AudioClip commandMutedBeat;
    public AudioClip beatMissSigh;
    public AudioClip beatSkipSigh;

    public AudioClip drumTop;
    public AudioClip drumRight;
    public AudioClip drumDown;
    public AudioClip drumLeft;
    AudioSource audioSourceSFX;
    AudioSource[] audioSources;


    public AudioClip[] mutedBeatClips;
    int[] mutedBeats = new int[4];
    int mutedBeatId;

    [Header("Public references")]
    public TeamController teamController;
    bool allowedToBeat;
    //beat track variables
    [Header("Beat timing variables")]
    [Range(0, 150)]
    public float beatsPerMinute = 80;
    [Range(0, 1)]
    public float errorMarginTime = .3f;


    int[] commandType;
    int commandCount = 0;
    int inactiveBeatCount = 0;  //how many beats after command are inactive

    //measure how long an active beat time has no input
    float beatFallTime;

    //count how long beat is active without an input
    private float beatActiveTime = 0f;
    new private bool enabled;
    public float invokeTime;

    public float gridSize = 0.75f;

    public void startMoveInput()
    {
        shouldGetMoveInput = true;
    }
    public void stopMoveInput()
    {
        shouldGetMoveInput = false;
        clearMoveInput();
    }
    public bool hasBeatInput
    {
        get
        {
            return enabled;
        }
        set
        {
            enabled = value;
            if (!enabled)
                beatActiveTime = Time.time;
        }
    }

    bool lastBeatHasInput = true;   //true means no, false means yes, used with offset along with hasBeatInput

    //ui flash colour variables
    Color flashColor = new Color(255f, 255f, 255f, 1);
    [Header("Sprite Display Variables")]
    public Image drumTopSprite;
    public Image drumRightSprite;
    public Image drumBottomSprite;
    public Image drumLeftSprite;
    Image currentDrumSprite = null;
    [Range(0.25f, 0.5f)]
    public float spriteFlashTime = 0.5f;

    PlayerController player;

    //fever variables
    //bool fever;
    //float feverTimeHold;
    //public Image feverSprite;

    public float extraLayerVolume = 0.3f;

    float lastPlayerMasterBeat;
    float lastAllowBeat;
    
    void Start()
    {
        allowedToBeat = true;
        hasBeatInput = false;

        inactiveBeatCount = 0;

        invokeTime = 60f / beatsPerMinute;
        teamController.secondsToBeats = (int)(invokeTime * 4);

        commandType = new int[4] { 0, 0, 0, 0 };
        audioSourceSFX = GetComponent<AudioSource>();
        audioSources = GetComponentsInChildren<AudioSource>();
        player = FindObjectOfType<PlayerController>();
        player.gridSize = gridSize;
        //audioSources = GetComponents<AudioSource>();
        //masterBeat = audioSources[0];
        //commandMutedBeat = audioSources[1];
        //beatMissSigh = audioSources[2];
        //beatSkipSigh = audioSources[3];

        //drumTop = audioSources[4];
        //drumRight = audioSources[5];
        //drumBottom = audioSources[6];
        //drumLeft = audioSources[7];

        beatFallTime = errorMarginTime;
        StartCoroutine(startBeat());
        //invokeTime = 60f / beatsPerMinute;
        //audioSources[1].Play();
        //lastPlayerMasterBeat = errorMarginTime / 2f;
        //lastAllowBeat = 0;

    }

    IEnumerator startBeat()
    {
        yield return new WaitForSeconds(0.5f);
        float invokeTime = 60f / beatsPerMinute;

        InvokeRepeating("PlayMasterBeat", errorMarginTime / 2f, invokeTime);
        InvokeRepeating("AllowBeat", 0f, invokeTime);

        yield return new WaitForSeconds(errorMarginTime / 2f);


        for (int i = 1; i < audioSources.Length; i++)
        {
            audioSources[i].time = 0;
            if (audioSources[i].isPlaying)
            {

                audioSources[i].Play();
            }
        }
        audioSources[1].Play();
        //audioSources[2].Play();
        //GameManager.Instance.GetComponent<AudioSource>().Play();
    }
    public void addAudioSource(int i)
    {
        if (i <= 0)
        {
            return;
        }
        Debug.Log("audio sources time " + audioSources[1].time + " " + audioSources[2].time);
        audioSources[i].time = audioSources[1].time;
        audioSources[i].Play();
        audioSources[i].volume = 0;
        audioSources[i].DOKill();
        DOTween.To(() => audioSources[i].volume, x => audioSources[i].volume = x, extraLayerVolume, 0.5f);
        // Tween a Vector3 called myVector to 3,4,8 in 1 second
        //DOTween.To(() => myVector, x => myVector = x, new Vector3(3, 4, 8), 1);
        // Tween a float called myFloat to 52 in 1 second
        //DOTween.To(() => myFloat, x => myFloat = x, 52, 1);
    }
    public void FixedUpdate()
    {
        if(audioSources[1].clip.length - audioSources[1].time < 1)
        {
            audioSources[1].time = 0;
            audioSources[1].Play();
            for(int i = 1; i < audioSources.Length; i++)
            {
                if (audioSources[i].isPlaying)
                {

                    audioSources[i].time = 0;
                    audioSources[i].Play();
                }
            }
        }
        //if (lastAllowBeat - audioSources[1].time >10)
        //{
        //    lastPlayerMasterBeat = errorMarginTime / 2f;
        //    lastAllowBeat = 0;
        //}
        //    if (audioSources[1].time - lastAllowBeat >= invokeTime-0.05f)
        //{
        //    AllowBeat();
        //    lastAllowBeat += invokeTime;
        //}
        //if (audioSources[1].time - lastPlayerMasterBeat >= invokeTime - 0.05f)
        //{
        //    PlayMasterBeat();
        //    lastPlayerMasterBeat += invokeTime;
        //}
    }
    public void removeAudioSource(int i)
    {
        if (i <= 0)
        {
            return;
        }
        audioSources[i].Stop();

        audioSources[i].volume = extraLayerVolume;
        audioSources[i].DOKill();
        DOTween.To(() => audioSources[i].volume, x => audioSources[i].volume = x, 0, 0.5f);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            CancelInvoke();

            StartCoroutine(startBeat());
        }
        beatFallTime -= Time.deltaTime;
        if (beatFallTime < 0f)
        {
            allowedToBeat = false;

            //Debug.Log("not allow!");
            if (commandType[3] != 0)
            {
                bool commandMatched = SetInput(commandType);
                if (commandMatched)
                {
                    commandCount++;
                    inactiveBeatCount = 4;      //4 beats after input are inactive
                }
                else
                {
                    inactiveBeatCount = 0;
                    commandCount = 0;
                }
                clearCommand();
            }
        }

        if (allowedToBeat && hasBeatInput && Input.anyKeyDown)
        {      //double beat per master beat
            print("double beat not allowed");
            //hasBeatInput = false;
            lastBeatHasInput = true;
            clearCommand();
        }

        GetDrumInputs();


        if (!allowedToBeat &&(Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.RightArrow)))
        {                     //mistiming beat with master beat
             audioSourceSFX.PlayOneShot(beatMissSigh);

            clearCommand();
            commandCount = 0;
        }

        if (inactiveBeatCount > 0 &&  (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {               //interrupting command
            clearCommand();
            commandCount = 0;
            //do physical motion stop here
        }


        if (Time.time - beatActiveTime >= errorMarginTime && lastBeatHasInput && allowedToBeat)
        {      //skipping a master beat
            lastBeatHasInput = true;
            clearCommand();
            //addMoveInput(-1);
            clearMoveInput();
        }

        if (currentDrumSprite != null)
        {
            Image temporaryReference = currentDrumSprite;
            temporaryReference.color = Color.Lerp(currentDrumSprite.color, Color.clear, spriteFlashTime);
        }


        //continuos beats required to maintain fever
        //if (commandCount >= 4)
        //{
        //    fever = true;
        //    feverSprite.gameObject.SetActive(true);
        //}

        //if (inactiveBeatCount >= 0)
        //{
        //    feverTimeHold = Time.time;
        //}
        //if (Time.time - feverTimeHold >= ((errorMarginTime) * 2) + 1f && fever)
        //{
        //    commandCount = 0;
        //    fever = false;
        //    feverSprite.gameObject.SetActive(false);
        //}
    }

    void clearCommand()
    {

        Array.Clear(commandType, 0, commandType.Length);
        EventPool.Trigger("BeatClear");
    }


    void AllowBeat()
    {
        beatFallTime = errorMarginTime;

        //if (inactiveBeatCount == 0)
        //    teamController.resetSpritesToIdle();

        allowedToBeat = true;
        //Debug.Log("allow!");
        inactiveBeatCount--;
        if (hasBeatInput)
        {
            hasBeatInput = false;
        }
    }
    void PlayMasterBeat()
    {
        //player.Move();
        EventPool.Trigger("Beat");
        if ((inactiveBeatCount) >= 0)
        {
            //use map to find beat
            //audioSourceSFX.PlayOneShot(commandMutedBeat);
            audioSourceSFX.PlayOneShot(mutedBeatClips[ mutedBeats[mutedBeatId]-1]);
            mutedBeatId ++;
        }
        else
        {

            audioSourceSFX.PlayOneShot(masterBeat);
        }
    }

    bool SetInput(int[] commandType)
    {
        bool commandMatched = teamController.GetInput(commandType, ref mutedBeats);
        mutedBeatId = 0;
        return commandMatched;
    }
    public List<int> moveInput = new List<int>();
    void addMoveInput(int i)
    {
        if (shouldGetMoveInput)
        {

            moveInput.Add(i);
            EventPool.Trigger("player move input");
        }
    }
    public void clearMoveInput()
    {
        moveInput.Clear();
    }

    void GetDrumInputs()
    {
        if (player.isConversation || !GameManager.Instance.isInGame || player.isDead)
        {
            return;
        }

        if (allowedToBeat && !hasBeatInput)
        {

            if (!Input.GetKey(KeyCode.Space) && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                clearCommand();
                commandCount = 0;
            }

            if (inactiveBeatCount < 0 && Input.GetKey(KeyCode.Space))
            {
                for (int i = 0; i < 4; i++)
                {
                    if (commandType[i] == 0)
                    {
                        if (Input.GetKeyDown(KeyCode.LeftArrow))
                        {
                            Debug.Log("command id " + i);
                            Debug.Log("left");
                            commandType[i] = 1;
                            hasBeatInput = true;
                            audioSourceSFX.PlayOneShot(drumLeft);
                            //currentDrumSprite = drumLeftSprite;
                            //currentDrumSprite.color = flashColor;
                            EventPool.Trigger("BeatDown", i, 1);
                            break;
                        }
                        else
                        if (Input.GetKeyDown(KeyCode.RightArrow))
                        {
                            commandType[i] = 2;
                            hasBeatInput = true;
                            audioSourceSFX.PlayOneShot(drumRight);
                            //currentDrumSprite = drumRightSprite;
                            //currentDrumSprite.color = flashColor;
                            EventPool.Trigger("BeatDown", i, 2);
                            break;
                        }
                        else
                        if (Input.GetKeyDown(KeyCode.UpArrow))
                        {
                            commandType[i] = 3;
                            hasBeatInput = true;
                            audioSourceSFX.PlayOneShot(drumTop);
                            //currentDrumSprite = drumTopSprite;
                            //currentDrumSprite.color = flashColor;
                            EventPool.Trigger("BeatDown", i, 3);
                            break;
                        }
                        else
                        if (Input.GetKeyDown(KeyCode.DownArrow))
                        {
                            commandType[i] = 4;
                            hasBeatInput = true;
                            audioSourceSFX.PlayOneShot(drumDown);
                            //currentDrumSprite = drumBottomSprite;
                            //currentDrumSprite.color = flashColor;
                            EventPool.Trigger("BeatDown", i, 4);
                            break;
                        }
                    }


                }
            }
            //else
            {
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {

                    hasBeatInput = true;
                    //MoveController.Instance.Move();
                    player.Move(new Vector2(-1, 0));
                    addMoveInput(1);
                    //Array.Clear(commandType, 0, commandType.Length);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {

                    hasBeatInput = true;
                    //MoveController.Instance.Move();
                    player.Move(new Vector2(1, 0));
                    addMoveInput(2);

                    //Array.Clear(commandType, 0, commandType.Length);
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {

                    hasBeatInput = true;
                    //MoveController.Instance.Move();
                    player.Move(new Vector2(0, 1));
                    addMoveInput(3);

                    //Array.Clear(commandType, 0, commandType.Length);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {

                    hasBeatInput = true;
                    //MoveController.Instance.Move();
                    player.Move(new Vector2(0, -1));
                    addMoveInput(4);

                    // Array.Clear(commandType, 0, commandType.Length);
                }
            }
        }
        lastBeatHasInput = !hasBeatInput;
    }
}