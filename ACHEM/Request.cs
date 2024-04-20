public class Request
{
	public byte type = byte.MaxValue;

	public byte cmd = byte.MaxValue;

	public Request()
	{
	}

	public Request(byte type, byte cmd)
	{
		this.type = type;
		this.cmd = cmd;
	}
}
