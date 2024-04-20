public class RequestItemRemove : Request
{
	public int CharItemID;

	public int BankID;

	public RequestItemRemove()
	{
		type = 10;
		cmd = 4;
	}

	public RequestItemRemove(int charItemID, int bankID)
	{
		type = 10;
		cmd = 4;
		CharItemID = charItemID;
		BankID = bankID;
	}
}
