using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIAugmentReroll : UIMenuWindow
{
	public List<InventoryItem> currentItems;

	public List<InventoryItem> displayedItems = new List<InventoryItem>();

	public GameObject itemListTemplate;

	public UITable itemListTable;

	public UIScrollView itemListScroll;

	private List<UIInventoryItem> uiInventoryItems;

	protected UIInventoryItem uiSelectedItem;

	private ObjectPool<GameObject> inventoryItemPool;

	private List<GameObject> EquipSlotItems = new List<GameObject>();

	public GameObject itemPicker;

	public UIAugmentRerollItemPickDetail itemPickerManager;

	public GameObject rerollMainUI;

	public UIAugmentRerollDetail rerollMainUIManager;

	public UILabel windowTitle;

	public UIScrollView scrollView;

	public UITable InventoryTable;

	public GameObject confirmationWindow;

	public GameObject confirmationCloseWindow;

	public GameObject confirmationBackWindow;

	public UISprite confirmationModifierOldIcon;

	public UISprite confirmationModifierNewIcon;

	public UILabel confirmationModifierOldName;

	public UILabel confirmationModifierNewName;

	public UIAugmentRerollItemPickDetail picker;

	private InventoryItem itemSelected;

	private bool recievedCurrency;

	private bool recievedItem;

	private InventoryItem cachedItem;

	public static UIAugmentReroll Instance { get; private set; }

	public int CurrentBank { get; protected set; }

	public static void Load()
	{
		if (Instance == null)
		{
			Instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/AugmentReroll"), UIManager.Instance.transform).GetComponent<UIAugmentReroll>();
			Instance.Init();
		}
	}

	public void OnEnable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnBack.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBackClick));
	}

	public void OnDisable()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnClose.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnCloseClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnBack.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Remove(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnBackClick));
	}

	public static void Toggle()
	{
		if (Instance == null)
		{
			Load();
		}
		else
		{
			Instance.Close();
		}
	}

	public void SwitchToItemPicker()
	{
		rerollMainUI.gameObject.SetActive(value: false);
		itemPickerManager.RefreshCurrentItems();
		List<InventoryItem> list = new List<InventoryItem>();
		foreach (InventoryItem currentItem in currentItems)
		{
			if (currentItem.HasStats)
			{
				list.Add(currentItem);
			}
		}
		list = list.OrderByDescending((InventoryItem x) => x.GetCombatPower()).ToList();
		if (list.Count == 0)
		{
			Notification.ShowText("No valid items!");
			Back();
			return;
		}
		windowTitle.text = "Pick item to Augment";
		itemPicker.SetActive(value: true);
		itemListTemplate.SetActive(value: true);
		clearTable();
		itemPickerManager.populateList(list);
		InventoryTable.Reposition();
		scrollView.ResetPosition();
		itemListTemplate.SetActive(value: false);
	}

	public void onClickCloseWindow()
	{
		Close();
	}

	public void SwitchToRerollMain(GameObject itemClicked)
	{
		windowTitle.text = "Augment Reroll";
		if (itemClicked.GetComponent<UIInventoryItem>().Item is InventoryItem inventoryItem)
		{
			itemSelected = inventoryItem;
		}
		rerollMainUIManager = rerollMainUI.GetComponent<UIAugmentRerollDetail>();
		rerollMainUI.SetActive(value: true);
		rerollMainUIManager.init(itemClicked.GetComponent<UIInventoryItem>(), itemSelected.MaxHealthControl, itemSelected.AttackControl, itemSelected.ArmorControl, itemSelected.EvasionControl, itemSelected.CritControl, itemSelected.HasteControl);
		itemPicker.SetActive(value: false);
	}

	public void ShowConfirmationWindowReroll()
	{
		SetUpConfirmationWindow();
		int modifierRarity = rerollMainUIManager.item.modifierRarity;
		int rarity = (int)rerollMainUIManager.newItemMod.rarity;
		if (modifierRarity < 2)
		{
			confirmationModifierOldIcon.gameObject.SetActive(value: false);
		}
		else
		{
			confirmationModifierOldIcon.gameObject.SetActive(value: true);
			confirmationModifierOldIcon.spriteName = ItemModifier.getModifierIcon(modifierRarity);
		}
		if (rarity < 2)
		{
			confirmationModifierNewIcon.gameObject.SetActive(value: false);
		}
		else
		{
			confirmationModifierNewIcon.gameObject.SetActive(value: true);
			confirmationModifierNewIcon.spriteName = ItemModifier.getModifierIcon(rarity);
		}
		confirmationModifierOldName.text = rerollMainUIManager.item.modifier.name;
		confirmationModifierNewName.text = rerollMainUIManager.resultModifierLabel.text;
	}

	public override void OnBackClick(GameObject go)
	{
		OpenBackWarning();
	}

	public void CloseConfirmationWindow()
	{
		confirmationWindow.SetActive(value: false);
	}

	public void SetUpConfirmationWindow()
	{
		confirmationWindow.SetActive(value: true);
	}

	public void RepositionTable()
	{
		itemListTable.Reposition();
	}

	public void sendRerollConfirm()
	{
		Game.Instance.SendItemModifierRerollConfirmation();
		Session.MyPlayerData.modifierConfirmed += rerollConfirmationRecieved;
	}

	public void rerollConfirmationRecieved(InventoryItem item)
	{
		switch ((int)item.modifier.rarity)
		{
		case 1:
			AudioManager.Play2DSFX("SFX_Common_Augment");
			break;
		case 2:
			AudioManager.Play2DSFX("SFX_Uncommon_Augment");
			break;
		case 3:
			AudioManager.Play2DSFX("SFX_Rare_Augment");
			break;
		case 4:
			AudioManager.Play2DSFX("SFX_Epic_Augment");
			break;
		case 5:
			AudioManager.Play2DSFX("SFX_Legendary_Augment");
			break;
		case 6:
			AudioManager.Play2DSFX("SFX_Mythical_Augment");
			break;
		}
		Session.MyPlayerData.modifierConfirmed -= rerollConfirmationRecieved;
		Close();
	}

	protected override void Init()
	{
		base.Init();
		Setup();
	}

	protected virtual void Setup()
	{
		currentItems = Session.MyPlayerData.allItems[CurrentBank].Where((InventoryItem x) => !x.IsHidden).ToList();
		uiInventoryItems = new List<UIInventoryItem>();
		inventoryItemPool = new ObjectPool<GameObject>(itemListTemplate);
		itemListTemplate.SetActive(value: false);
		itemPickerManager.init(itemListTemplate, InventoryTable);
		if (lblGold != null)
		{
			lblGold.text = Session.MyPlayerData.Gold.ToString("n0");
		}
		if (lblMC != null)
		{
			lblMC.text = Session.MyPlayerData.MC.ToString("n0");
		}
		Refresh();
		SwitchToItemPicker();
		AudioManager.Play2DSFX("UI_Bag_Open");
	}

	public void closeConfrimWindow()
	{
		confirmationWindow.SetActive(value: false);
	}

	public override void Close()
	{
		base.Close();
		AudioManager.Play2DSFX("UI_Bag_Close");
	}

	public override void OnCloseClick(GameObject go)
	{
		OpenCloseWarning();
	}

	public void OpenCloseWarning()
	{
		if (rerollMainUIManager.newItemMod != null && rerollMainUIManager.newItemMod.rarity > ItemModifier.ModifierRarity.common)
		{
			confirmationCloseWindow.SetActive(value: true);
		}
		else
		{
			Close();
		}
	}

	public void CloseCloseWarning()
	{
		confirmationCloseWindow.SetActive(value: false);
	}

	public void OpenBackWarning()
	{
		if (rerollMainUIManager.newItemMod != null && rerollMainUIManager.newItemMod.rarity > ItemModifier.ModifierRarity.common)
		{
			confirmationBackWindow.SetActive(value: true);
		}
		else
		{
			SwitchToItemPicker();
		}
	}

	public void CloseBackWarning()
	{
		confirmationBackWindow.SetActive(value: false);
	}

	private void ClearAll()
	{
		foreach (UIInventoryItem uiInventoryItem in uiInventoryItems)
		{
			uiInventoryItem.gameObject.transform.SetAsLastSibling();
			inventoryItemPool.Release(uiInventoryItem.gameObject);
		}
		uiInventoryItems.Clear();
		foreach (GameObject equipSlotItem in EquipSlotItems)
		{
			UnityEngine.Object.Destroy(equipSlotItem);
		}
		EquipSlotItems.Clear();
	}

	protected void Refresh(bool shouldReset = true)
	{
		ClearAll();
		itemPickerManager.RefreshCurrentItems();
		itemListTable.Reposition();
		if (shouldReset)
		{
			itemListScroll.ResetPosition();
		}
		else
		{
			itemListScroll.InvalidateBounds();
		}
	}

	public void clearTable()
	{
		foreach (Transform child in InventoryTable.GetChildList())
		{
			UnityEngine.Object.Destroy(child.gameObject);
		}
	}

	protected override void Destroy()
	{
		base.Destroy();
		Instance = null;
	}

	protected override void Resume()
	{
		base.Resume();
		Refresh();
	}
}
