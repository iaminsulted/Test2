public class ResponseMergeRemove : Response
{
	public int MergeID;

	public ResponseMergeRemove()
	{
		type = 28;
		cmd = 7;
	}
}
