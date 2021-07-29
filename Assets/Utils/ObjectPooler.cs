using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pool;

[System.Serializable]

public class ObjectPoolItem
{
    public GameObject objectToPool;
    public int amountToPool = 50;
    public bool shouldExpand = true;
}

public class ObjectPooler : Singleton<ObjectPooler>
{

    public List<ObjectPoolItem> itemsToPool;
    Dictionary<string, ObjectPoolItem> itemByName;
    public Dictionary<string, List<GameObject>> pooledObjects;

    public Transform colliderCacheParent;
    public GameObject colliderPrefab;


    // Use this for initialization
    void Start()
    {
        pooledObjects = new Dictionary<string, List<GameObject>>();
        itemByName = new Dictionary<string, ObjectPoolItem>();
        foreach (ObjectPoolItem item in itemsToPool)
        {
            addPooledItems(item, transform);
        }
        var prefabs = Resources.LoadAll("pooling", typeof(GameObject));
        foreach(GameObject p in prefabs)
        {
            if (!pooledObjects.ContainsKey(p.name))
            {
                ObjectPoolItem item = new ObjectPoolItem();
                item.objectToPool = p;
                addPooledItems(item,transform);
            }
        }

        ObjectPoolItem item2 = new ObjectPoolItem();
        item2.objectToPool = colliderPrefab;
        item2.shouldExpand = false;
        addPooledItems(item2, colliderCacheParent);

        EventPool.OptIn(EventPool.startGameEvent, resetAll);


    }
    public void resetAll()
    {
        foreach(Transform trans in transform)
        {
            
            trans.GetComponent<PoolObject>().returnBack();
        }
    }
    void addPooledItems(ObjectPoolItem item, Transform trans)
    {

        itemByName[item.objectToPool.name] = item;
        pooledObjects[item.objectToPool.name] = new List<GameObject>();
        for (int i = 0; i < item.objectToPool.GetComponent<PoolObject>().cacheCount; i++)
        {
            GameObject obj = (GameObject)Instantiate(item.objectToPool, trans);
            obj.SetActive(false);
            pooledObjects[item.objectToPool.name].Add(obj);
        }
    }

    public GameObject GetPooledObject(string name)
    {
        if (!pooledObjects.ContainsKey(name))
        {
            Debug.LogError(name+ " does not existed in pool");
        }
        for (int i = 0; i < pooledObjects[name].Count; i++)
        {
            if (!pooledObjects[name][i].activeInHierarchy)
            {
                return pooledObjects[name][i];
            }
        }
        //foreach (ObjectPoolItem item in itemsToPool)
        var item = itemByName[name];
        {
                if (item.shouldExpand)
                {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(false);
                    pooledObjects[name].Add(obj);
                    return obj;
                }
        }
        return null;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
