using System;
using System.Linq;
using UnityEngine;

public class UIBankInventory : UIInventory
{
	public UIButton DeleteButton;

	private bool isSetup;

	public InventoryItem selectedItem => selectedItems.FirstOrDefault();

	public void Load(int bankID)
	{
		base.CurrentBank = bankID;
		if (!isSetup)
		{
			isSetup = true;
			Setup();
		}
		else
		{
			Refresh();
		}
	}

	public void AllBanksLoaded()
	{
		Refresh();
	}

	protected override void SetSelectedItem(UIInventoryItem selectedItem)
	{
		base.SetSelectedItem(selectedItem);
		DeleteButton.normalSprite = (selectedItem.Item.IsLootBoxItem ? "Shard-Recycle" : "Icon_Trash");
	}

	public void SetSelectedInventoryItem(InventoryItem selectedItem)
	{
		selectedItems.Clear();
		selectedItems.Add(selectedItem);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		UIEventListener uIEventListener = UIEventListener.Get(DeleteButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnBtnDeleteClick));
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		UIEventListener uIEventListener = UIEventListener.Get(DeleteButton.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnBtnDeleteClick));
	}

	private void OnBtnDeleteClick(GameObject go)
	{
		if (selectedItem != null)
		{
			if (selectedItem.IsEquipped)
			{
				Notification.ShowText("Item is currently equipped.");
			}
			else if (!selectedItem.IsRemovable)
			{
				Notification.ShowText("Item cannot be deleted.");
			}
			else if (selectedItem.IsLootBoxItem)
			{
				Confirmation.Show("Confirm", "Convert [" + selectedItem.RarityColor + "]'" + selectedItem.Name + "'[-] into " + Item.GetLootBoxItemTokenSellPrice(selectedItem.Rarity) + " Treasure Shards?", ConfirmationDust);
			}
			else if (selectedItem.Qty > 1)
			{
				Confirmation.Show("Confirm", "Delete [" + selectedItem.RarityColor + "]'" + selectedItem.Name + " x" + selectedItem.Qty + "'[-]?", ConfirmationCallback);
			}
			else
			{
				Confirmation.Show("Confirm", "Delete [" + selectedItem.RarityColor + "]'" + selectedItem.Name + "'[-]?", ConfirmationCallback);
			}
		}
	}

	private void ConfirmationCallback(bool confirm)
	{
		if (confirm)
		{
			Game.Instance.SendItemRemoveRequest(selectedItem.CharItemID);
		}
	}

	private void ConfirmationDust(bool confirm)
	{
		if (confirm)
		{
			Game.Instance.SendItemDustRequest(selectedItem.CharItemID);
		}
	}

	public void ToggleGlobalSearch()
	{
		base.CurrentBank = -1;
		Refresh();
	}
}
