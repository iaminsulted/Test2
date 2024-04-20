using System.Collections.Generic;

public class ResponseDebug : Response
{
	public List<string> strings = new List<string>();

	public List<int> ints = new List<int>();

	public List<float> floats = new List<float>();

	public static int Count;

	public ResponseDebug()
	{
		type = 22;
	}
}
