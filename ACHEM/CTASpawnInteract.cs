using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Spawn Interact")]
public class CTASpawnInteract : ClientTriggerAction
{
	public string Title;

	public NPCSpawn NPCSpawn;

	protected override void OnExecute()
	{
		Entities.Instance.GetNpcBySpawnId(NPCSpawn.ID)?.NPCInteract();
	}
}
