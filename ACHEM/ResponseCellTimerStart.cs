public class ResponseCellTimerStart : Response
{
	public string description;

	public float time;

	public ResponseCellTimerStart(string description, float time)
	{
		type = 8;
		cmd = 8;
		this.description = description;
		this.time = time;
	}
}
