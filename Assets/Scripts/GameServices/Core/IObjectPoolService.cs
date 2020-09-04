using UnityEngine;
using System.Collections.Generic;

public interface IObjectPoolService
{
    void AddObject(ObjectType objectType, GameObject gameObject);
    GameObject GetObject(ObjectType objectType, GettingOptions options = GettingOptions.OnlyHided);
    GameObject GetRandObject(ObjectType objectType, GettingOptions options = GettingOptions.OnlyHided);
    List<GameObject> GetObjects(ObjectType objectType, int count, GettingOptions options = GettingOptions.OnlyHided);
}