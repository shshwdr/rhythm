using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridItem : MonoBehaviour
{
    protected float gridSize;
    // Start is called before the first frame update
    void Start()
    {

        gridSize = GameMaster.Instance.gridSize;
        transform.position = Utils.snapToGridCenter(gridSize, transform.position);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
