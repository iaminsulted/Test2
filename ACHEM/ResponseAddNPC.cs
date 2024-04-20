public class ResponseAddNPC : Response
{
	public int SpawnListNpcID;

	public int SpawnID;

	public int NpcID;

	public string Name;

	public int Level;

	public int Reaction;

	public ResponseAddNPC(int SpawnListNpcID, int SpawnID, int NpcID, string Name, int Level, int Reaction)
	{
		type = 46;
		cmd = 15;
		this.SpawnListNpcID = SpawnListNpcID;
		this.SpawnID = SpawnID;
		this.NpcID = NpcID;
		this.Name = Name;
		this.Level = Level;
		this.Reaction = Reaction;
	}
}
