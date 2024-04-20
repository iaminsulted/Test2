namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseGloryXpUpdate : Response
{
	public int ClassID;

	public int GloryXP;

	public ResponseGloryXpUpdate()
	{
		type = 17;
		cmd = 32;
	}

	public ResponseGloryXpUpdate(int classID, int gloryXP)
	{
		type = 17;
		cmd = 32;
		ClassID = classID;
		GloryXP = gloryXP;
	}
}
