using System.Collections.Generic;

public class ResponseEditNPC : Response
{
	public int relSpawnID;

	public int relNpcID;

	public int SpawnID;

	public float Respawn;

	public float Despawn;

	public float Aggro;

	public float Leash;

	public bool AllowRegen;

	public bool AutoChain;

	public float Chain;

	public int MoveOverride;

	public int SpawnListNpcID;

	public int NpcID;

	public string Name;

	public int Level;

	public float Rate;

	public List<int> Apops;

	public int TeamID;

	public Dictionary<string, string> AnimationOverrides;

	public int ReactionOverride;

	public int Speed;

	public bool UseRotation;

	public string Animations;

	public bool SequentialAnimations;

	public float MinTime;

	public float MaxTime;

	public ResponseEditNPC(int SpawnListNpcID, int relSpawnID, int relNpcID)
	{
		type = 46;
		cmd = 17;
		this.SpawnListNpcID = SpawnListNpcID;
		this.relSpawnID = relSpawnID;
		this.relNpcID = relNpcID;
	}
}
