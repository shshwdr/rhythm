using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyGroupShape {  line, cone}
public class GroupEnemyGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }


    static public List<GameObject> generateEnemyGroup(string enemyName, int count, EnemyGroupShape shape, Vector3 position, Vector3 positionDir,Transform parent, float sizeScale = 1, bool isCenter = false)
    {
        GameObject prefab = ObjectPooler.Instance.GetPooledObject(enemyName);
        float prefabSize = prefab.GetComponent<CircleCollider2D>().radius*2 *4f * sizeScale;
        float randomnessScale = 0.4f;
        List<GameObject> res = new List<GameObject>();
        List<Vector3> standardPosition = new List<Vector3>();
        switch (shape)
        {
            case EnemyGroupShape.line:

                if (isCenter)
                {
                    position += (positionDir * prefabSize) / 2;
                    for (int i = 0; i < count; i++)
                    {
                        standardPosition.Add(position);
                        position -= positionDir * prefabSize;
                    }
                }
                else
                {

                    for (int i = 0; i < count; i++)
                    {
                        standardPosition.Add(position);
                        position -= positionDir * prefabSize;
                    }
                }
                break;
            case EnemyGroupShape.cone:
                int k = 1;
                standardPosition.Add(position);
                for (int i = 1; i < 10; i++)
                {
                    for(int j = -i/2; j <= i/2; j++)
                    {
                        Vector3 newPos = position - positionDir * prefabSize * i + (Vector3) Vector2.Perpendicular(positionDir) * prefabSize * j;

                        standardPosition.Add(newPos);
                        k++;
                        if (k >= count)
                        {
                            break;
                        }
                    }
                    if (k >= count)
                    {
                        break;
                    }
                }
                break;

        }

        foreach (Vector3 v in standardPosition)
        {
            GameObject go = ObjectPooler.Instance.GetPooledObject(enemyName);
            if (!go)
            {
                return res;
            }
            //Vector3 randomedV = Utils.randomVector3_2d(v, randomnessScale*prefabSize);
            go.transform.position = v;
            go.GetComponent<PoolObject>().fetch();
            res.Add(go);
        }
        return res;
    }

}
