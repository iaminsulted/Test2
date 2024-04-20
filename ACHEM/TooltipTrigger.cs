using System;
using UnityEngine;

public class TooltipTrigger : MonoBehaviour
{
	private float tooltipDelay = 0.3f;

	private bool isPressed;

	private bool isTriggered;

	private float mTime;

	public bool Visible => isTriggered;

	public event Action<GameObject, bool> ToolTipTriggered;

	public event Action<GameObject> Clicked;

	protected void OnToolTipTriggered(bool show)
	{
		isTriggered = show;
		if (this.ToolTipTriggered != null)
		{
			this.ToolTipTriggered(base.gameObject, show);
		}
	}

	protected void OnClicked()
	{
		if (this.Clicked != null)
		{
			this.Clicked(base.gameObject);
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (isPressed && !isTriggered && RealTime.time > mTime + tooltipDelay)
		{
			OnToolTipTriggered(show: true);
		}
	}

	public void OnPress(bool isDown)
	{
		if (isDown)
		{
			isPressed = true;
			mTime = RealTime.time;
			return;
		}
		if (!isTriggered)
		{
			OnClicked();
		}
		isPressed = false;
		OnToolTipTriggered(show: false);
	}

	public void OnClick()
	{
		OnClicked();
	}

	public void OnTooltip(bool show)
	{
		OnToolTipTriggered(show);
		Tooltip.ShowAtMousePosition("This is a test tooltip! This is a test tooltip! This is a test tooltip! This is a test tooltip! This is a test tooltip! This is a test tooltip!");
	}
}
