using UnityEngine;

public class WindowResizeListener : MonoBehaviour
{
	private int lastWidth;

	private int lastHeight;

	public void Start()
	{
		lastWidth = Screen.width;
		lastHeight = Screen.height;
	}

	public void Update()
	{
		float num = (float)SettingsManager.CurrentResolution.width / (float)SettingsManager.CurrentResolution.height;
		int num2 = Screen.width;
		int num3 = Screen.height;
		if (lastWidth != num2)
		{
			num3 = (int)Mathf.Round((float)num2 / num);
			Screen.SetResolution(num2, num3, fullscreen: false);
		}
		else if (lastHeight != num3)
		{
			num2 = (int)Mathf.Round((float)num3 * num);
			Screen.SetResolution(num2, num3, fullscreen: false);
		}
		lastWidth = num2;
		lastHeight = num3;
	}
}
