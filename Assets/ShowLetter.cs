using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLetter : EventActivator
{
    public GameObject letterToRead;
    public string letterName;
    SpriteRenderer renderer;
    Collider2D collider;

    protected override void Start()
    {
        base.Start();
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        renderer.enabled = false;
        collider.enabled = false;
    }

    protected override void clearRoom()
    {
        base.clearRoom();
        renderer.enabled = true;
        collider.enabled = true;

    }

    public void readLetter()
    {
        //letterToRead.SetActive(true);
        letterToRead.GetComponent<LetterToRead>().loadLetter(letterName);
        letterToRead.GetComponent<UIView>().Show();
    }

    public void hideLetter()
    {

        letterToRead.GetComponent<UIView>().Hide();
        //letterToRead.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
