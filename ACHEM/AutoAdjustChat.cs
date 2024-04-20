using System;
using UnityEngine;

public class AutoAdjustChat : MonoBehaviour
{
	public UIWidget ChatWindowSpace;

	public UIWidget UIChatWindow;

	private void Start()
	{
		Resize();
	}

	private void OnEnable()
	{
		UIWidget chatWindowSpace = ChatWindowSpace;
		chatWindowSpace.onChange = (UIWidget.OnDimensionsChanged)Delegate.Combine(chatWindowSpace.onChange, new UIWidget.OnDimensionsChanged(OnChatSpaceChanged));
	}

	private void OnChatSpaceChanged()
	{
		Resize();
	}

	public void OnDisable()
	{
		UIWidget chatWindowSpace = ChatWindowSpace;
		chatWindowSpace.onChange = (UIWidget.OnDimensionsChanged)Delegate.Remove(chatWindowSpace.onChange, new UIWidget.OnDimensionsChanged(OnChatSpaceChanged));
	}

	public void Resize()
	{
		base.transform.position = ChatWindowSpace.transform.position;
		if (ChatWindowSpace.width < UIChatWindow.width)
		{
			base.transform.localPosition = new Vector2(base.transform.localPosition.x, base.transform.localPosition.y + 100f);
		}
	}
}
