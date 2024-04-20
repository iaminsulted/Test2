using System.Linq;
using UnityEngine;

public class UIChooseActionSlot : MonoBehaviour
{
	public CombatSpellSlot SlotNumber;

	public UIInventoryItemDetail UIInventoryItemDetail;

	public UISprite Icon;

	public GameObject Lock;

	public InputAction Action;

	public UILabel LabelKey;

	private void Awake()
	{
		if (UIGame.ControlScheme == ControlScheme.HANDHELD)
		{
			LabelKey.gameObject.SetActive(value: false);
		}
	}

	private void OnEnable()
	{
		int itemID = SettingsManager.GetActionSlotID(SlotNumber);
		InventoryItem inventoryItem = Session.MyPlayerData.items.FirstOrDefault((InventoryItem p) => p.ID == itemID);
		if (itemID == 0 || inventoryItem == null)
		{
			Icon.spriteName = "aq3dui-goldbordercircle";
		}
		else
		{
			Icon.spriteName = inventoryItem.Icon;
		}
		Lock.SetActive(CheckLock());
		LabelKey.text = SettingsManager.GetHotkeyByAction(Action);
	}

	private bool CheckLock()
	{
		return Game.Instance.LevelReqForAction(Action) > Entities.Instance.me.Level;
	}

	public void OnClick()
	{
		if (SlotNumber == CombatSpellSlot.Passive)
		{
			Debug.LogWarning("UIChooseActionSlot int SlotNumber needs a value");
		}
		else if (!CheckLock())
		{
			UIInventoryItemDetail.EquipToSlot(SlotNumber);
			InventoryItem item = UIInventoryItemDetail.GetItem();
			if (item != null)
			{
				Icon.spriteName = item.Icon;
			}
		}
	}
}
