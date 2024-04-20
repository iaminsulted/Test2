using System;
using UnityEngine;

public class UIItemDetails : MonoBehaviour
{
	private enum Mode
	{
		Equip,
		Unequip
	}

	public UIButton btnClose;

	public UIButton btnPreview;

	public UILabel lblName;

	public UILabel lblLevel;

	public UILabel lblStats;

	public UILabel lblDesc;

	public UISprite Icon;

	public UISprite SprGuardian;

	public UITable table;

	private Item item;

	private Mode mode;

	private bool visible = true;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				base.gameObject.SetActive(visible);
			}
		}
	}

	public void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
	}

	public void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
	}

	public void Show()
	{
		base.gameObject.SetActive(value: true);
	}

	public void LoadInventoryItem(Item item)
	{
		Visible = true;
		this.item = item;
		lblName.text = "[" + item.RarityColor + "]" + item.Name + "[-]";
		Icon.spriteName = item.Icon;
		SprGuardian.gameObject.SetActive(item.IsGuardian);
		lblLevel.text = item.GetTagline(showPower: false);
		lblStats.gameObject.SetActive(item.HasStats);
		lblStats.text = Session.MyPlayerData.GetComparisonStatText(item);
		lblDesc.text = item.GetDescription();
		btnPreview.isEnabled = item.HasPreview;
		table.Reposition();
	}

	public virtual void OnCloseClick(GameObject go)
	{
		Close();
	}

	public void Close()
	{
		if (UIPreview.instance != null)
		{
			UIPreview.instance.Close();
		}
		Visible = false;
	}

	public void OnPreviewClick()
	{
		UIPreview.Show(item);
	}

	public Item GetItem()
	{
		return item;
	}

	private void OnItemUpdate(InventoryItem iItem)
	{
		if (item == iItem)
		{
			LoadInventoryItem(iItem);
		}
	}
}
