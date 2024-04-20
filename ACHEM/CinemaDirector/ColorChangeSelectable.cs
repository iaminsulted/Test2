using System.Collections.Generic;
using CinemaDirector.Helpers;
using UnityEngine;
using UnityEngine.UI;

namespace CinemaDirector;

[CutsceneItem("uGUI", "Change Color Selectable", new CutsceneItemGenre[] { CutsceneItemGenre.ActorItem })]
public class ColorChangeSelectable : CinemaActorAction, IRevertable
{
	public enum ColorBlockChoices
	{
		normalColor,
		highlightedColor,
		pressedColor,
		disabledColor
	}

	[SerializeField]
	public ColorBlockChoices colorField;

	[SerializeField]
	private Color colorValue = Color.white;

	[SerializeField]
	private RevertMode editorRevertMode;

	[SerializeField]
	private RevertMode runtimeRevertMode;

	private Color initialColor;

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
				Selectable component = transform.GetComponent<Selectable>();
				if (component != null)
				{
					list2.Add(new RevertInfo(this, component, "colors", component.colors));
				}
			}
		}
		return list2.ToArray();
	}

	public override void Trigger(GameObject actor)
	{
		if (!(actor != null))
		{
			return;
		}
		Selectable component = actor.GetComponent<Selectable>();
		if (component != null)
		{
			switch ((int)colorField)
			{
			case 0:
				initialColor = component.colors.normalColor;
				break;
			case 1:
				initialColor = component.colors.highlightedColor;
				break;
			case 2:
				initialColor = component.colors.pressedColor;
				break;
			case 3:
				initialColor = component.colors.disabledColor;
				break;
			}
		}
	}

	public override void SetTime(GameObject actor, float time, float deltaTime)
	{
		if (actor != null && time > 0f && time <= base.Duration)
		{
			UpdateTime(actor, time, deltaTime);
		}
	}

	public override void UpdateTime(GameObject actor, float runningTime, float deltaTime)
	{
		if (!(actor != null))
		{
			return;
		}
		float t = runningTime / base.Duration;
		Selectable component = actor.GetComponent<Selectable>();
		if (component != null)
		{
			ColorBlock colors = component.colors;
			Color color = Color.Lerp(initialColor, colorValue, t);
			switch ((int)colorField)
			{
			case 0:
				colors.normalColor = color;
				break;
			case 1:
				colors.highlightedColor = color;
				break;
			case 2:
				colors.pressedColor = color;
				break;
			case 3:
				colors.disabledColor = color;
				break;
			}
			component.colors = colors;
		}
	}

	public override void End(GameObject actor)
	{
		if (!(actor != null))
		{
			return;
		}
		Selectable component = actor.GetComponent<Selectable>();
		if (component != null)
		{
			ColorBlock colors = component.colors;
			switch ((int)colorField)
			{
			case 0:
				colors.normalColor = colorValue;
				break;
			case 1:
				colors.highlightedColor = colorValue;
				break;
			case 2:
				colors.pressedColor = colorValue;
				break;
			case 3:
				colors.disabledColor = colorValue;
				break;
			}
			if (component != null)
			{
				component.colors = colors;
			}
		}
	}
}
