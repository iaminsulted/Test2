using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D), typeof(MeshFilter), typeof(MeshRenderer))]
public class PolygonDrawScript : MonoBehaviour
{
	private PolygonCollider2D _poly;

	private MeshFilter _filter;

	private MeshRenderer _renderer;

	private PolygonCollider2D Poly => _poly ?? (_poly = base.gameObject.GetComponent<PolygonCollider2D>());

	private MeshFilter Filter => _filter ?? (_filter = base.gameObject.GetComponent<MeshFilter>());

	private MeshRenderer Renderer => _renderer ?? (_renderer = base.gameObject.GetComponent<MeshRenderer>());

	public void Start()
	{
		RebuildMesh();
	}

	public void RebuildMesh()
	{
		int[] triangles = new Triangulator(Poly.points).Triangulate();
		Vector3[] array = new Vector3[Poly.points.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = new Vector3(Poly.points[i].x, Poly.points[i].y, 0f);
		}
		Mesh mesh = new Mesh
		{
			vertices = array,
			triangles = triangles
		};
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		Filter.mesh = mesh;
	}
}
