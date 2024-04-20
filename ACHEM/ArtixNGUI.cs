using UnityEngine;

public static class ArtixNGUI
{
	public static int CurrentIndex(this UIPopupList popupList)
	{
		return popupList.items.IndexOf(popupList.value);
	}

	public static void SetToIndex(this UIPopupList popupList, int index)
	{
		if (index >= 0 && index < popupList.items.Count)
		{
			popupList.Set(popupList.items[index]);
		}
	}

	public static float GetValue(this UISlider slider, float minValue, float maxValue)
	{
		return Mathf.Lerp(minValue, maxValue, slider.value);
	}

	public static float GetValue(this UISlider slider, Vector2 range)
	{
		return slider.GetValue(range.x, range.y);
	}

	public static void SetValue(this UISlider slider, float value, float minValue, float maxValue)
	{
		slider.value = (value - minValue) / (maxValue - minValue);
	}

	public static void SetValue(this UISlider slider, float value, Vector2 range)
	{
		slider.SetValue(value, range.x, range.y);
	}
}
