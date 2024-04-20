public class ResponseItemRemove : Response
{
	public int CharItemID;

	public int BankID;

	public ResponseItemRemove()
	{
		type = 10;
		cmd = 4;
	}

	public ResponseItemRemove(int charItemID, int bankID)
	{
		type = 10;
		cmd = 4;
		CharItemID = charItemID;
		BankID = bankID;
	}
}
