using System;
using System.Collections.Generic;

public class ApopRequest
{
	public readonly List<int> ids;

	public readonly Action<List<NPCIA>> SetApops;

	public bool IsComplete;

	public ApopRequest(List<int> apopIds, Action<List<NPCIA>> setApopsAction)
	{
		ids = apopIds;
		SetApops = setApopsAction;
		IsComplete = false;
	}
}
