using UnityEngine;

[AddComponentMenu("Interactivity/Machines/Parent Machine")]
public class ParentMachine : CollideMachine
{
	public float velocityThreshold;

	private GameObject target;

	private void Start()
	{
		target = new GameObject();
		target.name = "ParentTarget";
		target.transform.parent = base.transform;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		EntityController entityController = Entities.Instance.me?.entitycontroller;
		if (entityController != null && entityController.transform.parent == target.transform)
		{
			entityController.transform.parent = entityController.originalParent;
		}
	}

	public override void OnTriggerEnter(Collider other)
	{
		EntityController component = other.gameObject.GetComponent<EntityController>();
		if (component != null)
		{
			other.transform.parent = target.transform;
			if (component.Entity.isMe)
			{
				((ClientMovementController)Entities.Instance.me.moveController).BroadcastMovement(forcesync: true, jump: false, forceFullPacket: true);
				AEC.getInstance().sendRequest(new RequestMachineCollision(ID, isDb));
			}
		}
		else if (other.gameObject.GetComponent<PetMovementController>() != null)
		{
			other.transform.parent = target.transform;
		}
	}

	public override void OnTriggerExit(Collider other)
	{
		EntityController component = other.gameObject.GetComponent<EntityController>();
		if (component != null)
		{
			other.transform.parent = component.originalParent;
			if (component.Entity.isMe)
			{
				((ClientMovementController)Entities.Instance.me.moveController).BroadcastMovement(forcesync: true, jump: false, forceFullPacket: true);
				AEC.getInstance().sendRequest(new RequestMachineCollision(ID, isDb));
			}
		}
		else
		{
			PetMovementController component2 = other.gameObject.GetComponent<PetMovementController>();
			if (component2 != null)
			{
				other.transform.parent = component2.originalParent;
			}
		}
	}
}
