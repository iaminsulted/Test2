using UnityEngine;

[AddComponentMenu("Interactivity/Machine Actions/MA Set CT State")]
public class MASetCTState : ListenerAction
{
	public ClientTrigger clientTrigger;

	public byte state;

	public bool onlyUser;
}
