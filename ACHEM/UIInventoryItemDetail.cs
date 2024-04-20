using System;
using Assets.Scripts.NetworkClient.CommClasses;
using StatCurves;
using UnityEngine;

public class UIInventoryItemDetail : MonoBehaviour
{
	private enum Mode
	{
		Equip,
		Unequip
	}

	public Action onClose;

	public UIButton btnClose;

	public UIButton btnDelete;

	public UIButton btnPreview;

	public UIButton btnEquip;

	public UIButton btnCostume;

	public UIButton btnUnequip;

	public UIButton btnEquipToSlot;

	public UILabel lblName;

	public UILabel lblLevel;

	public UILabel lblStats;

	public UILabel lblDesc;

	public UILabel lblUnequip;

	public UILabel CurrentEquip;

	public UILabel CurrentCosmetic;

	public UILabel lblPowerNumeric;

	public UILabel lblPowerText;

	public UILabel lblInfusionText;

	public UILabel lblModifierName;

	public UILabel lblModifierNameNoSprite;

	public UISprite Icon;

	public UISprite SprGuardian;

	public UISprite SprDelete;

	public UISprite ClassIcon;

	public UISprite modifierIcon;

	public UITable table;

	public UITable ButtonTable;

	public UIInventory UIInventory;

	public UIScrollView DescriptionScroll;

	public GameObject SelectActionSlotWindow;

	public GameObject PowerParent;

	public GameObject InfusionParent;

	private InventoryItem item;

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
		Session.MyPlayerData.ItemEquipped += OnItemUpdate;
		Session.MyPlayerData.ItemUnequipped += OnItemUpdate;
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnEquip.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBtnActionClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnCostume.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnBtnActionClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnUnequip.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnBtnActionClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnDelete.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnBtnDeleteClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(btnEquipToSlot.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnEquipToSlotClick));
	}

	public void OnDisable()
	{
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.ItemEquipped -= OnItemUpdate;
			Session.MyPlayerData.ItemUnequipped -= OnItemUpdate;
		}
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnEquip.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBtnActionClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnCostume.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnBtnActionClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnUnequip.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnBtnActionClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnDelete.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnBtnDeleteClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(btnEquipToSlot.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnEquipToSlotClick));
	}

	public void LoadInventoryItem(InventoryItem item)
	{
		Visible = true;
		SelectActionSlotWindow.SetActive(value: false);
		this.item = item;
		btnEquip.gameObject.SetActive(value: false);
		btnCostume.gameObject.SetActive(value: false);
		btnUnequip.gameObject.SetActive(value: false);
		lblName.text = "[" + item.RarityColor + "]" + item.Name + "[-]";
		Icon.spriteName = item.Icon;
		if (ClassIcon != null)
		{
			ClassIcon.gameObject.SetActive(!string.IsNullOrEmpty(item.IconFg));
			ClassIcon.spriteName = item.IconFg;
		}
		SprGuardian.gameObject.SetActive(item.IsGuardian);
		lblLevel.text = item.GetTagline(showPower: false);
		if (item.HasStats)
		{
			InfusionParent.SetActive(value: true);
			lblInfusionText.text = item.PowerOffset + "/" + item.InvTimesInfusable;
			if (item.modifier.rarity >= ItemModifier.ModifierRarity.uncommon)
			{
				lblModifierName.gameObject.SetActive(value: true);
				lblModifierNameNoSprite.gameObject.SetActive(value: false);
				modifierIcon.gameObject.SetActive(value: true);
				lblModifierName.text = item.modifier.name;
				int rarity = (int)item.modifier.rarity;
				modifierIcon.spriteName = setModifierIcon(rarity);
			}
			else
			{
				modifierIcon.gameObject.SetActive(value: false);
				lblModifierName.gameObject.SetActive(value: false);
				lblModifierNameNoSprite.gameObject.SetActive(value: true);
				lblModifierNameNoSprite.text = item.modifier.name;
			}
		}
		else
		{
			InfusionParent.SetActive(value: false);
		}
		lblStats.gameObject.SetActive(item.HasStats);
		lblStats.text = Session.MyPlayerData.GetComparisonStatText(item);
		lblDesc.text = item.GetDescription();
		if (item.GetCombatPower() > 0 && item.Type != ItemType.Map && item.Type != ItemType.HouseItem)
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
		if (item.IsEquipType)
		{
			if (item.IsEquipped)
			{
				mode = Mode.Unequip;
				btnUnequip.gameObject.SetActive(value: true);
				lblUnequip.text = "Unequip";
			}
			else
			{
				mode = Mode.Equip;
				btnEquip.gameObject.SetActive(value: true);
				btnCostume.gameObject.SetActive(item.Type != ItemType.Pet);
				btnEquip.isEnabled = item.HasStats || (item.IsTool && !item.IsCosmetic) || item.Type == ItemType.Pet;
			}
			CurrentEquip.text = FindCurrentEquip();
			CurrentCosmetic.text = FindCurrentCosmetic();
		}
		else if (item.IsUsable)
		{
			btnUnequip.gameObject.SetActive(value: true);
			lblUnequip.text = (item.IsUsable ? "Use" : (InterfaceColors.Chat.Red.ToBBCode() + "Level " + item.DisplayLevel));
		}
		btnEquipToSlot.gameObject.SetActive(item.IsUsable);
		btnUnequip.isEnabled = item.IsEquipType || item.IsUsable;
		btnPreview.isEnabled = item.HasPreview;
		table.Reposition();
		ButtonTable.Reposition();
		btnDelete.normalSprite = ((item.IsLootBoxItem && item.IsRemovable) ? "Shard-Recycle" : "Icon_Trash");
		DescriptionScroll?.ResetPosition();
	}

	private string FindCurrentEquip()
	{
		EquipItemSlot slot = (item.IsWeapon ? EquipItemSlot.Weapon : item.EquipSlot);
		InventoryItem equippedItem = Session.MyPlayerData.GetEquippedItem(slot);
		if (equippedItem != null)
		{
			return "[b]Currently[/b] " + equippedItem.Name;
		}
		return "[b]Currently[/b] None";
	}

	private string FindCurrentCosmetic()
	{
		InventoryItem cosmeticItem = Session.MyPlayerData.GetCosmeticItem(item.EquipSlot);
		if (cosmeticItem != null)
		{
			return "[b]Currently[/b] " + cosmeticItem.Name;
		}
		return "[b]Currently[/b] None";
	}

	public virtual void OnCloseClick(GameObject go)
	{
		Close();
		onClose?.Invoke();
	}

	public void Close()
	{
		SelectActionSlotWindow.SetActive(value: false);
		Visible = false;
	}

	private void OnBtnActionClick(GameObject go)
	{
		Player me = Entities.Instance.me;
		if (item.Type != ItemType.Pet && item.IsUsable)
		{
			if (item.IsGuardian && !Session.MyPlayerData.IsGuardian())
			{
				ConfirmGuardian();
			}
			else if (item.Level > me.Level)
			{
				Notification.ShowText("Requires Level " + item.Level + " to use");
			}
			else
			{
				Game.Instance.SendItemUseRequest(item);
			}
			return;
		}
		if (me.serverState == Entity.State.InCombat)
		{
			Notification.ShowText("Cannot change gear during combat.");
			return;
		}
		if (me.serverState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot change gear while you are dead.");
			return;
		}
		if (me.serverState == Entity.State.Interacting)
		{
			Notification.ShowText("Cannot change gear while interacting.");
			return;
		}
		bool flag = go.name == "ButtonEquip";
		if (mode == Mode.Equip)
		{
			if (item.IsGuardian && !Session.MyPlayerData.IsGuardian())
			{
				ConfirmGuardian();
			}
			else if (item.IsTool && item.TradeSkillLevel > me.tradeSkillLevel[item.TradeSkillID])
			{
				Notification.ShowText("Requires " + Enum.GetName(typeof(TradeSkillType), item.TradeSkillID) + " Level " + item.TradeSkillLevel + " to equip");
			}
			else if (item.Level > me.Level)
			{
				Notification.ShowText("Requires Level " + item.Level + " to equip");
			}
			else
			{
				InventoryItem.Equip equipId = (flag ? InventoryItem.Equip.Stat : InventoryItem.Equip.Cosmetic);
				Game.Instance.SendEquipRequest(item.CharItemID, equipId);
			}
		}
		else
		{
			if (mode != Mode.Unequip)
			{
				return;
			}
			if (item.IsWeapon && item.IsStatEquip)
			{
				Notification.ShowText("Weapons cannot be unequipped.");
				return;
			}
			EquipItemSlot weaponRequired = me.baseAsset.WeaponRequired;
			InventoryItem equippedItem = Session.MyPlayerData.GetEquippedItem(EquipItemSlot.Weapon);
			if (equippedItem != null && item.IsCosmeticEquip && item.EquipSlot == weaponRequired && equippedItem.EquipSlot != weaponRequired)
			{
				Notification.ShowText("Your current class requires a Cosmetic " + Item.GetSlotText(weaponRequired) + " to be equipped.");
			}
			else
			{
				Game.Instance.SendUnequipRequest(item.CharItemID);
			}
		}
	}

	private void ConfirmGuardian()
	{
		Confirmation.Show("Guardian Only", "You need to be a Guardian to use this item, would you like to become a Guardian?", delegate(bool b)
		{
			if (b)
			{
				UIIAPStore.Show();
			}
		});
	}

	private void OnBtnDeleteClick(GameObject go)
	{
		if (item.IsEquipped)
		{
			Notification.ShowText("Item is currently equipped.");
		}
		else if (!item.IsRemovable)
		{
			Notification.ShowText("Item cannot be deleted.");
		}
		else if (item.IsLootBoxItem)
		{
			Confirmation.Show("Confirm", "Convert [" + item.RarityColor + "]'" + item.Name + "'[-] into " + Item.GetLootBoxItemTokenSellPrice(item.Rarity) + " Treasure Shards?", ConfirmationDust);
		}
		else if (item.Qty > 1)
		{
			Confirmation.Show("Confirm", "Delete [" + item.RarityColor + "]'" + item.Name + " x" + item.Qty + "'[-]?", ConfirmationCallback);
		}
		else
		{
			Confirmation.Show("Confirm", "Delete [" + item.RarityColor + "]'" + item.Name + "'[-]?", ConfirmationCallback);
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

	public void OnEquipToSlotClick(GameObject go)
	{
		if (item.IsUsable)
		{
			if (!SelectActionSlotWindow.activeSelf)
			{
				SelectActionSlotWindow.SetActive(value: true);
			}
			else
			{
				SelectActionSlotWindow.SetActive(value: false);
			}
		}
	}

	public void EquipToSlot(CombatSpellSlot slotNumber)
	{
		SelectActionSlotWindow.SetActive(value: false);
		InventoryItem inventoryItem = GetItem();
		if (inventoryItem != null)
		{
			Session.MyPlayerData.EquipItemToSlot(inventoryItem, slotNumber);
		}
	}

	public InventoryItem GetItem()
	{
		return item;
	}

	private void OnBtnUseClick(GameObject go)
	{
		Game.Instance.SendItemUseRequest(item);
	}

	private void ConfirmationCallback(bool confirm)
	{
		if (confirm)
		{
			Game.Instance.SendItemRemoveRequest(item.CharItemID);
		}
	}

	private void ConfirmationDust(bool confirm)
	{
		if (confirm)
		{
			Game.Instance.SendItemDustRequest(item.CharItemID);
		}
	}

	private void OnItemUpdate(InventoryItem iItem)
	{
		if (item == iItem)
		{
			LoadInventoryItem(iItem);
		}
	}

	public string setModifierIcon(int modifierRarity)
	{
		return ItemModifier.getModifierIcon(modifierRarity);
	}

	public void openGraph()
	{
		ItemModifier.OpenAugmentGraph(item.modifier);
	}
}
