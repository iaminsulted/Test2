using System;
using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class UICustomActionSelection : UIWindow
{
	public enum Mode
	{
		Item,
		CrossSkill,
		PvpAction,
		TravelForm
	}

	public Transform ToolTipPosition;

	private static UICustomActionSelection instance;

	public UILabel lblTitle;

	public UIButton btnClose;

	public GameObject actionPrefab;

	public UIInventoryTabs inventoryTabs;

	private Transform container;

	private List<UICustomActionListItem> actionItems;

	private UIItem selectedItem;

	private ObjectPool<GameObject> actionPool;

	private UIItemDetails uiInventoryItemDetail;

	private Mode mode;

	private CombatSpellSlot slotNumber;

	private UIInventory.SortType sortType = UIInventory.SortType.Newest;

	private UIInventory.FilterType filterType;

	public static void Load(Mode mode, CombatSpellSlot slotNumber)
	{
		if (instance == null || mode != instance.mode)
		{
			UIWindow.ClearWindows();
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/CustomActionSelection"), UIManager.Instance.transform).GetComponent<UICustomActionSelection>();
			instance.mode = mode;
			instance.slotNumber = slotNumber;
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		container = actionPrefab.transform.parent;
		actionItems = new List<UICustomActionListItem>();
		actionPool = new ObjectPool<GameObject>(actionPrefab);
		actionPrefab.SetActive(value: false);
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		lblTitle.text = GetTitle(mode);
		if (mode == Mode.Item)
		{
			inventoryTabs.Init(null);
			UIInventoryTabs uIInventoryTabs = inventoryTabs;
			uIInventoryTabs.onFilterButtonClicked = (Action<UIInventory.FilterType, UIInventory.SortType, UIInventory.FilterType>)Delegate.Combine(uIInventoryTabs.onFilterButtonClicked, new Action<UIInventory.FilterType, UIInventory.SortType, UIInventory.FilterType>(OnFilterClick));
			UIInventoryTabs uIInventoryTabs2 = inventoryTabs;
			uIInventoryTabs2.onSortButtonClicked = (Action<UIInventory.SortType>)Delegate.Combine(uIInventoryTabs2.onSortButtonClicked, new Action<UIInventory.SortType>(OnSortClick));
		}
		else
		{
			inventoryTabs.gameObject.SetActive(value: false);
		}
		Refresh();
	}

	private void OnFilterClick(UIInventory.FilterType filterType, UIInventory.SortType sortType, UIInventory.FilterType previousFilterType)
	{
		this.filterType = filterType;
		this.sortType = sortType;
		Refresh();
	}

	private void OnSortClick(UIInventory.SortType sortType)
	{
		this.sortType = sortType;
		Refresh();
	}

	private void OnCloseClick(GameObject go)
	{
		Close();
	}

	private void OnItemClick(UICustomActionListItem uiItem)
	{
		IUIItem item = uiItem.item;
		if (!(item is SpellTemplate spellTemplate))
		{
			if (item is InventoryItem item2)
			{
				Session.MyPlayerData.EquipItemToSlot(item2, slotNumber);
			}
		}
		else if (Entities.Instance.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowWarning("Cannot swap during combat");
		}
		else if (spellTemplate.isPvpAction)
		{
			CombatSpellSlot combatSpellSlot = ((slotNumber == CombatSpellSlot.PvpSpell1) ? CombatSpellSlot.PvpSpell2 : CombatSpellSlot.PvpSpell1);
			if (SettingsManager.GetActionSlotID(slotNumber) == spellTemplate.ID)
			{
				Notification.ShowWarning("Already equipped");
				return;
			}
			if (SettingsManager.GetActionSlotID(combatSpellSlot) == spellTemplate.ID)
			{
				Session.MyPlayerData.SetPvpAction(combatSpellSlot, SettingsManager.GetActionSlotID(slotNumber));
				Session.MyPlayerData.SetPvpAction(slotNumber, spellTemplate.ID);
			}
			else
			{
				Dictionary<int, int> pvpActionIDs = new Dictionary<int, int>
				{
					{
						(int)combatSpellSlot,
						SettingsManager.GetActionSlotID(combatSpellSlot)
					},
					{
						(int)slotNumber,
						spellTemplate.ID
					}
				};
				Game.Instance.SendPvpActionEquipRequest(pvpActionIDs);
			}
		}
		else
		{
			if (Session.MyPlayerData.CurrentCrossSkill == spellTemplate.ID)
			{
				Notification.ShowWarning("Already equipped");
				return;
			}
			Game.Instance.SendCrossSkillEquipRequest(spellTemplate.ID);
		}
		Close();
	}

	public void Refresh()
	{
		foreach (UICustomActionListItem actionItem in actionItems)
		{
			actionItem.Clicked -= OnItemClick;
			actionItem.gameObject.transform.SetAsLastSibling();
			actionPool.Release(actionItem.gameObject);
		}
		actionItems.Clear();
		AddActionItemsByMode(mode);
		container.GetComponent<UIGrid>().Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private string GetTitle(Mode mode)
	{
		return mode switch
		{
			Mode.Item => "Usable Items", 
			Mode.CrossSkill => "Cross Skills", 
			Mode.PvpAction => "PvP Spells", 
			Mode.TravelForm => "Travel Forms", 
			_ => "", 
		};
	}

	private void AddActionItemsByMode(Mode mode)
	{
		switch (mode)
		{
		case Mode.CrossSkill:
		{
			foreach (SpellTemplate item in (from spellT in Session.MyPlayerData.UnlockedCrossSkillIDs.Select(SpellTemplates.GetBaseSpell)
				orderby Session.MyPlayerData.combatClassList.FirstOrDefault((CombatClass c) => c.CrossSkillID == spellT.ID)?.SortOrder
				select spellT).ToList())
			{
				AddItemToList().SetSpellInfo(item, ToolTipPosition);
			}
			break;
		}
		case Mode.Item:
		{
			List<InventoryItem> list = Session.MyPlayerData.items.Where((InventoryItem item) => item.IsUsable).ToList();
			if (filterType == UIInventory.FilterType.AllItems)
			{
				list = list.Where((InventoryItem x) => UIInventory.GetCategoriesForItem(x).Contains(UIInventory.FilterType.AllItems)).ToList();
			}
			else if (filterType == UIInventory.FilterType.Item)
			{
				list = list.Where((InventoryItem x) => UIInventory.GetCategoriesForItem(x).Contains(UIInventory.FilterType.Item)).ToList();
			}
			else if (filterType == UIInventory.FilterType.Consumable)
			{
				list = list.Where((InventoryItem x) => UIInventory.GetCategoriesForItem(x).Contains(UIInventory.FilterType.Consumable)).ToList();
			}
			else if (filterType == UIInventory.FilterType.TravelForm)
			{
				list = list.Where((InventoryItem x) => UIInventory.GetCategoriesForItem(x).Contains(UIInventory.FilterType.TravelForm)).ToList();
			}
			else if (filterType == UIInventory.FilterType.Crystal)
			{
				list = list.Where((InventoryItem x) => UIInventory.GetCategoriesForItem(x).Contains(UIInventory.FilterType.Crystal)).ToList();
			}
			list = UIInventory.GetSortedItems(sortType, list).ToList();
			{
				foreach (InventoryItem item2 in list)
				{
					AddItemToList().SetItemInfo(item2, ToolTipPosition);
				}
				break;
			}
		}
		case Mode.PvpAction:
		{
			foreach (SpellTemplate pvpAction in SpellTemplates.GetPvpActions())
			{
				AddItemToList().SetSpellInfo(pvpAction, ToolTipPosition);
			}
			break;
		}
		case Mode.TravelForm:
		{
			foreach (InventoryItem item3 in Session.MyPlayerData.items.Where((InventoryItem item) => item.IsTravelForm && item.Type != ItemType.Consumable).ToList())
			{
				AddItemToList().SetItemInfo(item3, ToolTipPosition);
			}
			break;
		}
		}
	}

	private UICustomActionListItem AddItemToList()
	{
		GameObject obj = actionPool.Get();
		obj.transform.SetParent(container, worldPositionStays: false);
		obj.SetActive(value: true);
		UICustomActionListItem componentInChildren = obj.GetComponentInChildren<UICustomActionListItem>();
		actionItems.Add(componentInChildren);
		componentInChildren.Clicked += OnItemClick;
		return componentInChildren;
	}
}
