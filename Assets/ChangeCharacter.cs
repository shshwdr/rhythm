using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCharacter : MonoBehaviour
{

    public GameObject normalCharacter;
    public GameObject fireCharacter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void changeCharacter()
    {
        normalCharacter.SetActive(false);
        fireCharacter.SetActive(true);
        GetComponent<PlayerInventory>().spendCoin(20);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
