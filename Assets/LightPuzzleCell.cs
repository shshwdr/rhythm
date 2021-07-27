using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LightPuzzleCell : MonoBehaviour
{
    public bool isLight = true;
    TurnAllLightPuzzle puzzle;
    public Vector2 positionIndex;

    SpriteRenderer renderer;
    public Sprite turnedOn;
    public Sprite turnedOff;
    bool originalState;
    bool isClear;

    private void Start()
    {
        puzzle = GetComponentInParent<TurnAllLightPuzzle>();
        renderer = GetComponent<SpriteRenderer>();
        updateLight();
        originalState = isLight;
    }

    public void Init(Vector2 index)
    {
        if (index.x < 0)
        {
            isClear = true;
        }
        else
        {

        }
        positionIndex = index;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "puzzle")
        {

            if (isClear)
            {
                puzzle.resetPuzzle();
            }
            else
            {
                puzzle.toggleLight(positionIndex);

            }
        }
    }
    public void reset()
    {
        isLight = originalState;
        updateLight();
    }
    public void toggleLight()
    {
        isLight = !isLight;
        updateLight();
    }
    void updateLight()
    {
        if (isClear)
        {
            renderer.enabled = false;
            return;
        }

        if (isLight)
        {
            renderer.sprite = turnedOn;
        }
        else
        {

            renderer.sprite = turnedOff;
        }
    }
}