using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetInsturction : MonoBehaviour
{
    public int instructionId;
    TeamController teamController;
    bool got = false;
    // Start is called before the first frame update
    void Start()
    {

        teamController = FindObjectOfType<TeamController>();
    }
    public void getInstruction()
    {
        if (!got)
        {
            got = true;
            if (instructionId >= 0)
            {
                teamController.getInstruction(instructionId);
            }

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
