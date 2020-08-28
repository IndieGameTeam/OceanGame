using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
    private class ObjectCache
    {
        public List<GameObject> gameObjects = new List<GameObject>();
    }

    private Dictionary<int, ObjectCache> cachedObjects = new Dictionary<int, ObjectCache>();

    public void AddObject(ObjectType objectType, GameObject gameObject)
    {
        AddObject((int)objectType, gameObject);
    }
    public void AddObject(int id, GameObject gameObject)
    {
        if(!cachedObjects.ContainsKey(id))
        {
            cachedObjects.Add(id, new ObjectCache());
        }

        cachedObjects[id].gameObjects.Add(gameObject);
        gameObject.SetActive(false);
    }

    public GameObject GetObject(ObjectType objectType)
    {
        return GetObject((int)objectType);
    }
    public GameObject GetObject(int id)
    {
        if(!cachedObjects.ContainsKey(id))
        {
            return null;
        }

        var cache = cachedObjects[id];
        var cachedObject = cache.gameObjects.Find(x => x.activeSelf == false);

        cachedObject?.SetActive(true);

        return cachedObject;
    }
}
