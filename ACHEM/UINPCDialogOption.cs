using System;
using UnityEngine;

public class UINPCDialogOption : MonoBehaviour
{
	public NPCIA npcia;

	public string title;

	public UILabel buttonTitle;

	public UISprite icon;

	public UISprite GuideIcon;

	public PulseColor pulse;

	public UISprite Lock;

	public event Action<UINPCDialogOption> Clicked;

	public void OnClick()
	{
		if (this.Clicked != null)
		{
			this.Clicked(this);
		}
	}
}
