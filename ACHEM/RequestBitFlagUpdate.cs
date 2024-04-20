public class RequestBitFlagUpdate : Request
{
	public string Name;

	public byte Index;

	public bool Value;

	public RequestBitFlagUpdate()
	{
		type = 17;
		cmd = 9;
	}

	public RequestBitFlagUpdate(string name, byte index, bool value)
	{
		type = 17;
		cmd = 9;
		Name = name;
		Index = index;
		Value = value;
	}
}
