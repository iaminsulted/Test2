using System;

[Serializable]
public class InitTxnResponse
{
	public InitTxnR response;

	protected void AddResponse(InitTxnR rsp)
	{
		response = rsp;
	}
}
