using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeAlternateAudioClips : MonoBehaviour
{
    public AudioClip[] brioBefore;
    public AudioClip[] briodAfter;
    public void changeTo(int i)
    {
        if(i == 0)
        {
            GetComponent<AbstractTypewriterEffect>().alternateAudioClips = brioBefore;
        }
        else
        {

            GetComponent<AbstractTypewriterEffect>().alternateAudioClips = briodAfter;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
