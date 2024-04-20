using UnityEngine;

[AddComponentMenu("Interactivity/Requirements/IA NPC State Required")]
public class IANPCStateRequired : InteractionRequirement
{
	public NPCSpawn Spawn;

	public int NPCSpawnID;

	public byte State;

	public bool Not;

	public void Awake()
	{
		RegisterStateUpdateListener();
	}

	public void OnDestroy()
	{
		if (Spawn != null)
		{
			Spawn.StateUpdated -= Machine_StateUpdated;
		}
	}

	public void RegisterStateUpdateListener()
	{
		if (Spawn != null)
		{
			Spawn.StateUpdated -= Machine_StateUpdated;
			Spawn.StateUpdated += Machine_StateUpdated;
		}
	}

	private void Machine_StateUpdated(byte obj)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		if (Spawn == null)
		{
			return false;
		}
		bool flag = Spawn.State == State;
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	public void SetSpawn(NPCSpawn spawn)
	{
		Spawn = spawn;
		Spawn.StateUpdated += Machine_StateUpdated;
	}
}
