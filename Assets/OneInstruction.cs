using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneInstruction : MonoBehaviour
{
    public Sprite[] dirSprite;
    Image renderer;
    int originSpriteIndex = 0;
    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<Image>();
    }

    public void beatDown()
    {

        updateRenderer(true);
        renderer.color = Color.white;
    }

    public void beatClear()
    {

        updateRenderer(false);
        renderer.color = Color.white;
    }

    public void beatDisable()
    {

        updateRenderer(false);
         renderer.color = Color.grey;
    }

    public void Init(int dir)
    {
        var degree = new Vector3(0, 0, 180);
        switch (dir)
        {
            case 0:
                originSpriteIndex = 0;
                break;
            case 1:
                originSpriteIndex = 1;
                degree = new Vector3(0, 0, 180);
                GetComponent<RectTransform>().rotation = Quaternion.Euler(degree);
                break;
            case 2:
                originSpriteIndex = 1;
                degree = new Vector3(0, 0, 0);
                GetComponent<RectTransform>().rotation = Quaternion.Euler(degree);
                break;
            case 3:
                originSpriteIndex = 1;
                degree = new Vector3(0, 0, 90);
                GetComponent<RectTransform>().rotation = Quaternion.Euler(degree);
                break;
            case 4:
                originSpriteIndex = 1;
                degree = new Vector3(0, 0, -90);
                GetComponent<RectTransform>().rotation = Quaternion.Euler(degree);
                break;
        }
        updateRenderer(false);
    }

    void updateRenderer(bool isActive)
    {

        renderer.sprite = dirSprite[originSpriteIndex+(isActive?2:0)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
