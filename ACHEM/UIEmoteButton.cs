using System;
using UnityEngine;

public class UIEmoteButton : MonoBehaviour
{
	public Emote Emote;

	public UILabel Text;

	public event Action<Emote> Clicked;

	public void Load(Emote emote)
	{
		Text.text = emote.name;
		Emote = emote;
	}

	public void OnClick()
	{
		if (this.Clicked != null)
		{
			this.Clicked(Emote);
		}
	}

	public void OnTooltip(bool show)
	{
		if (show)
		{
			Tooltip.ShowAtPosition(string.Concat("[892800]" + Emote.name + "[-]", "\n[000000]Command: /", Emote.cmd, "[-]"), UIGame.Instance.MouseToolTipPosition, UIWidget.Pivot.TopRight);
		}
		else
		{
			Tooltip.Hide();
		}
	}
}
