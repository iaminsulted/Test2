using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Random Action")]
public class MARandomAction : ListenerAction
{
	public List<MachineAction> Actions;

	public List<int> Rates;

	public bool Exclusive;
}
