public class RequestJoinQueue : Request
{
	public int queueID;

	public string password;

	public bool createPrivate;

	public RequestJoinQueue(int queueID, string password = "", bool createPrivate = false)
	{
		type = 36;
		cmd = 4;
		this.queueID = queueID;
		this.password = password;
		this.createPrivate = createPrivate;
	}
}
