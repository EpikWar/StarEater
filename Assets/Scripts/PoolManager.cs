using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static List<PooledObject> pools = new();

    private GameObject emptyPoolHolder;
    private static GameObject emptyBulletHolder;

    public enum PoolName
    {
        Bullet,
        None
    }

    public static PoolName PoolingName;


    private void Awake()
    {
        SetupHolders();
    }


    private void SetupHolders()
    {
        emptyPoolHolder = new GameObject("Pool Holder");

        emptyBulletHolder = new GameObject("Bullet Holder");
        emptyBulletHolder.transform.SetParent(emptyPoolHolder.transform);
    }

    public static GameObject SpawnObject(GameObject objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation,
        PoolName poolName = PoolName.None)
    {
        //find pool wia name
        PooledObject pool = pools.FirstOrDefault(p => p.pooledObjectName == objectToSpawn.name);

        //if there is no created pool 
        if (pool == null) {
            pool = new PooledObject { pooledObjectName = objectToSpawn.name };
            pools.Add(pool);
        }

        GameObject objToSpawn = pool.inactiveObject.FirstOrDefault();

        //if no inactive object
        if (objToSpawn == null) {
            GameObject parentObject = SetParentObject(poolName);

            objToSpawn = Instantiate(objectToSpawn, spawnPosition, spawnRotation);

            if (parentObject != null) {
                objToSpawn.transform.SetParent(parentObject.transform);
            }
        }
        else {
            objToSpawn.transform.position = spawnPosition;
            objToSpawn.transform.rotation = spawnRotation;
            objToSpawn.SetActive(true);
            pool.inactiveObject.Remove(objToSpawn);
        }

        //?
        return objToSpawn;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public static void ReturnToPool(GameObject obj)
    {
        //removing "(clone) from obj.name"
        string objName = obj.name.Substring(0, obj.name.Length - 7);

        //find pool wia name
        PooledObject pool = pools.FirstOrDefault(p => p.pooledObjectName == objName);

        if (pool == null) {
            Debug.LogWarning("Pool not created for " + obj.name);
        }
        else {
            obj.SetActive(false);
            pool.inactiveObject.Add(obj);
        }
    }

    private static GameObject SetParentObject(PoolName poolName)
    {
        return poolName switch
        {
            PoolName.Bullet => emptyBulletHolder,
            PoolName.None => null,
            _ => null
        };
    }
}
