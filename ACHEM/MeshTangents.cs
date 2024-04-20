using UnityEngine;

public class MeshTangents
{
	public static void calculateMeshTangents(Mesh mesh)
	{
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		int num = triangles.Length;
		int num2 = vertices.Length;
		Vector3[] array = new Vector3[num2];
		Vector3[] array2 = new Vector3[num2];
		Vector4[] array3 = new Vector4[num2];
		long num3 = 0L;
		long num4 = 0L;
		for (num3 = 0L; num3 < num; num3 += 3)
		{
			long num5 = triangles[num3];
			long num6 = triangles[num3 + 1];
			num4 = triangles[num3 + 2];
			Vector3 vector = vertices[num5];
			Vector3 vector2 = vertices[num6];
			Vector3 vector3 = vertices[num4];
			Vector3 vector4 = uv[num5];
			Vector3 vector5 = uv[num6];
			Vector3 vector6 = uv[num4];
			float num7 = vector2.x - vector.x;
			float num8 = vector3.x - vector.x;
			float num9 = vector2.y - vector.y;
			float num10 = vector3.y - vector.y;
			float num11 = vector2.z - vector.z;
			float num12 = vector3.z - vector.z;
			float num13 = vector5.x - vector4.x;
			float num14 = vector6.x - vector4.x;
			float num15 = vector5.y - vector4.y;
			float num16 = vector6.y - vector4.y;
			float num17 = 1f / (num13 * num16 - num14 * num15);
			Vector3 vector7 = new Vector3((num16 * num7 - num15 * num8) * num17, (num16 * num9 - num15 * num10) * num17, (num16 * num11 - num15 * num12) * num17);
			Vector3 vector8 = new Vector3((num13 * num8 - num14 * num7) * num17, (num13 * num10 - num14 * num9) * num17, (num13 * num12 - num14 * num11) * num17);
			array[num5] += vector7;
			array[num6] += vector7;
			array[num4] += vector7;
			array2[num5] += vector8;
			array2[num6] += vector8;
			array2[num4] += vector8;
		}
		for (num3 = 0L; num3 < num2; num3++)
		{
			Vector3 normal = normals[num3];
			Vector3 tangent = array[num3];
			Vector3.OrthoNormalize(ref normal, ref tangent);
			array3[num3].x = tangent.x;
			array3[num3].y = tangent.y;
			array3[num3].z = tangent.z;
			array3[num3].w = ((Vector3.Dot(Vector3.Cross(normal, tangent), array2[num3]) < 0f) ? (-1f) : 1f);
		}
		mesh.tangents = array3;
	}
}
