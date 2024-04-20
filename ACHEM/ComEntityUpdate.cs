using System;
using System.Collections.Generic;
using AQ3DServer.GameServer.CommClasses;

public class ComEntityUpdate
{
	public Entity.Type entityType;

	public int entityID;

	public int XP;

	public bool isCasterUpdate;

	public ComEntityUpdateType updateType;

	public EntityAsset overrideAsset;

	public CombatSolver.SpellResult spellResult;

	public int spellActionId;

	public List<ComEffect> effects = new List<ComEffect>();

	public List<Effect.Operation> effectOperations = new List<Effect.Operation>();

	public bool isEffectsFinal;

	public List<AuraOperation> auraOperations = new List<AuraOperation>();

	public ComDash dash;

	public Entity.State state;

	public float respawnTime;

	public Entity.Reaction react;

	public StateEmote stateEmote;

	public Entity.Type targetType;

	public int targetID = -1;

	public bool isStatusUpdate;

	public Entity.StatusType statusMap;

	public bool isCastCancel;

	public Dictionary<int, float> statDeltas = new Dictionary<int, float>();

	public Dictionary<int, float> statValues = new Dictionary<int, float>();

	public float[] statsCurrent;

	public float[] statsBaseline;

	public float rawHpDelta;

	public DateTime time;

	public bool IsAssetUpdate
	{
		get
		{
			return (updateType & ComEntityUpdateType.Asset) == ComEntityUpdateType.Asset;
		}
		private set
		{
			if (value)
			{
				updateType |= ComEntityUpdateType.Asset;
			}
			else
			{
				updateType &= ~ComEntityUpdateType.Asset;
			}
		}
	}

	public EntityAsset OverrideAsset
	{
		get
		{
			return overrideAsset;
		}
		set
		{
			overrideAsset = value;
			IsAssetUpdate = true;
		}
	}

	public ComEntityUpdate()
	{
		time = DateTime.Now;
	}
}
