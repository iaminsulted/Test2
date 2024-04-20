using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Housing;
using StatCurves;
using UnityEngine;

public class MyPlayerData
{
	public const int MAX_FRIENDS = 80;

	public const int MAX_BANK_VAULT = 100;

	public int BankVaultCost;

	public int HouseSlotCost;

	public int HouseSlotMax;

	public int HouseItemMaxPerSlot;

	public int HouseMaxPlayers;

	public int UserID;

	public int ID;

	public int XP;

	public int XPToLevel;

	public int EndGame;

	public int LevelCap;

	public int Gold;

	public int MC;

	public HashSet<int> LoadedBanks = new HashSet<int>();

	public Dictionary<int, List<InventoryItem>> allItems = new Dictionary<int, List<InventoryItem>>();

	public List<Merge> merges;

	public List<CharClass> charClasses;

	public List<CombatClass> combatClassList = new List<CombatClass>();

	public List<AreaData> areaList;

	public List<RegionData> regionList;

	public List<FriendData> friendsList = new List<FriendData>();

	public List<IMessage> friendRequests = new List<IMessage>();

	public List<ReportRecord> recordList = new List<ReportRecord>();

	public int reportCooldownMins = 5;

	public List<int> CurQuests;

	public Dictionary<int, int> CurQuestObjectives;

	public Dictionary<int, QuestString> QuestStrings;

	public Dictionary<int, int> QuestChains;

	public List<int> questAreas = new List<int>();

	public int AccessLevel;

	public Dictionary<string, long> BitFields;

	public Dictionary<TradeSkillType, int> tradeSkillXP;

	public Dictionary<TradeSkillType, int> tradeSkillXPToLevel;

	public string MOTD;

	public int LatencyNotifyThreshold;

	public int LatencyDisconnectThreshold;

	private string Params;

	private Dictionary<string, string> GameParams = new Dictionary<string, string>();

	public List<CharDailyTask> charDailyTasks = new List<CharDailyTask>();

	public List<DailyTask> serverDailyTasks = new List<DailyTask>();

	public DuelResult duelResult;

	public List<Leaderboard> leaderboards = new List<Leaderboard>();

	public PvpPlayerRecords pvpPlayerRecords = new PvpPlayerRecords();

	public bool hasLeft;

	public bool showNotification;

	public bool isPvp;

	public int queueCount;

	public int maxQueueCount;

	public float queueDelayPenalty;

	public bool devMode;

	public float XPMultiplier = 1f;

	public float GoldMultiplier = 1f;

	public float CXPMultiplier = 1f;

	public float DailyQuestMultiplier = 1f;

	public float DailyTaskMultiplier = 1f;

	public Dictionary<string, InventoryTabsRecord> InventoryTabsRecords = new Dictionary<string, InventoryTabsRecord>();

	public List<MailMessage> mailbox = new List<MailMessage>();

	public List<RewardItem> mailrewards = new List<RewardItem>();

	public List<Item> mailRewardsList = new List<Item>();

	public List<HouseData> PersonalHouseData;

	public Dictionary<int, HouseData> PublicHouseData;

	public int PublicHouseDataCount;

	private Guild guild;

	public ResponseGuildInvite guildInvite;

	public int BankVaultCount;

	public int RewardIndex;

	public DateTime RewardDate;

	public Dictionary<ProductID, ProductOffer> ProductOffers = new Dictionary<ProductID, ProductOffer>();

	public int CastMachineID = -1;

	private Quest availableQuest;

	private Quest currentlyTrackedQuest;

	private int currentCrossSkill;

	private Dictionary<CombatSpellSlot, int> currentPvpActions = new Dictionary<CombatSpellSlot, int>();

	public List<InventoryItem> items
	{
		get
		{
			return allItems[0];
		}
		set
		{
			allItems[0] = value;
		}
	}

	public Dictionary<int, int> HouseItemCounts { get; private set; }

	public Guild Guild
	{
		get
		{
			return guild;
		}
		set
		{
			guild = value;
		}
	}

	public bool IsInGuild => Guild != null;

	public int BagSlots
	{
		get
		{
			int num = 90;
			if (CheckBitFlag("iu0", 6))
			{
				num += 40;
			}
			if (CheckBitFlag("iu0", 3))
			{
				num += 10;
			}
			if (AccessLevel >= 50)
			{
				num += 100;
			}
			return num;
		}
	}

	public int BankSlots
	{
		get
		{
			if (CheckBitFlag("iu0", 16))
			{
				return 60;
			}
			if (CheckBitFlag("iu0", 6))
			{
				return 50;
			}
			return 40;
		}
	}

	public int CurrentBankVault
	{
		get
		{
			int num = BankVaultCount;
			if (CheckBitFlag("ic1", 23))
			{
				num++;
			}
			if (CheckBitFlag("ic6", 33))
			{
				num++;
			}
			return num;
		}
	}

	public int AverageItemPower
	{
		get
		{
			float num = 0f;
			foreach (InventoryItem item in items)
			{
				if (item.IsStatEquip)
				{
					num += (float)item.GetCombatPower();
				}
			}
			return Mathf.CeilToInt(num / (float)ItemSlots.TotalPowerSlots);
		}
	}

	public bool IsCastingMachine => CastMachineID > -1;

	public CharClass CurrentClass => charClasses.FirstOrDefault((CharClass x) => x.bEquip);

	public int EquippedClassID => CurrentClass.ClassID;

	public int EquippedClassXP
	{
		get
		{
			return CurrentClass.ClassXP;
		}
		set
		{
			CurrentClass.ClassXP = value;
		}
	}

	public int EquippedClassRank => CurrentClass.ClassRank;

	public int ScaledClassRank
	{
		get
		{
			if (!Game.Instance.AreaData.HasPvp && !Game.Instance.AreaData.IsPvpLobby)
			{
				return EquippedClassRank;
			}
			return 10;
		}
	}

	public int EquippedClassGloryRank => CurrentClass.ClassGloryRank;

	public int TotalClassRank => charClasses.Where((CharClass x) => !x.ToCombatClass().IsSkin).Sum((CharClass x) => x.ClassRank);

	public List<int> UnlockedCrossSkillIDs => (from x in charClasses
		where x.ClassRank >= 10 || Game.Instance.AreaData.HasPvp || Game.Instance.AreaData.IsPvpLobby
		select x.ToCombatClass().Spells.Last()).Distinct().ToList();

	public int GloryLevel
	{
		get
		{
			int num = 0;
			foreach (CharClass charClass in charClasses)
			{
				num += charClass.ClassGloryRank;
			}
			return num;
		}
	}

	public bool HasCurrentlyTrackedQuest => CurrentlyTrackedQuest != null;

	public Quest CurrentlyTrackedQuest
	{
		get
		{
			return currentlyTrackedQuest;
		}
		private set
		{
			if (currentlyTrackedQuest != value)
			{
				currentlyTrackedQuest = value;
				if (this.CurrentlyTrackedQuestUpdated != null)
				{
					this.CurrentlyTrackedQuestUpdated(CurrentlyTrackedQuest);
				}
			}
		}
	}

	public bool HasAvailableQuest => availableQuest != null;

	public Quest AvailableQuest
	{
		get
		{
			return availableQuest;
		}
		set
		{
			if (availableQuest != value)
			{
				availableQuest = value;
				if (this.AvailableQuestUpdated != null)
				{
					this.AvailableQuestUpdated(availableQuest);
				}
			}
		}
	}

	public int CurrentCrossSkill
	{
		get
		{
			return currentCrossSkill;
		}
		set
		{
			if (currentCrossSkill != value)
			{
				currentCrossSkill = value;
				this.CrossSkillEquipped?.Invoke(currentCrossSkill);
			}
		}
	}

	public List<SpellTemplate> MySpells
	{
		get
		{
			Entity me = Entities.Instance.me;
			List<SpellTemplate> list = (from ID in me.CombatClass.Spells
				where ID != me.CombatClass.CrossSkillID
				select SpellTemplates.Get(ID, me.effects, me.ScaledClassRank, me.CombatClass.ID, me.comboState.Get(ID))).ToList();
			list.Add(SpellTemplates.Get(CurrentCrossSkill, me.effects, me.ScaledClassRank, me.CombatClass.ID, me.comboState.Get(CurrentCrossSkill)));
			list.RemoveAll((SpellTemplate skill) => skill == null);
			return list;
		}
	}

	public bool CanAddMoreFriends => friendsList.Count < 80;

	public int ClientPublicHouseDataVersion { get; private set; } = -1;


	public HouseDataCategory ClientPublicHouseDataCategory { get; set; }

	public event Action StarterPackUpdated;

	public event Action DevModeToggled;

	public event Action QuestStateUpdated;

	public event Action AreasReceived;

	public event Action DataSynced;

	public event Action FriendsUpdated;

	public event Action FriendRequestReceived;

	public event Action FriendRequestUpdated;

	public event Action<bool> GuildsUpdated;

	public event Action<string> GuildNameChanged;

	public event Action<string> GuildTagChanged;

	public event Action<string> GuildMOTDUpdated;

	public event Action DailyTaskUpdated;

	public event Action<int, DateTime, int> DailyRewardUpdated;

	public event Action CurrencyUpdated;

	public event Action<int, int> ClassXPUpdated;

	public event Action<int, int> GloryXPUpdated;

	public event Action<int, int> ClassRankUpdated;

	public event Action<int> QuestAdded;

	public event Action<int> QuestRemoved;

	public event Action<int> QuestTurnedIn;

	public event Action<int> ClassAdded;

	public event Action<int> ClassEquipped;

	public event Action<int> BankCountUpdated;

	public event Action<int, int> XPUpdated;

	public event Action<int, int> QuestObjectiveUpdated;

	public event Action<int, int> QuestStringUpdated;

	public event Action<int, int, int, InventoryItem> ItemTransferred;

	public event Action<InventoryItem> ItemAdded;

	public event Action<InventoryItem> ItemRemoved;

	public event Action<InventoryItem> ItemUpdated;

	public event Action<InventoryItem> ItemEquipped;

	public event Action<InventoryItem> ItemUnequipped;

	public event Action InventoryReload;

	public event Action<int, int> TradeSkillXPUpdated;

	public event Action<InventoryItem> InfusionFinished;

	public event Action<ItemModifier> modifierRerolled;

	public event Action<InventoryItem> modifierConfirmed;

	public event Action<Merge> MergeAdded;

	public event Action<Merge> MergeRemoved;

	public event Action<int, List<InventoryItem>> BankLoaded;

	public event Action<Dictionary<int, List<InventoryItem>>> AllBanksLoaded;

	public event Action<string, byte, bool> BitFlagUpdated;

	public event Action<Dictionary<int, HouseData>, HouseDataCategory, bool> PublicHouseDataAdded;

	public event Action<List<HouseData>> PersonalHouseDataAdded;

	public event Action<ComHouseItem> OnHouseItemAdded;

	public event Action<int, int, int> OnHouseItemRemoved;

	public event Action OnHouseItemClearAll;

	public event Action<Quest> CurrentlyTrackedQuestUpdated;

	public event Action<Quest> AvailableQuestUpdated;

	public event Action<int> CrossSkillEquipped;

	public event Action<CombatSpellSlot, int> PvpActionEquipped;

	public event Action<InventoryItem, CombatSpellSlot> UpdateActionItem;

	public void SetAllItems(Dictionary<int, List<InventoryItem>> items)
	{
		allItems = items;
		if (items != null)
		{
			foreach (KeyValuePair<int, List<InventoryItem>> item in items)
			{
				Items.Add(((IEnumerable<InventoryItem>)item.Value).Select((Func<InventoryItem, Item>)((InventoryItem iItem) => iItem)).ToList());
			}
		}
		if (allItems == null)
		{
			allItems = new Dictionary<int, List<InventoryItem>>();
			allItems[0] = new List<InventoryItem>();
		}
	}

	public void SetHouseItemCounts(Dictionary<int, int> hItemCounts)
	{
		if (hItemCounts == null)
		{
			HouseItemCounts = new Dictionary<int, int>();
		}
		else
		{
			HouseItemCounts = hItemCounts;
		}
	}

	public int CountItemsInBank(int bankID = 0)
	{
		return allItems[bankID].Count((InventoryItem p) => p.TakesBagSpace);
	}

	public bool HasBadgeID(int badgeID)
	{
		return HasBadge(Badges.Get(badgeID));
	}

	public bool HasBadge(Badge badge)
	{
		if (badge == null)
		{
			return false;
		}
		return CheckBitFlag(badge.BitFlagName, (byte)badge.BitFlagIndex);
	}

	public bool BitFlagExists(string name, byte index)
	{
		if (!string.IsNullOrEmpty(name))
		{
			return index > 0;
		}
		return false;
	}

	public bool CheckBitFlag(string name, byte index)
	{
		if (!BitFlagExists(name, index))
		{
			return true;
		}
		if (BitFields.TryGetValue(name, out var value))
		{
			return Util.BitGet(value, index);
		}
		return false;
	}

	public void SetBitFlag(string name, byte index, bool value)
	{
		if (BitFlagExists(name, index))
		{
			if (BitFields.TryGetValue(name, out var value2))
			{
				BitFields[name] = (value ? Util.BitSet(value2, index) : Util.BitClear(value2, index));
			}
			if (this.BitFlagUpdated != null)
			{
				this.BitFlagUpdated(name, index, value);
			}
		}
	}

	public bool HasMaxOfUniqueItem(Item item)
	{
		return allItems.Values.Sum((List<InventoryItem> x) => x.Where((InventoryItem y) => y.ID == item.ID).Sum((InventoryItem z) => z.Qty)) >= item.MaxStack;
	}

	public int GetItemCount(int itemID)
	{
		return allItems.Values.Sum((List<InventoryItem> x) => x.Where((InventoryItem y) => y.ID == itemID).Sum((InventoryItem z) => z.Qty));
	}

	public string CanAddItem(Item item)
	{
		if (!HasRoomInInventory(item))
		{
			return "Inventory is full";
		}
		if (item.IsUnique && allItems.Values.Any((List<InventoryItem> x) => x.Any((InventoryItem y) => y.ID == item.ID && y.BankID > 0)))
		{
			return "You have " + item.Name + " in the bank. You cannot have multiple of this item.";
		}
		if (item.IsUnique && (HasMaxOfUniqueItem(item) || GetItemCount(item.ID) + item.Qty > item.MaxStack))
		{
			if (item.MaxStack == 1)
			{
				return "You cannot have any more " + item.Name;
			}
			return "You cannot have any more stacks of " + item.Name;
		}
		return "";
	}

	public bool AreBestItemsEquipped()
	{
		foreach (EquipItemSlot value in Enum.GetValues(typeof(EquipItemSlot)))
		{
			if (ItemSlots.IsPowerSlot(value))
			{
				InventoryItem equippedItem = GetEquippedItem(value);
				InventoryItem bestItemBySlot = GetBestItemBySlot(value);
				if (equippedItem != bestItemBySlot)
				{
					return false;
				}
			}
		}
		return true;
	}

	public InventoryItem GetBestItemBySlot(EquipItemSlot slot)
	{
		bool isWeapon = ItemSlots.IsWeaponSlot(slot);
		return (from i in items
			where i.CanBeEquipped && i.BankID == 0 && ((isWeapon && i.IsWeapon) || i.EquipSlot == slot) && !i.IsCosmetic
			orderby i.GetCombatPower() descending
			select i).FirstOrDefault();
	}

	public bool IsGuardian()
	{
		return CheckBitFlag("iu0", 6);
	}

	public void UpdateMC(int mc)
	{
		if (MC != mc)
		{
			MC = mc;
			OnCurrencyUpdated();
			AudioManager.Play2DSFX("UI_Dragoncrystal_Transaction");
		}
	}

	public void AddGold(int gold)
	{
		Gold += gold;
		OnCurrencyUpdated();
		AudioManager.Play2DSFX("sfx_engine_gold");
	}

	public void UpdateGold(int gold)
	{
		if (Gold != gold)
		{
			Gold = gold;
			OnCurrencyUpdated();
			AudioManager.Play2DSFX("sfx_engine_gold");
		}
	}

	protected void OnCurrencyUpdated()
	{
		if (this.CurrencyUpdated != null)
		{
			this.CurrencyUpdated();
		}
	}

	public void AddXP(int xp)
	{
		XP += xp;
		OnXPUpdated();
	}

	public void UpdateXP(int xp)
	{
		XP = xp;
		OnXPUpdated();
	}

	public void SetXP(int xp, int xptolevel)
	{
		XP = xp;
		XPToLevel = xptolevel;
		OnXPUpdated();
	}

	protected void OnXPUpdated()
	{
		if (this.XPUpdated != null)
		{
			this.XPUpdated(XP, XPToLevel);
		}
	}

	public void AddClassXP(int classID, int classXP)
	{
		if (classXP > 0)
		{
			charClasses.First((CharClass x) => x.ClassID == classID).ClassXP += classXP;
			OnClassXPUpdated(classID);
		}
	}

	public void UpdateClassXP(int classID, int classXP)
	{
		CharClass mClass = charClasses.First((CharClass x) => x.ClassID == classID);
		if (mClass != null && mClass.ClassXP != classXP)
		{
			mClass.ClassXP = classXP;
			OnClassXPUpdated(classID);
		}
		if (mClass.ToCombatClass().IsSkin)
		{
			charClasses.First((CharClass x) => x.ClassID == mClass.ToCombatClass().SkinClassID).ClassXP = classXP;
			return;
		}
		for (int i = 0; i < charClasses.Count; i++)
		{
			if (charClasses[i].ToCombatClass().SkinClassID == classID)
			{
				charClasses[i].ClassXP = classXP;
			}
		}
	}

	public void UpdateGloryXP(int classID, int gloryXP)
	{
		CharClass charClass = charClasses.First((CharClass x) => x.ClassID == classID);
		if (charClass != null)
		{
			charClass.ClassGlory += gloryXP;
			OnGloryXPUpdated(classID, gloryXP);
		}
	}

	public void UpdateTradeSkillXP(TradeSkillType tradeSkill, int xp)
	{
		tradeSkillXP[tradeSkill] = xp;
		OnTradeSkillXPUpdated(tradeSkill);
	}

	public void SetTradeSkillXP(TradeSkillType tradeSkill, int xp, int xpToLevel)
	{
		tradeSkillXPToLevel[tradeSkill] = xpToLevel;
		tradeSkillXP[tradeSkill] = xp;
		OnTradeSkillXPUpdated(tradeSkill);
	}

	public void OnTradeSkillXPUpdated(TradeSkillType tradeSkill)
	{
		if (this.TradeSkillXPUpdated != null)
		{
			this.TradeSkillXPUpdated(tradeSkillXP[tradeSkill], tradeSkillXPToLevel[tradeSkill]);
		}
	}

	public int GetClassXP(int classID)
	{
		return charClasses.First((CharClass x) => x.ClassID == classID).ClassXP;
	}

	public void OnClassXPUpdated(int classID)
	{
		if (this.ClassXPUpdated != null && Session.MyPlayerData.EquippedClassID == classID)
		{
			this.ClassXPUpdated(classID, GetClassXP(classID));
		}
	}

	public void OnGloryXPUpdated(int classID, int gloryXP)
	{
		if (this.GloryXPUpdated != null)
		{
			this.GloryXPUpdated(classID, gloryXP);
		}
	}

	public void OnClassRankUpdated(int classID)
	{
		if (this.ClassRankUpdated != null)
		{
			this.ClassRankUpdated(classID, GetClassRank(classID));
		}
	}

	public bool HasClassUsingWeaponSlot(EquipItemSlot equipSlot)
	{
		if (!ItemSlots.IsWeaponSlot(equipSlot))
		{
			return true;
		}
		return charClasses.Any((CharClass c) => c.ToCombatClass().WeaponRequired == equipSlot);
	}

	public void OnDevModeToggled()
	{
		if (this.DevModeToggled != null)
		{
			this.DevModeToggled();
		}
	}

	protected void OnQuestStateUpdated()
	{
		if (this.QuestStateUpdated != null)
		{
			this.QuestStateUpdated();
		}
	}

	protected void OnItemAdded(InventoryItem iItem)
	{
		if (this.ItemAdded != null)
		{
			this.ItemAdded(iItem);
		}
	}

	protected void OnItemRemoved(InventoryItem iItem)
	{
		if (this.ItemRemoved != null)
		{
			this.ItemRemoved(iItem);
		}
	}

	protected void OnItemUpdated(InventoryItem iItem)
	{
		if (this.ItemUpdated != null)
		{
			this.ItemUpdated(iItem);
		}
	}

	protected void OnItemEquipped(InventoryItem iItem)
	{
		if (this.ItemEquipped != null)
		{
			this.ItemEquipped(iItem);
		}
	}

	protected void OnItemUnequipped(InventoryItem iItem)
	{
		if (this.ItemUnequipped != null)
		{
			this.ItemUnequipped(iItem);
		}
	}

	public InventoryItem GetItem(int CharItemID)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].CharItemID == CharItemID)
			{
				return items[i];
			}
		}
		return null;
	}

	public int GetItemIndex(int CharItemID)
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i].CharItemID == CharItemID)
			{
				return i;
			}
		}
		return -1;
	}

	public int GetInventoryItemCount(int itemID)
	{
		int num = 0;
		foreach (InventoryItem item in allItems[0])
		{
			if (item.ID == itemID)
			{
				num += item.Qty;
			}
		}
		return num;
	}

	public string GetComparisonStatText(Item item)
	{
		if (!item.HasStats)
		{
			return "";
		}
		string text = "";
		InventoryItem inventoryItem = GetEquippedItem(item.EquipSlot) ?? new InventoryItem();
		if (item.DisplayMaxHealth > 0f)
		{
			text = text + GetStatComparison(inventoryItem.DisplayMaxHealth, item.DisplayMaxHealth, Stat.Health) + "\n";
		}
		if (item.DisplayAttack > 0f)
		{
			text = text + GetStatComparison(inventoryItem.DisplayAttack, item.DisplayAttack, Stat.Attack) + "\n";
		}
		if (item.DisplayArmor > 0f)
		{
			text = text + GetStatComparison(inventoryItem.DisplayArmor, item.DisplayArmor, Stat.Armor) + "\n";
		}
		if (item.DisplayEvasion > 0f)
		{
			text = text + GetStatComparison(inventoryItem.DisplayEvasion, item.DisplayEvasion, Stat.Evasion) + "\n";
		}
		if (item.DisplayCrit > 0f)
		{
			text = text + GetStatComparison(inventoryItem.DisplayCrit, item.DisplayCrit, Stat.Crit) + "\n";
		}
		if (item.DisplayHaste > 0f)
		{
			text = text + GetStatComparison(inventoryItem.DisplayHaste, item.DisplayHaste, Stat.Haste) + "\n";
		}
		return text.Trim('\n');
	}

	public string GetComparisonStatText(Item item, Item initialItem)
	{
		if (!item.HasStats)
		{
			return "";
		}
		string text = "";
		if (item.DisplayMaxHealth > 0f)
		{
			text = text + GetStatComparison(initialItem.DisplayMaxHealth, item.DisplayMaxHealth, Stat.Health) + "\n";
		}
		if (item.DisplayAttack > 0f)
		{
			text = text + GetStatComparison(initialItem.DisplayAttack, item.DisplayAttack, Stat.Attack) + "\n";
		}
		if (item.DisplayArmor > 0f)
		{
			text = text + GetStatComparison(initialItem.DisplayArmor, item.DisplayArmor, Stat.Armor) + "\n";
		}
		if (item.DisplayEvasion > 0f)
		{
			text = text + GetStatComparison(initialItem.DisplayEvasion, item.DisplayEvasion, Stat.Evasion) + "\n";
		}
		if (item.DisplayCrit > 0f)
		{
			text = text + GetStatComparison(initialItem.DisplayCrit, item.DisplayCrit, Stat.Crit) + "\n";
		}
		if (item.DisplayHaste > 0f)
		{
			text = text + GetStatComparison(initialItem.DisplayHaste, item.DisplayHaste, Stat.Haste) + "\n";
		}
		return text.Trim('\n');
	}

	public static string GetStatComparison(float equippedValue, float compareValue, Stat displayStat)
	{
		string text = "000000";
		string text2 = "";
		float num = compareValue - equippedValue;
		if (equippedValue < compareValue)
		{
			text = "005900";
			text2 = " +" + num;
		}
		else if (equippedValue > compareValue)
		{
			text = "ad0000";
			text2 = " " + num;
		}
		else
		{
			text = "5e5e5e";
			text2 = " +0";
		}
		return "[000000]" + compareValue + " " + displayStat.ToString() + "[" + text + "]" + text2 + "[-]";
	}

	public InventoryItem GetEquippedItem(EquipItemSlot slot)
	{
		return items.FirstOrDefault((InventoryItem i) => ((ItemSlots.IsWeaponSlot(slot) && i.IsWeapon) || i.EquipSlot == slot) && i.IsStatEquip && i.BankID == 0);
	}

	public InventoryItem GetCosmeticItem(EquipItemSlot slot)
	{
		return items.FirstOrDefault((InventoryItem i) => i.EquipSlot == slot && i.IsCosmeticEquip && i.BankID == 0);
	}

	public void EquipItem(int CharItemID, InventoryItem.Equip equipID)
	{
		InventoryItem item = GetItem(CharItemID);
		if (item == null)
		{
			return;
		}
		EquipItemSlot weaponRequired = Entities.Instance.me.baseAsset.WeaponRequired;
		bool flag = equipID == InventoryItem.Equip.Stat && item.IsWeapon && !item.IsTool && item.EquipSlot != weaponRequired && GetCosmeticItem(weaponRequired) == null;
		foreach (InventoryItem item2 in items)
		{
			if (flag && item2.IsWeapon && item2.EquipID == InventoryItem.Equip.Stat && item2.EquipSlot == weaponRequired)
			{
				item2.EquipID = InventoryItem.Equip.Cosmetic;
				OnItemEquipped(item2);
			}
			else if (item2.EquipID == equipID && ((equipID == InventoryItem.Equip.Stat && item.IsWeapon && item2.IsWeapon) || item2.EquipSlot == item.EquipSlot))
			{
				item2.EquipID = InventoryItem.Equip.None;
				OnItemUnequipped(item2);
			}
		}
		AudioManager.Play2DSFX("UI_Equip_Gear");
		item.EquipID = equipID;
		OnItemEquipped(item);
	}

	public void UnequipItem(int charItemID)
	{
		InventoryItem item = GetItem(charItemID);
		if (item != null)
		{
			item.EquipID = InventoryItem.Equip.None;
			OnItemUnequipped(item);
			AudioManager.Play2DSFX("UI_Equip_Gear");
		}
	}

	public void AddItem(InventoryItem inventoryItem)
	{
		items.Add(inventoryItem);
		UpdateQuestObjective(inventoryItem);
		OnItemAdded(inventoryItem);
	}

	public void RemoveItem(int charItemID, int bankID)
	{
		InventoryItem item = GetItem(charItemID);
		if (item != null)
		{
			items.Remove(item);
			OnItemRemoved(item);
			UpdateQuestObjective(item);
		}
		else if (LoadedBanks.Contains(bankID))
		{
			InventoryItem inventoryItem = allItems[bankID].Find((InventoryItem p) => p.CharItemID == charItemID);
			if (inventoryItem != null)
			{
				allItems[bankID].Remove(inventoryItem);
				OnItemRemoved(inventoryItem);
			}
		}
	}

	public void UpdateItem(int charItemID, int quantity, int bankID)
	{
		if (!LoadedBanks.Contains(bankID))
		{
			return;
		}
		InventoryItem inventoryItem = allItems[bankID].Find((InventoryItem p) => p.CharItemID == charItemID);
		if (inventoryItem != null)
		{
			inventoryItem.Qty = quantity;
			OnItemUpdated(inventoryItem);
			UpdateQuestObjective(inventoryItem);
			if (inventoryItem.Qty <= 0)
			{
				RemoveItem(charItemID, bankID);
			}
		}
	}

	public void InfuseConfirmation(InventoryItem charItem)
	{
		if (charItem != null)
		{
			items[GetItemIndex(charItem.CharItemID)] = charItem;
		}
		this.InfusionFinished?.Invoke(charItem);
	}

	public void ExtractConfirmation(InventoryItem charItem)
	{
		if (charItem != null)
		{
			items[GetItemIndex(charItem.CharItemID)] = charItem;
		}
	}

	public void ItemRerollResponse(ItemModifier modifier)
	{
		this.modifierRerolled?.Invoke(modifier);
	}

	public void ItemModifierRerollConfirmation(InventoryItem charItem)
	{
		if (charItem != null)
		{
			items[GetItemIndex(charItem.CharItemID)] = charItem;
			this.modifierConfirmed?.Invoke(charItem);
		}
	}

	public void SellItem(int charItemID, int quantity)
	{
		InventoryItem item = GetItem(charItemID);
		if (item != null)
		{
			if (quantity >= item.Qty)
			{
				RemoveItem(charItemID, 0);
			}
			else
			{
				UpdateItem(charItemID, item.Qty - quantity, 0);
			}
		}
	}

	public void SetBankCount(int count)
	{
		BankVaultCount = count;
		if (this.BankCountUpdated != null)
		{
			this.BankCountUpdated(CurrentBankVault);
		}
	}

	public void ReloadInventory()
	{
		if (this.InventoryReload != null)
		{
			this.InventoryReload();
		}
	}

	public void TransferItem(int charitemid, int fromBank, int toBank, int transferID)
	{
		InventoryItem inventoryItem = null;
		if (LoadedBanks.Contains(fromBank))
		{
			inventoryItem = allItems[fromBank].Find((InventoryItem p) => p.CharItemID == charitemid);
		}
		if (inventoryItem != null)
		{
			allItems[fromBank].Remove(inventoryItem);
			inventoryItem.BankID = toBank;
			inventoryItem.TransferID = transferID;
			if (LoadedBanks.Contains(toBank))
			{
				allItems[toBank].Add(inventoryItem);
			}
			if (this.ItemTransferred != null)
			{
				this.ItemTransferred(charitemid, fromBank, toBank, inventoryItem);
			}
			if (fromBank == 0 && this.ItemRemoved != null)
			{
				this.ItemRemoved(inventoryItem);
			}
			if (toBank == 0 && this.ItemAdded != null)
			{
				this.ItemAdded(inventoryItem);
			}
		}
	}

	public void SetBankItems(int bankID, List<InventoryItem> items)
	{
		LoadedBanks.Add(bankID);
		allItems[bankID] = items ?? new List<InventoryItem>();
		this.BankLoaded?.Invoke(bankID, allItems[bankID]);
	}

	public void SetAllBankItems(Dictionary<int, List<InventoryItem>> items)
	{
		foreach (KeyValuePair<int, List<InventoryItem>> item in items)
		{
			LoadedBanks.Add(item.Key);
			allItems[item.Key] = ((items[item.Key] == null) ? new List<InventoryItem>() : items[item.Key]);
		}
		this.AllBanksLoaded?.Invoke(items);
	}

	public bool HasItem(int itemID, int bankID = 0)
	{
		return allItems[bankID].FirstOrDefault((InventoryItem x) => x.ID == itemID) != null;
	}

	public bool HasItemInInventory(int itemID)
	{
		return items.FirstOrDefault((InventoryItem x) => x.ID == itemID) != null;
	}

	public bool HasItemInBank(int itemID)
	{
		return allItems.Values.Any((List<InventoryItem> x) => x.Any((InventoryItem y) => y.ID == itemID && y.BankID > 0));
	}

	public bool HasCharItem(int charItemID)
	{
		return items.Find((InventoryItem p) => p.CharItemID == charItemID) != null;
	}

	public bool HasCharItem(int charItemID, out InventoryItem charItem)
	{
		charItem = items.Find((InventoryItem p) => p.CharItemID == charItemID);
		return charItem != null;
	}

	public bool IsStackableItem(int itemID)
	{
		return Items.Get(itemID).MaxStack > 1;
	}

	public bool HasSlotsAvailableInInventory(int slotsNeeded)
	{
		if (slotsNeeded <= 0)
		{
			return true;
		}
		return CountItemsInBank() + slotsNeeded <= BagSlots;
	}

	public bool HasRoomInInventory(Item itemToAdd)
	{
		return HasRoomInInventory(new List<Item> { itemToAdd });
	}

	public bool HasRoomInInventory(List<Item> itemsToAdd)
	{
		int num = 0;
		if (itemsToAdd == null || itemsToAdd.Count == 0)
		{
			return true;
		}
		foreach (Item item in itemsToAdd)
		{
			if (!item.TakesBagSpace)
			{
				continue;
			}
			int num2 = item.Qty;
			if (HasItemInInventory(item.ID))
			{
				InventoryItem[] array = items.Where((InventoryItem x) => x.ID == item.ID).ToArray();
				foreach (InventoryItem inventoryItem in array)
				{
					if (inventoryItem.Qty < inventoryItem.MaxStack)
					{
						int num3 = item.MaxStack - inventoryItem.Qty;
						if (num3 > num2)
						{
							num3 = num2;
						}
						num2 -= num3;
						if (num2 <= 0)
						{
							break;
						}
					}
				}
				if (num2 > 0)
				{
					int num4 = num2 / item.MaxStack;
					int num5 = num2 % item.MaxStack;
					int num6 = num4 + ((num5 > 0) ? 1 : 0);
					num += num6;
					if (!HasSlotsAvailableInInventory(num))
					{
						return false;
					}
				}
			}
			else
			{
				int num7 = num2 / item.MaxStack;
				int num8 = num2 % item.MaxStack;
				int num9 = num7 + ((num8 > 0) ? 1 : 0);
				num += num9;
				if (!HasSlotsAvailableInInventory(num))
				{
					return false;
				}
			}
		}
		return HasSlotsAvailableInInventory(num);
	}

	public bool MeetsClassUnlockReqs(int classID)
	{
		return CombatClass.GetClassByID(classID)?.UnlockReqs.All((ClassUnlockReq req) => req.PlayerMeetsReq()) ?? true;
	}

	public bool IsClassAvailable(CombatClass combatClass)
	{
		if (combatClass != null && CheckBitFlag(combatClass.BitFlagName, combatClass.BitFlagIndex) && GetQSValue(combatClass.QSIndex) >= combatClass.QSValue)
		{
			if (combatClass.bStaff)
			{
				return AccessLevel >= 50;
			}
			return true;
		}
		return false;
	}

	public bool OwnsClass(int classID)
	{
		if (classID <= 0)
		{
			return true;
		}
		return charClasses.Any((CharClass t) => classID == t.ClassID);
	}

	public void AddClass(CharClass charClass)
	{
		if (charClasses.FirstOrDefault((CharClass x) => x.ClassID == charClass.ClassID) == null)
		{
			charClasses.Add(charClass);
			this.ClassAdded?.Invoke(charClass.ClassID);
			Debug.Log("Added CharClass #" + charClass.ClassID + " to MyPlayerData CharClasses");
		}
		else
		{
			Debug.Log("Error - Already owns classID " + charClass.ClassID);
		}
	}

	public void EquipClass(int classID)
	{
		if (charClasses.FirstOrDefault((CharClass x) => x.ClassID == classID) == null)
		{
			return;
		}
		AudioManager.Play2DSFX("UI_Change_Class");
		foreach (CharClass charClass in charClasses)
		{
			charClass.bEquip = charClass.ClassID == classID;
		}
		this.ClassEquipped?.Invoke(classID);
	}

	public void SetPvpAction(CombatSpellSlot slot, int spellID)
	{
		int pvpAction = GetPvpAction(slot);
		currentPvpActions[slot] = spellID;
		SettingsManager.SetActionSlotID(slot, spellID);
		if (pvpAction != spellID)
		{
			this.PvpActionEquipped?.Invoke(slot, spellID);
		}
	}

	public int GetPvpAction(CombatSpellSlot slot)
	{
		if (currentPvpActions.TryGetValue(slot, out var value))
		{
			return value;
		}
		return 0;
	}

	public List<int> GetPvpActions()
	{
		List<int> list = new List<int>();
		foreach (UICustomActionButton customPvpButton in UIGame.Instance.ActionBar.customPvpButtons)
		{
			CombatSpellSlot slotNumber = customPvpButton.SlotNumber;
			list.Add(SettingsManager.GetActionSlotID(slotNumber));
		}
		return list;
	}

	public void EquipItemToSlot(InventoryItem item, CombatSpellSlot slotNumber)
	{
		SettingsManager.SetActionSlotID(slotNumber, item.ID);
		this.UpdateActionItem?.Invoke(item, slotNumber);
	}

	public void AddQuest(int questID)
	{
		if (!CurQuests.Contains(questID))
		{
			CurQuests.Add(questID);
			Quest quest = Quests.Get(questID);
			if (!quest.IsSagaQuest)
			{
				questAreas.Add(quest.MapID);
			}
			this.QuestAdded?.Invoke(questID);
			OnQuestStateUpdated();
		}
	}

	public bool HasQuest(int questID)
	{
		return CurQuests.Contains(questID);
	}

	public bool IsQuestComplete(int questID)
	{
		if (!CurQuests.Contains(questID))
		{
			return false;
		}
		Quest quest = Quests.Get(questID);
		if (quest == null)
		{
			return false;
		}
		foreach (QuestObjective objective in quest.Objectives)
		{
			if (GetQuestObjectiveProgress(objective) < objective.Qty)
			{
				return false;
			}
		}
		return true;
	}

	public bool CanSendUniqueRewardToBank(Quest quest)
	{
		return quest.Rewards.Where((QuestRewardItem x) => x.IsUnique).Any((QuestRewardItem x) => HasItemInBank(x.ID) && GetItemCount(x.ID) < x.MaxStack && x.MaxStack > 1);
	}

	public string CanTurnIn(Quest quest, Queue<int> ChosenRewardIDs)
	{
		foreach (QuestObjective objective in quest.Objectives)
		{
			if (objective.Type == QuestObjectiveType.Turnin)
			{
				int num = items.Where((InventoryItem x) => x.ID == objective.RefID).Sum((InventoryItem x) => x.Qty);
				int num2 = items.Where((InventoryItem x) => x.ID == objective.RefID && !x.IsEquipped).Sum((InventoryItem x) => x.Qty);
				if (num < objective.Qty)
				{
					return "Required item not found";
				}
				if (num2 < objective.Qty)
				{
					return "Required item is currently equipped";
				}
			}
		}
		if (!HasRoomInInventory(quest.Rewards.Where((QuestRewardItem x) => !x.IsUnique || (x.IsUnique && GetItemCount(x.ID) == 0)).Select((Func<QuestRewardItem, Item>)((QuestRewardItem x) => x)).ToList()))
		{
			return "Not enough inventory space for the quest rewards!";
		}
		if (quest.Rewards.Where((QuestRewardItem x) => x.IsUnique && ChosenRewardIDs.Contains(x.ID)).Any((QuestRewardItem x) => GetItemCount(x.ID) + x.Qty > x.MaxStack))
		{
			return "Unique item is at max stack";
		}
		if (quest.Rewards.Where((QuestRewardItem x) => x.IsUnique).Any((QuestRewardItem x) => GetItemCount(x.ID) + x.Qty > x.MaxStack))
		{
			return "Unique item is at max stack";
		}
		return "";
	}

	public string CanCraft(Merge merge)
	{
		if (merge.IsUnique && GetItemCount(merge.ID) >= merge.MaxStack)
		{
			if (allItems.Values.Any((List<InventoryItem> x) => x.Any((InventoryItem y) => y.ID == merge.ID && y.BankID > 0)))
			{
				return "You have '" + merge.Name + "' in bank. You cannot have any more of this item!";
			}
			return "You cannot have any more '" + merge.Name + "'.";
		}
		if (Gold < merge.MergeCost)
		{
			return "You don't have enough gold.";
		}
		foreach (MergeItem mat in merge.MergeItems)
		{
			int num = items.Where((InventoryItem x) => x.ID == mat.ID).Sum((InventoryItem x) => x.Qty);
			int num2 = items.Where((InventoryItem x) => x.ID == mat.ID && !x.IsEquipped).Sum((InventoryItem x) => x.Qty);
			if (num < mat.Qty)
			{
				return "Required item not found";
			}
			if (num2 < mat.Qty)
			{
				return "Required item is currently equipped";
			}
		}
		return "";
	}

	public bool IsItemEquipped(int itemID)
	{
		foreach (InventoryItem item in items)
		{
			if (item.ID == itemID && item.IsEquipped)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsQuestObjectiveCompleted(int questID, int QOID)
	{
		Quest quest = Quests.Get(questID);
		if (quest == null)
		{
			return false;
		}
		foreach (QuestObjective objective in quest.Objectives)
		{
			if (objective.ID == QOID)
			{
				return GetQuestObjectiveProgress(objective) >= objective.Qty;
			}
		}
		return false;
	}

	public bool IsQuestObjectiveInProgress(int questID, int QOID)
	{
		Quest quest = Quests.Get(questID);
		if (quest == null)
		{
			return false;
		}
		foreach (QuestObjective objective in quest.Objectives)
		{
			if (objective.ID == QOID)
			{
				return GetQuestObjectiveProgress(objective) < objective.Qty;
			}
		}
		return false;
	}

	public int GetQuestObjectiveProgress(QuestObjective qo)
	{
		if (qo.Type == QuestObjectiveType.Turnin)
		{
			return GetInventoryItemCount(qo.RefID);
		}
		if (qo.Type == QuestObjectiveType.QuestString)
		{
			return GetQSValue(qo.RefID);
		}
		if (CurQuestObjectives.ContainsKey(qo.ID))
		{
			return CurQuestObjectives[qo.ID];
		}
		return 0;
	}

	public string GetQuestObjectiveProgressText(QuestObjective qo)
	{
		if (qo.Type == QuestObjectiveType.QuestString)
		{
			return ((GetQuestObjectiveProgress(qo) >= qo.Qty) ? 1 : 0) + "/1 " + qo.Desc;
		}
		return GetQuestObjectiveProgress(qo) + "/" + qo.Qty + " " + qo.Desc;
	}

	public bool HasTalkObjectiveInProgress(int mapID, int cellID, int spawnID)
	{
		foreach (int curQuest in CurQuests)
		{
			Quest quest = Quests.Get(curQuest);
			if (quest == null)
			{
				continue;
			}
			foreach (QuestObjective objective in quest.Objectives)
			{
				if (objective.Type == QuestObjectiveType.Talk && objective.MapID == mapID && objective.CellID == cellID && objective.RefID == spawnID && GetQuestObjectiveProgress(objective) < objective.Qty)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void UpdateQuestObjective(int qoid, int qty)
	{
		CurQuestObjectives[qoid] = qty;
		foreach (int curQuest in CurQuests)
		{
			Quest quest = Quests.Get(curQuest);
			if (quest == null)
			{
				continue;
			}
			foreach (QuestObjective objective in quest.Objectives)
			{
				if (objective.Type != 0 && objective.ID == qoid)
				{
					if (this.QuestObjectiveUpdated != null)
					{
						this.QuestObjectiveUpdated(quest.ID, objective.ID);
					}
					OnQuestStateUpdated();
				}
			}
		}
	}

	private void UpdateQuestObjective(InventoryItem item)
	{
		foreach (int curQuest in CurQuests)
		{
			Quest quest = Quests.Get(curQuest);
			if (quest == null)
			{
				continue;
			}
			foreach (QuestObjective objective in quest.Objectives)
			{
				if (objective.Type == QuestObjectiveType.Turnin && objective.RefID == item.ID)
				{
					if (this.QuestObjectiveUpdated != null)
					{
						this.QuestObjectiveUpdated(quest.ID, objective.ID);
					}
					OnQuestStateUpdated();
				}
			}
		}
	}

	public void TurnInQuest(int questID, int qsInc)
	{
		Quest quest = Quests.Get(questID);
		if (quest != null)
		{
			if (quest.QSIndex > 0 && !quest.IsRepeatable && GetQSValue(quest.QSIndex) == quest.QSValue)
			{
				SetQSValue(quest.QSIndex, quest.QSValue + qsInc);
			}
			RemoveQuest(questID);
			if (this.QuestTurnedIn != null)
			{
				this.QuestTurnedIn(questID);
			}
			OnQuestStateUpdated();
		}
	}

	public void RemoveQuest(int questID)
	{
		Quest quest = Quests.Get(questID);
		if (!CurQuests.Contains(questID) || quest == null)
		{
			return;
		}
		foreach (QuestObjective objective in quest.Objectives)
		{
			if (objective.Type != 0 && objective.Type != QuestObjectiveType.QuestString)
			{
				CurQuestObjectives.Remove(objective.ID);
			}
		}
		CurQuests.Remove(questID);
		questAreas.Remove(quest.MapID);
		if (questID == (int)SettingsManager.TrackedQuestID)
		{
			ClearTrackedQuest();
		}
		if (this.QuestRemoved != null)
		{
			this.QuestRemoved(questID);
		}
		OnQuestStateUpdated();
	}

	public bool IsQuestAvailable(Quest quest)
	{
		if (quest == null)
		{
			return false;
		}
		if (HasQuest(quest.ID))
		{
			return true;
		}
		if (!quest.IsDaily && BitFlagExists(quest.BitFlagNameAwarded, quest.BitFlagIndexAwarded) && CheckBitFlag(quest.BitFlagNameAwarded, quest.BitFlagIndexAwarded))
		{
			return false;
		}
		if (!QSRequirementsMet(quest))
		{
			return false;
		}
		if (quest.bStaff && Session.MyPlayerData.AccessLevel < 100 && Main.Environment < Environment.Content)
		{
			return false;
		}
		return true;
	}

	public bool IsQuestAcceptable(Quest quest)
	{
		if (quest == null)
		{
			return false;
		}
		if (HasQuest(quest.ID))
		{
			return true;
		}
		if (!IsQuestAvailable(quest))
		{
			return false;
		}
		if (Entities.Instance.me.Level < quest.RequiredLevel)
		{
			return false;
		}
		if (BitFlagExists(quest.BitFlagNameRequired, quest.BitFlagIndexRequired) && !CheckBitFlag(quest.BitFlagNameRequired, quest.BitFlagIndexRequired))
		{
			return false;
		}
		if (BitFlagExists(quest.BitFlagNameAwarded, quest.BitFlagIndexAwarded) && CheckBitFlag(quest.BitFlagNameAwarded, quest.BitFlagIndexAwarded))
		{
			return false;
		}
		if (HasDailyQuestOfSameIndex(quest))
		{
			return false;
		}
		if (!MeetsClassUnlockReqs(quest.ClassUnlockableRequirement) && !OwnsClass(quest.ClassUnlockableRequirement))
		{
			return false;
		}
		return true;
	}

	public string GetQuestLockInfo(Quest quest)
	{
		if (quest == null)
		{
			return null;
		}
		if (Entities.Instance.me.Level < quest.RequiredLevel)
		{
			return "Level requirement not met.";
		}
		if (BitFlagExists(quest.BitFlagNameRequired, quest.BitFlagIndexRequired) && !CheckBitFlag(quest.BitFlagNameRequired, quest.BitFlagIndexRequired))
		{
			return quest.RequiredBadgeName + " Badge required to do this quest.";
		}
		if (BitFlagExists(quest.BitFlagNameAwarded, quest.BitFlagIndexAwarded) && CheckBitFlag(quest.BitFlagNameAwarded, quest.BitFlagIndexAwarded))
		{
			return "A " + quest.AwardedBadgeName + " quest has already been completed today.";
		}
		if (HasDailyQuestOfSameIndex(quest))
		{
			return "Another " + quest.AwardedBadgeName + " quest is already in your quest log.";
		}
		if (!MeetsClassUnlockReqs(quest.ClassUnlockableRequirement) && !OwnsClass(quest.ClassUnlockableRequirement))
		{
			string name = CombatClass.GetClassByID(quest.ClassUnlockableRequirement).Name;
			return "Complete " + name + " class requirements to do this quest.";
		}
		if (!IsQuestAvailable(quest))
		{
			return "Quest requirement not met.";
		}
		return null;
	}

	private bool QSRequirementsMet(Quest quest)
	{
		if (quest.QSIndex > 0 && GetQSValue(quest.QSIndex) != quest.QSValue && (!quest.IsRepeatable || GetQSValue(quest.QSIndex) < quest.QSValue))
		{
			return false;
		}
		return quest.QSRequirements.All((IAQuestStringRequiredCore x) => x.IsRequirementMet(this));
	}

	public bool HasDailyQuestOfSameIndex(Quest quest)
	{
		if (!quest.IsDaily)
		{
			return false;
		}
		foreach (int curQuest in CurQuests)
		{
			Quest quest2 = Quests.Get(curQuest);
			if (quest2 != null && quest2.ID != quest.ID && quest2.IsDaily && quest.BitFlagNameAwarded == quest2.BitFlagNameAwarded && quest2.BitFlagIndexAwarded == quest.BitFlagIndexAwarded)
			{
				return true;
			}
		}
		return false;
	}

	public int GetQSValue(int index)
	{
		if (QuestChains.ContainsKey(index))
		{
			return QuestChains[index];
		}
		return 0;
	}

	public void TrackQuest(int qid)
	{
		SettingsManager.TrackedQuestID.Set(qid);
		CurrentlyTrackedQuest = Quests.Get(qid);
	}

	public void ClearTrackedQuest()
	{
		SettingsManager.TrackedQuestID.Set(0);
		CurrentlyTrackedQuest = null;
	}

	public void UpdateTrackedQuest()
	{
		if ((int)SettingsManager.TrackedQuestID > 0)
		{
			if (!HasQuest(SettingsManager.TrackedQuestID))
			{
				SettingsManager.TrackedQuestID.Set(0, updatePref: false, savePrefsToDisk: false, broadcastUpdate: false);
			}
			Quest quest = Quests.Get(SettingsManager.TrackedQuestID);
			CurrentlyTrackedQuest = quest;
		}
	}

	public void SetQSValue(int index, int value)
	{
		QuestChains[index] = value;
		if (this.QuestStringUpdated != null)
		{
			this.QuestStringUpdated(index, value);
		}
	}

	public void GetQSValue()
	{
		string text = "";
		string text2 = "";
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num5 = -1;
		int num6 = -1;
		if (QuestStrings.TryGetValue(Game.Instance.AreaData.SagaID, out var value))
		{
			num = value.QuestIndex;
			num3 = value.MaxValue;
			Quest quest = (from x in (from x in Quests.GetSagaQuests(num)
					orderby x.QSValue
					select x).ToList()
				where HasQuest(x.ID)
				select x).FirstOrDefault();
			if (quest != null)
			{
				num2 = quest.QSValue;
			}
		}
		if (CurrentlyTrackedQuest != null && QuestStrings.TryGetValue(CurrentlyTrackedQuest.QSIndex, out var value2))
		{
			num4 = value2.QuestIndex;
			num6 = value2.MaxValue;
			num5 = CurrentlyTrackedQuest.QSValue;
		}
		Chat.Notify("SAGA QUEST STRING:", "[ccff99]");
		text = ((num < 0) ? (text + "There are no saga quests in this area.") : (text + string.Format(arg1: (num2 < 0) ? "You don't have a quest in this saga" : num2.ToString(), format: "QSIndex = {0}\nQSValue = {1}\nQSMaxValue = {2}", arg0: num, arg2: num3)));
		Chat.Notify(text, "[ffffff]");
		Chat.Notify("CURRENTLY TRACKED QUEST STRING:", "[99ccff]");
		text2 = ((num4 < 0) ? (text2 + "You are not currently tracking a quest.") : (text2 + $"QSIndex = {num4}\nQSValue = {num5}\nQSMaxValue = {num6}"));
		Chat.Notify(text2, "[ffffff]");
	}

	public void GetMapInfo()
	{
		string text = "";
		if (Game.Instance.AreaData != null)
		{
			text = text + "MapID = " + Game.CurrentAreaID + "\nJoinCommand = " + Game.Instance.AreaData.name + "\nDisplayName = " + Game.Instance.AreaData.displayName + "\nCellID = " + Game.Instance.AreaData.currentCellID;
			Chat.Notify(text, "[ffffff]");
		}
	}

	private char IntToQuestStringChar(int val)
	{
		if (val >= 0 && val <= 9)
		{
			return (char)(val + 48);
		}
		if (val >= 10 && val <= 35)
		{
			return (char)(val + 55);
		}
		if (val >= 36 && val <= 61)
		{
			return (char)(val + 61);
		}
		return '0';
	}

	private int QuestStringCharToInt(char ch)
	{
		if (ch >= '0' && ch <= '9')
		{
			return ch - 48;
		}
		if (ch >= 'A' && ch <= 'Z')
		{
			return ch - 55;
		}
		if (ch >= 'a' && ch <= 'z')
		{
			return ch - 61;
		}
		return 0;
	}

	public void UpdateDailyReward(int day, DateTime date, int itemID)
	{
		RewardIndex = day;
		RewardDate = date;
		if (this.DailyRewardUpdated != null)
		{
			this.DailyRewardUpdated(day, date, itemID);
		}
	}

	public void SetStarterPack(ProductID productID, DateTime expireUTC)
	{
		ProductOffers[productID] = new ProductOffer(productID, expireUTC);
		if (this.StarterPackUpdated != null)
		{
			this.StarterPackUpdated();
		}
	}

	public void SetAreaList(List<AreaData> areas, List<RegionData> regions)
	{
		areaList = areas;
		regionList = regions;
		if (this.AreasReceived != null)
		{
			Debug.Log("AREAS RECEIVED!!!! >>>> ");
			this.AreasReceived();
		}
	}

	public bool HasQuestInArea(int areaID)
	{
		return questAreas.Contains(areaID);
	}

	public bool HasQuestInRegion(int regionID)
	{
		foreach (AreaData item in areaList.Where((AreaData x) => x.regionID == regionID).ToList())
		{
			if (questAreas.Contains(item.id))
			{
				return true;
			}
		}
		return false;
	}

	public void LoadQuestAreas()
	{
		questAreas.Clear();
		foreach (int curQuest in CurQuests)
		{
			Quest quest = Quests.Get(curQuest);
			if (quest != null && !quest.IsSagaQuest)
			{
				questAreas.Add(quest.MapID);
			}
		}
	}

	public int GetClassCostInDCs(CombatClass combatClass)
	{
		Item item = Items.Get(combatClass.ClassTokenID);
		if (item == null)
		{
			return 0;
		}
		int inventoryItemCount = GetInventoryItemCount(combatClass.ClassTokenID);
		return (combatClass.ClassTokenCost - inventoryItemCount) * item.Cost;
	}

	public int GetClassRank(int classID)
	{
		return charClasses.FirstOrDefault((CharClass x) => x.ClassID == classID)?.ClassRank ?? 0;
	}

	public void MergeAdd(Merge m)
	{
		if (merges.FirstOrDefault((Merge x) => x.MergeID == m.MergeID) == null)
		{
			merges.Add(m);
			if (this.MergeAdded != null)
			{
				this.MergeAdded(m);
			}
		}
	}

	public void MergeRemove(int mergeID)
	{
		Merge merge = merges.FirstOrDefault((Merge x) => x.MergeID == mergeID);
		if (merge != null)
		{
			merges.Remove(merge);
			if (this.MergeRemoved != null)
			{
				this.MergeRemoved(merge);
			}
		}
	}

	public void OnDataSync()
	{
		if (this.DataSynced != null)
		{
			this.DataSynced();
		}
	}

	public void UpdateFriends(List<FriendData> list)
	{
		friendsList = list;
		if (this.FriendsUpdated != null)
		{
			this.FriendsUpdated();
		}
	}

	public void AddFriendRequest(IMessage rf)
	{
		friendRequests.Add(rf);
		if (this.FriendRequestReceived != null)
		{
			this.FriendRequestReceived();
		}
	}

	public void removeFriendRequest(IMessage rf)
	{
		if (friendRequests.Contains(rf))
		{
			friendRequests.Remove(rf);
		}
		if (this.FriendRequestUpdated != null)
		{
			this.FriendRequestUpdated();
		}
	}

	public bool IsFriendsWith(string name)
	{
		foreach (FriendData friends in friendsList)
		{
			if (friends.strName.ToLower().TrimEnd(' ') == name.ToLower().TrimEnd(' '))
			{
				return true;
			}
		}
		return false;
	}

	public bool IsStyleAvailable(BaseStyle style)
	{
		if (!CheckBitFlag(style.BitflagName, style.BitflagIndex))
		{
			return false;
		}
		if (GetQSValue(style.QuestStringIndex) < style.QuestStringValue)
		{
			return false;
		}
		return true;
	}

	public CombatClass GetClassBySpellID(int spellID)
	{
		return combatClassList.FirstOrDefault((CombatClass c) => c.Spells.Contains(spellID) && !c.IsSkin);
	}

	public bool OwnsProduct(ProductDetail productdetail)
	{
		if (BitFlagExists(productdetail.BitFlagName, (byte)productdetail.BitFlagIndex))
		{
			return CheckBitFlag(productdetail.BitFlagName, (byte)productdetail.BitFlagIndex);
		}
		return false;
	}

	public void SetGameParams(string gameParams)
	{
		if (gameParams == null)
		{
			return;
		}
		Params = gameParams;
		string[] array = Params.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split(':');
			if (array2.Length == 2)
			{
				GameParams.Add(array2[0], array2[1]);
			}
		}
	}

	public string GetGameParam(string key)
	{
		if (GameParams == null)
		{
			return null;
		}
		if (GameParams.ContainsKey(key))
		{
			return GameParams[key];
		}
		return null;
	}

	public void UpdateGuildInfo(Guild guild)
	{
		Guild = guild;
		this.GuildsUpdated?.Invoke(guild == null);
	}

	public void UpdateDailyTaskInfo(CharDailyTask task)
	{
		for (int i = 0; i < Session.MyPlayerData.charDailyTasks.Count; i++)
		{
			if (Session.MyPlayerData.charDailyTasks[i].taskID == task.taskID)
			{
				Session.MyPlayerData.charDailyTasks[i].curQty = task.curQty;
			}
		}
		this.DailyTaskUpdated?.Invoke();
	}

	public void UpdateServerDailyTaskInitialization(List<DailyTask> dailyTasks, List<CharDailyTask> charDailyTasks)
	{
		Session.MyPlayerData.serverDailyTasks = dailyTasks;
		Session.MyPlayerData.charDailyTasks = charDailyTasks;
	}

	public void UpdateGuildInfo(bool leaveGuild = false)
	{
		this.GuildsUpdated?.Invoke(leaveGuild);
	}

	public void ChangeGuildNameTag(string name = null, string tag = null)
	{
		if (name != null)
		{
			this.GuildNameChanged?.Invoke(name);
			Guild.name = name;
		}
		if (tag != null)
		{
			this.GuildTagChanged?.Invoke(tag);
			Guild.tag = tag;
		}
	}

	public void ChangeGuildTax(int newTax = 10)
	{
		Guild.TaxRate = newTax;
	}

	public void MOTDUpdated(string motd)
	{
		this.GuildMOTDUpdated?.Invoke(motd);
		Guild.MOTD = motd;
	}

	public void AddToReportRecord(string user, DateTime now)
	{
		recordList.Add(new ReportRecord(user, now));
	}

	public int RecordChecker(string user, DateTime time)
	{
		int num = recordList.FindIndex((ReportRecord x) => x.user == user);
		if (num != -1)
		{
			if (DateTime.Compare(recordList[num].reportTime.AddMinutes(reportCooldownMins), time) >= 0)
			{
				return 0;
			}
			return 1;
		}
		return 1;
	}

	public void UpdatePvpRecords(PvpPlayerRecords inRecords)
	{
		pvpPlayerRecords.WinsTotal += inRecords.WinsTotal;
		pvpPlayerRecords.LossesTotal += inRecords.LossesTotal;
		pvpPlayerRecords.KillsTotal += inRecords.KillsTotal;
		pvpPlayerRecords.DeathsTotal += inRecords.DeathsTotal;
		pvpPlayerRecords.AssistsTotal += inRecords.AssistsTotal;
		pvpPlayerRecords.ObjectivesTakenTotal += inRecords.ObjectivesTakenTotal;
		pvpPlayerRecords.ObjectivesDefendedTotal += inRecords.ObjectivesDefendedTotal;
	}

	public void AutoEquipPotion(int LootBagID, int itemID)
	{
		if (itemID != 1893 && itemID != 298)
		{
			return;
		}
		CombatSpellSlot combatSpellSlot = UIGame.Instance.ActionBar.ReturnFirstEmptySlot();
		if (combatSpellSlot != 0)
		{
			EquipItemToSlot(items.Find((InventoryItem x) => x.ID == itemID), combatSpellSlot);
		}
	}

	public void AddPersonalHouseData(List<HouseData> hDataList)
	{
		if (PersonalHouseData == null)
		{
			PersonalHouseData = new List<HouseData>();
		}
		PersonalHouseData.AddRange(hDataList);
		this.PersonalHouseDataAdded?.Invoke(hDataList);
	}

	public void AddPublicHouseData(Dictionary<int, HouseData> hDataDict, HouseDataCategory serverListCategory, int serverListVersion, int serverListSize, bool isReversed)
	{
		if (PublicHouseData == null || ClientPublicHouseDataVersion != serverListVersion || ClientPublicHouseDataCategory != serverListCategory || serverListCategory == HouseDataCategory.Query)
		{
			PublicHouseData = new Dictionary<int, HouseData>();
			PublicHouseDataCount = serverListSize;
			ClientPublicHouseDataCategory = serverListCategory;
			ClientPublicHouseDataVersion = serverListVersion;
		}
		if (hDataDict != null)
		{
			foreach (KeyValuePair<int, HouseData> item in hDataDict)
			{
				PublicHouseData[item.Key] = item.Value;
			}
		}
		this.PublicHouseDataAdded?.Invoke(hDataDict, serverListCategory, isReversed);
	}

	public void HouseItemAdd(ComHouseItem hItem)
	{
		if (HouseItemCounts.ContainsKey(hItem.ItemID))
		{
			HouseItemCounts[hItem.ItemID]++;
		}
		else
		{
			HouseItemCounts[hItem.ItemID] = 1;
		}
		this.OnHouseItemAdded?.Invoke(hItem);
	}

	public void HouseItemRemove(int itemID, int houseItemID, int houseID)
	{
		if (HouseItemCounts.ContainsKey(itemID))
		{
			HouseItemCounts[itemID]--;
		}
		this.OnHouseItemRemoved?.Invoke(itemID, houseItemID, houseID);
	}

	public void HouseItemClearAll(Dictionary<int, int> houseItemCounts)
	{
		if (houseItemCounts == null)
		{
			HouseItemCounts = new Dictionary<int, int>();
		}
		else
		{
			HouseItemCounts = houseItemCounts;
		}
		this.OnHouseItemClearAll?.Invoke();
	}
}
