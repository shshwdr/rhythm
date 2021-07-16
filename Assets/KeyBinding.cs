using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBinding : Singleton<KeyBinding>
{
    public Dictionary<KeyCode, string> keyCodeToString = new Dictionary<KeyCode, string>()
    {
        { KeyCode.UpArrow,"up" },
        { KeyCode.DownArrow,"down" },
        { KeyCode.LeftArrow,"left" },
        { KeyCode.RightArrow,"right" },

    };

    Dictionary<string, List<KeyCode>> stringToKeyCode = new Dictionary<string, List<KeyCode>>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool isKeyPressed(string s, int mode = 0)
    {
        if (stringToKeyCode.ContainsKey(s))
        {
            foreach(var keycode in stringToKeyCode[s])
            {
                if (Input.GetKeyDown(keycode))
                {
                    return true;
                }
            }
        }
        else
        {
            Debug.LogError(s + " key does not existed");
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
