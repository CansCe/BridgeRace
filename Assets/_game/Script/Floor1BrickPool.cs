using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Floor1BrickPool : MonoBehaviour
{
    public static Floor1BrickPool instance;
    public List<GameObject> pooledObjects;
    public int amountToPool;

    void Awake()
    {
        instance = this;
        pooledObjects = new List<GameObject>();
    }

    void Start()
    {
        
    }
    public void AddToThePool(GameObject obj)
    {
        pooledObjects.Add(obj);
    }
    public GameObject GetPooledBrick()
    {
        here:
        int rand = Random.Range(0, pooledObjects.Count);
        if (pooledObjects[rand].activeInHierarchy == true)
        {
            goto here;
        }
        else
        {
            return pooledObjects[rand];
        }
    }
}
