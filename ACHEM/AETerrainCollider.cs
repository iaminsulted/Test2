using UnityEngine;

public class AETerrainCollider : AECollider
{
	public float[,] heightmap;

	public float scaleX;

	public float scaleZ;

	public AEVector3 boundingMin;

	public AEVector3 boundingMax;

	public void InitHeightMap(AEMeshCollider mesh)
	{
		Center = new AEVector3(mesh.Center.X, mesh.Center.Y, mesh.Center.Z);
		boundingMin = new AEVector3(float.MaxValue, float.MaxValue, float.MaxValue);
		boundingMax = new AEVector3(float.MinValue, float.MinValue, float.MinValue);
		for (int i = 0; i < mesh.vertices.Length; i++)
		{
			boundingMin = AEVector3.Min(boundingMin, mesh.vertices[i]);
			boundingMax = AEVector3.Max(boundingMax, mesh.vertices[i]);
		}
		Debug.Log("boundingMin: " + boundingMin);
		Debug.Log("boundingMax: " + boundingMax);
		AEVector3[] array = new AEVector3[3]
		{
			mesh.vertices[mesh.triangles[0]],
			mesh.vertices[mesh.triangles[1]],
			mesh.vertices[mesh.triangles[2]]
		};
		for (int j = 1; j < 3; j++)
		{
			if ((double)Mathf.Abs(array[0].Z - array[j].Z) <= 0.01)
			{
				scaleX = Mathf.Abs(array[0].X - array[j].X);
			}
			if ((double)Mathf.Abs(array[0].X - array[j].X) <= 0.01)
			{
				scaleZ = Mathf.Abs(array[0].Z - array[j].Z);
			}
		}
		Debug.Log("scaleX: " + scaleX);
		Debug.Log("scaleZ: " + scaleZ);
		int num = Mathf.RoundToInt((boundingMax.X - boundingMin.X) / scaleX + 1f);
		int num2 = Mathf.RoundToInt((boundingMax.Z - boundingMin.Z) / scaleZ + 1f);
		Debug.Log(num);
		Debug.Log(num2);
		heightmap = new float[num, num2];
		for (int k = 0; k < mesh.vertices.Length; k++)
		{
			AEVector3 aEVector = mesh.vertices[k];
			int num3 = Mathf.RoundToInt((aEVector.X - boundingMin.X) / scaleX);
			int num4 = Mathf.RoundToInt((aEVector.Z - boundingMin.Z) / scaleZ);
			heightmap[num3, num4] = aEVector.Y;
		}
	}
}
