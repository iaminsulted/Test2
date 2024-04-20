using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class SkinnedMeshCombiner
{
	private static List<Transform> bones = new List<Transform>();

	private static List<BoneWeight> boneWeights = new List<BoneWeight>();

	private static List<CombineInstance> combineInstances = new List<CombineInstance>();

	private static List<SkinnedMeshRenderer> smRenderers = new List<SkinnedMeshRenderer>();

	private static List<Matrix4x4> bindPoses = new List<Matrix4x4>();

	public static SkinnedMeshRenderer CombineMeshes(GameObject o, SkinnedMeshRenderer[] renderers)
	{
		return CombineMeshes(o, renderers.ToList());
	}

	public static SkinnedMeshRenderer CombineMeshes(GameObject o, GameObject[] gameObjects)
	{
		smRenderers.Clear();
		foreach (GameObject gameObject in gameObjects)
		{
			if ((bool)gameObject.GetComponent<SkinnedMeshRenderer>())
			{
				smRenderers.Add(gameObject.GetComponent<SkinnedMeshRenderer>());
			}
		}
		return CombineMeshes(o, smRenderers);
	}

	public static SkinnedMeshRenderer CombineMeshes(GameObject o, List<SkinnedMeshRenderer> renderers)
	{
		Material sharedMaterial = renderers[0].sharedMaterial;
		Transform transform = o.transform;
		Transform parent = o.transform.parent;
		Vector3 localPosition = transform.localPosition;
		Quaternion localRotation = transform.localRotation;
		Vector3 localScale = transform.localScale;
		transform.parent = null;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		bones.Clear();
		boneWeights.Clear();
		combineInstances.Clear();
		bindPoses.Clear();
		int num = 0;
		for (int i = 0; i < renderers.Count; i++)
		{
			SkinnedMeshRenderer skinnedMeshRenderer = renderers[i];
			if (!renderers[i].gameObject.activeSelf)
			{
				continue;
			}
			BoneWeight[] array = skinnedMeshRenderer.sharedMesh.boneWeights;
			for (int j = 0; j < array.Length; j++)
			{
				BoneWeight item = array[j];
				item.boneIndex0 += num;
				item.boneIndex1 += num;
				item.boneIndex2 += num;
				item.boneIndex3 += num;
				boneWeights.Add(item);
			}
			num += skinnedMeshRenderer.bones.Length;
			Transform[] array2 = skinnedMeshRenderer.bones;
			for (int k = 0; k < array2.Length; k++)
			{
				bones.Add(array2[k]);
				if (k < skinnedMeshRenderer.sharedMesh.bindposes.Length)
				{
					_ = ref skinnedMeshRenderer.sharedMesh.bindposes[k];
					bindPoses.Add(skinnedMeshRenderer.sharedMesh.bindposes[k] * skinnedMeshRenderer.transform.worldToLocalMatrix);
				}
				else
				{
					bindPoses.Add(skinnedMeshRenderer.transform.worldToLocalMatrix * o.transform.localToWorldMatrix);
				}
			}
			Mesh mesh = new Mesh();
			skinnedMeshRenderer.BakeMesh(mesh);
			CombineInstance combineInstance = default(CombineInstance);
			combineInstance.mesh = mesh;
			combineInstance.transform = skinnedMeshRenderer.transform.localToWorldMatrix;
			CombineInstance item2 = combineInstance;
			combineInstances.Add(item2);
			if (!skinnedMeshRenderer.name.ToLower().Contains("head"))
			{
				Vector2[] source = new Vector2[item2.mesh.vertexCount];
				item2.mesh.SetUVs(1, source.ToList());
			}
			Object.DestroyImmediate(skinnedMeshRenderer.gameObject);
		}
		GameObject gameObject = new GameObject("Model");
		gameObject.transform.parent = o.transform;
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.identity;
		SkinnedMeshRenderer skinnedMeshRenderer2 = gameObject.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer2.shadowCastingMode = ShadowCastingMode.On;
		skinnedMeshRenderer2.receiveShadows = true;
		skinnedMeshRenderer2.quality = SkinQuality.Bone4;
		skinnedMeshRenderer2.material = sharedMaterial;
		Mesh mesh2 = new Mesh();
		mesh2.CombineMeshes(combineInstances.ToArray(), mergeSubMeshes: true, useMatrices: true);
		mesh2.uv3 = null;
		mesh2.RecalculateBounds();
		skinnedMeshRenderer2.sharedMesh = mesh2;
		skinnedMeshRenderer2.bones = bones.ToArray();
		skinnedMeshRenderer2.sharedMesh.boneWeights = boneWeights.ToArray();
		skinnedMeshRenderer2.sharedMesh.bindposes = bindPoses.ToArray();
		bones.Clear();
		boneWeights.Clear();
		combineInstances.Clear();
		bindPoses.Clear();
		transform.SetParent(parent, worldPositionStays: false);
		transform.localPosition = localPosition;
		transform.localRotation = localRotation;
		transform.localScale = localScale;
		return skinnedMeshRenderer2;
	}

	public static void CombineMeshes(GameObject o)
	{
		smRenderers.Clear();
		o.GetComponentsInChildren(includeInactive: true, smRenderers);
		CombineMeshes(o, smRenderers);
	}
}
