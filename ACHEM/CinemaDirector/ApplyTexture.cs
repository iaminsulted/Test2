using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Renderer", "Apply Texture", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class ApplyTexture : CinemaActorEvent, IRevertable
{
	public Texture texture;

	private Texture initialTexture;

	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

	public RevertMode EditorRevertMode
	{
		get
		{
			return editorRevertMode;
		}
		set
		{
			editorRevertMode = value;
		}
	}

	public RevertMode RuntimeRevertMode
	{
		get
		{
			return runtimeRevertMode;
		}
		set
		{
			runtimeRevertMode = value;
		}
	}

	public override void Trigger(GameObject actor)
	{
		Renderer component = actor.GetComponent<Renderer>();
		if (component != null && texture != null)
		{
			initialTexture = component.sharedMaterial.mainTexture;
			component.sharedMaterial.mainTexture = texture;
		}
	}

	public override void Reverse(GameObject actor)
	{
		Renderer component = actor.GetComponent<Renderer>();
		if (component != null && texture != null)
		{
			component.sharedMaterial.mainTexture = initialTexture;
		}
	}

	public RevertInfo[] CacheState()
	{
		List<Transform> list = new List<Transform>(GetActors());
		List<RevertInfo> list2 = new List<RevertInfo>();
		for (int i = 0; i < list.Count; i++)
		{
			Renderer component = list[i].GetComponent<Renderer>();
			if (component != null)
			{
				list2.Add(new RevertInfo(this, component.sharedMaterial, "mainTexture", component.sharedMaterial.mainTexture));
			}
		}
		return list2.ToArray();
	}
}
