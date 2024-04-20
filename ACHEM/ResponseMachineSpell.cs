using System.Collections.Generic;

public class ResponseMachineSpell : Response
{
	public int SpellId;

	public List<ComEntityUpdate> EntityUpdates = new List<ComEntityUpdate>();
}
