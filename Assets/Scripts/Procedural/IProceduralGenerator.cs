using UnityEngine;

public interface IProceduralGenerator
{
    GameObject Create(float seed, Material material);
}