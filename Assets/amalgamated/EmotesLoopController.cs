using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotesLoopController : MonoBehaviour
{
    public float loopTimeMin = 0.5f;
    public float loopTimeMax = 2f;
    float nextEmotesTime = -1f;
    float currentEmotesTimer = 0f;
    protected EmotesController emotesController;
    EmoteType[] emotesType = {EmoteType.heart,EmoteType.happy,EmoteType.shy };
    // Start is called before the first frame update
    void Start()
    {

        emotesController = GetComponentInChildren<EmotesController>();
        nextEmotesTime = Random.Range(loopTimeMin, loopTimeMax);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentEmotesTimer >= nextEmotesTime)
        {
            currentEmotesTimer = 0;
            nextEmotesTime = Random.Range(loopTimeMin, loopTimeMax);
            int nextEmoteType = Random.Range(0, emotesType.Length);
            emotesController.showEmote(emotesType[nextEmoteType]);
        }
        currentEmotesTimer += Time.deltaTime;
    }
}
