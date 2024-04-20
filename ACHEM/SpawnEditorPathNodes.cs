using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/SpawnEditorPathNodes")]
public class SpawnEditorPathNodes : ClientTriggerAction
{
	public int SpawnID;

	protected override void OnExecute()
	{
		if (UINPCEditor.Instance != null)
		{
			UINPCEditor.Instance.OpenPath(SpawnID);
		}
	}
}
