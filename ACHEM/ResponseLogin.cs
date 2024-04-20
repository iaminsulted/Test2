using System;
using System.Collections.Generic;
using StatCurves;

public class ResponseLogin : Response
{
	public int status;

	public string message;

	public int UserID;

	public int ID;

	public byte Level;

	public int XP;

	public int XPToLevel;

	public int LevelCap;

	public int EndGame;

	public int Gold;

	public int MC;

	public Dictionary<int, List<InventoryItem>> items;

	public List<Merge> merges;

	public List<CharClass> charClasses;

	public List<int> Quests;

	public Dictionary<int, int> QuestObjectives;

	public Dictionary<int, QuestString> QuestStrings;

	public Dictionary<int, int> QuestChains;

	public int AccessLevel;

	public Dictionary<string, long> BitFields;

	public DateTime Server;

	public DateTime Login;

	public int BankVault;

	public int BankVaultCost;

	public int RewardIndex;

	public DateTime RewardDate;

	public int NewsApopID;

	public byte NewsApopState;

	public int NewsApopVersion;

	public byte NewsApopCooldown;

	public string MOTD;

	public int LatencyNotifyThreshold;

	public int LatencyDisconnectThreshold;

	public string GameParams;

	public int GuildID;

	public int DuelOpponentID;

	public int HouseSlotCost;

	public int HouseSlotMax;

	public int HouseItemMaxPerSlot;

	public int HouseMaxPlayers;

	public List<CombatClass> classes;

	public List<Item> tokens;

	public List<Item> resources;

	public Dictionary<ProductID, ProductOffer> productOffers;

	public Dictionary<ProductID, ProductDetail> ProductPackages;

	public Dictionary<TradeSkillType, int> tradeSkillXP;

	public Dictionary<TradeSkillType, int> tradeSkillXPToLevel;

	public Dictionary<int, SoundTrack> soundTracks;

	public List<Item> GuildPowerItemList;
}
