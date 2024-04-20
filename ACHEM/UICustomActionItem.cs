using System.Linq;
using StatCurves;

public class UICustomActionItem : UICustomActionButton
{
	public UILabel Quantity;

	public InventoryItem item { get; private set; }

	public override bool IsAssigned => item != null;

	protected override void Start()
	{
		base.Start();
		UpdateLock();
		if (!IsLocked())
		{
			UpdateDataFromDisk();
		}
		SprCooldown.gameObject.SetActive(value: false);
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		Session.MyPlayerData.UpdateActionItem += OnItemEquipped;
		Session.MyPlayerData.ItemUpdated += OnItemUpdated;
		Session.MyPlayerData.ItemRemoved += OnItemRemoved;
		Entities.Instance.me.LevelUpdated += OnLevelUpdated;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		Session.MyPlayerData.UpdateActionItem -= OnItemEquipped;
		Session.MyPlayerData.ItemUpdated -= OnItemUpdated;
		Session.MyPlayerData.ItemRemoved -= OnItemRemoved;
		Entities.Instance.me.LevelUpdated -= OnLevelUpdated;
	}

	public override void Refresh()
	{
		UpdateItem(item, SlotNumber);
	}

	private void OnItemEquipped(InventoryItem item, CombatSpellSlot slotNumber)
	{
		Notification.ShowText(item.Name + " equipped");
		UpdateItem(item, slotNumber);
	}

	private void UpdateItem(InventoryItem item, CombatSpellSlot slotNumber)
	{
		if (slotNumber != SlotNumber)
		{
			return;
		}
		if (item == null)
		{
			ShowEmpty();
			return;
		}
		this.item = item;
		if (item.SpellID > 0)
		{
			UpdateSpell(item.SpellID);
		}
		else
		{
			base.spellT = null;
		}
		Icon.spriteName = item.Icon;
		EmptyIcon.gameObject.SetActive(value: false);
		Quantity.text = item.Qty.ToString();
		Quantity.gameObject.SetActive(item.Type == ItemType.Consumable);
	}

	private void OnLevelUpdated()
	{
		CheckForUnlock();
	}

	private void OnItemUpdated(InventoryItem updatedItem)
	{
		if (item != null && updatedItem.ID == item.ID && Quantity.gameObject.activeSelf)
		{
			Quantity.text = updatedItem.Qty.ToString();
		}
	}

	private void OnItemRemoved(InventoryItem removedItem)
	{
		if (item != null && removedItem.ID == item.ID)
		{
			SettingsManager.SetActionSlotID(SlotNumber, 0);
			UpdateItem(null, SlotNumber);
		}
	}

	protected override string GetTooltip()
	{
		if (item == null)
		{
			return "";
		}
		return item.GetToolTip();
	}

	protected override void UpdateLock()
	{
		base.UpdateLock();
		if (!IsLocked())
		{
			UpdateItem(item, SlotNumber);
		}
	}

	protected override void ShowEmpty()
	{
		base.ShowEmpty();
		item = null;
		Quantity.gameObject.SetActive(value: false);
	}

	protected void UpdateDataFromDisk()
	{
		int itemID = SettingsManager.GetActionSlotID(SlotNumber);
		item = Session.MyPlayerData.items.FirstOrDefault((InventoryItem p) => p.ID == itemID);
		UpdateItem(item, SlotNumber);
	}

	protected override void OpenSelectMenu()
	{
		if (!IsLocked())
		{
			UICustomActionSelection.Load(UICustomActionSelection.Mode.Item, SlotNumber);
		}
	}

	protected override bool IsLocked()
	{
		return Game.Instance.LevelReqForAction(Action) > Entities.Instance.me.Level;
	}
}
