using System;
using System.Collections.Generic;

public class PartyManager
{
	public static ResponsePartyInvite PartyInvite;

	public static Dictionary<int, PlayerPartyData> MemberData { get; private set; } = new Dictionary<int, PlayerPartyData>();


	public static int LeaderID { get; private set; }

	public static bool IsPrivate { get; private set; }

	public static List<Player> Members
	{
		get
		{
			List<Player> list = new List<Player>();
			foreach (int key in MemberData.Keys)
			{
				Player playerById = Entities.Instance.GetPlayerById(key);
				if (playerById != null && playerById != Entities.Instance.me)
				{
					list.Add(playerById);
				}
			}
			return list;
		}
	}

	public static bool IsInParty => MemberData.Count > 1;

	public static bool IsLeader
	{
		get
		{
			if (IsInParty)
			{
				return LeaderID == Entities.Instance.me.ID;
			}
			return false;
		}
	}

	public static bool IsPrivateLeader
	{
		get
		{
			if (IsLeader)
			{
				return IsPrivate;
			}
			return false;
		}
	}

	public static int MemberCount => MemberData.Count;

	public static event Action PartyUpdated;

	public static void SetPartyData(Dictionary<int, PlayerPartyData> memberData, int leaderID, bool isPrivate)
	{
		MemberData = memberData;
		LeaderID = leaderID;
		IsPrivate = isPrivate;
		PartyManager.PartyUpdated?.Invoke();
	}

	public static void UpdatePrivacy(bool isPrivate)
	{
		IsPrivate = isPrivate;
		PartyManager.PartyUpdated?.Invoke();
	}

	public static void Clear()
	{
		MemberData.Clear();
		LeaderID = 0;
		PartyInvite = null;
		PartyManager.PartyUpdated?.Invoke();
	}

	public static void AddPartyInvite(ResponsePartyInvite req)
	{
		PartyInvite = req;
	}

	public static void RemovePartyInvite()
	{
		PartyInvite = null;
	}

	public static bool IsMember(int playerID)
	{
		return MemberData.ContainsKey(playerID);
	}

	public static bool IsPartyFull()
	{
		return MemberCount >= 5;
	}

	public static int GetMemberIDByName(string name)
	{
		foreach (KeyValuePair<int, PlayerPartyData> memberDatum in MemberData)
		{
			if (memberDatum.Value.name == name)
			{
				return memberDatum.Key;
			}
		}
		return 0;
	}
}
