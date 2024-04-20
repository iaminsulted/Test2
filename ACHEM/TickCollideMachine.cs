using UnityEngine;

[AddComponentMenu("Interactivity/Machines/Tick Collide Machine")]
public class TickCollideMachine : CollideMachine
{
	public float TickRate;

	private float elapsed;

	public override void OnTriggerEnter(Collider other)
	{
		if (CanCollide() && other.gameObject.layer == Layers.PLAYER_ME)
		{
			elapsed = 0f;
			((ClientMovementController)Entities.Instance.me.moveController).BroadcastMovement(forcesync: true);
		}
	}

	public override void OnTriggerStay(Collider other)
	{
		if (CanCollide() && other.gameObject.layer == Layers.PLAYER_ME)
		{
			elapsed += Time.deltaTime;
			if (elapsed >= TickRate)
			{
				((ClientMovementController)Entities.Instance.me.moveController).BroadcastMovement(forcesync: true);
				elapsed = 0f;
			}
		}
	}
}
