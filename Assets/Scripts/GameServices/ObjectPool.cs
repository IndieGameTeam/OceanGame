using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : IObjectPoolService
{
    private class ObjectCache
    {
        public List<GameObject> gameObjects = new List<GameObject>();
    }

    private Dictionary<ObjectType, List<GameObject>> cachedObjects = new Dictionary<ObjectType, List<GameObject>>();
    
    public ObjectPool()
    {
        foreach(var name in Enum.GetNames(typeof(ObjectType)))
        {
            ObjectType type = (ObjectType)Enum.Parse(typeof(ObjectType), name);

            cachedObjects.Add(type, new List<GameObject>());
        }
    }

    public void AddObject(ObjectType objectType, GameObject gameObject)
    {
        cachedObjects[objectType].Add(gameObject);
        gameObject.SetActive(false);
    }
    public GameObject GetObject(ObjectType objectType, GettingOptions options = GettingOptions.OnlyHided)
    {
        var cache = cachedObjects[objectType];
        var cachedObject = cache.Find(x => IsValid(x, options));

        cachedObject?.SetActive(true);

        return cachedObject;
    }
    public GameObject GetRandObject(ObjectType objectType, GettingOptions options = GettingOptions.OnlyHided)
    {
        var tempList = GetObjects(objectType, int.MaxValue, options);

        if (tempList == null || tempList.Count == 0)
        {
            return null;
        }

        return tempList[UnityEngine.Random.Range(0, tempList.Count)];
    }
    public List<GameObject> GetObjects(ObjectType objectType, int count, GettingOptions options = GettingOptions.OnlyHided)
    {
        var tempList = cachedObjects[objectType].FindAll(x => IsValid(x, options));

        return tempList;
    }

    private bool IsValid(GameObject gameObject, GettingOptions options)
    {
        return
            options == GettingOptions.All ||
            (options == GettingOptions.OnlyShowed && gameObject.activeSelf == true) ||
            (options == GettingOptions.OnlyHided && gameObject.activeSelf == false);
    }
}
