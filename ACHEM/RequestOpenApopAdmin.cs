using System.Collections.Generic;

public class RequestOpenApopAdmin : Request
{
	public List<int> Apops;

	public RequestOpenApopAdmin(List<int> Apops)
	{
		type = 46;
		cmd = 22;
		this.Apops = new List<int>(Apops);
	}
}
