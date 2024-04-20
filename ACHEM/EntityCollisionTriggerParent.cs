using System.Collections.Generic;
using UnityEngine;

public class EntityCollisionTriggerParent : EntityCollisionTrigger
{
	private GameObject target;

	private List<EntityController> entities = new List<EntityController>();

	private void Start()
	{
		target = new GameObject();
		target.name = "ParentTarget";
		target.transform.parent = base.transform;
	}

	private void OnTriggerEnter(Collider other)
	{
		EntityController component = other.gameObject.GetComponent<EntityController>();
		if (component != null && CheckEntityCollision(component.Entity))
		{
			other.transform.parent = target.transform;
			entities.Add(component);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		EntityController component = other.gameObject.GetComponent<EntityController>();
		if (component != null && CheckEntityCollision(component.Entity))
		{
			other.transform.parent = component.originalParent;
			entities.Remove(component);
		}
	}

	private void OnDestroy()
	{
		foreach (EntityController entity in entities)
		{
			if (entity != null && entity.transform.parent == target.transform)
			{
				entity.transform.parent = entity.originalParent;
			}
		}
	}
}
