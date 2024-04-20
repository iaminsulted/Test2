using System.Collections.Generic;

public class ResponseMachineListenerUpdate : Response
{
	public List<ComMachineListener> listeners;

	public ResponseMachineListenerUpdate()
	{
		type = 19;
		cmd = 16;
	}

	public ResponseMachineListenerUpdate(List<ComMachineListener> listeners)
	{
		type = 19;
		cmd = 16;
		this.listeners = listeners;
	}
}
