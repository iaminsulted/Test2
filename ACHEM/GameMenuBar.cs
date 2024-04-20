using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameMenuBar : MonoBehaviour
{
	public UIButton btnMenu;

	public UIButton btnInventory;

	public UIButton btnFriendRequests;

	public UIButton btnTravel;

	public UIButton btnLoot;

	public UIButton btnStarterPack;

	public UIButton btnNewStats;

	public UIButton btnAdventures;

	public UIButton btnShop;

	public UIButton btnDev;

	public GameObject menuNotif;

	public bool anyNotifs;

	private UICraftingNotification craftNotif;

	private UITitleNotification titleNotif;

	private UIInventoryNotification inventoryNotif;

	private UIFriendsListNotification friendsNotif;

	private UIDailyTaskNotification dailyTaskNotif;

	private DailyChestNotificationIcon dailyChestNotif;

	private const int shopApopId = 5646;

	public List<NPCIA> Apops = new List<NPCIA>();

	public List<int> ApopIDs = new List<int> { 2872, 2865 };

	public List<int> ApopIDs2 = new List<int> { 5646 };

	public void OnEnable()
	{
		Session.MyPlayerData.FriendRequestReceived += OnFriendRequestReceived;
		Session.MyPlayerData.FriendRequestUpdated += OnFriendRequestUpdated;
		Session.MyPlayerData.StarterPackUpdated += OnStarterPackUpdated;
		Session.MyPlayerData.DataSynced += OnDataSynced;
		Session.MyPlayerData.DevModeToggled += OnDevModeToggled;
		Entities.Instance.me.LevelUpdated += OnLevelUpdated;
		LootBags.BagAdded += OnBagCountChanged;
		LootBags.BagRemoved += OnBagCountChanged;
		LootBags.Cleared += OnBagCountCleared;
		SettingsManager.DevBtnAlwaysShowUpdated += OnDevBtnAlwaysShowUpdated;
	}

	public void OnDisable()
	{
		Session.MyPlayerData.FriendRequestReceived -= OnFriendRequestReceived;
		Session.MyPlayerData.FriendRequestUpdated -= OnFriendRequestUpdated;
		Session.MyPlayerData.StarterPackUpdated -= OnStarterPackUpdated;
		Session.MyPlayerData.DataSynced -= OnDataSynced;
		Entities.Instance.me.LevelUpdated -= OnLevelUpdated;
		LootBags.BagAdded -= OnBagCountChanged;
		LootBags.BagRemoved -= OnBagCountChanged;
		LootBags.Cleared -= OnBagCountCleared;
		SettingsManager.DevBtnAlwaysShowUpdated -= OnDevBtnAlwaysShowUpdated;
	}

	public void Start()
	{
		UIEventListener uIEventListener = UIEventListener.Get(btnMenu.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnMenuClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnInventory.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnInventoryClicked));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnFriendRequests.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnFriendsClicked));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnTravel.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnBtnTravel));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnLoot.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnBtnLoot));
		UIEventListener uIEventListener6 = UIEventListener.Get(btnStarterPack.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnStarterPackClick));
		UIEventListener uIEventListener7 = UIEventListener.Get(btnAdventures.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnAdventureClick));
		UIEventListener uIEventListener8 = UIEventListener.Get(btnShop.gameObject);
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener8.onClick, new UIEventListener.VoidDelegate(OnShopClick));
		if (btnDev != null)
		{
			UIEventListener uIEventListener9 = UIEventListener.Get(btnDev.gameObject);
			uIEventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener9.onClick, new UIEventListener.VoidDelegate(OnDevClick));
		}
		craftNotif = menuNotif.GetComponent<UICraftingNotification>();
		titleNotif = menuNotif.GetComponent<UITitleNotification>();
		inventoryNotif = menuNotif.GetComponent<UIInventoryNotification>();
		friendsNotif = menuNotif.GetComponent<UIFriendsListNotification>();
		dailyTaskNotif = menuNotif.GetComponent<UIDailyTaskNotification>();
		dailyChestNotif = menuNotif.GetComponent<DailyChestNotificationIcon>();
		btnFriendRequests.gameObject.SetActive(value: false);
		btnTravel.gameObject.SetActive(Entities.Instance.me.Level >= 5);
		btnMenu.GetComponentInChildren<DailyChestNotificationIcon>(includeInactive: true).gameObject.SetActive(Entities.Instance.me.Level >= 3);
		UpdateLootButton();
		UpdateStarterPackButton();
		UpdateDevButton();
		InvokeRepeating("CheckMenuNotif", 0f, 2f);
		ApopDownloader.GetApops(ApopIDs, null);
		ApopDownloader.GetApops(ApopIDs2, null);
	}

	private void Awake()
	{
		ApopDownloader.GetApops(ApopIDs, null);
		ApopDownloader.GetApops(ApopIDs2, null);
	}

	private void OnDevBtnAlwaysShowUpdated(bool alwaysShow)
	{
		UpdateDevButton();
	}

	private void OnStarterPackUpdated()
	{
		UpdateStarterPackButton();
	}

	private void OnDataSynced()
	{
		UpdateStarterPackButton();
	}

	private void UpdateStarterPackButton()
	{
		ProductOffer productOffer = null;
		if (Session.MyPlayerData.ProductOffers != null)
		{
			productOffer = Session.MyPlayerData.ProductOffers.Values.FirstOrDefault((ProductOffer p) => p.ProductID == ProductID.ADVENTURERS_PACK && p.ExpireUTC > GameTime.ServerTime && !Session.MyPlayerData.OwnsProduct(Products.ProductPackages[p.ProductID]));
		}
		btnStarterPack.gameObject.SetActive(productOffer != null);
		if (productOffer != null)
		{
			Invoke("UpdateStarterPackButton", (float)(productOffer.ExpireUTC - GameTime.ServerTime).TotalSeconds);
		}
	}

	private void OnLevelUpdated()
	{
		btnTravel.gameObject.SetActive(Entities.Instance.me.Level >= 3);
		btnMenu.GetComponentInChildren<DailyChestNotificationIcon>(includeInactive: true).gameObject.SetActive(Entities.Instance.me.Level >= 3);
	}

	public void OnBagCountChanged(int lootID)
	{
		UpdateLootButton();
	}

	private void OnBagCountCleared()
	{
		UpdateLootButton();
	}

	private void OnDevModeToggled()
	{
		UpdateDevButton();
	}

	private void UpdateDevButton()
	{
		if (btnDev != null)
		{
			if ((Session.MyPlayerData.devMode || (bool)SettingsManager.DevBtnAlwaysShow) && Session.MyPlayerData.AccessLevel >= 100)
			{
				btnDev.gameObject.SetActive(value: true);
			}
			else
			{
				btnDev.gameObject.SetActive(value: false);
			}
		}
	}

	private void UpdateLootButton()
	{
		btnLoot.gameObject.SetActive(LootBags.Count() > 0);
	}

	private void OnDevClick(GameObject go)
	{
		int result;
		if (UIWindow.CurrentWindow is UINPCDialog)
		{
			UIWindow.CurrentWindow.Close();
		}
		else if (Session.MyPlayerData.AccessLevel >= 100 && int.TryParse(SettingsManager.DevBtnApopID, out result))
		{
			ApopViewer.Show(result);
		}
	}

	protected void OnFriendsClicked(GameObject go)
	{
		UIFriendRequests.Show();
		btnFriendRequests.gameObject.SetActive(value: false);
	}

	protected void OnBtnTravel(GameObject go)
	{
		UIRegions.Toggle();
	}

	protected void OnBtnLoot(GameObject go)
	{
		UILoot.LoadAllNear();
	}

	protected void OnInventoryClicked(GameObject go)
	{
		if (UIWindow.CurrentWindow is UIInventory)
		{
			UIWindow.CurrentWindow.Close();
			return;
		}
		UIWindow.ClearWindows();
		UIInventory.Load();
	}

	protected void OnMenuClick(GameObject go)
	{
		if (UIWindow.CurrentWindow is UIMainMenu)
		{
			UIWindow.CurrentWindow.Close();
			return;
		}
		UIWindow.ClearWindows();
		UIMainMenu.Load();
	}

	private void OnStarterPackClick(GameObject go)
	{
		UIIAPStore.Show();
	}

	public void ShowStarterPackOffer()
	{
		if (btnStarterPack.gameObject.activeSelf)
		{
			btnStarterPack.GetComponent<CTAImageWindow>().Execute();
		}
	}

	private void OnAdventureClick(GameObject go)
	{
		ApopDownloader.GetApops(ApopIDs, LoadAdventureDialog);
		ApopDownloader.GetApops(ApopIDs2, null);
	}

	private void LoadAdventureDialog(List<NPCIA> loadedApops)
	{
		if (Entities.Instance.me.Level < 3)
		{
			MessageBox.Show("Adventures", "The book of adventures is available starting at Level 3!");
			return;
		}
		if (UIWindow.CurrentWindow is UINPCDialog)
		{
			UIWindow.CurrentWindow.Close();
			return;
		}
		UIWindow.ClearWindows();
		NPCIA apop = ApopMap.GetApop(2872);
		if (apop != null)
		{
			UINPCDialog.Load(new List<NPCIA> { apop }, apop.Title, null, null, clearWindows: false);
		}
		else
		{
			Debug.LogError("Adventure apop not found in ApopMap!");
		}
	}

	private void OnShopClick(GameObject go)
	{
		ApopDownloader.GetApops(ApopIDs, null);
		ApopDownloader.GetApops(ApopIDs2, LoadShopDialog);
	}

	private void LoadShopDialog(List<NPCIA> loadedApops)
	{
		if (UIWindow.CurrentWindow is UINPCDialog)
		{
			UIWindow.CurrentWindow.Close();
			return;
		}
		UIWindow.ClearWindows();
		NPCIA apop = ApopMap.GetApop(5646);
		if (apop != null)
		{
			UINPCDialog.Load(new List<NPCIA> { apop }, apop.Title, null, null, clearWindows: false);
		}
		else
		{
			Debug.LogError("Shop NPCIA not found in apop map!");
		}
	}

	public void Close()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	protected void OnFriendRequestReceived()
	{
		btnFriendRequests.gameObject.SetActive(value: true);
	}

	protected void OnFriendRequestUpdated()
	{
		btnFriendRequests.gameObject.SetActive(value: false);
	}

	public void ShowDelayedStatsButton()
	{
		btnNewStats.gameObject.SetActive(value: true);
		btnNewStats.transform.GetChild(0).gameObject.SetActive(value: true);
	}

	public void ShowNewStats()
	{
		UnityEngine.Object.Instantiate(Resources.Load("UIElements/New Stats") as GameObject, UIManager.Instance.transform);
		btnNewStats.gameObject.SetActive(value: false);
		UIWindow.ClearWindows();
	}

	public void MakeNewStatsButtonVisible()
	{
		btnNewStats.GetComponent<UISprite>().enabled = true;
	}

	public bool CheckIndividualNotif(GameObject g)
	{
		if (g.GetComponent<UINotificationIcon>() != null)
		{
			return g.GetComponent<UINotificationIcon>().ShouldIBeOn();
		}
		return false;
	}

	public void CheckMenuNotif()
	{
		anyNotifs = false;
		if (craftNotif.ShouldIBeOn() || titleNotif.ShouldIBeOn() || friendsNotif.ShouldIBeOn() || dailyTaskNotif.ShouldIBeOn() || dailyChestNotif.CheckDailyTimer())
		{
			anyNotifs = true;
		}
		menuNotif.SetActive(anyNotifs);
	}
}
