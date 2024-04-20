using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Load Class UI")]
public class CTALoadClassUI : ClientTriggerAction
{
	public int ClassID;

	protected override void OnExecute()
	{
		UICharClasses.LoadByID(ClassID);
	}
}
