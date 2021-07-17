using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OneInstruction : MonoBehaviour
{
    public Sprite[] dirSprite;
    Image renderer;
    // Start is called before the first frame update
    void Awake()
    {
        renderer = GetComponent<Image>();
    }

    public void beatDown()
    {
        renderer.color = Color.green;
    }

    public void beatClear()
    {

        renderer.color = Color.white;
    }

    public void beatDisable()
    {

        renderer.color = Color.grey;
    }

    public void Init(int dir)
    {
        renderer.sprite = dirSprite[dir];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
