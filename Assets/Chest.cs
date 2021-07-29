using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Sprite openedSprite;
    public int instructionId = -1;
    TeamController teamController;
    public List<int> openInstruction;
    bool opened = false;

    public List<int> getKey()
    {
        return openInstruction;
    }
    // Start is called before the first frame update
    void Start()
    {
        teamController = FindObjectOfType<TeamController>();
        GetComponentInChildren<OneInstructionRow>().Init(openInstruction.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void showPopup(int i)
    {
        string popupText = "";
        switch (i)
        {
            case 0:
                popupText = "Too Far Away";
                break;
        }
        GetComponentInChildren<ShowLocalPopup>().show(popupText);
    }

    public bool tryOpenChest(Vector3 openPosition)
    {
        if (opened)
        {
            return false;
        }
        if (Utils.nextToPositionInGrid(GameMaster.Instance.gridSize, openPosition, transform.position))
        {

            if (instructionId >= 0)
            {
                teamController.getInstruction(instructionId);
            }
            GetComponent<SpriteRenderer>().sprite = openedSprite;
            GetComponent<AudioSource>().Play();
            //Destroy(gameObject);
            return true;
        }
        else
        {
            showPopup(0);
            return false;
        }


    }
}
