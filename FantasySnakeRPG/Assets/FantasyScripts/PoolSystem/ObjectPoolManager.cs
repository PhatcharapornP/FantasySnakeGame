using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private List<PoolTypeData> poolDataList = new List<PoolTypeData>();

    private Dictionary<Globals.PoolType, Queue<BaseBoardUnit>> poolDict = new Dictionary<Globals.PoolType, Queue<BaseBoardUnit>>();

    public void Initialize(UnityAction callback)
    {
        for (int p = 0; p < poolDataList.Count; p++)
        {
            Queue<BaseBoardUnit> objQueue = new Queue<BaseBoardUnit>();
            for (int i = 0; i < poolDataList[p].PoolSize; i++)
            {
                BaseBoardUnit obj = Instantiate(poolDataList[p].Prefab);
                if (poolDataList[p].PoolContainer != null)
                    obj.transform.SetParent(poolDataList[p].PoolContainer);

                obj.name = poolDataList[p].PoolType.ToString() + i;
                obj.OnUnitSpawnInPool(poolDataList[p].PoolType);
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
    
    public BaseBoardUnit PickFromPool(Globals.PoolType tag)
    {
        if (!poolDict.ContainsKey(tag))
        {
            Debug.LogError($"pool with tag {tag} doesn't exit".InColor(Color.red));
            return null;
        }
    
        BaseBoardUnit objToSpawn = poolDict[tag].Dequeue(); //Get the 1st gameobject in the Queue from pool [tag]
        
        if (objToSpawn == null)
        {
            Debug.LogError($"tried to get obj from pool type {tag.ToString()} with null Piece class!".InColor(Color.red), gameObject);
            return null;
        }
        
        objToSpawn.OnUnitPullFromPool();
        poolDict[tag].Enqueue(objToSpawn); //Add objToSpawn back to the Queue as the last of the Queue again
        return objToSpawn;
    }
}

[Serializable]
public class PoolTypeData
{
    public Globals.PoolType PoolType;
    public BaseBoardUnit Prefab;
    public int PoolSize;
    public Transform PoolContainer;

}