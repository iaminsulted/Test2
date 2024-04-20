using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Transfer Map")]
public class CTATransferMapCore : ClientTriggerActionCore
{
	[HideInInspector]
	public int ID;

	[ReadOnly]
	public string UniqueID;

	public int MapID;

	public int CellID;

	public int SpawnID;

	public bool ShowConfirmation;

	public TransferPadControl transferPad;

	private void Awake()
	{
	}

	private void OnDestroy()
	{
		if (transferPad != null && Game.Instance != null && Game.Instance.transferPads != null && Game.Instance.transferPads.Contains(transferPad))
		{
			Game.Instance.transferPads.Remove(transferPad);
		}
	}

	protected override void OnExecute()
	{
		Game.Instance.SendTransferMapRequest(MapID, CellID, SpawnID, ShowConfirmation);
	}

	public void UpdateTransferPadMapID()
	{
	}
}
