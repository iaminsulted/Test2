using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public static class MeshUtil
{
	public static void CombineMesh(GameObject subject, SkinnedMeshRenderer[] allsmr)
	{
		if (allsmr.Length == 0)
		{
			throw new Exception("Mesh List is empty");
		}
		Transform transform = subject.transform;
		Matrix4x4 worldToLocalMatrix = transform.worldToLocalMatrix;
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		int num = 0;
		SkinnedMeshRenderer[] array = allsmr;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			num += skinnedMeshRenderer.sharedMesh.vertexCount;
		}
		List<Transform> list = new List<Transform>();
		List<Matrix4x4> list2 = new List<Matrix4x4>();
		BoneWeight[] array2 = new BoneWeight[num];
		int num2 = 0;
		int num3 = 0;
		List<CombineInstance> list3 = new List<CombineInstance>();
		List<Material> list4 = new List<Material>();
		array = allsmr;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer2 in array)
		{
			Transform[] bones = skinnedMeshRenderer2.bones;
			int[] array3 = new int[bones.Length];
			CombineInstance item = default(CombineInstance);
			item.mesh = skinnedMeshRenderer2.sharedMesh;
			item.transform = worldToLocalMatrix * skinnedMeshRenderer2.transform.localToWorldMatrix;
			for (int j = 0; j < bones.Length; j++)
			{
				int instanceID = bones[j].GetInstanceID();
				if (!dictionary.ContainsKey(instanceID))
				{
					dictionary.Add(instanceID, num2);
					list.Add(bones[j]);
					list2.Add(bones[j].worldToLocalMatrix * transform.localToWorldMatrix);
					num2++;
				}
				array3[j] = dictionary[instanceID];
			}
			BoneWeight[] boneWeights = skinnedMeshRenderer2.sharedMesh.boneWeights;
			for (int k = 0; k < boneWeights.Length; k++)
			{
				BoneWeight boneWeight = boneWeights[k];
				BoneWeight boneWeight2 = boneWeight;
				boneWeight2.boneIndex0 = array3[boneWeight.boneIndex0];
				boneWeight2.boneIndex1 = array3[boneWeight.boneIndex1];
				boneWeight2.boneIndex2 = array3[boneWeight.boneIndex2];
				boneWeight2.boneIndex3 = array3[boneWeight.boneIndex3];
				array2[num3] = boneWeight2;
				num3++;
			}
			for (int l = 0; l < skinnedMeshRenderer2.sharedMesh.subMeshCount; l++)
			{
				item.subMeshIndex = l;
				list3.Add(item);
			}
			Material[] sharedMaterials = skinnedMeshRenderer2.sharedMaterials;
			foreach (Material item2 in sharedMaterials)
			{
				if (!list4.Contains(item2))
				{
					list4.Add(item2);
				}
			}
		}
		GameObject gameObject = new GameObject("Model");
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		gameObject.layer = subject.layer;
		SkinnedMeshRenderer skinnedMeshRenderer3 = gameObject.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer3.shadowCastingMode = ShadowCastingMode.On;
		skinnedMeshRenderer3.receiveShadows = true;
		skinnedMeshRenderer3.quality = SkinQuality.Bone4;
		skinnedMeshRenderer3.sharedMesh = new Mesh();
		skinnedMeshRenderer3.sharedMesh.CombineMeshes(list3.ToArray());
		skinnedMeshRenderer3.bones = list.ToArray();
		skinnedMeshRenderer3.sharedMesh.bindposes = list2.ToArray();
		skinnedMeshRenderer3.sharedMesh.boneWeights = array2;
		if (list4.Count > 0)
		{
			skinnedMeshRenderer3.sharedMaterial = list4[0];
		}
		skinnedMeshRenderer3.sharedMesh.RecalculateBounds();
		array = allsmr;
		for (int i = 0; i < array.Length; i++)
		{
			UnityEngine.Object.Destroy(array[i].gameObject);
		}
	}

	public static void Bake(GameObject subject)
	{
		float time = Time.time;
		Component[] componentsInChildren = subject.GetComponentsInChildren<SkinnedMeshRenderer>();
		Component[] array = componentsInChildren;
		List<Component> list = new List<Component>();
		componentsInChildren = array;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = (SkinnedMeshRenderer)componentsInChildren[i];
			if (skinnedMeshRenderer.enabled)
			{
				list.Add(skinnedMeshRenderer);
			}
		}
		Matrix4x4 worldToLocalMatrix = subject.transform.worldToLocalMatrix;
		Hashtable hashtable = new Hashtable();
		int num = 0;
		int capacity = 49;
		foreach (SkinnedMeshRenderer item2 in list)
		{
			num += item2.sharedMesh.vertexCount;
		}
		Debug.Log(capacity + " bones");
		Debug.Log(num + " verts");
		List<Transform> list2 = new List<Transform>(capacity);
		List<Matrix4x4> list3 = new List<Matrix4x4>(capacity);
		BoneWeight[] array2 = new BoneWeight[num];
		int num2 = 0;
		int num3 = 0;
		List<CombineInstance> list4 = new List<CombineInstance>();
		List<Material> list5 = new List<Material>();
		for (int j = 0; j < list.Count; j++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer3 = (SkinnedMeshRenderer)list[j];
			CombineInstance item = default(CombineInstance);
			item.mesh = skinnedMeshRenderer3.sharedMesh;
			if (!(skinnedMeshRenderer3 != null) || !skinnedMeshRenderer3.enabled || !(item.mesh != null))
			{
				continue;
			}
			item.transform = worldToLocalMatrix * skinnedMeshRenderer3.transform.localToWorldMatrix;
			for (int k = 0; k < skinnedMeshRenderer3.bones.Length; k++)
			{
				if (!list2.Contains(skinnedMeshRenderer3.bones[k]))
				{
					list2.Add(skinnedMeshRenderer3.bones[k]);
					hashtable.Add(skinnedMeshRenderer3.bones[k].name, num2);
					list3.Add(skinnedMeshRenderer3.bones[k].worldToLocalMatrix * subject.transform.localToWorldMatrix);
					num2++;
				}
			}
			for (int l = 0; l < skinnedMeshRenderer3.sharedMesh.subMeshCount; l++)
			{
				item.subMeshIndex = l;
				list4.Add(item);
			}
			for (int m = 0; m < skinnedMeshRenderer3.sharedMesh.boneWeights.Length; m++)
			{
				array2[num3] = recalculateIndexes(skinnedMeshRenderer3.sharedMesh.boneWeights[m], hashtable, skinnedMeshRenderer3.bones);
				num3++;
			}
			for (int n = 0; n < skinnedMeshRenderer3.sharedMaterials.Length; n++)
			{
				if (!list5.Contains(skinnedMeshRenderer3.sharedMaterials[n]))
				{
					list5.Add(skinnedMeshRenderer3.sharedMaterials[n]);
				}
			}
		}
		SkinnedMeshRenderer[] componentsInChildren2 = subject.GetComponentsInChildren<SkinnedMeshRenderer>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			UnityEngine.Object.DestroyImmediate(componentsInChildren2[i].gameObject);
		}
		GameObject gameObject = new GameObject("Baked");
		SkinnedMeshRenderer skinnedMeshRenderer4 = gameObject.AddComponent<SkinnedMeshRenderer>();
		gameObject.transform.parent = subject.transform;
		skinnedMeshRenderer4.sharedMesh = new Mesh();
		skinnedMeshRenderer4.sharedMesh.CombineMeshes(list4.ToArray());
		skinnedMeshRenderer4.shadowCastingMode = ShadowCastingMode.On;
		skinnedMeshRenderer4.receiveShadows = true;
		skinnedMeshRenderer4.bones = list2.ToArray();
		skinnedMeshRenderer4.sharedMesh.bindposes = list3.ToArray();
		skinnedMeshRenderer4.sharedMesh.boneWeights = array2;
		skinnedMeshRenderer4.sharedMaterials = list5.ToArray();
		skinnedMeshRenderer4.sharedMesh.RecalculateBounds();
		skinnedMeshRenderer4.enabled = true;
		Debug.Log("Took " + (Time.time - time) + " seconds for " + subject.name);
	}

	private static BoneWeight recalculateIndexes(BoneWeight bw, Hashtable boneHash, Transform[] meshBones)
	{
		BoneWeight result = bw;
		result.boneIndex0 = (int)boneHash[meshBones[bw.boneIndex0].name];
		result.boneIndex1 = (int)boneHash[meshBones[bw.boneIndex1].name];
		result.boneIndex2 = (int)boneHash[meshBones[bw.boneIndex2].name];
		result.boneIndex3 = (int)boneHash[meshBones[bw.boneIndex3].name];
		return result;
	}

	public static void mirror(Mesh mesh)
	{
		Vector3[] vertices = mesh.vertices;
		Vector3[] normals = mesh.normals;
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector = vertices[i];
			vertices[i] = new Vector3(0f - vector.x, vector.y, vector.z);
			Vector3 vector2 = normals[i];
			normals[i] = new Vector3(0f - vector2.x, vector2.y, vector2.z);
		}
		int[] array = new int[mesh.triangles.Length];
		int[] triangles = mesh.triangles;
		for (int j = 0; j < triangles.Length; j++)
		{
			array[triangles.Length - j - 1] = triangles[j];
		}
		mesh.vertices = vertices;
		mesh.triangles = array;
		mesh.normals = normals;
		mesh.RecalculateBounds();
		MeshTangents.calculateMeshTangents(mesh);
	}

	public static Mesh mirrorB(Mesh subjectMesh)
	{
		Vector3[] vertices = subjectMesh.vertices;
		Vector3[] normals = subjectMesh.normals;
		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 vector = vertices[i];
			vertices[i] = new Vector3(0f - vector.x, vector.y, vector.z);
			Vector3 vector2 = normals[i];
			normals[i] = new Vector3(0f - vector2.x, vector2.y, vector2.z);
		}
		int[] array = new int[subjectMesh.triangles.Length];
		int[] triangles = subjectMesh.triangles;
		for (int j = 0; j < triangles.Length; j++)
		{
			array[triangles.Length - j - 1] = triangles[j];
		}
		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = array;
		mesh.normals = normals;
		mesh.colors = subjectMesh.colors;
		mesh.uv = subjectMesh.uv;
		mesh.uv2 = subjectMesh.uv2;
		mesh.RecalculateBounds();
		MeshTangents.calculateMeshTangents(mesh);
		return mesh;
	}

	private static int[] reverse(int[] arr)
	{
		int num = arr.Length;
		int[] array = new int[num];
		for (int i = 0; i < arr.Length; i++)
		{
			array[num - i - 1] = arr[i];
		}
		return array;
	}
}
