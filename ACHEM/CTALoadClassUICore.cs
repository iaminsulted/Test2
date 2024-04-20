using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Load Class UI")]
public class CTALoadClassUICore : ClientTriggerActionCore
{
	public int ClassID;

	protected override void OnExecute()
	{
		UICharClasses.LoadByID(ClassID);
	}
}
