using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Trigger Actions/CTA Transfer Random")]
public class CTATransferRandomCore : ClientTriggerActionCore
{
	public List<int> MapIDs = new List<int>();

	protected override void OnExecute()
	{
		int id = MapIDs[Random.Range(0, MapIDs.Count)];
		Game.Instance.SendAreaJoinRequest(id);
	}
}
