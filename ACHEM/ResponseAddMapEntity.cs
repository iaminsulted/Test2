public class ResponseAddMapEntity : Response
{
	public ComMapEntity Entity;

	public ComMachine comMachine;

	public ResponseAddMapEntity(ComMapEntity Entity, ComMachine comMachine)
	{
		type = 46;
		cmd = 26;
		this.Entity = Entity;
		this.comMachine = comMachine;
	}
}
