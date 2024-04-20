using UnityEngine;

namespace CinemaDirector;

[CutsceneItem("Transform", "Attach Children", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class AttachChildrenEvent : CinemaActorEvent
{
	public GameObject[] Children;

	public override void Trigger(GameObject actor)
	{
		if (actor != null && Children != null)
		{
			for (int i = 0; i < Children.Length; i++)
			{
				Children[i].transform.parent = actor.transform;
			}
		}
	}

	public override void Reverse(GameObject actor)
	{
	}
}
