using System.Collections.Generic;

public class ResponseCommandDialog : Response
{
	public List<string> args;

	public ResponseCommandDialog()
	{
		type = 34;
		cmd = 2;
	}
}
