public class ResponsePetInteract : Response
{
	public int playerID;

	public string animation;

	public ResponsePetInteract()
	{
		type = 17;
		cmd = 25;
	}

	public ResponsePetInteract(int playerID, string animation)
	{
		type = 17;
		cmd = 25;
		this.playerID = playerID;
		this.animation = animation;
	}
}
