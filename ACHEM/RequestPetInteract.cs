public class RequestPetInteract : Request
{
	public int playerID;

	public string animation;

	public RequestPetInteract()
	{
		type = 17;
		cmd = 25;
	}

	public RequestPetInteract(int playerID, string animation)
	{
		type = 17;
		cmd = 25;
		this.playerID = playerID;
		this.animation = animation;
	}
}
