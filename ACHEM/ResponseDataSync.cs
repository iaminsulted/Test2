using System;
using System.Collections.Generic;

public class ResponseDataSync : Response
{
	public int status;

	public int UserID;

	public int ID;

	public byte Level;

	public int XP;

	public int XPToLevel;

	public int ExpectedStat;

	public int Gold;

	public int MC;

	public Dictionary<int, List<InventoryItem>> items;

	public List<Merge> merges;

	public List<CharClass> charClasses;

	public Dictionary<int, bool> Quests;

	public Dictionary<int, int> QuestObjectives;

	public string QuestString;

	public Dictionary<string, long> BitFields;

	public float TS;

	public DateTime ServerTime;

	public int BankVault;

	public float XPMultiplier;

	public float GoldMultiplier;

	public float CXPMultiplier;

	public float DailyQuestMultiplier;

	public float DailyTaskMultiplier;
}
