using System;

[Serializable]
public class NPCSpawnInfo
{
	public int NPCID;

	public int Level = 1;

	public int Rate;

	public NPCSpawnInfo()
	{
	}

	public NPCSpawnInfo(int npcid, int level, int rate)
	{
		NPCID = npcid;
		Level = level;
		Rate = rate;
	}
}
