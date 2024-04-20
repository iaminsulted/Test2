using System.Collections.Generic;

public class ResponseCellJoin : Response
{
	public int areaID;

	public int cellID;

	public int scoreTarget;

	public Dictionary<int, int> teamScores = new Dictionary<int, int>();

	public float posX;

	public float posY;

	public float posZ;

	public float rotY;

	public Dictionary<int, List<int>> NpcSpawnInfos = new Dictionary<int, List<int>>();

	public List<ComEntity> entities = new List<ComEntity>();

	public List<ComMachine> machines = new List<ComMachine>();

	public List<ComMachineListener> listeners = new List<ComMachineListener>();

	public List<KeyValuePair<string, string>> areaFlags = new List<KeyValuePair<string, string>>();

	public List<LocatorTransfer> LocatorTransfers = new List<LocatorTransfer>();

	public List<ComLoot> lootBags = new List<ComLoot>();

	public List<ComSpawnMeta> spawnMetas = new List<ComSpawnMeta>();

	public List<ComMapEntity> mapEntities = new List<ComMapEntity>();

	public Dictionary<int, List<string>> NpcSfxGreetings;

	public Dictionary<int, List<string>> NpcSfxFarewells;

	public bool isTimerPaused;

	public float timerDuration;

	public string timerDescription;

	public float timerTimeStamp;

	public int soundTrackID;
}
