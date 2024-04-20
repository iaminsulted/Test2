public class RequestOpenInAdmin : Request
{
	public int NpcID;

	public RequestOpenInAdmin(int NpcID)
	{
		type = 46;
		cmd = 20;
		this.NpcID = NpcID;
	}
}
