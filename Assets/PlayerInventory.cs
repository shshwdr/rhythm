using Doozy.Examples;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int coins;
    public Dictionary<string, AutoShoot> abilityDict;
    AutoShoot[] abilities;

    public AudioClip coinAudio;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        abilities = GetComponents<AutoShoot>();
        audioSource = GetComponent<AudioSource>();
        //foreach(var a in abilities)
        //{
        //    abilityDict;
        //}
    }
    public void addCoin(int amount = 1)
    {
        Debug.Log("add coin");
        changeCoin(amount);

    }
    public void spendCoin(int amount = 10)
    {

        Debug.Log("spend coin");
        changeCoin(-amount);
    }

    void changeCoin(int amount)
    {

        coins += amount;
        //play sound
        Pool.EventPool.Trigger("coinChange");
        audioSource.PlayOneShot(coinAudio);
        DialogueLua.SetVariable("coins", coins);
    }

    public void addAbility(bool forever = false)
    {
        var selectedAbilityId = Random.Range(0, abilities.Length);
        var selectedAbility = abilities[selectedAbilityId];

        Debug.Log("add ability "+selectedAbility.name);
        selectedAbility.increaseLevel(forever);
        if(selectedAbility.currentLevel == 0)
        {

            PopupManager.Instance.ShowAchievement("Add Ability "+selectedAbility.displayName);
        }
        else
        {

            PopupManager.Instance.ShowAchievement("Ability " + selectedAbility.displayName+" Upgrade");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    addCoin(10);
        //}
    }
}
