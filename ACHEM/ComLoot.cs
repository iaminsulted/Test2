using System.Collections.Generic;

public class ComLoot
{
	public int ID;

	public int AreaID;

	public int CellID;

	public ComVector3 Position;

	public float Duration;

	public List<LootItem> Items;

	public float timeStamp;

	public float TimeRemaining => Duration - (GameTime.realtimeSinceServerStartup - timeStamp);
}
