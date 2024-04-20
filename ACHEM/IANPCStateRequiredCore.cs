public class IANPCStateRequiredCore : IARequiredCore
{
	public NPCSpawn Spawn;

	public byte State;

	public bool Not;

	public IANPCStateRequiredCore(NPCSpawn spawn)
	{
		Spawn = spawn;
		Spawn.StateUpdated += Machine_StateUpdated;
	}

	private void Machine_StateUpdated(byte obj)
	{
		OnRequirementUpdate();
	}

	public override bool IsRequirementMet(MyPlayerData myPlayerData)
	{
		bool flag = Spawn.State == State;
		if (!Not)
		{
			return flag;
		}
		return !flag;
	}

	~IANPCStateRequiredCore()
	{
		Spawn.StateUpdated -= Machine_StateUpdated;
	}
}
