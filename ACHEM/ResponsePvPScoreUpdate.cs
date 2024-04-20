using System.Collections.Generic;

internal class ResponsePvPScoreUpdate : Response
{
	public List<int> values;

	public bool start;

	public bool end;

	public ResponsePvPScoreUpdate(List<int> vals, bool _start, bool _end)
	{
		values = vals;
		start = _start;
		end = _end;
		type = 20;
		cmd = 8;
	}
}
