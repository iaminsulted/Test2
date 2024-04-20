using System;
using UnityEngine;

[RequireComponent(typeof(UIButton))]
public class UIJumpButton : MonoBehaviour
{
	public ActionBar actionBar;

	public Action Pressed;

	public void OnPress(bool pressed)
	{
		if (pressed)
		{
			Pressed?.Invoke();
		}
	}
}
