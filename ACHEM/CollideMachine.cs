using System.Collections.Generic;
using UnityEngine;

public abstract class CollideMachine : InteractiveMachine
{
	public EntityCollision entityCollision;

	public List<NPCSpawn> collisionTargets = new List<NPCSpawn>();

	public bool isDb;

	public virtual void OnTriggerEnter(Collider other)
	{
	}

	public virtual void OnTriggerExit(Collider other)
	{
	}

	public virtual void OnTriggerStay(Collider other)
	{
	}

	public virtual void TriggerCTActionEnter()
	{
		foreach (ClientTriggerAction cTAction in CTActions)
		{
			cTAction.Execute();
		}
	}

	public virtual void TriggerCTActionExit()
	{
	}

	public virtual void TriggerCTActionStay()
	{
		foreach (ClientTriggerAction cTAction in CTActions)
		{
			cTAction.Execute();
		}
	}

	protected bool CanCollide()
	{
		return Entities.Instance.me.serverState != Entity.State.Dead;
	}
}
