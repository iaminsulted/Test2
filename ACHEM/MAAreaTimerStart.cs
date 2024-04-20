using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Area Timer Start")]
public class MAAreaTimerStart : ListenerAction
{
	public string description;

	public float time;

	public bool showUI;

	public List<Machine> machines;
}
