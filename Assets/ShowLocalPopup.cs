using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowLocalPopup : MonoBehaviour
{
    public TMP_Text textLabel;
    public Transform panel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void hide()
    {

        panel.gameObject.SetActive(false);
    }
    public void show(string text)
    {
        Debug.Log("show local pop up " + text);
        textLabel.text = text;
        panel.DOKill();
        panel.gameObject.SetActive(true);
        panel.transform.localPosition = Vector3.zero;
        // Grab a free Sequence to use
        Sequence mySequence = DOTween.Sequence();
        // Add a movement tween at the beginning
        mySequence.Append(panel.DOLocalMoveY(100,1));
        mySequence.AppendInterval(1);
        mySequence.Append(panel.DOLocalMoveY(0, 1));
        mySequence.OnComplete(hide);
    }
}
