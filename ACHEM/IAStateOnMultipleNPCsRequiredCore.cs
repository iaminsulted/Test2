public class IAStateOnMultipleNPCsRequiredCore : IARequiredCore
{
	public int[] NPCSpawnIDs;

	public NPCSpawn[] Spawns;

	public byte State;

	public bool Not;

	public IAStateOnMultipleNPCsRequiredCore(NPCSpawn[] spawns)
	{
		if (spawns != null && spawns.Length >= 1)
		{
			Spawns = spawns;
			NPCSpawn[] spawns2 = Spawns;
			for (int i = 0; i < spawns2.Length; i++)
			{
				spawns2[i].StateUpdated += Machine_StateUpdated;
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

	~IAStateOnMultipleNPCsRequiredCore()
	{
		if (Spawns != null && Spawns.Length != 0)
		{
			NPCSpawn[] spawns = Spawns;
			int i = 0;
			for (; i < spawns.Length; i++)
			{
				spawns[i].StateUpdated -= Machine_StateUpdated;
			}
		}
	}
}
