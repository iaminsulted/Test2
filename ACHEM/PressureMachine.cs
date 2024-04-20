using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machines/Pressure Machine")]
public class PressureMachine : CollideTriggerMachine
{
	public List<MachineAction> exitActions;

	public List<ClientTriggerAction> exitCTActions;

	public List<MachineAction> stayActions;

	public List<ClientTriggerAction> stayCTActions;

	public float stayDuration;

	public bool stayTriggerOnce;

	public int targetCount;

	private float elapsed;

	public void LoadExitAndStayActionDBData(string jsonExitActions, string jsonExitCTActions, string jsonStayActions, string jsonStayCTActions)
	{
		LoadActions(jsonExitActions, exitActions);
		LoadCTActions(jsonExitCTActions, exitCTActions);
		LoadActions(jsonStayActions, stayActions);
		LoadCTActions(jsonStayCTActions, stayCTActions);
	}

	public override void OnTriggerStay(Collider other)
	{
		if (CanCollide() && other.gameObject.layer == Layers.PLAYER_ME)
		{
			elapsed += Time.deltaTime;
			if (elapsed >= stayDuration)
			{
				((ClientMovementController)Entities.Instance.me.moveController).BroadcastMovement(forcesync: true);
				elapsed = 0f;
			}
		}
	}

	public override void TriggerCTActionExit()
	{
		foreach (ClientTriggerAction exitCTAction in exitCTActions)
		{
			exitCTAction.Execute();
		}
	}

	public override void TriggerCTActionStay()
	{
		foreach (ClientTriggerAction stayCTAction in stayCTActions)
		{
			stayCTAction.Execute();
		}
	}
}
