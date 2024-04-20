using System.Collections.Generic;

public class ComSpawnMeta
{
	public List<ComNPCMeta> Spawns = new List<ComNPCMeta>();

	public int SpawnID;

	public bool IsDB;

	public bool AllowRegeneration;

	public float RespawnTime;

	public float DespawnTime;

	public float AggroRadius;

	public float LeashRadius;

	public bool AutoSpawn;

	public bool AutoChain;

	public float ChainRadius;

	public int MoveOverride;

	public int Speed;

	public bool UseRotation;

	public string Animations;

	public bool SequentialAnimations;

	public float MinTime;

	public float MaxTime;

	public string Requirements;

	public Dictionary<int, ComVector3> Path = new Dictionary<int, ComVector3>();

	public Dictionary<int, int> RotationY = new Dictionary<int, int>();

	public ComSpawnMeta(int SpawnID, bool IsDB)
	{
		this.SpawnID = SpawnID;
		this.IsDB = IsDB;
	}
}
