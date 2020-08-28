using UnityEngine;
using System.Collections.Generic;

public interface ITriangulationBuilder
{
    Mesh Triangulation(IEnumerable<Vector3> points);
}