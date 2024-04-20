public class RequestItemTransfer : Request
{
	public int CharItemID;

	public int FromBankID;

	public int ToBankID;

	public RequestItemTransfer()
	{
		type = 32;
		cmd = 2;
	}

	public RequestItemTransfer(int id, int from, int to)
	{
		type = 32;
		cmd = 2;
		CharItemID = id;
		FromBankID = from;
		ToBankID = to;
	}
}
