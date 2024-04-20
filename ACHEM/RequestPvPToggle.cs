public class RequestPvPToggle : Request
{
	public bool isPvPFlagged;

	public RequestPvPToggle(bool isPvPFlagged)
	{
		type = 20;
		cmd = 1;
		this.isPvPFlagged = isPvPFlagged;
	}
}
