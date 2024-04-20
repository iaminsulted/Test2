using System;
using UnityEngine;

public class UINewsOption : MonoBehaviour
{
	[HideInInspector]
	public NPCIA npcia;

	public UILabel label;

	public GameObject selected;

	public event Action<UINewsOption> Clicked;

	public void OnClick()
	{
		if (this.Clicked != null)
		{
			this.Clicked(this);
		}
	}
}
