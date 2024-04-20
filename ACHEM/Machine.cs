using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machines/Machine")]
public class Machine : BaseMachine
{
	public bool IsAutoTrigger;

	public List<ListenerAction> Actions = new List<ListenerAction>();

	public AreaEvent AreaEvent;

	public override void Trigger(bool checkRequirements)
	{
		AEC.getInstance().sendRequest(new RequestMachineTrigger(ID));
	}
}
