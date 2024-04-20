using UnityEngine;

public class InventoryScrollSurface : MonoBehaviour
{
	public UIScrollView draggablePanel;

	private void Start()
	{
		if (draggablePanel == null)
		{
			draggablePanel = NGUITools.FindInParents<UIScrollView>(base.gameObject);
		}
	}

	private void Update()
	{
	}

	private void OnScroll(float delta)
	{
		if (base.enabled && NGUITools.GetActive(base.gameObject) && draggablePanel != null)
		{
			draggablePanel.Scroll(delta);
		}
	}
}
