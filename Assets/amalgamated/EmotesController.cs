using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EmoteType { heart, angry,happy, heartBreak,confused, shy};

public class EmotesController : MonoBehaviour
{
    Dictionary<EmoteType, string> emoteDictionary;
    Animator animator;
    private void Awake()
    {

        animator = GetComponent<Animator>();
        emoteDictionary = new Dictionary<EmoteType, string>()
        {
            { EmoteType.heart , "heart" },
            { EmoteType.angry , "angry" },
            { EmoteType.happy , "happy" },
            { EmoteType.heartBreak , "heartBreak" },
            { EmoteType.confused , "confused" },
            { EmoteType.shy , "shy" },
        };
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    public void showEmote(EmoteType emote, bool loop = false)
    {
        CancelInvoke();
        if (loop)
        {
            StartCoroutine(showEmoteLoop(emote));
            Invoke("showEmoteLoop2", 0.0f);
        }
        else
        {

            var animationState = animator.GetCurrentAnimatorStateInfo(0);
            animator.SetTrigger(emoteDictionary[emote]);
        }

    }

    IEnumerator showEmoteLoop(EmoteType emote)
    {
        animator.SetTrigger(emoteDictionary[emote]);
        //Debug.Log("wait anim " + animator.GetCurrentAnimatorStateInfo(0).length);
        yield return new WaitForSeconds(0.3f);
        showEmoteLoop(emote);
    }

    void showEmoteLoop2()
    {
        var emote = EmoteType.heart;
        animator.SetTrigger(emoteDictionary[emote]);
        //Debug.Log("wait anim " + animator.GetCurrentAnimatorStateInfo(0).length);
        //yield return new WaitForSeconds(0.3f);
        showEmoteLoop(emote);
        Invoke("showEmoteLoop2", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
