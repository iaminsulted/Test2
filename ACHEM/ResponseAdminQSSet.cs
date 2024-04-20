public class ResponseAdminQSSet : Response
{
	public int index;

	public int value;

	public ResponseAdminQSSet()
	{
		type = 34;
		cmd = 1;
	}

	public ResponseAdminQSSet(int index, int value)
	{
		type = 34;
		cmd = 1;
		this.index = index;
		this.value = value;
	}
}
