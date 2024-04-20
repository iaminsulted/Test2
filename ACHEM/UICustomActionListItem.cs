using System;
using UnityEngine;

public class UICustomActionListItem : MonoBehaviour
{
	public UISprite Icon;

	public UILabel lblName;

	public UILabel lblInfo;

	public IUIItem item;

	private Transform toolTipPosition;

	public event Action<UICustomActionListItem> Clicked;

	public void SetSpellInfo(SpellTemplate spellT, Transform toolTipPosition)
	{
		this.toolTipPosition = toolTipPosition;
		item = spellT;
		lblName.text = "[000000]" + spellT.name + "[-]";
		Icon.spriteName = spellT.icon;
		lblInfo.text = (spellT.isPvpAction ? "[333333]PvP Action[-]" : ("[333333]" + Session.MyPlayerData.GetClassBySpellID(spellT.ID).Name + "[-]"));
	}

	public void SetItemInfo(InventoryItem item, Transform ToolTipPosition)
	{
		toolTipPosition = ToolTipPosition;
		this.item = item;
		lblName.text = "[" + item.RarityColor + "]" + item.Name + "[-]";
		lblInfo.text = item.GetTagline();
		Icon.spriteName = item.Icon;
		if (item.MaxStack > 1 && item.Qty > 1)
		{
			UILabel uILabel = lblName;
			uILabel.text = uILabel.text + " [000000]x" + item.Qty + "[-]";
		}
	}

	protected void OnClicked()
	{
		this.Clicked?.Invoke(this);
	}

	protected void OnClick()
	{
		AudioManager.Play2DSFX("sfx_engine_btnpress");
		OnClicked();
	}

	public void OnTooltip(bool show)
	{
		if (item != null)
		{
			if (show)
			{
				Tooltip.ShowAtPosition(item.ToolTipText, UIGame.Instance.MouseToolTipPosition, UIWidget.Pivot.TopRight);
			}
			else
			{
				Tooltip.Hide();
			}
		}
	}
}
