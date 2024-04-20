using System.Collections.Generic;

namespace Assets.Scripts.NetworkClient.CommClasses;

public class ResponseResetCD : Response
{
	public List<int> spellTemplateIDs;

	public float cooldown;

	public float timeStamp;

	public ResponseResetCD()
	{
		type = 12;
		cmd = 10;
	}
}
