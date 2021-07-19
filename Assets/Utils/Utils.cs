using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
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
        return Mathf.Round(origin / gridSize) * gridSize + gridSize/2f;
    }

    static float floatToGridIndexCenter(float gridSize,float origin)
    {
        return Mathf.RoundToInt((origin - gridSize / 2f) / gridSize);
    }

    public static Vector3 snapToGrid(float gridSize, Vector3 origin)
    {
        return new Vector3(snapFloat(gridSize, origin.x), snapFloat(gridSize, origin.y), snapFloat(gridSize, origin.z));
    }


    public static Vector3 snapToGridCenter(float gridSize, Vector3 origin)
    {
        return new Vector3(snapFloatCenter(gridSize, origin.x), snapFloatCenter(gridSize, origin.y), snapFloatCenter(gridSize, origin.z));
    }

    public static Vector2 positionToGridIndexCenter2d(float gridSize, Vector3 origin)
    {
        return new Vector2(floatToGridIndexCenter(gridSize, origin.x), floatToGridIndexCenter(gridSize, origin.y));
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
}
