public class ResponseTradeSell : Response
{
	public int CharItemID;

	public int Amount;

	public short Quantity = 1;

	public ResponseTradeSell()
	{
		type = 11;
		cmd = 2;
	}

	public ResponseTradeSell(int charItemID, int amount, short quantity)
	{
		type = 11;
		cmd = 2;
		CharItemID = charItemID;
		Amount = amount;
		Quantity = quantity;
	}
}
