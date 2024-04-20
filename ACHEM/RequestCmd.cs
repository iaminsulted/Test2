using System.Collections.Generic;

public class RequestCmd : Request
{
	public List<string> args = new List<string>();

	public Entity.Type etype;

	public int eID;

	public int questID;

	public int objectiveID;

	public RequestCmd()
	{
		type = 9;
	}
}
