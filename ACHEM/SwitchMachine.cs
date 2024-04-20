using System.Collections;
using UnityEngine;

[AddComponentMenu("Interactivity/Machines/Switch Machine")]
public class SwitchMachine : ClickMachine
{
	public float Cooldown;

	protected bool isOnCooldown;

	public override void SetState(byte state, int entityID = 0)
	{
		StartCoroutine(StartCooldown());
		base.SetState(state, entityID);
	}

	protected override bool IsInteractive()
	{
		if (IsRequirementMet())
		{
			return !isOnCooldown;
		}
		return false;
	}

	private IEnumerator StartCooldown()
	{
		isOnCooldown = true;
		base.IsActive = IsInteractive();
		yield return new WaitForSeconds(Cooldown);
		isOnCooldown = false;
		base.IsActive = IsInteractive();
	}
}
