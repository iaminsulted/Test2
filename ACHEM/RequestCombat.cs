using System.ComponentModel;

public class RequestCombat : Request
{
	public int ID;

	[DefaultValue(0)]
	public RequestCombatType rct;

	public int spellTemplateID;

	public int targetID;

	public Entity.Type targetType;

	[DefaultValue(0)]
	public int charItemID;

	[DefaultValue(0f)]
	public float posX;

	[DefaultValue(0f)]
	public float posY;

	[DefaultValue(0f)]
	public float posZ;

	public RequestCombat(byte combatCommand)
	{
		type = 12;
		cmd = combatCommand;
	}
}
