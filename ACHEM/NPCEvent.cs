using System.Collections.Generic;
using UnityEngine;

public class NPCEvent : MonoBehaviour
{
	public int ID;

	public NPCEventType Type;

	public int SpellID;

	public int PathNodeID;

	public float HealthPercent;

	public List<MachineAction> Actions;
}
