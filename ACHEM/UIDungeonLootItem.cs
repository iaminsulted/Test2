using UnityEngine;

public class UIDungeonLootItem : MonoBehaviour
{
	public UILabel labelName;

	public UISprite sprite;

	private Item item;

	public void Init(Item item)
	{
		this.item = item;
		labelName.text = "[" + item.RarityColor + "]" + item.Name + "[-]";
		sprite.spriteName = item.Icon;
		base.gameObject.SetActive(value: true);
	}

	public void OnTooltip(bool show)
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
