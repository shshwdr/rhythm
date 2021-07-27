using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterToRead : MonoBehaviour
{
    public TMP_Text textLabel;
    // Start is called before the first frame update
    void Start()
    {
        textLabel = GetComponentInChildren<TMP_Text>();
    }

    public void loadLetter(string letterId)
    {
        textLabel.text = letterId;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
