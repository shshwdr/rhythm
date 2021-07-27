using Doozy.Engine.UI;
using Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TurnAllLightPuzzle : MonoBehaviour
{
    public string roomId;
    public int size;
    public GameObject lightPrefab;
    Dictionary<Vector2, LightPuzzleCell> lightPuzzleCells;
    public bool isGenerating = true;
    float gridSize;
    bool finishedPuzzle;
    bool start;

    void finishPuzzle()
    {
        if (!finishedPuzzle)
        {

        finishedPuzzle = true;
        EventPool.Trigger<string>("clearRoom", roomId);
        }
    }

    private void Start()
    {
        roomId = GetComponentInParent<RoomEnemyGenerator>().roomId;
        gridSize = GameMaster.Instance.gridSize;
        lightPuzzleCells = new Dictionary<Vector2, LightPuzzleCell>();
        if (isGenerating)
        {
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    var go = Instantiate(lightPrefab, transform.position + new Vector3(i, j, 0) * gridSize, Quaternion.identity, transform);
                    go.GetComponent<LightPuzzleCell>().Init(new Vector2(i, j));
                    lightPuzzleCells[new Vector2(i, j)] = go.GetComponent<LightPuzzleCell>();
                }
            }
        }
        else
        {
            foreach(Transform child in transform)
            {
                var cell = child.GetComponent<LightPuzzleCell>();
                if (cell.positionIndex.x < 0)
                {
                    continue;
                }
                lightPuzzleCells[cell.positionIndex] = cell;
            }
        }

        for (int i = -1; i <= size; i++)
        {
            foreach (var j in new int[] { -1, size })
            {

                var go = Instantiate(lightPrefab, transform.position + new Vector3(i, j, 0) * gridSize, Quaternion.identity, transform);
                go.GetComponent<LightPuzzleCell>().Init(new Vector2(-1, -1));
            }
        }
        for (int i = 0; i < size; i++)
        {
            foreach (var j in new int[] { -1, size })
            {

                var go = Instantiate(lightPrefab, transform.position + new Vector3(j, i, 0) * gridSize, Quaternion.identity, transform);
                go.GetComponent<LightPuzzleCell>().Init(new Vector2(-1, -1));
            }
        }
    }

    public void toggleLight(Vector2 lightIndex)
    {
        if (finishedPuzzle)
        {
            return;
        }
        foreach(var dir in Utils.dir5V2)
        {
            var newDir = dir + lightIndex;
            if (lightPuzzleCells.ContainsKey(newDir))
            {
                lightPuzzleCells[newDir].toggleLight();
            }
        }
        if (!isGenerating)
        {
            bool allLight = true;
            foreach (var light in lightPuzzleCells.Values)
            {
                if (!light.isLight)
                {
                    allLight = false;
                    break;
                }
            }
            if (allLight)
            {
                Debug.Log("all light");
                finishPuzzle();
            }
        }
    }



    public void resetPuzzle()
    {
        if (finishedPuzzle)
        {
            return;
        }
        foreach (var light in lightPuzzleCells.Values)
        {
            light.reset();
        }
    }
}