using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIWindow : MonoBehaviour
{
	protected static List<UIWindow> windows = new List<UIWindow>();

	public UIRect ScaledWidget;

	private bool visible = true;

	public static int Count => windows.Count;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				base.gameObject.SetActive(visible);
			}
		}
	}

	public static UIWindow CurrentWindow
	{
		get
		{
			if (windows.Count > 0)
			{
				return windows[windows.Count - 1];
			}
			return null;
		}
	}

	public static event Action OnClearWindows;

	protected virtual void Init()
	{
		ModalWindow.Clear();
		windows.Add(this);
		ChangeSize();
	}

	public void ChangeSize()
	{
		if (ScaledWidget != null)
		{
			float num = SettingsManager.MenuSize;
			ScaledWidget.transform.localScale = new Vector3(num, num, num);
		}
	}

	public virtual void Close()
	{
		Destroy();
	}

	protected virtual void Destroy()
	{
		windows.Remove(this);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public static void ClearWindows()
	{
		for (int num = windows.Count - 1; num >= 0; num--)
		{
			windows[num].Destroy();
		}
		if (UIWindow.OnClearWindows != null)
		{
			UIWindow.OnClearWindows();
		}
	}

	protected virtual void Resume()
	{
		Visible = true;
	}

	protected static void ShowLastWindow()
	{
		if (windows.Count > 0)
		{
			windows[windows.Count - 1].Resume();
		}
	}

	protected static void HideCurrentWindow()
	{
		if (windows.Count > 0)
		{
			UIWindow uIWindow = windows[windows.Count - 1];
			if (uIWindow is UIStackingWindow)
			{
				uIWindow.Visible = false;
			}
			else
			{
				uIWindow.Close();
			}
		}
	}
}
