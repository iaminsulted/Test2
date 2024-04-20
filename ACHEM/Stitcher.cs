using UnityEngine;

public class Stitcher
{
	public GameObject Stitch(GameObject sourceClothing, GameObject targetAvatar)
	{
		TransformCatelog transformCatelog = new TransformCatelog(targetAvatar.transform);
		SkinnedMeshRenderer[] componentsInChildren = sourceClothing.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		GameObject gameObject = AddChild(sourceClothing, targetAvatar.transform);
		SkinnedMeshRenderer[] array = componentsInChildren;
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in array)
		{
			AddSkinnedMeshRenderer(skinnedMeshRenderer, gameObject).bones = TranslateTransforms(skinnedMeshRenderer.bones, transformCatelog);
		}
		return gameObject;
	}

	private GameObject AddChild(GameObject source, Transform parent)
	{
		GameObject gameObject = new GameObject(source.name);
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = source.transform.localPosition;
		gameObject.transform.localRotation = source.transform.localRotation;
		gameObject.transform.localScale = source.transform.localScale;
		gameObject.transform.localRotation = Quaternion.Euler(-90f, 0f, 0f);
		gameObject.layer = source.layer;
		return gameObject;
	}

	private SkinnedMeshRenderer AddSkinnedMeshRenderer(SkinnedMeshRenderer source, GameObject parent)
	{
		SkinnedMeshRenderer skinnedMeshRenderer = parent.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer.sharedMesh = source.sharedMesh;
		skinnedMeshRenderer.materials = source.sharedMaterials;
		return skinnedMeshRenderer;
	}

	private Transform[] TranslateTransforms(Transform[] sources, TransformCatelog transformCatelog)
	{
		Transform[] array = new Transform[sources.Length];
		for (int i = 0; i < sources.Length; i++)
		{
			array[i] = transformCatelog.Find(sources[i].name);
		}
		return array;
	}
}
