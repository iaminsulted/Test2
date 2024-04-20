public class RequestBankItems : Request
{
	public int RequestedBankID;

	public RequestBankItems()
	{
		type = 32;
		cmd = 1;
	}

	public RequestBankItems(int requestedID)
	{
		type = 32;
		cmd = 1;
		RequestedBankID = requestedID;
	}
}
