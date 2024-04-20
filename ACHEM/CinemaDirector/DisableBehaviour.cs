using CinemaSuite.Common;
using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Game Object", "Disable Behaviour", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class DisableBehaviour : CinemaActorEvent
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

	public override void Trigger(GameObject actor)
	{
		if (actor.GetComponent(Behaviour.GetType()) != null)
		{
			ReflectionHelper.GetProperty(Behaviour.GetType(), "enabled").SetValue(Behaviour, false, null);
		}
	}

	public override void Reverse(GameObject actor)
	{
		Component component = actor.GetComponent(Behaviour.GetType());
		if (component != null)
		{
			ReflectionHelper.GetProperty(Behaviour.GetType(), "enabled").SetValue(component, true, null);
		}
	}
}
