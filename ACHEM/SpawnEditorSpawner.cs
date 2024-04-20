using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/SpawnEditorSpawner")]
public class SpawnEditorSpawner : ClientTriggerAction
{
	public int SpawnID;

	protected override void OnExecute()
	{
		if (UINPCEditor.Instance != null)
		{
			UINPCEditor.Instance.OpenSpawnID(SpawnID);
		}
	}
}
