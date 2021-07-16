using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float moveDiff = 3f;
    Transform player;
    Vector3 playerOriginPosition;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        playerOriginPosition = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        var xdiff = player.transform.position.x - playerOriginPosition.x - transform.localPosition.x;
        if (Mathf.Abs(xdiff) > moveDiff)
        {
            transform.localPosition += new Vector3((xdiff > 0 ? 1 : -1) * moveDiff, 0, 0);
        }

        var ydiff = player.transform.position.y - playerOriginPosition.y - transform.localPosition.y;
        if (Mathf.Abs(ydiff) > moveDiff)
        {
            transform.localPosition += new Vector3(0,(ydiff > 0 ? 1 : -1) * moveDiff, 0);
        }
    }
}
