using Pool;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public TMP_Text coin;
    PlayerInventory playerInv;
    // Start is called before the first frame update
    void Start()
    {
        playerInv = FindObjectOfType<PlayerInventory>();
        EventPool.OptIn("coinChange", updateCoin);
    }

    public void updateCoin()
    {
        coin.text = playerInv.coins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
