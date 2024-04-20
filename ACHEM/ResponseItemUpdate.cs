public class ResponseItemUpdate : Response
{
	public int CharItemID;

	public int Quantity;

	public int BankID;

	public ResponseItemUpdate()
	{
		type = 10;
		cmd = 3;
	}

	public ResponseItemUpdate(int charItemID, int quantity, int bankID)
	{
		type = 10;
		cmd = 3;
		CharItemID = charItemID;
		Quantity = quantity;
		BankID = bankID;
	}
}
