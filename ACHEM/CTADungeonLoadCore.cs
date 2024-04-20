using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Dungeon Load")]
public class CTADungeonLoadCore : ClientTriggerActionCore
{
	[HideInInspector]
	public int ID;

	[ReadOnly]
	public string UniqueID;

	public List<int> DungeonIDs = new List<int>();

	protected override void OnExecute()
	{
		UIDungeons.Load(DungeonIDs);
	}
}
