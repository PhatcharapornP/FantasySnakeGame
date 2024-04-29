using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
            }
            
            poolDict.Add(poolDataList[p].PoolType,objQueue);

            if (p == poolDataList.Count - 1)
                callback?.Invoke();
        }
    }
    
    public BaseBoardUnit PickFromPool(Globals.PoolType tag)
    {
        if (!poolDict.ContainsKey(tag))
            return null;
    
        BaseBoardUnit objToSpawn = poolDict[tag].Dequeue(); //Get the 1st gameobject in the Queue from pool [tag]
        
        if (objToSpawn == null)
            return null;
        
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