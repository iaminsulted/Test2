internal class RequestEditApopText : Request
{
	public int apopID;

	public string updatedText;

	public RequestEditApopText()
	{
		type = 34;
		cmd = 8;
	}

	public RequestEditApopText(int _apopID, string _updatedText)
	{
		type = 34;
		cmd = 8;
		updatedText = _updatedText;
		apopID = _apopID;
	}
}
