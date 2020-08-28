using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : IProceduralGenerator
{
    private float noise = 0.5F;
    private float seed = 1.0F;
    private int pointsCount = 75;

    private List<Vector3> points = new List<Vector3>();
    private ITriangulationBuilder triangulationBuilder = new DelaunayTriangulation();

    public GameObject Create(float seed, Material material)
    {
        GameObject islandGameObject = new GameObject("Island");

        var meshFilter = islandGameObject.AddComponent<MeshFilter>();
        var meshRenderer = islandGameObject.AddComponent<MeshRenderer>();

        meshFilter.sharedMesh = GenerateMesh();
        meshRenderer.material = material;

        return islandGameObject;
    }

    private Mesh GenerateMesh()
    {
        for (int i = 0; i < pointsCount; i++)
        {
            float x = Random.value - 0.5F;
            float z = Random.value - 0.5F;
            float dst = Mathf.Sqrt(x * x + z * z);
            float y = Mathf.Clamp01(0.25F - dst * dst + (Mathf.PerlinNoise(x + seed, z + seed) - 0.5F) * noise);

            points.Add(new Vector3(x, y, z));
        }

        Mesh mesh = triangulationBuilder.Triangulation(points);

        points.Clear();

        return mesh;
    }
}