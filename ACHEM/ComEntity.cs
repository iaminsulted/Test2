using System.Collections.Generic;
using AQ3DServer.GameServer.CommClasses;
using StatCurves;

public class ComEntity
{
	public int ID;

	public Entity.Type type;

	public string name;

	public int ClassID;

	public int ClassXP;

	public int TotalClassRank;

	public long iu0;

	public long iu1;

	public int Level;

	public int ScaledLevel;

	public int XP;

	public int ExpectedStat;

	public int NPCID;

	public string Description;

	public int Portrait;

	public int Title;

	public string TitleName;

	public int duelOpponentID;

	public EntityAsset baseAsset;

	public EntityAsset overrideAsset;

	public float combatRadius = 0.55f;

	public int AccessLevel;

	public Dictionary<TradeSkillType, int> tradeSkillLevel;

	public int guildID;

	public string guildTag;

	public string guildName;

	public int SpawnID;

	public List<ComVector3> Path;

	public float PathTS;

	public float PathSpeed;

	public string NodeAnimations;

	public bool NodeSequentialAnimations;

	public bool NodeUseRotation;

	public int NodeRotationY;

	public string sfxAttack;

	public string sfxDeath;

	public int NamePlate;

	public NPCMovementBehavior MoveBehavior;

	public List<int> ApopIDs;

	public Dictionary<string, string> AnimationOverrides;

	public bool IsBoss;

	public float MaxHealthScale;

	public float AttackScale;

	public float ArmorScale;

	public float CritScale;

	public float HasteScale;

	public bool ForceDynamicScaling;

	public int DynamicPlayerCount;

	public Dictionary<int, List<int>> questObjectiveItems;

	public int areaID;

	public int cellID;

	public float posX;

	public float posY;

	public float posZ;

	public float rotY;

	public List<CombatSolver.Element> elements = new List<CombatSolver.Element>();

	public float[] statsBaseline;

	public float[] statsCurrent;

	public Dictionary<CombatSolver.Element, float> resists;

	public List<ComEffect> effects;

	public Entity.StatusType statusMap;

	public List<ComAura> auras;

	public bool isPvPFlagged;

	public int teamID;

	public Entity.State state;

	public StateEmote stateEmote;

	public Entity.PhaseState phaseState;

	public Entity.Reaction react;

	public SpellCastData spellCastData;

	public List<NpcSpell> npcSpells;

	public int summonerId;

	public Entity.Type summonerType;

	public bool isAFK;

	public bool IsTargetReticleHidden;

	public bool IsSheathed;
}
