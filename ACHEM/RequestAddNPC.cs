public class RequestAddNPC : Request
{
	public int SpawnID;

	public int NpcID;

	public int Level;

	public string Name;

	public int Reaction;

	public RequestAddNPC(int SpawnID, int NpcID, int Level, string Name, int Reaction)
	{
		type = 46;
		cmd = 6;
		this.SpawnID = SpawnID;
		this.NpcID = NpcID;
		this.Level = Level;
		this.Name = Name;
		this.Reaction = Reaction;
	}
}
