using System;

public class UIFullscreenWindow : UIWindow
{
	public static event Action<bool> OnFullscreen;

	protected virtual void OnEnable()
	{
		if (UIFullscreenWindow.OnFullscreen != null)
		{
			UIFullscreenWindow.OnFullscreen(obj: true);
		}
	}

	protected virtual void OnDisable()
	{
		if (UIFullscreenWindow.OnFullscreen != null)
		{
			UIFullscreenWindow.OnFullscreen(obj: false);
		}
	}
}
