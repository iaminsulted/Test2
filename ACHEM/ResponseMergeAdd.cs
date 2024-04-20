using System;

public class ResponseMergeAdd : Response
{
	public Merge merge;

	public DateTime TSComplete;

	public ResponseMergeAdd()
	{
		type = 28;
		cmd = 6;
	}
}
