using System.Collections.Generic;

public class ResponseCombatEffectPulse : Response
{
	public int code;

	public Entity.Type casterType;

	public int casterID;

	public List<Entity.Type> targetTypes;

	public List<int> targetIDs;

	public int effectTemplateID;

	public List<ComEntityUpdate> entityUpdates;
}
