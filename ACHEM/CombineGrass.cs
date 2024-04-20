using System.Collections.Generic;
using UnityEngine;

public class CombineGrass : MonoBehaviour
{
	private GrassVertexData[] grass;

	public Material mobileMaterial;

	private void Start()
	{
		MeshFilter meshFilter = base.gameObject.AddComponent<MeshFilter>();
		base.gameObject.AddComponent<MeshRenderer>().sharedMaterial = mobileMaterial;
		grass = GetComponentsInChildren<GrassVertexData>();
		CombineInstance[] array = new CombineInstance[grass.Length];
		int num = 0;
		GrassVertexData[] array2 = grass;
		foreach (GrassVertexData grassVertexData in array2)
		{
			Mesh sharedMesh = grassVertexData.gameObject.GetComponentInChildren<MeshFilter>().sharedMesh;
			Mesh mesh = new Mesh
			{
				vertices = sharedMesh.vertices,
				uv = sharedMesh.uv,
				triangles = sharedMesh.triangles,
				normals = sharedMesh.normals
			};
			int vertexCount = sharedMesh.vertexCount;
			List<Vector2> list = new List<Vector2>();
			List<Color> list2 = new List<Color>();
			for (int j = 0; j < vertexCount; j++)
			{
				list.Add(grassVertexData.uv2);
				list2.Add(grassVertexData.vertexColor);
			}
			mesh.SetUVs(1, list);
			mesh.SetColors(list2);
			array[num].mesh = mesh;
			array[num].transform = grassVertexData.gameObject.GetComponentInChildren<MeshFilter>().transform.localToWorldMatrix;
			num++;
			Object.Destroy(grassVertexData.gameObject);
		}
		meshFilter.mesh = new Mesh();
		meshFilter.mesh.CombineMeshes(array);
		Object.Destroy(this);
	}
}
