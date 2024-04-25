using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ObjectPoolManager : MonoBehaviour
{
    [FormerlySerializedAs("poolData")] [SerializeField] private List<PoolTypeData> poolDataList = new List<PoolTypeData>();

    private Dictionary<Globals.PoolType, Queue<GameObject>> poolDict = new Dictionary<Globals.PoolType, Queue<GameObject>>();

    public void Initialize(UnityAction callback)
    {
        for (int p = 0; p < poolDataList.Count; p++)
        {
            Queue<GameObject> objQueue = new Queue<GameObject>();
            for (int i = 0; i < poolDataList[p].PoolSize; i++)
            {
                GameObject obj = Instantiate(poolDataList[p].Prefab);
                if (poolDataList[p].PoolContainer != null)
                    obj.transform.SetParent(poolDataList[p].PoolContainer);

                obj.name = poolDataList[p].PoolType.ToString() + i;
                obj.SetActive(false);
                objQueue.Enqueue(obj);
                
                //TODO: Initialize whatever that needed when spawn
            }
            
            poolDict.Add(poolDataList[p].PoolType,objQueue);

            if (p == poolDataList.Count - 1)
            {
                //TODO: In case of callback, invoke here...
                callback?.Invoke();
            }
        }
    }
    
    public GameObject PickFromPool(Globals.PoolType tag)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogError($"pool with tag {tag} doesn't exit".InColor(Color.red));
            return null;
        }
    
        GameObject objToSpawn = poolDict[tag].Dequeue(); //Get the 1st gameobject in the Queue from pool [tag]
        objToSpawn.SetActive(true);
        poolDict[tag].Enqueue(objToSpawn); //Add objToSpawn back to the Queue as the last of the Queue again
        return objToSpawn;
    }

}

[Serializable]
public class PoolTypeData
{
    public Globals.PoolType PoolType;
    public GameObject Prefab;
    public int PoolSize;
    public Transform PoolContainer;

}