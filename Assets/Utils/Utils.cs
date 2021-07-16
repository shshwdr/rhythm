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
}
