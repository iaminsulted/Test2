using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TextureCompositor : MonoBehaviour
{
	public enum Region
	{
		Helm,
		Hair,
		BackHair,
		Weapon,
		Back,
		Full,
		Shield,
		Armor,
		FrontArm,
		FrontForeArm,
		FrontThigh,
		FrontCalf,
		FrontFoot,
		Belt,
		Torso,
		BackArm,
		BackForeArm,
		BackThigh,
		BackCalf,
		BackFoot
	}

	public static TextureCompositor Instance;

	public static Queue<CompositorRequest> requests = new Queue<CompositorRequest>();

	public static void Init(Transform parent)
	{
		GameObject obj = Object.Instantiate(Resources.Load<GameObject>("CompositorBlit/BlitCompositor"));
		obj.name = "Compositor";
		obj.transform.SetParent(parent, worldPositionStays: false);
		Instance = obj.GetComponent<TextureCompositor>();
	}

	public static void Composite(CompositorRequest cr)
	{
		Debug.Log("Add Composite request");
		requests.Enqueue(cr);
	}

	public abstract IEnumerator FinalizeJob(CompositorRequest cr);

	private void Start()
	{
	}

	private void Update()
	{
		while (requests.Count > 0)
		{
			StartCoroutine("FinalizeJob", requests.Dequeue());
		}
	}
}
