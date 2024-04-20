using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set Machine State")]
public class MASetMachineStateList : ListenerAction
{
	public byte state;

	public List<BaseMachine> machines;
}
