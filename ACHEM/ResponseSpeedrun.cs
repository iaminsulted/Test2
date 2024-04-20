public class ResponseSpeedrun : Response
{
	public ResponseSpeedrun(int type)
	{
		base.type = 7;
		if (type == 1)
		{
			cmd = 8;
		}
		else
		{
			cmd = 9;
		}
	}
}
