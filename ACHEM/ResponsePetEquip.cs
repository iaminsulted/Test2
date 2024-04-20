public class ResponsePetEquip : Response
{
	public int EntityID;

	public EquipItem PetEquipItem;

	public ResponsePetEquip()
	{
		type = 17;
		cmd = 19;
	}

	public ResponsePetEquip(int entityID, EquipItem petEquipItem)
	{
		type = 17;
		cmd = 19;
		EntityID = entityID;
		PetEquipItem = petEquipItem;
	}
}
