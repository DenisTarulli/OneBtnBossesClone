using System.Collections.Generic;
using UnityEngine;

public class ObjectFactory : Singleton<ObjectFactory>
{
    [SerializeField] private Object[] objects;

    private Dictionary<PoolObjectType, Object> objectsByName;

    private void Awake()
    {
        objectsByName = new Dictionary<PoolObjectType, Object>();

        foreach (var obj in objects)
        {
            objectsByName.Add(obj.ObjectType, obj);
        }
    }

    public Object CreateObject(PoolObjectType objectType, Transform newParent)
    {
        if (objectsByName.TryGetValue(objectType, out Object objectPrefab))
        {
            Object objectInstance = Instantiate(objectPrefab, newParent);

            return objectInstance;
        }

        else
        {
            Debug.LogWarning($"El objeto '{objectType}' no existe en la base de datos de objetos.");

            return null;
        }
    }
}
