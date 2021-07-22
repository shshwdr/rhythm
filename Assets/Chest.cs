using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public int instructionId = -1;
    TeamController teamController;
    public List<int> openInstruction;
    // Start is called before the first frame update
    void Start()
    {
        teamController = FindObjectOfType<TeamController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openChest()
    {
        if (instructionId >= 0)
        {
            teamController.getInstruction(instructionId);
        }
        Destroy(gameObject);
    }
}
