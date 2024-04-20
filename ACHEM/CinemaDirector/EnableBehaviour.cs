using System.Collections.Generic;
using CinemaDirector.Helpers;
using CinemaSuite.Common;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Game Object", "Enable Behaviour", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class EnableBehaviour : CinemaActorEvent, IRevertable
{
	public Component Behaviour;

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

	public RevertInfo[] CacheState()
	{
		List<Transform> list = new List<Transform>(GetActors());
		List<RevertInfo> list2 = new List<RevertInfo>();
		for (int i = 0; i < list.Count; i++)
		{
			Transform transform = list[i];
			if (transform != null)
			{
				Component component = transform.GetComponent(Behaviour.GetType());
				if (component != null)
				{
					bool flag = (bool)ReflectionHelper.GetProperty(Behaviour.GetType(), "enabled").GetValue(component, null);
					list2.Add(new RevertInfo(this, Behaviour, "enabled", flag));
				}
			}
		}
		return list2.ToArray();
	}

	public override void Trigger(GameObject actor)
	{
		if (actor.GetComponent(Behaviour.GetType()) != null)
		{
			ReflectionHelper.GetProperty(Behaviour.GetType(), "enabled").SetValue(Behaviour, true, null);
		}
	}

	public override void Reverse(GameObject actor)
	{
		Component component = actor.GetComponent(Behaviour.GetType());
		if (component != null)
		{
			ReflectionHelper.GetProperty(Behaviour.GetType(), "enabled").SetValue(component, false, null);
		}
	}
}
