using System.Collections.Generic;

public class ResponseEntityClass : Response
{
	public List<int> Spells;

	public ResponseEntityClass()
	{
		type = 17;
		cmd = 6;
	}

	public ResponseEntityClass(List<int> spells)
	{
		type = 17;
		cmd = 6;
		Spells = spells;
	}
}
