using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIMainMenu : UIMenuWindow
{
	private static UIMainMenu instance;

	public UIItem createlogin;

	public UIItem btnAddLoginAQ3D;

	public UIItem news;

	public UIItem inventory;

	public UIItem classes;

	public UIItem questlog;

	public UIItem maps;

	public UIItem merges;

	public UIItem potionstore;

	public UIItem btnFriends;

	public UIItem btnGuild;

	public UIItem btnCharStats;

	public UIItem btnUpgrade;

	public UIItem btnMail;

	public UIItem btnSelfieCam;

	public UIItem btnTreasureChest;

	public UIItem btnSupport;

	public UIItem btnAdventure;

	public UIItem btnDailyTasks;

	public UIItem btnPvp;

	public UIItem settings;

	public UIItem reportBug;

	public UIItem logout;

	public UIItem exit;

	public UIItem btnHousing;

	public UIGrid grid;

	public List<NPCIA> Apops = new List<NPCIA>();

	public List<int> ApopIDs = new List<int> { 2872, 2865 };

	public static void Load()
	{
		if (instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/MainMenu"), UIManager.Instance.transform).GetComponent<UIMainMenu>();
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		inventory.Clicked += OnInventoryClick;
		classes.Clicked += OnClassesClick;
		merges.Clicked += OnMergesClick;
		questlog.Clicked += OnQuestLogClick;
		maps.Clicked += OnMapsClick;
		potionstore.Clicked += OnPotionStoreClick;
		btnFriends.Clicked += OnFriendsClick;
		btnGuild.Clicked += OnGuildClicked;
		btnCharStats.Clicked += OnCharStateClick;
		btnUpgrade.Clicked += OnUpgradeClick;
		createlogin.Clicked += OnLinkGuestClick;
		btnAddLoginAQ3D.Clicked += OnAddLoginAQ3DClick;
		btnSelfieCam.Clicked += OnSelfieCamClick;
		btnTreasureChest.Clicked += OnTreasureChestClicked;
		btnSupport.Clicked += OnSupportClick;
		btnAdventure.Clicked += OnAdventureClick;
		btnDailyTasks.Clicked += OnDailyTasksClick;
		btnPvp.Clicked += OnPvpClick;
		news.Clicked += OnNewsClick;
		settings.Clicked += OnSettingsClick;
		reportBug.Clicked += OnBugClick;
		logout.Clicked += OnLogoutClick;
		exit.Clicked += OnExitClick;
		btnHousing.Clicked += OnHousingClicked;
		btnMail.Clicked += OnMailClick;
		UIAccountCreate.ConvertGuestSuccess = (Action)Delegate.Combine(UIAccountCreate.ConvertGuestSuccess, new Action(OnConvertGuestSuccess));
		UIAccountCreate.AddLoginAQ3DSuccess = (Action)Delegate.Combine(UIAccountCreate.AddLoginAQ3DSuccess, new Action(OnAddLoginAQ3DSuccess));
		news.gameObject.SetActive(UINews.IsAvailable());
		createlogin.gameObject.SetActive(Session.IsGuest);
		if (Platform.IsIOS)
		{
			if (PlayerPrefs.HasKey("JWT"))
			{
				string @string = PlayerPrefs.GetString("JWT");
				btnAddLoginAQ3D.gameObject.SetActive(@string != null && !Session.AppleUserHasAQ3DLogin && !Session.IsGuest);
			}
			else
			{
				btnAddLoginAQ3D.gameObject.SetActive(value: false);
			}
		}
		else
		{
			btnAddLoginAQ3D.gameObject.SetActive(value: false);
		}
		grid.Reposition();
	}

	private void Awake()
	{
		ApopDownloader.GetApops(ApopIDs, Instance_LoadEnd);
	}

	private void OnEnable()
	{
		CheckNotifs(btnFriends);
		CheckNotifs(btnDailyTasks);
		CheckNotifs(merges);
		CheckNotifs(btnCharStats);
	}

	private void OnTreasureChestClicked(UIItem item)
	{
		UIPreviewLoot.LoadShop();
	}

	protected override void Destroy()
	{
		base.Destroy();
		UIAccountCreate.ConvertGuestSuccess = (Action)Delegate.Remove(UIAccountCreate.ConvertGuestSuccess, new Action(OnConvertGuestSuccess));
		UIAccountCreate.AddLoginAQ3DSuccess = (Action)Delegate.Remove(UIAccountCreate.AddLoginAQ3DSuccess, new Action(OnAddLoginAQ3DSuccess));
		instance = null;
	}

	private void OnConvertGuestSuccess()
	{
		Close();
	}

	private void OnAddLoginAQ3DSuccess()
	{
		btnAddLoginAQ3D.gameObject.SetActive(value: false);
		Close();
	}

	private void OnNewsClick(UIItem item)
	{
		UINews.LoadCurrentNews();
	}

	private void OnSelfieCamClick(UIItem item)
	{
		Close();
		UISelfieCam.Load();
	}

	private void OnInventoryClick(UIItem item)
	{
		UIInventory.Load();
	}

	private void OnClassesClick(UIItem item)
	{
		UICharClasses.Load(Session.MyPlayerData.charClasses.FirstOrDefault((CharClass x) => x.bEquip));
	}

	private void OnMergesClick(UIItem item)
	{
		UIMerge.LoadMergeInventory();
	}

	private void OnQuestLogClick(UIItem item)
	{
		UIQuest.ShowLog();
	}

	private void OnPotionStoreClick(UIItem item)
	{
		UIMiniShop.LoadShop(12);
	}

	private void OnSettingsClick(UIItem item)
	{
		UISettings.Show();
	}

	private void OnBugClick(UIItem item)
	{
		Confirmation.OpenUrl(Main.BugReportURL);
	}

	private void OnSupportClick(UIItem item)
	{
		Confirmation.OpenUrl(Main.ContactSupportURL);
	}

	private void OnLogoutClick(UIItem item)
	{
		Game.Instance.Logout();
	}

	private void OnExitClick(UIItem item)
	{
		Confirmation.Show("Exit Game", "The application will close, are you sure?", delegate(bool b)
		{
			if (b)
			{
				Application.Quit();
			}
		});
	}

	private void OnMapsClick(UIItem item)
	{
		UIRegions.Show();
	}

	private void OnFriendsClick(UIItem item)
	{
		UIFriendsList.Show();
	}

	private void OnCharStateClick(UIItem item)
	{
		UICharInfo.Show(Entities.Instance.me);
	}

	private void OnUpgradeClick(UIItem item)
	{
		UIIAPStore.Show();
	}

	private void OnLinkGuestClick(UIItem item)
	{
		UIAccountCreate.Instance.ShowConvertGuest();
	}

	private void OnAddLoginAQ3DClick(UIItem item)
	{
		UIAccountCreate.Instance.ShowAddLoginAQ3D();
	}

	private void OnAdventureClick(UIItem item)
	{
		if (Entities.Instance.me.Level < 3)
		{
			MessageBox.Show("Adventures", "The book of adventures is available starting at Level 3!");
			return;
		}
		UINPCDialog.Load(new List<NPCIA> { ApopMap.GetApop(2872) }, ApopMap.GetApop(2872).Title, null, null, clearWindows: false);
	}

	private void OnGuildClicked(UIItem item)
	{
		UIGuild.Show();
	}

	private void OnDailyTasksClick(UIItem item)
	{
		UIDailyTasks.Show();
	}

	private void OnPvpClick(UIItem item)
	{
		UIPvPMainMenu.Show(Session.MyPlayerData.charClasses.FirstOrDefault((CharClass x) => x.bEquip));
	}

	private void OnHousingClicked(UIItem item)
	{
		UIHouseChooser.Show();
	}

	private void Instance_LoadEnd(List<NPCIA> loadedApops)
	{
		if (loadedApops == null || loadedApops.Count == 0)
		{
			string text = string.Join(",", ApopIDs.Select((int x) => x.ToString()).ToArray());
			Debug.LogError("Fix in admin. Apops not found: " + text);
		}
	}

	private void OnMailClick(UIItem item)
	{
		UIMail.Show();
	}

	public void CheckNotifs(UIItem notifBtn)
	{
		UINotificationIcon component = notifBtn.GetComponent<UINotificationIcon>();
		if (component != null)
		{
			component.ShouldIBeOn();
		}
	}
}
