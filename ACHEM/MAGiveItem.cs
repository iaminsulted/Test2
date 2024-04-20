using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Give Item")]
public class MAGiveItem : MachineAction
{
	public int ItemID;

	public int Quantity;

	public bool AutoEquip;
}
