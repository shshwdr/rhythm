using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PoolObject : MonoBehaviour
{

    public float returnTime = 5f;
    float currentTimer = 0;
    public int cacheCount = 50;
    protected Transform player;



    public void fetch()
    {
        gameObject.SetActive(true);
        currentTimer = 0;
    }

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;

    }

    public void returnBack(){

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (returnTime < 0)
        {
            return;
        }
        if (this == null || transform == null)
        {
            return;
        }
        if ((transform.position - player.position).sqrMagnitude >= 200)
        {

            returnBack();
        }
        currentTimer += Time.deltaTime;
        if (currentTimer >= returnTime)
        {
            returnBack();
        }
    }

}