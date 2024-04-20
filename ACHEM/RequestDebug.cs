using System.Collections.Generic;

public class RequestDebug : Request
{
	public List<string> strings = new List<string>();

	public List<int> ints = new List<int>();

	public List<float> floats = new List<float>();

	public RequestDebug()
	{
		type = 22;
	}
}
