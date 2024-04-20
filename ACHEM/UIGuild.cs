using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.Scripts.Game;
using StatCurves;
using UnityEngine;

public class UIGuild : UIMenuWindow
{
	private enum PageState
	{
		main,
		leaderboard,
		shop
	}

	public UIButton btnSummon;

	public UIButton btnCreateGuild;

	public UIButton btnGuildInfo;

	public UIButton btnGuildUpdateInfo;

	public UIButton btnLeave;

	public UIButton btnGuildNameChange;

	public UIButton btnGuildTagChange;

	public UIButton btnMOTDChange;

	public UIButton btnGoToGuildPowerStore;

	public UIButton btnMoreInfo;

	public UIButton btnGoToGuildLeaderboards;

	public UIInput guildName;

	public UIInput guildTag;

	public UILabel ServerStatus;

	public UILabel guildHeaderName;

	public UILabel guildShopHeaderName;

	public UILabel guildLeaderboardHeaderName;

	public UILabel guildInfo;

	public UILabel guildInfoName;

	public UILabel guildLevelLabel;

	public UILabel guildXpProgressLabel;

	public UISlider guildXpProgressSlider;

	public UILabel guildTaxRateLabel;

	public UILabel powerShopGuildGoldLabel;

	public UILabel infoScreenGoldLabel;

	public UIInput newGuildName;

	public UIInput newGuildTag;

	public UILabel updateGuildPageGuildName;

	public UIInput motd;

	public UISprite motdTextBox;

	public static UIGuild instance;

	public UIGuildMemberDetail detail;

	public GameObject BuyConfirmationWindow;

	public GameObject BuyConfirmationArmorIcon;

	public GameObject BuyConfirmationXpIcon;

	public GameObject BuyConfirmationGoldIcon;

	public GameObject BuyConfirmationAttackIcon;

	public static UIGuildShopItem.GuildPowerCategory ClickedItemPowerCategory;

	public UILabel buyConfirmationItemNameLabel;

	public UILabel buyConfirmationDescriptionLabel;

	public UILabel buyConfirmationCostLabel;

	public GameObject NotEnoughGoldWindow;

	public GameObject EditTaxWindow;

	public UIInput EditTaxInput;

	public GameObject EditTaxButton;

	public GameObject itemGOprefab;

	private Transform container;

	private List<UIInventoryGuildMember> itemGOs;

	public GameObject shopItemCategoryPrefab;

	public GameObject shopItemPrefab;

	private Transform shopItemContainer;

	private List<GameObject> shopItemGameObjects = new List<GameObject>();

	private List<UIGuildShopItem> shopListItems = new List<UIGuildShopItem>();

	public GameObject leaderboardItemPrefab;

	public GameObject thisGuildLeaderboardItem;

	private Transform leaderboardItemContainer;

	private List<GameObject> leaderboardItemGameObjects = new List<GameObject>();

	private List<UIGuildLeaderboardItem> leaderboardListItems = new List<UIGuildLeaderboardItem>();

	private ObjectPool<GameObject> itemGOpool;

	public UIGrid grid;

	public UITable shopTable;

	public UIGrid leaderboardGrid;

	private UIInventoryGuildMember selected;

	public GameObject CreateGuildContainer;

	public GameObject GuildContainer;

	public GameObject GuildInfoContainer;

	public GameObject GuildUpdateInfoContainer;

	public UILabel LeaveText;

	public GameObject MainPage;

	public GameObject PowerShopPage;

	public GameObject LeaderboardPage;

	public GameObject TaxLabelSprite;

	public static int tsLastGuildReload;

	public UILabel MotdCharacterCount;

	private Item itemToBePurchased;

	private DateTime tsLastLeaderboardRefresh = DateTime.MinValue;

	private PageState pageState;

	private bool taxRateChanged;

	public void UpdateMotdCharacterCount()
	{
		MotdCharacterCount.text = motd.text.Length + "/1024";
	}

	public static void Toggle()
	{
		if (instance == null)
		{
			Show();
		}
		else
		{
			instance.Close();
		}
	}

	public static void Show()
	{
		if (CheckAccess() && instance == null)
		{
			instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/UIGuild"), UIManager.Instance.transform).GetComponent<UIGuild>();
			instance.Init();
		}
	}

	private static bool CheckAccess()
	{
		if (Session.MyPlayerData.IsInGuild)
		{
			return true;
		}
		bool flag = Session.MyPlayerData.GetGameParam("Guilds") == "Guardian" && !Session.MyPlayerData.IsGuardian();
		bool flag2 = Entities.Instance.me.Level < 10;
		if (!flag && !flag2)
		{
			return true;
		}
		if (flag2 && flag)
		{
			MessageBox.Show("Create Guild", "You must reach Level " + 10 + " and become a Guardian to create a Guild. You may join a guild at any time.");
		}
		else if (flag2)
		{
			MessageBox.Show("Create Guild", "You must reach Level " + 10 + " to create a Guild. You may join a guild at any time.");
		}
		else
		{
			Confirmation.Show("Guardian Required", "You must be a Guardian to create a Guild. Go to the shop?", delegate(bool confirm)
			{
				if (confirm)
				{
					UIIAPStore.Show();
				}
			});
		}
		return false;
	}

	private void UpdateLevelAndXpUI()
	{
		Guild guild = Session.MyPlayerData.Guild;
		if (guild == null)
		{
			return;
		}
		guild.XpForCurrentLevel = Levels.GetXPToGuildLevel(guild.Level - 1, Session.MyPlayerData.LevelCap, guild.guildMembers.Count);
		guild.XpToNextLevel = Levels.GetXPToGuildLevel(guild.Level, Session.MyPlayerData.LevelCap, guild.guildMembers.Count);
		long num = guild.TotalXP - guild.XpForCurrentLevel;
		long num2 = guild.XpToNextLevel - guild.XpForCurrentLevel;
		if (num >= 0)
		{
			guildLevelLabel.text = "Guild lvl. " + guild.Level;
			if (guild.Level >= Session.MyPlayerData.LevelCap)
			{
				guildXpProgressLabel.text = "XP: MAX";
				guildXpProgressSlider.value = 1f;
			}
			else
			{
				guildXpProgressLabel.text = "( " + num.ToString("N0") + " / " + num2.ToString("N0") + " )";
				guildXpProgressSlider.value = (float)num / (float)num2;
			}
			guildTaxRateLabel.text = guild.TaxRate + "%";
			powerShopGuildGoldLabel.text = guild.Gold.ToString("N0");
			infoScreenGoldLabel.text = guild.Gold.ToString("N0");
		}
	}

	protected override void Init()
	{
		base.Init();
		UIEventListener uIEventListener = UIEventListener.Get(btnSummon.gameObject);
		uIEventListener.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener.onClick, new UIEventListener.VoidDelegate(OnSummonClick));
		UIEventListener uIEventListener2 = UIEventListener.Get(btnCreateGuild.gameObject);
		uIEventListener2.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener2.onClick, new UIEventListener.VoidDelegate(OnCreateGuildClick));
		UIEventListener uIEventListener3 = UIEventListener.Get(btnGuildInfo.gameObject);
		uIEventListener3.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener3.onClick, new UIEventListener.VoidDelegate(OnGuildInfoClick));
		UIEventListener uIEventListener4 = UIEventListener.Get(btnGuildUpdateInfo.gameObject);
		uIEventListener4.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener4.onClick, new UIEventListener.VoidDelegate(OnGuildUpdateInfoPanelClick));
		UIEventListener uIEventListener5 = UIEventListener.Get(btnLeave.gameObject);
		uIEventListener5.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener5.onClick, new UIEventListener.VoidDelegate(OnGuildLeaveClick));
		UIEventListener uIEventListener6 = UIEventListener.Get(btnGuildNameChange.gameObject);
		uIEventListener6.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener6.onClick, new UIEventListener.VoidDelegate(OnGuildNameChangeClick));
		UIEventListener uIEventListener7 = UIEventListener.Get(btnGuildTagChange.gameObject);
		uIEventListener7.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener7.onClick, new UIEventListener.VoidDelegate(OnGuildTagChangeClick));
		UIEventListener uIEventListener8 = UIEventListener.Get(btnMOTDChange.gameObject);
		uIEventListener8.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener8.onClick, new UIEventListener.VoidDelegate(UpdateMOTD));
		UIEventListener uIEventListener9 = UIEventListener.Get(btnGoToGuildPowerStore.gameObject);
		uIEventListener9.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener9.onClick, new UIEventListener.VoidDelegate(ShowMemberShop));
		UIEventListener uIEventListener10 = UIEventListener.Get(btnMoreInfo.gameObject);
		uIEventListener10.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener10.onClick, new UIEventListener.VoidDelegate(OnGuildInfoClick));
		UIEventListener uIEventListener11 = UIEventListener.Get(btnGoToGuildLeaderboards.gameObject);
		uIEventListener11.onClick = (UIEventListener.VoidDelegate)Delegate.Combine(uIEventListener11.onClick, new UIEventListener.VoidDelegate(GotoGuildLeaderboards));
		UIEventListener uIEventListener12 = UIEventListener.Get(TaxLabelSprite);
		uIEventListener12.onTooltip = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener12.onTooltip, new UIEventListener.BoolDelegate(TaxLabelOnToolTip));
		UIEventListener uIEventListener13 = UIEventListener.Get(guildXpProgressLabel.gameObject);
		uIEventListener13.onTooltip = (UIEventListener.BoolDelegate)Delegate.Combine(uIEventListener13.onTooltip, new UIEventListener.BoolDelegate(XpProgressLabelOnToolTip));
		container = itemGOprefab.transform.parent;
		shopItemContainer = shopItemPrefab.transform.parent;
		leaderboardItemContainer = leaderboardItemPrefab.transform.parent;
		shopItemPrefab.SetActive(value: false);
		shopItemCategoryPrefab.SetActive(value: false);
		leaderboardItemPrefab.SetActive(value: false);
		itemGOs = new List<UIInventoryGuildMember>();
		itemGOpool = new ObjectPool<GameObject>(itemGOprefab);
		itemGOprefab.SetActive(value: false);
		guildName.onValidate = InputGuildNameValidate;
		guildTag.onValidate = InputGuildTagValidate;
		if (Session.MyPlayerData.IsInGuild)
		{
			btnGuildUpdateInfo.gameObject.SetActive(Session.MyPlayerData.Guild.MyGuildRole == GuildRole.Leader);
			UpdateLevelAndXpUI();
		}
		newGuildName.onValidate = InputGuildNameValidate;
		newGuildTag.onValidate = InputGuildTagValidate;
		detail.gameObject.SetActive(value: false);
		detail.Refresh += refresh;
		CreateGuildContainer.SetActive(!Session.MyPlayerData.IsInGuild);
		GuildContainer.SetActive(Session.MyPlayerData.IsInGuild);
		GuildUpdateInfoContainer.SetActive(value: false);
		PowerShopPage.SetActive(value: false);
		LeaderboardPage.SetActive(value: false);
		BuyConfirmationWindow.SetActive(value: false);
		NotEnoughGoldWindow.SetActive(value: false);
		EditTaxWindow.SetActive(value: false);
		if (PlayerPrefs.HasKey("CurrentServer"))
		{
			string @string = PlayerPrefs.GetString("CurrentServer");
			if (!string.IsNullOrEmpty(@string))
			{
				ServerStatus.text = "Your server - " + @string;
			}
		}
		if (Session.MyPlayerData.IsInGuild)
		{
			Game.Instance.SendGuildUpdateRequest();
			if (Session.MyPlayerData.Guild.MyGuildRole != GuildRole.Leader)
			{
				EditTaxButton.SetActive(value: false);
			}
		}
		refresh();
	}

	private void OnEnable()
	{
		Session.MyPlayerData.GuildsUpdated += UpdateGuild;
		Session.MyPlayerData.GuildNameChanged += GuildNameChanged;
		Session.MyPlayerData.GuildTagChanged += GuildTagChanged;
	}

	private void OnDisable()
	{
		Session.MyPlayerData.GuildsUpdated -= UpdateGuild;
		Session.MyPlayerData.GuildNameChanged -= GuildNameChanged;
		Session.MyPlayerData.GuildTagChanged -= GuildTagChanged;
	}

	public void UpdateGuild(bool _)
	{
		refresh();
	}

	protected override void Resume()
	{
		base.Resume();
		refresh();
	}

	public void refresh()
	{
		CreateGuildContainer.SetActive(!Session.MyPlayerData.IsInGuild);
		MainPage.SetActive(Session.MyPlayerData.IsInGuild && pageState == PageState.main);
		GuildInfoContainer.SetActive(value: false);
		GuildUpdateInfoContainer.SetActive(value: false);
		UpdateLevelAndXpUI();
		UpdateMotdCharacterCount();
		if (!Session.MyPlayerData.IsInGuild)
		{
			return;
		}
		UILabel uILabel = guildHeaderName;
		UILabel uILabel2 = guildShopHeaderName;
		string text2 = (guildLeaderboardHeaderName.text = Session.MyPlayerData.Guild.name);
		string text4 = (uILabel2.text = text2);
		uILabel.text = text4;
		updateGuildPageGuildName.text = Session.MyPlayerData.Guild.name + "\n[" + Session.MyPlayerData.Guild.tag + "]";
		powerShopGuildGoldLabel.text = Session.MyPlayerData.Guild.Gold.ToString("N0");
		infoScreenGoldLabel.text = Session.MyPlayerData.Guild.Gold.ToString("N0");
		MOTDUpdated(Session.MyPlayerData.Guild.MOTD);
		motd.enabled = Session.MyPlayerData.Guild.MyGuildRole == GuildRole.Leader;
		motdTextBox.enabled = Session.MyPlayerData.Guild.MyGuildRole == GuildRole.Leader;
		btnMOTDChange.gameObject.SetActive(Session.MyPlayerData.Guild.MyGuildRole == GuildRole.Leader);
		foreach (UIInventoryGuildMember itemGO in itemGOs)
		{
			itemGO.Clicked -= OnGuildMemberClicked;
			itemGO.gameObject.transform.SetAsLastSibling();
			itemGOpool.Release(itemGO.gameObject);
		}
		itemGOs.Clear();
		foreach (GuildMember item in (from x in Session.MyPlayerData.Guild.guildMembers.Values
			orderby x.ServerID < 0, (int)x.GuildRole descending, x.Name
			select x).ToList())
		{
			GameObject obj = itemGOpool.Get();
			obj.transform.SetParent(container, worldPositionStays: false);
			obj.SetActive(value: true);
			UIInventoryGuildMember component = obj.GetComponent<UIInventoryGuildMember>();
			component.Init(item);
			component.Clicked += OnGuildMemberClicked;
			itemGOs.Add(component);
		}
		grid.Reposition();
		container.parent.GetComponent<UIScrollView>().ResetPosition();
		if (pageState == PageState.leaderboard)
		{
			GotoGuildLeaderboards(null);
		}
	}

	private void OnGuildMemberClicked(UIItem si)
	{
		if (selected != null)
		{
			selected.Selected = false;
		}
		selectFriend(si as UIInventoryGuildMember);
	}

	private void selectFriend(UIInventoryGuildMember guildMember)
	{
		selected = guildMember;
		selected.Selected = true;
		detail.gameObject.SetActive(guildMember.guildMember != Session.MyPlayerData.Guild.Me());
		detail.Init(guildMember.guildMember);
		guildMember.Selected = true;
	}

	public void OnSummonClick(GameObject go)
	{
		UISummonWord.Show();
	}

	public void OnCreateGuildClick(GameObject go)
	{
		ChatFilter chatFilter = new ChatFilter();
		ChatFilteredMessage chatFilteredMessage = chatFilter.ProfanityCheck(guildName.value, shouldCleanSymbols: false);
		ChatFilteredMessage chatFilteredMessage2 = chatFilter.ProfanityCheck(guildTag.value, shouldCleanSymbols: false);
		if (chatFilteredMessage.code > 0)
		{
			MessageBox.Show("Invalid Input", "Guild name is not allowed!");
		}
		else if (guildName.value.Length < 3)
		{
			MessageBox.Show("Invalid Input", "Guild name is too short!");
		}
		else if (guildName.value.Length > 30)
		{
			MessageBox.Show("Invalid Input", "Guild name is too long! (30 characters or less)");
		}
		else if (chatFilteredMessage2.code > 0)
		{
			MessageBox.Show("Invalid Input", "Guild tag is not allowed!");
		}
		else if (guildTag.value.Length < 3)
		{
			MessageBox.Show("Invalid Input", "Guild tag is too short! (Must be 3-5 characters)");
		}
		else if (guildTag.value.Length > 5)
		{
			MessageBox.Show("Invalid Input", "Guild tag is too long! (Must be 3-5 characters)");
		}
		else
		{
			Game.Instance.SendCreateGuildRequest(guildName.value, guildTag.value);
		}
	}

	private void OnGuildInfoClick(GameObject go)
	{
		guildInfoName.text = Session.MyPlayerData.Guild.name;
		GuildMember guildLeader = Session.MyPlayerData.Guild.GetGuildLeader();
		guildInfo.spacingY = 5;
		guildInfo.text = "Created By: " + ((guildLeader != null) ? guildLeader.Name : "") + "\n";
		UILabel uILabel = guildInfo;
		uILabel.text = uILabel.text + "Guild Tag: " + Session.MyPlayerData.Guild.tag + "\n";
		guildInfo.text += $"Guild Members: {Session.MyPlayerData.Guild.guildMembers.Count}/{Session.MyPlayerData.Guild.memberLimit}";
		GuildInfoContainer.SetActive(value: true);
		CreateGuildContainer.SetActive(value: false);
		GuildContainer.SetActive(value: false);
		GuildUpdateInfoContainer.SetActive(value: false);
		bool num = Session.MyPlayerData.Guild.MyGuildRole == GuildRole.Leader;
		if (!num)
		{
			MotdCharacterCount.gameObject.SetActive(value: false);
		}
		bool flag = num && Session.MyPlayerData.Guild.guildMembers.Count == 1;
		LeaveText.text = (flag ? "Disband Guild" : "Leave Guild");
	}

	private void OnGuildLeaveClick(GameObject go)
	{
		if (Session.MyPlayerData.Guild.MyGuildRole == GuildRole.Leader && Session.MyPlayerData.Guild.guildMembers.Count > 1)
		{
			MessageBox.Show("Cannot Leave", "You cannot leave the guild as a Leader. Promote someone else to Leader, or remove all members to disband the guild.");
			return;
		}
		if (Session.MyPlayerData.Guild.MyGuildRole == GuildRole.Leader && Session.MyPlayerData.Guild.guildMembers.Count == 1)
		{
			Confirmation.Show("Delete Guild", "This will permanently delete the guild. This cannot be undone. Are you sure?", delegate(bool b)
			{
				if (b)
				{
					Game.Instance.SendLeaveGuildRequest();
					Session.MyPlayerData.Guild = null;
					Close();
				}
			});
			return;
		}
		Confirmation.Show("Leave Guild", "Are you sure you want to leave the guild?", delegate(bool b)
		{
			if (b)
			{
				Game.Instance.SendLeaveGuildRequest();
				Session.MyPlayerData.Guild = null;
				Close();
			}
		});
	}

	private void OnGuildNameChangeClick(GameObject go)
	{
		if (Session.MyPlayerData.Guild.name == newGuildName.value)
		{
			MessageBox.Show("Guild Name is Invalid", "New name and current name of the guild are the same.");
		}
		else if (new ChatFilter().ProfanityCheck(newGuildName.value, shouldCleanSymbols: false).code > 0)
		{
			MessageBox.Show("Invalid Input", "Guild name is not allowed!");
		}
		else if (newGuildName.value.Length < 3)
		{
			MessageBox.Show("Invalid Input", "Guild name is too short!");
		}
		else if (newGuildName.value.Length > 30)
		{
			MessageBox.Show("Invalid Input", "Guild name is too long! (30 characters or less)");
		}
		else
		{
			Game.Instance.SendGuildChangeNameRequest(newGuildName.value, Session.MyPlayerData.Guild.guildID);
		}
	}

	private void OnGuildTagChangeClick(GameObject go)
	{
		if (Session.MyPlayerData.Guild.tag == newGuildTag.value)
		{
			MessageBox.Show("Guild Tag is Invalid", "New tag and current tag of the guild are the same.");
		}
		else if (new ChatFilter().ProfanityCheck(newGuildTag.value, shouldCleanSymbols: false).code > 0)
		{
			MessageBox.Show("Invalid Input", "Guild tag is not allowed!");
		}
		else if (newGuildTag.value.Length < 3)
		{
			MessageBox.Show("Invalid Input", "Guild tag is too short! (Must be 3-5 characters)");
		}
		else if (newGuildTag.value.Length > 5)
		{
			MessageBox.Show("Invalid Input", "Guild tag is too long! (Must be 3-5 characters)");
		}
		else
		{
			Game.Instance.SendGuildChangeTagRequest(newGuildTag.value, Session.MyPlayerData.Guild.guildID);
		}
	}

	private void OnGuildUpdateInfoPanelClick(GameObject go)
	{
		GuildInfoContainer.SetActive(value: false);
		CreateGuildContainer.SetActive(value: false);
		GuildContainer.SetActive(value: false);
		GuildUpdateInfoContainer.SetActive(value: true);
	}

	public override void OnBackClick(GameObject go)
	{
		BuyConfirmationWindow.SetActive(value: false);
		NotEnoughGoldWindow.SetActive(value: false);
		if (GuildInfoContainer.activeSelf)
		{
			GuildInfoContainer.SetActive(value: false);
			GuildUpdateInfoContainer.SetActive(value: false);
			GuildContainer.SetActive(Session.MyPlayerData.IsInGuild);
			CreateGuildContainer.SetActive(!Session.MyPlayerData.IsInGuild);
		}
		else if (GuildUpdateInfoContainer.activeSelf)
		{
			GuildInfoContainer.SetActive(value: true);
			GuildUpdateInfoContainer.SetActive(value: false);
			GuildContainer.SetActive(value: false);
			CreateGuildContainer.SetActive(value: false);
		}
		else if (PowerShopPage.activeSelf)
		{
			GuildContainer.SetActive(value: true);
			PowerShopPage.SetActive(value: false);
		}
		else if (LeaderboardPage.activeSelf)
		{
			GuildContainer.SetActive(value: true);
			LeaderboardPage.SetActive(value: false);
		}
		else
		{
			base.OnBackClick(go);
		}
	}

	private char InputGuildNameValidate(string text, int charIndex, char addedChar)
	{
		if (Regex.IsMatch(text + addedChar, "^([A-Za-z]+[ '-]?)+$"))
		{
			return addedChar;
		}
		return '\0';
	}

	private char InputGuildTagValidate(string text, int charIndex, char addedChar)
	{
		if (Regex.IsMatch(text + addedChar, "^[A-Za-z]+$"))
		{
			return addedChar;
		}
		return '\0';
	}

	public void GuildNameChanged(string name)
	{
		UILabel uILabel = guildHeaderName;
		string text2 = (guildShopHeaderName.text = name);
		uILabel.text = text2;
		updateGuildPageGuildName.text = name + "\n[" + Session.MyPlayerData.Guild.tag + "]";
		newGuildName.value = "";
		newGuildName.value = "";
		guildInfoName.text = name;
		GuildMember guildLeader = Session.MyPlayerData.Guild.GetGuildLeader();
		guildInfo.text = "Created By: " + ((guildLeader != null) ? guildLeader.Name : "") + "\n";
		UILabel uILabel2 = guildInfo;
		uILabel2.text = uILabel2.text + "Guild Tag: " + Session.MyPlayerData.Guild.tag + "\n";
		guildInfo.text += $"Guild Members: {Session.MyPlayerData.Guild.guildMembers.Count}/{Session.MyPlayerData.Guild.memberLimit}";
	}

	public void GuildTagChanged(string tag)
	{
		updateGuildPageGuildName.text = Session.MyPlayerData.Guild.name + "\n[" + tag + "]";
		newGuildTag.value = "";
		GuildMember guildLeader = Session.MyPlayerData.Guild.GetGuildLeader();
		guildInfo.text = "Created By: " + ((guildLeader != null) ? guildLeader.Name : "") + "\n";
		UILabel uILabel = guildInfo;
		uILabel.text = uILabel.text + "Guild Tag: " + tag + "\n";
		guildInfo.text += $"Guild Members: {Session.MyPlayerData.Guild.guildMembers.Count}/{Session.MyPlayerData.Guild.memberLimit}";
	}

	public void MOTDUpdated(string motd)
	{
		this.motd.value = motd;
	}

	public void UpdateMOTD(GameObject go)
	{
		if (Session.MyPlayerData.Guild.MyGuildRole != GuildRole.Leader)
		{
			return;
		}
		if (new ChatFilter().ProfanityCheck(motd.value, shouldCleanSymbols: false).code > 0)
		{
			MessageBox.Show("Invalid Input", "MOTD is not allowed!");
			return;
		}
		if (motd.value.Length > 200)
		{
			MessageBox.Show("Invalid Input", "Guild MOTD is too long! (200 characters or less)");
		}
		Game.Instance.SendGuildMOTDUpdate(motd.value, Session.MyPlayerData.Guild.guildID);
	}

	public void ShowMainPage(GameObject go)
	{
		pageState = PageState.main;
		MainPage.SetActive(value: true);
		PowerShopPage.SetActive(value: false);
	}

	public void EditTaxButtonClicked()
	{
		EditTaxWindow.SetActive(value: true);
		EditTaxInput.text = Session.MyPlayerData.Guild.TaxRate.ToString();
	}

	public void CloseEditTaxWindow()
	{
		EditTaxWindow.SetActive(value: false);
		if (taxRateChanged)
		{
			RequestUpdateGuildTax r = new RequestUpdateGuildTax(Session.MyPlayerData.Guild.TaxRate, Entities.Instance.me.guildID);
			Game.Instance.aec.sendRequest(r);
		}
	}

	public void TaxInputValueChanged()
	{
		if (int.TryParse(EditTaxInput.value, out var result))
		{
			if (result < 0)
			{
				EditTaxInput.value = "0";
				result = 0;
			}
			if (result > 50)
			{
				EditTaxInput.value = "50";
				result = 50;
			}
			if (result != Session.MyPlayerData.Guild.TaxRate && result >= 0 && result <= 100)
			{
				Session.MyPlayerData.Guild.TaxRate = result;
				guildTaxRateLabel.text = result + "%";
				taxRateChanged = true;
			}
		}
	}

	public void ShowMemberShop(GameObject go)
	{
		if (Shops.Get(569, ShopType.Shop) != null)
		{
			LoadMemberShop();
			return;
		}
		Game.Instance.SendShopLoadRequest(569);
		Game.Instance.ShopLoaded += ShopDataLoaded;
	}

	public void ShopDataLoaded(Shop shop, string message)
	{
		if (shop != null && shop.ID == 569)
		{
			Game.Instance.ShopLoaded -= ShopDataLoaded;
			LoadMemberShop();
		}
	}

	public void LoadMemberShop()
	{
		pageState = PageState.shop;
		MainPage.SetActive(value: false);
		PowerShopPage.SetActive(value: true);
		if (tsLastGuildReload > System.Environment.TickCount || tsLastGuildReload < System.Environment.TickCount - 30000)
		{
			Game.Instance.aec.sendRequest(new RequestGuildInfo(Entities.Instance.me.guildID));
			tsLastGuildReload = System.Environment.TickCount;
		}
		Shop shop = Shops.Get(569, ShopType.Shop);
		if (shop != null && shopItemGameObjects.Count == 0)
		{
			addGuildPowerCategory("XP", shop, UIGuildShopItem.GuildPowerCategory.Xp);
			addGuildPowerCategory("GOLD", shop, UIGuildShopItem.GuildPowerCategory.Gold);
			addGuildPowerCategory("ARMOR", shop, UIGuildShopItem.GuildPowerCategory.Armor);
			addGuildPowerCategory("ATK", shop, UIGuildShopItem.GuildPowerCategory.Attack, "Attack");
		}
		shopTable.Reposition();
		shopItemContainer.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	private void addGuildPowerCategory(string title, Shop powerShop, UIGuildShopItem.GuildPowerCategory guildPowerCategory, string secondarySearchTerm = "")
	{
		GameObject obj = UnityEngine.Object.Instantiate(shopItemCategoryPrefab);
		GuildPowerDropdown component = obj.GetComponent<GuildPowerDropdown>();
		component.lblTitle.text = title;
		obj.transform.SetParent(shopItemContainer, worldPositionStays: false);
		obj.SetActive(value: true);
		addGuildPowerItemToCategory(title, component, powerShop, guildPowerCategory, secondarySearchTerm);
	}

	private void addGuildPowerItemToCategory(string searchString, GuildPowerDropdown guildPowerDropdown, Shop powerShop, UIGuildShopItem.GuildPowerCategory guildPowerCategory, string secondarySearchTerm = "")
	{
		foreach (ShopItem item in powerShop.Items)
		{
			if (item.Name.Contains(searchString, StringComparison.InvariantCultureIgnoreCase) || (secondarySearchTerm.Length > 0 && item.Name.Contains(secondarySearchTerm, StringComparison.InvariantCultureIgnoreCase)))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(shopItemPrefab);
				UIGuildShopItem component = gameObject.GetComponent<UIGuildShopItem>();
				component.Init(item, Session.MyPlayerData.Guild.Level, item.GuildLevel, guildPowerCategory);
				component.uiGuild = this;
				gameObject.transform.SetParent(shopItemContainer, worldPositionStays: false);
				gameObject.SetActive(value: true);
				shopItemGameObjects.Add(gameObject);
				shopListItems.Add(component);
				if (Session.MyPlayerData.Guild.MyGuildRole != GuildRole.Leader && Session.MyPlayerData.Guild.MyGuildRole != GuildRole.Officer)
				{
					component.BuyButtonObject.SetActive(value: false);
					component.Lock.SetActive(value: false);
					component.LevelRequiredLabel.gameObject.SetActive(value: false);
				}
				guildPowerDropdown.Contents.Add(gameObject);
			}
		}
	}

	public void TaxLabelOnToolTip(GameObject go, bool state)
	{
		Tooltip.ShowAtMousePosition("This % of gold gained" + System.Environment.NewLine + "goes towards guild funds.", UIWidget.Pivot.Bottom, Color.black);
	}

	public void XpProgressLabelOnToolTip(GameObject go, bool state)
	{
		Tooltip.ShowAtMousePosition("All XP gained by members contributes" + System.Environment.NewLine + "towards the Guild Level", UIWidget.Pivot.Bottom, Color.black);
	}

	private void clearLeaderBoardObjects()
	{
		foreach (GameObject leaderboardItemGameObject in leaderboardItemGameObjects)
		{
			UnityEngine.Object.Destroy(leaderboardItemGameObject);
		}
		leaderboardItemGameObjects.Clear();
		leaderboardListItems.Clear();
	}

	public void GotoGuildLeaderboards(GameObject go)
	{
		if ((DateTime.UtcNow - tsLastLeaderboardRefresh).TotalMinutes > 10.0)
		{
			Game.Instance.aec.sendRequest(new RequestGuildLeaderboardEntries());
			tsLastLeaderboardRefresh = DateTime.UtcNow;
		}
		if (Guild.GuildLeaderboardEntries == null)
		{
			Debug.LogError("Guild leaderboard entries not received.");
			return;
		}
		pageState = PageState.leaderboard;
		MainPage.SetActive(value: false);
		LeaderboardPage.SetActive(value: true);
		clearLeaderBoardObjects();
		if (leaderboardItemGameObjects.Count == 0)
		{
			UIGuildLeaderboardItem component = thisGuildLeaderboardItem.GetComponent<UIGuildLeaderboardItem>();
			Guild guild = Session.MyPlayerData.Guild;
			component.Init(new GuildLeaderboardEntry
			{
				Name = guild.name,
				XP = guild.MonthlyXP
			}, 0);
			int num = 0;
			foreach (GuildLeaderboardEntry guildLeaderboardEntry in Guild.GuildLeaderboardEntries)
			{
				num++;
				GameObject gameObject = UnityEngine.Object.Instantiate(leaderboardItemPrefab);
				UIGuildLeaderboardItem component2 = gameObject.GetComponent<UIGuildLeaderboardItem>();
				component2.Init(guildLeaderboardEntry, num);
				gameObject.transform.SetParent(leaderboardItemContainer, worldPositionStays: false);
				gameObject.SetActive(value: true);
				leaderboardItemGameObjects.Add(gameObject);
				leaderboardListItems.Add(component2);
			}
		}
		StartCoroutine(repositionLeaderboardGridRoutine());
	}

	private IEnumerator repositionLeaderboardGridRoutine()
	{
		leaderboardGrid.Reposition();
		leaderboardItemContainer.parent.GetComponent<UIScrollView>().ResetPosition();
		yield return null;
		leaderboardGrid.Reposition();
		leaderboardItemContainer.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	public void ShowBuyConfirmation(Item item)
	{
		bool flag = false;
		SpellTemplate baseSpell = SpellTemplates.GetBaseSpell(item.SpellID);
		foreach (Effect effect in Entities.Instance.me.effects)
		{
			foreach (SpellAction baseAction in baseSpell.baseActions)
			{
				foreach (SpellEffect spellEffect in baseAction.spellEffects)
				{
					if (spellEffect.effectID == effect.template.ID)
					{
						flag = true;
					}
				}
			}
		}
		itemToBePurchased = item;
		if (Session.MyPlayerData.Guild.Gold < itemToBePurchased.Cost)
		{
			NotEnoughGoldWindow.SetActive(value: true);
			return;
		}
		BuyConfirmationWindow.SetActive(value: true);
		selectBuyConfirmationCategoryIcon();
		if (flag)
		{
			buyConfirmationItemNameLabel.text = "Refresh " + item.Name + "?";
		}
		else
		{
			buyConfirmationItemNameLabel.text = item.Name;
		}
		buyConfirmationDescriptionLabel.text = item.GetDescriptionForGuildPowerConfirmation() + System.Environment.NewLine + System.Environment.NewLine + item.GetDescriptionForGuildPower().Trim();
		buyConfirmationCostLabel.text = item.Cost.ToString();
	}

	private void selectBuyConfirmationCategoryIcon()
	{
		BuyConfirmationArmorIcon.SetActive(value: false);
		BuyConfirmationAttackIcon.SetActive(value: false);
		BuyConfirmationGoldIcon.SetActive(value: false);
		BuyConfirmationXpIcon.SetActive(value: false);
		switch (ClickedItemPowerCategory)
		{
		case UIGuildShopItem.GuildPowerCategory.Armor:
			BuyConfirmationArmorIcon.SetActive(value: true);
			break;
		case UIGuildShopItem.GuildPowerCategory.Xp:
			BuyConfirmationXpIcon.SetActive(value: true);
			break;
		case UIGuildShopItem.GuildPowerCategory.Gold:
			BuyConfirmationGoldIcon.SetActive(value: true);
			break;
		case UIGuildShopItem.GuildPowerCategory.Attack:
			BuyConfirmationAttackIcon.SetActive(value: true);
			break;
		}
	}

	public void CloseBuyConfirmation()
	{
		BuyConfirmationWindow.SetActive(value: false);
	}

	public void CloseNotEnoughGoldWindow()
	{
		NotEnoughGoldWindow.SetActive(value: false);
	}

	public void PurchaseConfirmed()
	{
		CloseBuyConfirmation();
		if (Session.MyPlayerData.Guild.Gold >= itemToBePurchased.Cost)
		{
			powerShopGuildGoldLabel.text = Session.MyPlayerData.Guild.Gold.ToString("N0");
			RequestGuildBuyPower requestGuildBuyPower = new RequestGuildBuyPower();
			requestGuildBuyPower.GuildID = Entities.Instance.me.guildID;
			requestGuildBuyPower.ItemID = itemToBePurchased.ID;
			Game.Instance.aec.sendRequest(requestGuildBuyPower);
		}
	}
}
