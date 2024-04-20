using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class UIInfusion : UIMenuWindow
{
	public List<InventoryItem> currentItems;

	public List<InventoryItem> displayedItems = new List<InventoryItem>();

	public GameObject itemListTemplate;

	public GameObject itemListTemplateExtract;

	public UITable itemListTable;

	public UIScrollView itemListScroll;

	private List<UIInventoryItem> uiInventoryItems;

	protected UIInventoryItem uiSelectedItem;

	private ObjectPool<GameObject> inventoryItemPool;

	private List<GameObject> EquipSlotItems = new List<GameObject>();

	public GameObject initialUI;

	public GameObject itemPicker;

	public UILabel windowTitle;

	public UIScrollView scrollView;

	public GameObject infuseUI;

	public UILabel infuseUITargetName;

	public UILabel infuseUITargetPower;

	public UISprite infuseUITargetIcon;

	public UILabel infuseUITargetInfusionCap;

	public GameObject infuseUICatalyst;

	public UILabel infuseUICatalystName;

	public UILabel infuseUICatalystCost;

	public UIButton infuseUIButton;

	public UILabel infuseUIResultName;

	public UILabel infuseUIResultPower;

	public UISprite infuseUIResultIcon;

	public UILabel infuseUIResultStats;

	public UILabel infuseUIResultInfusionCap;

	public UILabel infuseUIGoldCost;

	public GameObject extractUI;

	public UILabel extractUITargetName;

	public UILabel extractUITargetPower;

	public UISprite extractUITargetIcon;

	public UILabel extractUITargetInfusionCap;

	public UILabel extractUICatalystName;

	public UILabel extractUICatalystCost;

	public UILabel extractUICapstoneName;

	public UILabel extractUICapstoneCost;

	public GameObject extractUIButton;

	public UILabel extractUIResultName;

	public UILabel extractUIResultPower;

	public UISprite extractUIResultIcon;

	public UILabel extractUIResultStats;

	public UILabel extractUIResultInfusionCap;

	public UILabel extractUIGoldCost;

	public UITable InventoryTable;

	public GameObject grayedOut;

	public GameObject confirmationWindow;

	public UILabel confirmationText;

	public UILabel confirmationItemName;

	public UILabel confirmationItemTagLine;

	public UISprite confirmationItemIcon;

	public GameObject confirmationButtonInfuse;

	public UILabel confirmationButtonText;

	public GameObject confirmationButtonExtract;

	public UILabel confirmationButtonExtractText;

	public UILabel confirmationGoldCost;

	public GameObject WaitingUI;

	public GameObject TempGameObject;

	private InventoryItem itemSelected;

	private bool recievedCurrency;

	private bool recievedItem;

	private InventoryItem cachedItem;

	public static UIInfusion Instance { get; private set; }

	public int CurrentBank { get; protected set; }

	public static void Load()
	{
		if (Instance == null)
		{
			Instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/Infusion"), UIManager.Instance.transform).GetComponent<UIInfusion>();
			Instance.Init();
		}
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

	public void SwitchToInitialSelect()
	{
		windowTitle.text = "Infusion";
		cachedItem = null;
		recievedCurrency = false;
		recievedItem = false;
		initialUI.SetActive(value: true);
		itemPicker.SetActive(value: false);
		infuseUI.SetActive(value: false);
		extractUI.SetActive(value: false);
		confirmationWindow.SetActive(value: false);
	}

	public void SwitchToItemPickerInfuse()
	{
		RefreshCurrentItems();
		List<InventoryItem> list = new List<InventoryItem>();
		foreach (InventoryItem currentItem in currentItems)
		{
			if (currentItem.HasStats && currentItem.PowerOffset < currentItem.InvTimesInfusable)
			{
				list.Add(currentItem);
			}
		}
		list = list.OrderByDescending((InventoryItem x) => x.GetCombatPower()).ToList();
		if (list.Count == 0)
		{
			Notification.ShowText("No valid items!");
			SwitchToInitialSelect();
			return;
		}
		windowTitle.text = "Pick item to Infuse";
		initialUI.SetActive(value: false);
		itemPicker.SetActive(value: true);
		itemListTemplate.SetActive(value: true);
		itemListTemplateExtract.SetActive(value: false);
		clearTable();
		foreach (InventoryItem item in list)
		{
			GameObject obj = Object.Instantiate(itemListTemplate, InventoryTable.transform);
			obj.GetComponent<UIInventoryItem>().Item = item;
			UIInventoryItem component = obj.GetComponent<UIInventoryItem>();
			component.Item = item;
			component.Init(component.Item);
			component.IconEquipped.gameObject.SetActive(value: false);
			component.IconCostume.gameObject.SetActive(value: false);
		}
		InventoryTable.Reposition();
		scrollView.ResetPosition();
		itemListTemplate.SetActive(value: false);
	}

	public void SwitchToItemPickerExtract()
	{
		RefreshCurrentItems();
		List<InventoryItem> list = new List<InventoryItem>();
		foreach (InventoryItem currentItem in currentItems)
		{
			if (currentItem.Level == Session.MyPlayerData.EndGame && !currentItem.IsCosmetic && currentItem.PowerOffset > 0)
			{
				list.Add(currentItem);
			}
		}
		list = (from x in list
			orderby x.GetTradeSkillPower() descending, x.Rarity descending
			select x).ToList();
		if (list.Count == 0)
		{
			Notification.ShowText("No valid items!");
			SwitchToInitialSelect();
			return;
		}
		windowTitle.text = "Pick item to Extract";
		initialUI.SetActive(value: false);
		itemPicker.SetActive(value: true);
		itemListTemplateExtract.SetActive(value: true);
		clearTable();
		foreach (InventoryItem item in list)
		{
			GameObject obj = Object.Instantiate(itemListTemplateExtract, InventoryTable.transform);
			obj.GetComponent<UIInventoryItem>().Item = item;
			UIInventoryItem component = obj.GetComponent<UIInventoryItem>();
			component.Item = item;
			component.Init(component.Item);
			component.IconEquipped.gameObject.SetActive(value: false);
			component.IconCostume.gameObject.SetActive(value: false);
		}
		InventoryTable.Reposition();
		scrollView.ResetPosition();
		itemListTemplateExtract.SetActive(value: false);
		itemListTemplate.SetActive(value: false);
	}

	public void onClickCloseWindow()
	{
		Close();
	}

	public void SwitchToInfusionMain(GameObject itemClicked)
	{
		windowTitle.text = "Infusion";
		itemSelected = (InventoryItem)itemClicked.GetComponent<UIInventoryItem>().Item;
		itemPicker.SetActive(value: false);
		infuseUI.SetActive(value: true);
		infuseUITargetName.text = "[" + itemSelected.RarityColor + "]" + itemSelected.Name;
		int combatPower = itemSelected.GetCombatPower();
		infuseUITargetPower.text = "[000000]" + combatPower;
		infuseUITargetIcon.spriteName = itemSelected.Icon;
		infuseUITargetInfusionCap.text = "[000000]" + itemSelected.PowerOffset + "/" + itemSelected.InvTimesInfusable;
		if (itemSelected.Level == Session.MyPlayerData.EndGame)
		{
			infuseUICatalyst.SetActive(value: true);
			infuseUIGoldCost.gameObject.SetActive(value: false);
			int itemCount = Session.MyPlayerData.GetItemCount(Infusion.GetUpgradeRuneIDForLevel(itemSelected.Level));
			int infusionRuneCost = Infusion.GetInfusionRuneCost(itemSelected.DisplayPowerOffset);
			Item item = Items.Get(Infusion.GetUpgradeRuneIDForLevel(itemSelected.Level));
			infuseUICatalystName.text = "[" + item.RarityColor + "]" + item.Name;
			infuseUICatalystCost.text = ((itemCount >= infusionRuneCost) ? "[000000]" : "[ad0000]");
			UILabel uILabel = infuseUICatalystCost;
			uILabel.text = uILabel.text + itemCount + "[-] / [000000]" + infusionRuneCost + "[-]";
			if (itemCount < infusionRuneCost)
			{
				grayedOut.SetActive(value: true);
				infuseUIButton.GetComponent<UIButton>().isEnabled = false;
			}
			else if (itemCount >= infusionRuneCost)
			{
				grayedOut.SetActive(value: false);
			}
		}
		else
		{
			infuseUIGoldCost.gameObject.SetActive(value: true);
			infuseUICatalyst.SetActive(value: false);
		}
		Item item2 = new Item(itemSelected, 1);
		item2.PowerOffset++;
		item2.RecalculateStats();
		infuseUIResultInfusionCap.text = "[005900]" + item2.PowerOffset + "[-][000000]/" + itemSelected.InvTimesInfusable + "[-]";
		infuseUIResultIcon.spriteName = item2.Icon;
		infuseUIResultPower.text = "[005900]" + (combatPower + 1) + "[-]";
		infuseUIResultName.text = "[" + itemSelected.RarityColor + "]" + item2.Name;
		infuseUIResultStats.text = Session.MyPlayerData.GetComparisonStatText(item2, itemSelected);
		infuseUIGoldCost.text = Infusion.GetInfusionGoldCost(itemSelected.PowerOffset, itemSelected.Level).ToString() ?? "";
	}

	public void SwitchToExtractionMain(GameObject itemClicked)
	{
		windowTitle.text = "Extraction";
		itemSelected = (InventoryItem)itemClicked.GetComponent<UIInventoryItem>().Item;
		itemPicker.SetActive(value: false);
		extractUI.SetActive(value: true);
		int combatPower = itemSelected.GetCombatPower();
		extractUITargetName.text = "[" + itemSelected.RarityColor + "]" + itemSelected.Name;
		extractUITargetPower.text = combatPower.ToString();
		extractUITargetIcon.spriteName = itemSelected.Icon;
		extractUITargetInfusionCap.text = itemSelected.PowerOffset + "/" + itemSelected.InvTimesInfusable;
		Item item = Items.Get(Infusion.GetUpgradeRuneIDForLevel(itemSelected.Level));
		int num = 0;
		for (int i = 0; i < itemSelected.PowerOffset; i++)
		{
			num += Infusion.GetInfusionRuneCost(i);
		}
		extractUICatalystName.text = "";
		extractUICatalystCost.text = "[" + item.RarityColor + "]" + item.Name + "[-][000000] x" + num;
		Item item2 = new Item(itemSelected, 1);
		item2.PowerOffset = 0;
		item2.RecalculateStats();
		extractUIResultInfusionCap.text = "[ad0000]" + item2.PowerOffset + "[-][000000]/" + itemSelected.InvTimesInfusable + "[-]";
		extractUIResultIcon.spriteName = item2.Icon;
		extractUIResultPower.text = "[ad0000]" + item2.GetCombatPower() + "[-]";
		extractUIResultName.text = "[" + itemSelected.RarityColor + "]" + item2.Name;
		extractUIResultStats.text = Session.MyPlayerData.GetComparisonStatText(item2, itemSelected);
		extractUIGoldCost.text = Infusion.GetExtractionGoldCost(itemSelected.PowerOffset, itemSelected.Level).ToString() ?? "";
	}

	public void SendInfusionRequest()
	{
		CloseConfirmationWindow();
		InventoryItem inventoryItem = itemSelected;
		if (inventoryItem != null)
		{
			Game.Instance.SendInfuseItemRequest(inventoryItem);
		}
		Session.MyPlayerData.InfusionFinished += MyPlayerData_InfusionFinished;
		Session.MyPlayerData.ItemUpdated += MyPlayerData_ItemUpdated;
	}

	private void MyPlayerData_ItemUpdated(InventoryItem invItem)
	{
		if (invItem.ID == Infusion.GetUpgradeRuneIDForLevel(Session.MyPlayerData.EndGame))
		{
			recievedCurrency = true;
			recievedBothResponses();
			Session.MyPlayerData.ItemUpdated -= MyPlayerData_ItemUpdated;
		}
	}

	private void MyPlayerData_InfusionFinished(InventoryItem invItem)
	{
		cachedItem = invItem;
		recievedItem = true;
		WaitingUI.SetActive(value: false);
		Session.MyPlayerData.InfusionFinished -= MyPlayerData_InfusionFinished;
		RefreshCurrentItems();
		int displayPowerOffset = invItem.DisplayPowerOffset;
		int invTimesInfusable = invItem.InvTimesInfusable;
		AudioManager.Play2DSFX("SFX_WeaponInfusion");
		if (displayPowerOffset < invTimesInfusable)
		{
			if (invItem.InvLevel != Session.MyPlayerData.EndGame)
			{
				TempGameObject.GetComponent<UIInventoryItem>().Item = cachedItem;
				SwitchToInfusionMain(TempGameObject);
			}
			else
			{
				recievedBothResponses();
			}
		}
		else
		{
			SwitchToInitialSelect();
		}
	}

	private void recievedBothResponses()
	{
		if (recievedItem && recievedCurrency)
		{
			recievedCurrency = false;
			recievedItem = false;
			TempGameObject.GetComponent<UIInventoryItem>().Item = cachedItem;
			SwitchToInfusionMain(TempGameObject);
		}
	}

	public void SendExtractionRequest()
	{
		CloseConfirmationWindow();
		infuseUI.SetActive(value: false);
		WaitingUI.SetActive(value: true);
		InventoryItem inventoryItem = itemSelected;
		if (inventoryItem != null)
		{
			Game.Instance.SendExtractItemRequest(inventoryItem);
		}
		SwitchToInitialSelect();
	}

	public void ShowConfirmationWindowInfuse()
	{
		SetUpConfirmationWindow();
		confirmationText.text = "Are you sure you want to infuse this item?";
		if (itemSelected.Level == Session.MyPlayerData.EndGame)
		{
			confirmationGoldCost.gameObject.SetActive(value: false);
		}
		else
		{
			confirmationGoldCost.gameObject.SetActive(value: true);
			confirmationGoldCost.text = Infusion.GetInfusionGoldCost(itemSelected.PowerOffset, itemSelected.Level).ToString() ?? "";
		}
		confirmationButtonExtract.SetActive(value: false);
		confirmationButtonInfuse.SetActive(value: true);
		confirmationItemName.text = "[" + itemSelected.RarityColor + "]" + itemSelected.Name;
		confirmationItemTagLine.text = itemSelected.GetCombatPower() + " Power";
		confirmationItemIcon.spriteName = itemSelected.Icon;
		confirmationButtonText.text = "Infuse";
	}

	public override void OnBackClick(GameObject go)
	{
		if (infuseUI.activeInHierarchy)
		{
			RefreshCurrentItems();
			SwitchToInitialSelect();
			SwitchToItemPickerInfuse();
		}
		else if (extractUI.activeInHierarchy)
		{
			RefreshCurrentItems();
			SwitchToInitialSelect();
			SwitchToItemPickerExtract();
		}
		else if (itemPicker.activeInHierarchy)
		{
			RefreshCurrentItems();
			SwitchToInitialSelect();
		}
		else
		{
			base.OnBackClick(go);
		}
	}

	public void ShowConfirmationWindowExtract()
	{
		SetUpConfirmationWindow();
		confirmationText.text = "Are you sure you want to extract from this item?";
		confirmationGoldCost.text = Infusion.GetExtractionGoldCost(itemSelected.PowerOffset, itemSelected.Level).ToString() ?? "";
		confirmationButtonExtract.SetActive(value: true);
		confirmationButtonInfuse.SetActive(value: false);
		confirmationItemName.text = "[" + itemSelected.RarityColor + "]" + itemSelected.Name;
		confirmationItemTagLine.text = itemSelected.GetCombatPower() + " Power";
		confirmationItemIcon.spriteName = itemSelected.Icon;
		confirmationButtonText.text = "Extract";
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
		if (lblGold != null)
		{
			lblGold.text = Session.MyPlayerData.Gold.ToString("n0");
		}
		if (lblMC != null)
		{
			lblMC.text = Session.MyPlayerData.MC.ToString("n0");
		}
		Refresh();
		AudioManager.Play2DSFX("UI_Bag_Open");
	}

	public override void Close()
	{
		base.Close();
		AudioManager.Play2DSFX("UI_Bag_Close");
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
			Object.Destroy(equipSlotItem);
		}
		EquipSlotItems.Clear();
	}

	protected void Refresh(bool shouldReset = true)
	{
		ClearAll();
		RefreshCurrentItems();
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
			Object.Destroy(child.gameObject);
		}
	}

	private void RefreshCurrentItems()
	{
		if (CurrentBank >= 0)
		{
			currentItems = Session.MyPlayerData.allItems[CurrentBank].Where((InventoryItem x) => !x.IsHidden).ToList();
			return;
		}
		currentItems.Clear();
		foreach (KeyValuePair<int, List<InventoryItem>> allItem in Session.MyPlayerData.allItems)
		{
			if (allItem.Key != 0)
			{
				currentItems.AddRange(allItem.Value.Where((InventoryItem x) => !x.IsHidden).ToList());
			}
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
