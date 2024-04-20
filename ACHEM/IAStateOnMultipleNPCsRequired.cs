public class IAStateOnMultipleNPCsRequired : InteractionRequirement
{
	public int[] NPCSpawnIDs;

	public NPCSpawn[] Spawns;

	public byte State;

	public bool Not;

	public void Awake()
	{
		RegisterStateUpdateListener();
	}

	public void OnDestroy()
	{
		if (Spawns == null || Spawns.Length == 0)
		{
			return;
		}
		NPCSpawn[] spawns = Spawns;
		foreach (NPCSpawn nPCSpawn in spawns)
		{
			if (nPCSpawn != null)
			{
				nPCSpawn.StateUpdated -= Machine_StateUpdated;
			}
		}
	}

	public void RegisterStateUpdateListener()
	{
		if (Spawns != null && Spawns.Length != 0)
		{
			NPCSpawn[] spawns = Spawns;
			foreach (NPCSpawn obj in spawns)
			{
				obj.StateUpdated -= Machine_StateUpdated;
				obj.StateUpdated += Machine_StateUpdated;
			}
		}
	}

	private void Machine_StateUpdated(byte obj)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		if (Spawns == null)
		{
			return false;
		}
		bool flag = true;
		NPCSpawn[] spawns = Spawns;
		for (int i = 0; i < spawns.Length; i++)
		{
			if (spawns[i].State != State)
			{
				flag = false;
			}
		}
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	public void SetSpawn(NPCSpawn[] spawns)
	{
		Spawns = spawns;
		NPCSpawn[] spawns2 = Spawns;
		for (int i = 0; i < spawns2.Length; i++)
		{
			spawns2[i].StateUpdated += Machine_StateUpdated;
		}
	}
}
