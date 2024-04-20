using UnityEngine;

public class UIItemTooltip : MonoBehaviour
{
	public Item item;

	public void SetItem(Item item)
	{
		this.item = item;
	}

	public void OnTooltip(bool show)
	{
		if (item != null)
		{
			if (show)
			{
				Tooltip.ShowAtMousePosition(item.GetToolTip(), UIWidget.Pivot.BottomRight);
			}
			else
			{
				Tooltip.Hide();
			}
		}
	}
}
