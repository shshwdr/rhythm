using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    static public Vector2[] dir4V2 = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1), };
    static public Vector2[] dir5V2 = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1), new Vector2(0, 0), };
    static public bool randomBool()
    {
        return Random.Range(0, 2) > 0;
    }

    static public Vector3 randomVector3(Vector3 origin, float randomness)
    {
        return new Vector3(origin.x + randomFromZero(randomness), origin.y + randomFromZero(randomness), origin.z + randomFromZero(randomness));
    }

    static public Vector3 randomVector3_2d(Vector3 origin, float randomness)
    {
        return new Vector3(origin.x + randomFromZero(randomness), origin.y + randomFromZero(randomness), origin.z);
    }

    static public float randomFromZero(float randomness)
    {
        return Random.Range(-randomness, randomness);
    }

    public static T randomEnumValue<T>()
    {
        var values = System.Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(random);
    }

    static float snapFloat(float gridSize,float origin)
    {
        return Mathf.Round(origin / gridSize) * gridSize;
    }

    static float snapFloatCenter(float gridSize, float origin)
    {
        return Mathf.Round((origin - gridSize / 2f) / gridSize) * gridSize + gridSize/2f;
    }

    static float floatToGridIndexCenter(float gridSize,float origin)
    {
        return Mathf.RoundToInt((origin - gridSize / 2f) / gridSize);
    }

    public static Vector3 snapToGrid(float gridSize, Vector3 origin)
    {
        return new Vector3(snapFloat(gridSize, origin.x), snapFloat(gridSize, origin.y), snapFloat(gridSize, origin.z));
    }

    public static int distanceToIndex(float gridSize, float distance)
    {
        return Mathf.RoundToInt(distance / gridSize);
    }
    public static int distanceCenterToIndex(float gridSize, float distance)
    {
        return Mathf.RoundToInt((distance - gridSize/2f) / gridSize);
    }

    public static Vector3 snapToGridCenter(float gridSize, Vector3 origin)
    {
        return new Vector3(snapFloatCenter(gridSize, origin.x), snapFloatCenter(gridSize, origin.y), snapFloatCenter(gridSize, origin.z));
    }

    public static Vector2 positionToGridIndexCenter2d(float gridSize, Vector3 origin)
    {
        return new Vector2(floatToGridIndexCenter(gridSize, origin.x), floatToGridIndexCenter(gridSize, origin.y));
    }

    public static bool nextToPositionInGrid(float gridSize, Vector3 p1, Vector3 p2)
    {
        var positionIndex1 = positionToGridIndexCenter2d(gridSize, p1);
        var positionIndex2 = positionToGridIndexCenter2d(gridSize, p2);
        if((positionIndex1 - positionIndex2).magnitude <= 1.1f)
        {
            return true;
        }
        return false;
    }

    public static Vector2 chaseDir2d(Vector3 chaser, Vector3 chasee)
    {
        var diff = chasee - chaser;
        if(Mathf.Abs( diff.x) > Mathf.Abs(diff.y))
        {
            return new Vector2(diff.x > 0 ? 1 : -1, 0);
        }
        else
        {

            return new Vector2(0,diff.y > 0 ? 1 : -1);
        }
    }

    public static Vector2 chaseDir2dSecond(Vector3 chaser, Vector3 chasee)
    {
        var diff = chasee - chaser;
        if (Mathf.Abs(diff.x) <= Mathf.Abs(diff.y))
        {
            return new Vector2(diff.x > 0 ? 1 : -1, 0);
        }
        else
        {

            return new Vector2(0, diff.y > 0 ? 1 : -1);
        }
    }


    static public void destroyAllChildren(Transform tran)
    {
        foreach (Transform child in tran)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    static public void setActiveOfAllChildren(Transform tran, bool active = false)
    {
        foreach (Transform child in tran)
        {
            child.gameObject.SetActive(active);
        }
    }
}
