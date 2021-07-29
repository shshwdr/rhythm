using DG.Tweening;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickWithBeat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("beat", 0, GameMaster.Instance.invokeTime);
        beat();
        EventPool.OptIn("Beat",beat);
    }
    void beat()
    {
        transform.DOShakeScale(0.2f);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
