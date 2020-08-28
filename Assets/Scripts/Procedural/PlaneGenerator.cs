using System.Collections.Generic;
using UnityEngine;

using GameServices.Extensions;

public class PlaneGenerator : MonoBehaviour
{
    public int WidthSegmentsCount = 5;
    public int LengthSegmentsCount = 5;

    [ContextMenu("Generate")]
    private void Generate()
    {
        gameObject.GetOrAddComponent<MeshRenderer>();
        gameObject.GetOrAddComponent<MeshFilter>().sharedMesh = CreatePlane();
    }

    private Mesh CreatePlane()
    {
        Vector3 scale = new Vector3(1F / WidthSegmentsCount, 1, 1F / LengthSegmentsCount);
        Vector3 offset = new Vector3(WidthSegmentsCount, 0, LengthSegmentsCount) * -0.5F;

        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();
        List<Vector3> vertices = new List<Vector3>();

        float uvWidth = 1F / WidthSegmentsCount;
        float uvHeight = 1F / LengthSegmentsCount;

        float vertexWidth = 1F / WidthSegmentsCount;
        float vertexHeight = 1F / LengthSegmentsCount;

        for (int y = 0; y < LengthSegmentsCount; y++)
            for (int x = 0; x < WidthSegmentsCount; x++)
            {
                Vector3 vertex = Vector3.Scale(new Vector3(x, 0, y) + offset, scale);
                Vector2 uvPoint = new Vector2(x / (float)WidthSegmentsCount, y / (float)LengthSegmentsCount);

                int verticesCount = vertices.Count;

                vertices.Add(vertex);
                vertices.Add(vertex + new Vector3(0, 0, vertexHeight));
                vertices.Add(vertex + new Vector3(vertexWidth, 0, vertexHeight));
                vertices.Add(vertex + new Vector3(vertexWidth, 0, -0));

                uv.Add(uvPoint);
                uv.Add(uvPoint + new Vector2(0, uvHeight));
                uv.Add(uvPoint + new Vector2(uvWidth, uvHeight));
                uv.Add(uvPoint + new Vector2(uvWidth, 0));

                triangles.Add(verticesCount);
                triangles.Add(verticesCount + 1);
                triangles.Add(verticesCount + 2);

                triangles.Add(verticesCount);
                triangles.Add(verticesCount + 2);
                triangles.Add(verticesCount + 3);
            }

        //for (int y = 0; y < LengthSegmentsCount + 1; y++)
        //    for (int x = 0; x < WidthSegmentsCount + 1; x++)
        //    {
        //        Vector3 vertex = Vector3.Scale(new Vector3(x, 0, y) + offset, scale);
        //        Vector2 uvPoint = new Vector2(x / (float)WidthSegmentsCount, y / (float)LengthSegmentsCount);

        //        vertices.Add(vertex);
        //        uv.Add(uvPoint);

        //        if (x < WidthSegmentsCount && y < LengthSegmentsCount)
        //        {
        //            triangles.Add(GetPlaneTriangleIndex(x, y));
        //            triangles.Add(GetPlaneTriangleIndex(x, y + 1));
        //            triangles.Add(GetPlaneTriangleIndex(x + 1, y + 1));

        //            triangles.Add(GetPlaneTriangleIndex(x, y));
        //            triangles.Add(GetPlaneTriangleIndex(x + 1, y + 1));
        //            triangles.Add(GetPlaneTriangleIndex(x + 1, y));
        //        }
        //    }

        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();

        return mesh;
    }

    private int GetPlaneTriangleIndex(int x, int y)
    {
        return x + y * (LengthSegmentsCount + 1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.Scale(transform.localScale, new Vector3(1, 0 ,1)));
    }
}
