using System.Linq;
using UnityEngine;

public class UIMergeListItem : MonoBehaviour
{
	public UILabel labelName;

	public UILabel labelQty;

	public UISprite sprite;

	public UISprite SpriteForeground;

	public int onHandQty;

	public int goalQty = 1;

	private MergeItem item;

	public bool CheckQuantity => onHandQty >= goalQty;

	public void Init(UIMergeDetail detail, MergeItem item)
	{
		this.item = item;
		labelName.text = "[" + item.RarityColor + "]" + item.Name + "[-]";
		sprite.spriteName = item.Icon;
		if (SpriteForeground != null)
		{
			SpriteForeground.gameObject.SetActive(!string.IsNullOrEmpty(item.IconFg));
			SpriteForeground.spriteName = item.IconFg;
		}
		base.gameObject.SetActive(value: true);
		onHandQty = 0;
		goalQty = item.Qty;
		InventoryItem[] array = Session.MyPlayerData.items.Where((InventoryItem x) => x.ID == item.ItemID).ToArray();
		if (array != null && array.Length != 0)
		{
			onHandQty = array.Sum((InventoryItem x) => x.Qty);
			if (onHandQty >= goalQty)
			{
				labelQty.color = InterfaceColors.Stats.Green;
			}
			else
			{
				labelQty.color = InterfaceColors.Stats.Red;
			}
		}
		labelQty.text = onHandQty + " / " + goalQty;
	}

	public void OnTooltip(bool show)
	{
		if (item != null)
		{
			if (show)
			{
				Tooltip.ShowAtMousePosition(item.GetToolTip(showStats: false), UIWidget.Pivot.BottomRight);
			}
			else
			{
				Tooltip.Hide();
			}
		}
	}
}
