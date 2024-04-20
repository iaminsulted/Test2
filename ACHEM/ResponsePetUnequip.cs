public class ResponsePetUnequip : Response
{
	public int EntityID;

	public ResponsePetUnequip()
	{
		type = 17;
		cmd = 20;
	}

	public ResponsePetUnequip(int entityID)
	{
		type = 17;
		cmd = 20;
		EntityID = entityID;
	}
}
