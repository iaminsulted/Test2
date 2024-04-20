using UnityEngine;

public class NGUIAnchorToParent : MonoBehaviour
{
	public UIWidget widget;

	private void Start()
	{
		UIRoot uIRoot = NGUITools.FindInParents<UIRoot>(base.gameObject);
		widget.SetAnchor(uIRoot.transform);
		Debug.Log("root transform = " + uIRoot.transform);
	}

	private void Update()
	{
	}
}
