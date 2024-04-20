using System.Collections.Generic;
using Assets.Scripts.NetworkClient.CommClasses;

public class ComAreaData
{
	public int id = -1;

	public string name = "Default";

	public string displayName = "";

	public int maxusers;

	public bool isScaling;

	public bool isDungeon;

	public bool isChallenge;

	public bool isSeasonal;

	public bool invertDynamicScaling;

	public bool isLegacyDynamicScaling;

	public float dynamicScalingAmount;

	public float bossDynScalingAmount;

	public int SagaID;

	public int SagaCutsceneID;

	public Dictionary<int, CellData> cellMap;

	public List<int> quests;

	public string RequirementText;

	public string UnlockBitFlagName;

	public int UnlockBitFlagIndex;

	public PvpType PvpType;

	public int SetupTime;

	public int ExpireTime;

	public float SetupTS;

	public MatchState MatchState;

	public int MinScalingLevel;

	public int MaxScalingLevel;

	public Quest SagaQuest;

	public ComHouseData HouseJoinData;
}
