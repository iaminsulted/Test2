using UnityEngine;

public class UIScaleAdjustDPI : MonoBehaviour
{
	public UIRoot root;

	public float ScaleFactor = 1f;

	private void Awake()
	{
	}

	private void ApplyScale()
	{
		base.transform.localScale = Vector3.one * ScaleFactor;
		int num = (int)(700f * ScaleFactor);
		if ((num & 1) == 1)
		{
			num++;
		}
		root.minimumHeight = num;
		root.UpdateScale();
	}
}
