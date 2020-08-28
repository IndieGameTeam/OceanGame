using System.Collections.Generic;
using UnityEngine;

public class DelaunayTriangulation : ITriangulationBuilder
{
	private class Triangle
	{
		private DelaunayTriangulation triangulation;

		private int Index0;
		private int Index1;
		private int Index2;

		private Vector2 center;
		private float sqrRadius;

		public Triangle(DelaunayTriangulation triangulation, int index0, int index1, int index2)
		{
			this.triangulation = triangulation;

			Index0 = index0;
			Index1 = index1;
			Index2 = index2;

			Vector3 v0 = this.triangulation.vertices[index0];
			Vector3 v1 = this.triangulation.vertices[index1];
			Vector3 v2 = this.triangulation.vertices[index2];

			float c = 2.0f * ((v1.x - v0.x) * (v2.z - v0.z) - (v1.z - v0.z) * (v2.x - v0.x));

			float x = ((v2.z - v0.z) * (sq(v1.x) - sq(v0.x) + sq(v1.z) - sq(v0.z)) + (v0.z - v1.z) * (sq(v2.x) - sq(v0.x) + sq(v2.z) - sq(v0.z))) / c;
			float y = ((v0.x - v2.x) * (sq(v1.x) - sq(v0.x) + sq(v1.z) - sq(v0.z)) + (v1.x - v0.x) * (sq(v2.x) - sq(v0.x) + sq(v2.z) - sq(v0.z))) / c;

			center = new Vector2(x, y);
			sqrRadius = Vector2.Distance(new Vector2(v0.x, v0.z), center);
		}

		public IEnumerable<Vector3> GetVertices()
		{
			return new Vector3[]
			{
				triangulation.vertices[Index0],
				triangulation.vertices[Index1],
				triangulation.vertices[Index2]
			};
		}

		public IEnumerable<Triangle> Divide(int newIndex)
		{
			return new Triangle[]
			{
				new Triangle(triangulation, Index0, Index1, newIndex),
				new Triangle(triangulation, Index1, Index2, newIndex),
				new Triangle(triangulation, Index2, Index0, newIndex)
			};
		}

		public bool Contains(params int[] indexes)
		{
			foreach(int index in indexes)
			{
				if (Index0 == index ||
					Index1 == index ||
					Index2 == index)
				{
					return true;
				}
			}

			return false;
		}

		public bool IsInCircle(Vector3 point)
		{
			return Vector2.Distance(new Vector2(point.x, point.z), center) < sqrRadius;
		}
	}

	static float sq(float v)
	{
		return v * v;
	}

	private List<Triangle> triangles;
	private List<Vector3> vertices;

	private List<Vector3> superVertices;
	private List<Vector3> rectVertices;

	private const float sqrt3 = 1.7320F;

	public DelaunayTriangulation()
	{
		superVertices = new List<Vector3>();
		rectVertices = new List<Vector3>();
		vertices = new List<Vector3>();
		triangles = new List<Triangle>();
	}

	public Mesh Triangulation(IEnumerable<Vector3> points)
	{
		Rect rect = new Rect();

		rect.xMin = rect.yMin = float.MaxValue;
		rect.xMax = rect.yMax = float.MinValue;

		foreach (Vector3 point in points)
		{
			if(rect.xMin > point.x)
			{
				rect.xMin = point.x;
			}
			
			if (rect.xMax < point.x)
			{
				rect.xMax = point.x;
			}

			if (rect.yMin > point.z)
			{
				rect.yMin = point.z;
			}

			if (rect.yMax < point.z)
			{
				rect.yMax = point.z;
			}
		}

		Setup(rect);

		foreach (Vector3 point in points)
		{
			Add(point);
		}

		List<int> triangles = new List<int>();
		List<Vector3> vertices = new List<Vector3>();

		foreach (Triangle triangle in this.triangles.FindAll(x => !x.Contains(0, 1, 2)))
		{
			triangles.Add(vertices.Count);
			triangles.Add(vertices.Count + 1);
			triangles.Add(vertices.Count + 2);

			vertices.AddRange(triangle.GetVertices());
		}

		Mesh mesh = new Mesh()
		{ 
			vertices = vertices.ToArray(),
			triangles = triangles.ToArray()
		};

		mesh.RecalculateNormals();

		return mesh;
	}

	private void Setup(Rect rect)
	{
		Clear();

		Vector2 center = rect.center;

		float width = rect.width;
		float height = rect.height;

		float radius = Mathf.Sqrt(width * width + height * height) / 2.0f * 1.25f;

		Vector3 v1 = new Vector3(center.x - sqrt3 * radius, 0, center.y - radius);
		Vector3 v2 = new Vector3(center.x + sqrt3 * radius, 0, center.y - radius);
		Vector3 v3 = new Vector3(center.x, 0, center.y + 2.0f * radius);

		superVertices.Add(v1);
		superVertices.Add(v2);
		superVertices.Add(v3);

		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);

		Triangle t = new Triangle(this, 2, 1, 0);

		triangles.Add(t);

		rectVertices.Add(new Vector3(rect.xMin, 0, rect.yMin));
		rectVertices.Add(new Vector3(rect.xMax, 0, rect.yMin));
		rectVertices.Add(new Vector3(rect.xMax, 0, rect.yMax));
		rectVertices.Add(new Vector3(rect.xMin, 0, rect.yMax));

		Add(rectVertices[0]);
		Add(rectVertices[1]);
		Add(rectVertices[2]);
		Add(rectVertices[3]);
	}

	private void Clear()
	{
		superVertices.Clear();
		vertices.Clear();
		triangles.Clear();
		rectVertices.Clear();
	}

	private void Add(Vector3 vertex)
	{
		int vIndex = vertices.Count;

		vertices.Add(vertex);

		List<Triangle> nextTriangles = new List<Triangle>();
		List<Triangle> newTriangles = new List<Triangle>();

		foreach (Triangle triangle in triangles)
		{
			if (triangle.IsInCircle(vertex))
			{
				newTriangles.AddRange(triangle.Divide(vIndex));
			}
			else
			{
				nextTriangles.Add(triangle);
			}
		}

		for (int ti = 0; ti < newTriangles.Count; ti++)
		{
			Triangle triangle = newTriangles[ti];

			bool isIllegal = false;

			for (int vi = 0; vi < vertices.Count; vi++)
			{
				if (IsIllegalTriangle(triangle, vi))
				{
					isIllegal = true;
					break;
				}
			}
			if (!isIllegal)
			{
				nextTriangles.Add(triangle);
			}
		}

		triangles = nextTriangles;
	}

	private bool IsIllegalTriangle(Triangle triangle, int index)
	{
		if (triangle.Contains(index))
		{
			return false;
		}

		return triangle.IsInCircle(vertices[index]);
	}
}