using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.NetworkClient.CommClasses;
using StatCurves;
using UnityEngine;

public class UIShopItemDetail : MonoBehaviour
{
	private enum Mode
	{
		Buy,
		Sell
	}

	public UIButton btnClose;

	public UIButton btnAction;

	public UIButton btnPreview;

	public UILabel lblName;

	public UILabel lblAction;

	public UILabel lblPrice;

	public UILabel lblLevel;

	public UILabel lblStats;

	public UILabel lblDesc;

	public UILabel lblRequirement;

	public UILabel lblBalance;

	public UILabel lblPowerNumeric;

	public UILabel lblPowerText;

	public UILabel lblInfusionCap;

	public UISprite Icon;

	public UISprite IconCurrency;

	public UISprite IconCurrencyFg;

	public UISprite SprGuardian;

	public UISprite SprBalanceIcon;

	public UISprite SprBalanceIconFg;

	public UITable table;

	public UIItemTooltip CostItemTooltip;

	public GameObject goBalance;

	public GameObject PowerParent;

	public GameObject InfusionParent;

	private Shop shop;

	private ShopType ShopType;

	public Item item;

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

	public void LoadBuyItem(ShopItem item, Shop shop, ShopType shopType, int tokenID)
	{
		Visible = true;
		this.shop = shop;
		ShopType = shopType;
		this.item = item;
		mode = Mode.Buy;
		if (!item.IsAvailable() && !string.IsNullOrEmpty(item.GetLockInfo()))
		{
			lblRequirement.text = item.GetLockInfo();
			lblRequirement.gameObject.SetActive(value: true);
		}
		else
		{
			lblRequirement.gameObject.SetActive(value: false);
		}
		lblName.text = "[" + item.RarityColor + "]" + item.Name + "[-]";
		lblAction.text = mode.ToString();
		Icon.spriteName = item.Icon;
		SprGuardian.gameObject.SetActive(item.IsGuardian);
		lblLevel.text = item.GetTagline(showPower: false);
		lblStats.gameObject.SetActive(item.HasStats);
		lblStats.text = Session.MyPlayerData.GetComparisonStatText(item);
		if (item.HasStats)
		{
			InfusionParent.SetActive(value: true);
			lblInfusionCap.text = "0/" + item.DisplayTimesInfusable;
		}
		else
		{
			InfusionParent.SetActive(value: false);
		}
		lblDesc.text = item.GetDescription();
		if (item.GetCombatPower() > 0)
		{
			PowerParent.SetActive(value: true);
			lblPowerNumeric.text = item.GetCombatPower().ToString();
			lblPowerText.ResetAndUpdateAnchors();
		}
		else if (item.GetTradeSkillPower() > 0)
		{
			PowerParent.SetActive(value: true);
			lblPowerNumeric.text = item.GetTradeSkillPower().ToString();
			lblPowerText.ResetAndUpdateAnchors();
		}
		else
		{
			PowerParent.SetActive(value: false);
		}
		if (item.TokenID > 0)
		{
			Item item2 = Items.Get(item.TokenID);
			UISprite sprBalanceIcon = SprBalanceIcon;
			string spriteName = (IconCurrency.spriteName = ((item2 == null) ? "" : item2.Icon));
			sprBalanceIcon.spriteName = spriteName;
			if (IconCurrencyFg != null)
			{
				IconCurrencyFg.gameObject.SetActive(!string.IsNullOrEmpty(item2.IconFg));
				IconCurrencyFg.spriteName = item2.IconFg;
			}
			if (SprBalanceIconFg != null)
			{
				SprBalanceIconFg.gameObject.SetActive(!string.IsNullOrEmpty(item2.IconFg));
				SprBalanceIconFg.spriteName = item2.IconFg;
			}
			CostItemTooltip.SetItem(item2);
			lblPrice.text = (item.IsLootBoxItem ? item.TokenQty.ToString() : (item.TokenQty + " " + Items.Get(item.TokenID).Name));
			lblAction.text = ((item.TokenQty <= 0) ? "Get" : "Buy");
			lblBalance.text = Session.MyPlayerData.items.Where((InventoryItem x) => x.ID == item.TokenID).Sum((InventoryItem x) => x.Qty).ToString();
		}
		else
		{
			IconCurrency.spriteName = (item.IsMC ? "DragonGem" : "Coin");
			IconCurrencyFg.gameObject.SetActive(value: false);
			CostItemTooltip.SetItem(null);
			lblPrice.text = item.Cost.ToString();
			lblAction.text = ((item.Cost <= 0) ? "Get" : "Buy");
		}
		goBalance.SetActive(item.TokenID != tokenID);
		IconCurrency.UpdateAnchors();
		btnPreview.isEnabled = item.HasPreview;
		table.Reposition();
	}

	public void LoadSellItem(InventoryItem item)
	{
		Visible = true;
		this.item = item;
		mode = Mode.Sell;
		lblRequirement.gameObject.SetActive(value: false);
		lblName.text = "[" + item.RarityColor + "]" + item.Name + "[-]";
		lblAction.text = mode.ToString();
		Icon.spriteName = item.Icon;
		SprGuardian.gameObject.SetActive(item.IsGuardian);
		lblPrice.text = item.SellPrice.ToString();
		lblLevel.text = item.GetTagline(showPower: false);
		lblStats.gameObject.SetActive(item.HasStats);
		lblStats.text = Session.MyPlayerData.GetComparisonStatText(item);
		lblDesc.text = item.GetDescription();
		if (item.GetCombatPower() > 0)
		{
			PowerParent.SetActive(value: true);
			lblPowerNumeric.text = item.GetCombatPower().ToString();
			lblPowerText.ResetAndUpdateAnchors();
		}
		else if (item.GetTradeSkillPower() > 0)
		{
			PowerParent.SetActive(value: true);
			lblPowerNumeric.text = item.GetTradeSkillPower().ToString();
			lblPowerText.ResetAndUpdateAnchors();
		}
		else
		{
			PowerParent.SetActive(value: false);
		}
		IconCurrency.spriteName = "Coin";
		IconCurrencyFg.gameObject.SetActive(value: false);
		CostItemTooltip.SetItem(null);
		goBalance.SetActive(value: false);
		IconCurrency.UpdateAnchors();
		btnPreview.isEnabled = item.HasPreview;
		table.Reposition();
	}

	public void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnAction.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBtnActionClick));
		Session.MyPlayerData.ItemAdded += OnItemUpdated;
		Session.MyPlayerData.ItemRemoved += OnItemUpdated;
		Session.MyPlayerData.ItemUpdated += OnItemUpdated;
	}

	public void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnAction.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBtnActionClick));
		Session.MyPlayerData.ItemAdded -= OnItemUpdated;
		Session.MyPlayerData.ItemRemoved -= OnItemUpdated;
		Session.MyPlayerData.ItemUpdated -= OnItemUpdated;
	}

	private void OnItemUpdated(InventoryItem invItem)
	{
		if (mode != 0)
		{
			return;
		}
		ShopItem shopItem = item as ShopItem;
		if (shopItem.TokenID == invItem.ID)
		{
			Items.Get(shopItem.TokenID);
			lblBalance.text = Session.MyPlayerData.items.Where((InventoryItem x) => x.ID == shopItem.TokenID).Sum((InventoryItem x) => x.Qty).ToString();
		}
	}

	public void Close()
	{
		Visible = false;
	}

	public virtual void OnCloseClick(GameObject go)
	{
		Close();
	}

	private void OnBtnActionClick(GameObject go)
	{
		if (mode == Mode.Buy)
		{
			ShopItem shopItem = item as ShopItem;
			if (CheckForBuyError(shopItem) && shopItem != null)
			{
				if (shopItem.MaxStack > 1)
				{
					ConfirmationSlider.ConfirmBuy(shopItem, ConfirmationSlider.ActionType.Buy, ConfirmationCallbackStackBuy);
				}
				else if (!Session.MyPlayerData.HasClassUsingWeaponSlot(shopItem.EquipSlot))
				{
					ConfirmationSpend.ConfirmBuy(shopItem, ConfirmationCallback, "[930000]NOTE: You have not unlocked any classes that can equip this weapon as a cosmetic.[-]");
				}
				else if (shopItem.CurrencyCost > 0)
				{
					ConfirmationSpend.ConfirmBuy(shopItem, ConfirmationCallback);
				}
				else
				{
					Game.Instance.SendBuyRequest(shop.ID, shopItem.ID, shopItem.Qty, ShopType);
				}
			}
		}
		else if (mode == Mode.Sell)
		{
			if (((InventoryItem)item).IsEquipped)
			{
				Notification.ShowText("Item is currently equipped.");
			}
			else if (!item.IsRemovable)
			{
				Notification.ShowText("Item cannot be sold.");
			}
			else if (item.IsLootBoxItem)
			{
				Confirmation.Show("Confirm", "Item cannot be sold. Would you like to convert [" + item.RarityColor + "]'" + item.Name + "'[-] into " + Item.GetLootBoxItemTokenSellPrice(item.Rarity) + " Treasure Shards instead?", ConfirmationDust);
			}
			else if (item.Qty > 1)
			{
				ConfirmationSlider.ConfirmSell((InventoryItem)item, ConfirmationSlider.ActionType.Sell, ConfirmationCallbackStackSell);
			}
			else
			{
				ConfirmationSpend.Show("Confirm", "Sell '" + item.Name + "' for " + ((InventoryItem)item).SellPrice + " Gold?", "Gold", ((InventoryItem)item).SellPrice, "Sell", ConfirmationCallback);
			}
		}
	}

	private bool CheckForBuyError(ShopItem shopItem)
	{
		string text = shopItem.GetLockInfo();
		if (string.IsNullOrEmpty(text))
		{
			text = "You do not meet this item's requirements";
		}
		if (!Session.MyPlayerData.CheckBitFlag(shopItem.BitFlagName, shopItem.BitFlagIndex) && Products.ProductPackages.Values.Any((ProductDetail p) => p.BitFlagName == shopItem.BitFlagName && p.BitFlagIndex == shopItem.BitFlagIndex))
		{
			Confirmation.Show("Item Locked", text, ConfirmationCallbackBuyMC);
			return false;
		}
		if (!shopItem.IsAvailable())
		{
			MessageBox.Show("Item Locked", text);
			return false;
		}
		if (shopItem.IsGuardian && !Session.MyPlayerData.IsGuardian())
		{
			Confirmation.Show("Guardian Only", "You need to be a Guardian to use this item, would you like to become a Guardian?", delegate(bool b)
			{
				if (b)
				{
					UIIAPStore.Show();
				}
			});
			return false;
		}
		if (!Session.MyPlayerData.HasRoomInInventory(shopItem))
		{
			Notification.ShowText("Inventory is full");
			return false;
		}
		if (shopItem.IsUnique && Session.MyPlayerData.allItems.Values.Any((List<InventoryItem> x) => x.Any((InventoryItem y) => y.ID == shopItem.ID && y.BankID > 0)))
		{
			Notification.ShowText("Item already in your bank. Cannot have multiple of this item.");
			return false;
		}
		if (shopItem.IsUnique && Session.MyPlayerData.HasMaxOfUniqueItem(shopItem))
		{
			if (shopItem.MaxStack == 1)
			{
				Notification.ShowText("Cannot have multiple of this item");
			}
			else
			{
				Notification.ShowText("Cannot have multiple stacks of this item");
			}
			return false;
		}
		if (shopItem.TokenID > 0 && Session.MyPlayerData.GetItemCount(shopItem.TokenID) < shopItem.TokenQty)
		{
			if (shop.CollectionBadgeID == 0 || !Session.MyPlayerData.HasBadgeID(shop.CollectionBadgeID))
			{
				Notification.ShowText("Insufficient funds");
				return false;
			}
		}
		else if (shopItem.TokenID <= 0 && !shopItem.IsMC && Session.MyPlayerData.Gold < shopItem.Cost * shopItem.Qty)
		{
			if (shop.CollectionBadgeID == 0 || !Session.MyPlayerData.HasBadgeID(shop.CollectionBadgeID))
			{
				Notification.ShowText("Insufficient gold");
				return false;
			}
		}
		else if (shopItem.TokenID <= 0 && shopItem.IsMC && Session.MyPlayerData.MC < shopItem.Cost * shopItem.Qty && (shop.CollectionBadgeID == 0 || !Session.MyPlayerData.HasBadgeID(shop.CollectionBadgeID)))
		{
			Confirmation.Show("Insufficient Funds", "You do not have enough Dragon Crystals to purchase the item. Would you like to buy Dragon Crystals?", ConfirmationCallbackBuyMC);
			return false;
		}
		return true;
	}

	private void ConfirmationCallback(bool confirm)
	{
		if (confirm)
		{
			if (mode == Mode.Buy)
			{
				Game.Instance.SendBuyRequest(shop.ID, item.ID, item.Qty, ShopType);
			}
			else if (mode == Mode.Sell)
			{
				Game.Instance.SendSellRequest(((InventoryItem)item).CharItemID);
				Close();
			}
		}
	}

	private void ConfirmationDust(bool confirm)
	{
		if (confirm)
		{
			Game.Instance.SendItemDustRequest(((InventoryItem)item).CharItemID);
			Close();
		}
	}

	private void ConfirmationCallbackMulti(bool confirm)
	{
		if (confirm)
		{
			Game.Instance.SendSellRequest(((InventoryItem)item).CharItemID);
			Close();
		}
		else
		{
			Game.Instance.SendSellRequest(((InventoryItem)item).CharItemID, item.Qty);
		}
	}

	private void ConfirmationCallbackStackSell(bool confirm, int qty)
	{
		if (confirm && qty > 0)
		{
			Game.Instance.SendSellRequest(((InventoryItem)item).CharItemID, qty);
			Close();
		}
	}

	private void ConfirmationCallbackStackBuy(bool confirm, int qty)
	{
		if (confirm && qty > 0)
		{
			Game.Instance.SendBuyRequest(shop.ID, item.ID, qty, ShopType);
			Close();
		}
	}

	private void ConfirmationCallbackBuyMC(bool confirm)
	{
		if (confirm)
		{
			UIIAPStore.Show();
		}
	}

	public void OnPreviewClick()
	{
		if (item.Type == ItemType.Map)
		{
			Confirmation.Show("Join Preview Map?", "You will be taken to a preview of this map, are you sure you'd like to join?", delegate(bool ok)
			{
				if (ok)
				{
					AEC.getInstance().sendRequest(new RequestHouseCommand(Com.CmdHousing.MapPreview, 0, item.ID));
				}
			});
		}
		else
		{
			UIPreview.Show(item);
		}
	}
}
