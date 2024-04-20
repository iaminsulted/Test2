using System.Collections.Generic;
using Assets.Scripts.NetworkClient.CommClasses;
using UnityEngine;

public class AreaData
{
	public int id = -1;

	public int regionID;

	public string name = "Default";

	public string displayName = "";

	public int maxusers;

	public float soloDifficultyFactor;

	public bool isScaling;

	public bool isDungeon;

	public bool isChallenge;

	public bool isSeasonal;

	public bool invertDynamicScaling;

	public bool isLegacyDynamicScaling;

	public float dynamicScalingAmount;

	public float bossDynScalingAmount;

	public int SagaID;

	public Dictionary<int, CellData> cellMap;

	public int SoundTrackID;

	public int currentCellID = -1;

	public List<int> quests;

	public Dictionary<int, Quest> cutsceneQuests;

	public Dictionary<int, List<int>> NpcSpawnInfos = new Dictionary<int, List<int>>();

	public List<KeyValuePair<string, string>> areaFlags = new List<KeyValuePair<string, string>>();

	public string UnlockBitFlagName;

	public int UnlockBitFlagIndex;

	public PvpType PvpType;

	public int SetupTime;

	public int ExpireTime;

	public MatchState MatchState;

	public float SetupTS;

	public List<ComSpawnMeta> spawnMetas = new List<ComSpawnMeta>();

	public Dictionary<GameObject, int> npcPathNodes = new Dictionary<GameObject, int>();

	public GameObject dbSpawns;

	public Dictionary<int, string> spawnReqs = new Dictionary<int, string>();

	public List<ComMapEntity> mapEntityPlayerSpawners = new List<ComMapEntity>();

	public List<ComMapEntity> mapEntityTransferPads = new List<ComMapEntity>();

	public List<ComMapEntity> mapEntityMachines = new List<ComMapEntity>();

	public float cellTimerDuration;

	public string cellTimerDescription;

	public bool isCellTimerPaused;

	public int MinScalingLevel;

	public int MaxScalingLevel;

	public Dictionary<int, List<string>> NpcSfxGreetings = new Dictionary<int, List<string>>();

	public Dictionary<int, List<string>> NpcSfxFarewells = new Dictionary<int, List<string>>();

	public int JoinDialogue;

	public Quest SagaQuest;

	public bool IsHouse;

	public ComHouseData HouseJoinData;

	public bool IsInstance
	{
		get
		{
			if (!isDungeon)
			{
				return HasPvp;
			}
			return true;
		}
	}

	public bool IsDynamicScaling
	{
		get
		{
			if (!isDungeon || invertDynamicScaling)
			{
				if (!isDungeon)
				{
					return invertDynamicScaling;
				}
				return false;
			}
			return true;
		}
	}

	public bool DoRewardsScale
	{
		get
		{
			if (isScaling)
			{
				if (isDungeon)
				{
					return isSeasonal;
				}
				return true;
			}
			return false;
		}
	}

	public bool HasPvp
	{
		get
		{
			if (PvpType != 0)
			{
				return PvpType != PvpType.Lobby;
			}
			return false;
		}
	}

	public bool IsPvpLobby => PvpType == PvpType.Lobby;

	public AreaData()
	{
	}

	public AreaData(ComAreaData comAreaData)
	{
		id = comAreaData.id;
		name = comAreaData.name;
		displayName = comAreaData.displayName;
		maxusers = comAreaData.maxusers;
		isScaling = comAreaData.isScaling;
		isDungeon = comAreaData.isDungeon;
		isChallenge = comAreaData.isChallenge;
		isSeasonal = comAreaData.isSeasonal;
		invertDynamicScaling = comAreaData.invertDynamicScaling;
		isLegacyDynamicScaling = comAreaData.isLegacyDynamicScaling;
		dynamicScalingAmount = comAreaData.dynamicScalingAmount;
		bossDynScalingAmount = comAreaData.bossDynScalingAmount;
		SagaID = comAreaData.SagaID;
		cellMap = comAreaData.cellMap;
		quests = comAreaData.quests ?? new List<int>();
		UnlockBitFlagName = comAreaData.UnlockBitFlagName;
		UnlockBitFlagIndex = comAreaData.UnlockBitFlagIndex;
		PvpType = comAreaData.PvpType;
		SetupTime = comAreaData.SetupTime;
		ExpireTime = comAreaData.ExpireTime;
		MatchState = comAreaData.MatchState;
		SetupTS = comAreaData.SetupTS;
		MinScalingLevel = comAreaData.MinScalingLevel;
		MaxScalingLevel = comAreaData.MaxScalingLevel;
		SagaQuest = comAreaData.SagaQuest;
		HouseJoinData = comAreaData.HouseJoinData;
	}
}
