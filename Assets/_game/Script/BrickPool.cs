using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BrickPool : MonoBehaviour
{
    public static BrickPool instance;
    public List<GameObject> pooledObjects;

    void Awake()
    {
        instance = this;
        pooledObjects = new List<GameObject>();
    }
    public void AddToThePool(GameObject obj)
    {
        pooledObjects.Add(obj);
    }
    public GameObject GetPooledBrick()
    {
        here:
        int rand = Random.Range(0, pooledObjects.Count-1);
        if (pooledObjects[rand].activeInHierarchy == true)
        {
            goto here;
        }
        else
        {
            return pooledObjects[rand];
        }
    }

    public void ResetBrick()
    {
        //make all the brick inactive active again 
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].SetActive(true);
        }
    }
}
