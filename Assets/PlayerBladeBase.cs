using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBladeBase:MonoBehaviour
{
    public List<Vector2> bladePositions;
    GameObject bladePrefab;
    Transform bladeParent;
    float invokeTime;
    float gridSize;
    protected virtual void Start()
    {
        bladePrefab = Resources.Load<GameObject>("PlayerBlade");
        var go = new GameObject("bladeParent");
        go.transform.parent = transform;
        go.transform.position = Vector3.zero;
        bladeParent = go.transform;
       // invokeTime = GameMaster.Instance.invokeTime;
        gridSize = GameMaster.Instance.gridSize;
        foreach (var position in bladePositions)
        {
            var blade = Instantiate(bladePrefab, transform.position+ (Vector3)position*gridSize, Quaternion.identity, bladeParent);
            blade.SetActive(false);
        }
    }

    public void showBlade()
    {
        invokeTime = GameMaster.Instance.invokeTime;
        foreach (Transform child in bladeParent)
        {
            child.gameObject.SetActive(true);
        }
        //hide after four beat
        Invoke("hideBlade", invokeTime * 4);
    }

    public void hideBlade()
    {
        foreach (Transform child in bladeParent)
        {
            child.gameObject.SetActive(false);
        }
    }
}