using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Dungeon Load")]
public class CTADungeonLoad : ClientTriggerAction
{
	[HideInInspector]
	public int ID;

	[ReadOnly]
	public string UniqueID;

	public List<int> DungeonIDs = new List<int>();

	public TransferPadControl transferPad;

	private void Awake()
	{
		transferPad = base.gameObject.AddComponent<TransferPadControl>();
		transferPad.UniqueID = UniqueID;
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
		UIDungeons.Load(DungeonIDs);
	}

	public void UpdateTransferPadMapID()
	{
		transferPad.Areas.AddRange(DungeonIDs);
	}
}
