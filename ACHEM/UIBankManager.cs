using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIBankManager : UIStackingWindow
{
	public UIBankInventory Inventory;

	public UIBankInventory Bank;

	public BankTabGroup BankGroup;

	public int InventoryTotalSize;

	public int BankTotalSize;

	public int BankCost;

	public UIInventoryTabs inventoryTabs;

	public Action<int, int> SelectItemAction;

	public static UIBankManager Instance { get; private set; }

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		InventoryTotalSize = Session.MyPlayerData.BagSlots;
		BankTotalSize = Session.MyPlayerData.BankSlots;
		LoadInitialData();
	}

	private void OnEnable()
	{
		UIBankInventory bank = Bank;
		bank.onRefreshItems = (Action<List<int>>)Delegate.Combine(bank.onRefreshItems, new Action<List<int>>(OnBankRefreshItems));
		BankTabGroup bankGroup = BankGroup;
		bankGroup.SelectTabAction = (Action<int>)Delegate.Combine(bankGroup.SelectTabAction, new Action<int>(ChangeBank));
		Session.MyPlayerData.BankLoaded += BankLoaded;
		Session.MyPlayerData.BankCountUpdated += BankAdded;
		Session.MyPlayerData.AllBanksLoaded += AllBanksLoaded;
		Session.MyPlayerData.ItemTransferred += ItemTransferred;
	}

	private void OnDisable()
	{
		BankTabGroup bankGroup = BankGroup;
		bankGroup.SelectTabAction = (Action<int>)Delegate.Remove(bankGroup.SelectTabAction, new Action<int>(ChangeBank));
		UIBankInventory bank = Bank;
		bank.onRefreshItems = (Action<List<int>>)Delegate.Remove(bank.onRefreshItems, new Action<List<int>>(OnBankRefreshItems));
		if (Session.MyPlayerData != null)
		{
			Session.MyPlayerData.BankLoaded -= BankLoaded;
			Session.MyPlayerData.BankCountUpdated -= BankAdded;
			Session.MyPlayerData.AllBanksLoaded -= AllBanksLoaded;
			Session.MyPlayerData.ItemTransferred -= ItemTransferred;
		}
	}

	private void OnBankRefreshItems(List<int> itemCounts)
	{
		inventoryTabs.RefreshCategoryCounts(itemCounts, null);
	}

	public void DeselectAll()
	{
		Inventory.DeselectAll();
		Bank.DeselectAll();
	}

	public void BankDataRequest(int bankId)
	{
		AEC.getInstance().sendRequest(new RequestBankItems(bankId));
	}

	public void AllBanksDataRequest(List<int> haveToDownload)
	{
		AEC.getInstance().sendRequest(new RequestAllBankItems(haveToDownload));
	}

	public void BankLoaded(int bankID, List<InventoryItem> items)
	{
		Bank.Load(bankID);
	}

	public void AllBanksLoaded(Dictionary<int, List<InventoryItem>> banks)
	{
		Bank.AllBanksLoaded();
	}

	public void BankAdded(int count)
	{
		Session.MyPlayerData.LoadedBanks.Add(count);
		Session.MyPlayerData.allItems[count] = new List<InventoryItem>();
		ChangeBank(count);
		BankGroup.Init(count, count, scrollToBottom: true);
		MessageBox.Show("New Bank Vault", "Congratulations! A new Bank Vault has been added to your account.");
	}

	private void LoadInitialData()
	{
		inventoryTabs.Init(null);
		Inventory.inventoryTabs = inventoryTabs;
		Bank.inventoryTabs = inventoryTabs;
		Inventory.Load(0);
		BankCost = Session.MyPlayerData.BankVaultCost;
		BankGroup.Init(Session.MyPlayerData.CurrentBankVault, 1);
		if (Session.MyPlayerData.CurrentBankVault > 0)
		{
			if (!Session.MyPlayerData.LoadedBanks.Contains(1))
			{
				BankDataRequest(1);
				return;
			}
			Bank.Load(1);
		}
		Inventory.Refresh();
		Bank.Refresh();
	}

	private void ItemTransferred(int charItemID, int fromBankID, int toBankID, InventoryItem item)
	{
		inventoryTabs.SetSortType(UIInventory.SortType.MostRecentlyAdded);
		if (Inventory.CurrentBank == fromBankID || Inventory.CurrentBank == -1)
		{
			Inventory.DeselectAll();
			Bank.SetSelectedInventoryItem(item);
			Inventory.Refresh(shouldReset: false);
			Bank.Refresh();
		}
		else
		{
			Bank.DeselectAll();
			Inventory.SetSelectedInventoryItem(item);
			Inventory.Refresh();
			Bank.Refresh(shouldReset: false);
		}
	}

	public static void Show()
	{
		if (Instance == null)
		{
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("BankSystem"), UIManager.Instance.transform);
			obj.name = "BankSystem";
			Instance = obj.GetComponent<UIBankManager>();
			Instance.Init();
		}
	}

	private void ChangeBank(int bankid)
	{
		if (Session.MyPlayerData.LoadedBanks.Contains(bankid))
		{
			Bank.Load(bankid);
		}
		else
		{
			BankDataRequest(bankid);
		}
	}

	public void GetAllBankData()
	{
		List<int> list = new List<int>();
		for (int i = 1; i <= Session.MyPlayerData.CurrentBankVault; i++)
		{
			if (!Session.MyPlayerData.LoadedBanks.Contains(i))
			{
				list.Add(i);
			}
		}
		AllBanksDataRequest(list);
	}

	public void BankToInv()
	{
		InventoryItem selectedItem = Bank.selectedItem;
		if (selectedItem != null)
		{
			TransferItem(selectedItem.BankID, 0, selectedItem);
		}
	}

	public void InvToBank()
	{
		InventoryItem selectedItem = Inventory.selectedItem;
		if (selectedItem != null)
		{
			TransferItem(0, Bank.CurrentBank, selectedItem);
		}
	}

	private void TransferItem(int fromID, int toID, InventoryItem item)
	{
		if (item.IsEquipped)
		{
			Notification.ShowText("Item is currently equipped");
			return;
		}
		if (toID == -1)
		{
			Notification.ShowText("Cannot transfer into bank while viewing all vaults");
			return;
		}
		int num = Session.MyPlayerData.allItems[toID].Where((InventoryItem x) => x.ID == item.ID && item.MaxStack > 1).ToArray().Sum((InventoryItem x) => item.MaxStack - x.Qty);
		bool flag = Session.MyPlayerData.CountItemsInBank(toID) >= ((toID == 0) ? Session.MyPlayerData.BagSlots : Session.MyPlayerData.BankSlots);
		InventoryItem inventoryItem = Session.MyPlayerData.allItems[toID].Find((InventoryItem x) => x.ID == item.ID && item.MaxStack > 1);
		if (num == 0 && flag)
		{
			Notification.ShowText((toID == 0) ? "Inventory is full" : "Bank Vault is full");
		}
		else if (inventoryItem == null && toID > 0 && Session.MyPlayerData.allItems[toID].Count >= Session.MyPlayerData.BankSlots)
		{
			Notification.ShowText("Bank Vault is full");
		}
		else if (item.IsNontransferable)
		{
			Notification.ShowText("Item is non-transferable");
		}
		else
		{
			AEC.getInstance().sendRequest(new RequestItemTransfer(item.CharItemID, fromID, toID));
		}
	}

	public void PurchaseAdditionalSlot()
	{
		if (Session.MyPlayerData.MC >= BankCost)
		{
			ConfirmationSpend.Show("Buy Bank Vault", "Would you like to purchase another bank vault?", "gems", BankCost, "Buy", delegate(bool confirm)
			{
				if (confirm)
				{
					AEC.getInstance().sendRequest(new RequestBankPurchase());
				}
			});
			return;
		}
		Confirmation.Show("Buy Bank Vault", "Purchase another bank vault for " + BankCost + " Dragon Crystals? You do not have enough Dragon Crystals, would you like to get more now?", "Yes", "Cancel", delegate(bool confirm)
		{
			if (confirm)
			{
				UIIAPStore.Show();
			}
		});
	}
}
