using System.Collections.Generic;
using UnityEngine;

public class GrassReseed : MonoBehaviour
{
	private Mesh mesh;

	private Vector3[] points;

	private Vector2[] uvs;

	private List<Vector3> newPoints = new List<Vector3>();

	private void Awake()
	{
		MeshFilter component = GetComponent<MeshFilter>();
		if (component == null || component.sharedMesh == null || !component.sharedMesh.isReadable)
		{
			Object.Destroy(this);
			return;
		}
		mesh = GetComponent<MeshFilter>().sharedMesh;
		points = mesh.vertices;
		uvs = mesh.uv2;
		Reseed();
	}

	private void Reseed()
	{
		for (int i = 0; i < mesh.vertexCount; i++)
		{
			if (Physics.Raycast(new Ray(points[i] + Vector3.up * 0.15f, Vector3.down), out var _))
			{
				newPoints.Add(points[i]);
			}
			else
			{
				newPoints.Add(points[i]);
			}
		}
		mesh.SetVertices(newPoints);
		Object.Destroy(this);
	}
}
