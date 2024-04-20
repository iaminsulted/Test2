using UnityEngine;

public class MobileScaler : MonoBehaviour
{
	public UIRoot root;

	public bool usingConstrained;

	public float narrowestAspectRatio;

	public float widestAspectRatio;

	public float narrowestMinHeight;

	public float widestMinHeight;

	private void Start()
	{
		float value = (float)Screen.width / (float)Screen.height;
		if (!usingConstrained)
		{
			root.minimumHeight = (int)Mathf.Clamp((int)Mathf.Lerp(narrowestMinHeight, widestMinHeight, Mathf.InverseLerp(narrowestAspectRatio, widestAspectRatio, value)), widestMinHeight, narrowestMinHeight);
		}
		else
		{
			root.manualHeight = (int)Mathf.Clamp((int)Mathf.Lerp(narrowestMinHeight, widestMinHeight, Mathf.InverseLerp(narrowestAspectRatio, widestAspectRatio, value)), widestMinHeight, narrowestMinHeight);
		}
	}
}
