using System;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    PlayerBullet,
    EnemyBullet,
    EnemyCone,
    EnemyObstacle,
    EnemySpecialBullet
}

[Serializable]
public class PoolInfo
{
    public PoolObjectType type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject container;

    [HideInInspector] public List<GameObject> pool = new();
}

public class PoolManager : Singleton<PoolManager>
{    
    [SerializeField] private List<PoolInfo> listOfPools;
    [SerializeField] private Vector3 defaultObjectPosition;

    private void Start()
    {
        for (int i = 0; i < listOfPools.Count; i++)
        {
            FillPool(listOfPools[i]);
        }
    }

    private void FillPool(PoolInfo info)
    {
        for (int i = 0; i < info.amount; i++)
        {
            GameObject objInstance = SpawnObject(info.type, info.container.transform);
            objInstance.SetActive(false);
            objInstance.transform.position = defaultObjectPosition;
            info.pool.Add(objInstance);
        }
    }

    public GameObject GetPooledObject(PoolObjectType type)
    {
        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        GameObject objInstance;

        if (pool.Count > 0)
        {
            objInstance = pool[^1];
            pool.Remove(objInstance);
        }
        else
        {
            objInstance = Instantiate(selected.prefab, selected.container.transform);
        }

        return objInstance;
    }

    public void CoolObject(GameObject obj, PoolObjectType type)
    {
        obj.SetActive(false);
        obj.transform.position = defaultObjectPosition;

        PoolInfo selected = GetPoolByType(type);
        List<GameObject> pool = selected.pool;

        if (!pool.Contains(obj))
            pool.Add(obj);
    }

    private PoolInfo GetPoolByType(PoolObjectType type)
    {
        for (int i = 0; i < listOfPools.Count; i++)
        {
            if (type == listOfPools[i].type)
                return listOfPools[i];
        }

        return null;
    }

    public GameObject SpawnObject(PoolObjectType objectType, Transform newParent)
    {
        Object objectToInstantiate = ObjectFactory.Instance.CreateObject(objectType, newParent);

        return objectToInstantiate.gameObject;
    }
}
