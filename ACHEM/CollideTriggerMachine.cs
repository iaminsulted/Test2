using UnityEngine;

[AddComponentMenu("Interactivity/Machines/Collide Trigger Machine")]
public class CollideTriggerMachine : CollideMachine
{
	public CollisionMode collisionMode;

	public override void OnTriggerEnter(Collider other)
	{
		if (!Game.Instance.TesterMode && CanCollide() && other.gameObject.layer == Layers.PLAYER_ME && collisionMode == CollisionMode.Enter)
		{
			((ClientMovementController)Entities.Instance.me.moveController).BroadcastMovement(forcesync: true, jump: false, forceFullPacket: true);
			AEC.getInstance().sendRequest(new RequestMachineCollision(ID, isDb));
			playSfx();
		}
	}

	public override void OnTriggerExit(Collider other)
	{
		if (CanCollide() && other.gameObject.layer == Layers.PLAYER_ME && collisionMode == CollisionMode.Exit)
		{
			((ClientMovementController)Entities.Instance.me.moveController).BroadcastMovement(forcesync: true, jump: false, forceFullPacket: true);
			AEC.getInstance().sendRequest(new RequestMachineCollision(ID, isDb));
			playSfx();
		}
	}
}
