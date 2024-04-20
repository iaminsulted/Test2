using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridAnchor : UIWidgetContainer
{
	public UIGrid grid;

	public UISprite divider;

	public int GridSpacingThreshold;

	public float UpDownOffset;

	private void OnValidate()
	{
		if (grid != null)
		{
			UIGrid uIGrid = grid;
			uIGrid.onReposition = (UIGrid.OnReposition)Delegate.Remove(uIGrid.onReposition, new UIGrid.OnReposition(OnReposition));
			UIGrid uIGrid2 = grid;
			uIGrid2.onReposition = (UIGrid.OnReposition)Delegate.Combine(uIGrid2.onReposition, new UIGrid.OnReposition(OnReposition));
		}
	}

	private void Awake()
	{
		UIGrid uIGrid = grid;
		uIGrid.onReposition = (UIGrid.OnReposition)Delegate.Remove(uIGrid.onReposition, new UIGrid.OnReposition(OnReposition));
		UIGrid uIGrid2 = grid;
		uIGrid2.onReposition = (UIGrid.OnReposition)Delegate.Combine(uIGrid2.onReposition, new UIGrid.OnReposition(OnReposition));
	}

	private void OnDestroy()
	{
		UIGrid uIGrid = grid;
		uIGrid.onReposition = (UIGrid.OnReposition)Delegate.Remove(uIGrid.onReposition, new UIGrid.OnReposition(OnReposition));
	}

	private void OnReposition()
	{
		List<Transform> childList = grid.GetChildList();
		if (childList.Count == 0)
		{
			base.transform.position = grid.transform.position;
			return;
		}
		Transform transform = childList.Last();
		float y = NGUIMath.CalculateAbsoluteWidgetBounds(transform).size.y;
		if (childList.Count >= GridSpacingThreshold)
		{
			base.transform.position = transform.transform.position + Vector3.down * UpDownOffset * y;
			divider?.gameObject.SetActive(value: true);
		}
		else
		{
			base.transform.position = transform.transform.position + Vector3.down * y;
			divider?.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
	}
}
