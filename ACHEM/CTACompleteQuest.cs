using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Complete Quest")]
public class CTACompleteQuest : ClientTriggerAction
{
	public int ID;

	protected override void OnExecute()
	{
		Game.Instance.SendQuestCompleteRequest(ID);
	}
}
