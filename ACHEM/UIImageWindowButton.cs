using System;
using UnityEngine;

public class UIImageWindowButton : MonoBehaviour
{
	public int ID;

	public UILabel Label;

	public UISprite Icon;

	public UIImageWindow ParentScreen;

	public ClientTriggerAction MyAction;

	public event Action Clicked;

	private void OnClick()
	{
		if (MyAction != null)
		{
			MyAction.Execute();
		}
		if (this.Clicked != null)
		{
			this.Clicked();
		}
	}

	public void Init(ClientTriggerAction a, string label)
	{
		MyAction = a;
		if (Label != null)
		{
			Label.text = label;
		}
	}
}
