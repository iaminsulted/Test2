using UnityEngine;

public class SafeArea : MonoBehaviour
{
	public UIRoot root;

	public UIWidget widget;

	private float mPixelSizeAdjustment;

	private void Start()
	{
		root = NGUITools.FindInParents<UIRoot>(base.gameObject);
		widget = GetComponent<UIWidget>();
		UpdateAnchors();
	}

	[ContextMenu("Execute")]
	public void UpdateAnchors()
	{
		float num = 2f / (base.transform.lossyScale.x * (float)Screen.height);
		mPixelSizeAdjustment = num;
		Rect safeArea = Screen.safeArea;
		widget.transform.localPosition = new Vector3(safeArea.x + safeArea.width / 2f - (float)(Screen.width / 2), safeArea.y + safeArea.height / 2f - (float)(Screen.height / 2), 0f) * mPixelSizeAdjustment;
		widget.height = Mathf.CeilToInt(safeArea.height * mPixelSizeAdjustment);
		widget.width = Mathf.CeilToInt((float)widget.height * safeArea.width / safeArea.height);
	}
}
