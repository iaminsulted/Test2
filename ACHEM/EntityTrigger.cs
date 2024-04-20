using Assets.Scripts.Game;
using UnityEngine;

public class EntityTrigger : MonoBehaviour, IClickable, IInteractable
{
	private Entity entity;

	public void Awake()
	{
		base.gameObject.layer = Layers.CLICKIES;
	}

	public void Start()
	{
		RetrieveEntity();
	}

	public void OnDestroy()
	{
		if (entity != null)
		{
			entity.ServerStateChanged -= OnEntityDeath;
		}
	}

	public void OnClick(Vector3 hitpoint)
	{
		if (entity == null)
		{
			Start();
		}
		entity?.Interact();
	}

	public void OnDoubleClick()
	{
		if (entity == null)
		{
			Start();
		}
		if (entity != null)
		{
			entity.Interact();
			if (Entities.Instance.me.CanAttack(entity))
			{
				Game.Instance.ApplyAction(InputAction.Spell_1);
			}
		}
	}

	public void OnPress(Game.InteractableRaycastResult raycastResult, bool state)
	{
	}

	public void OnHover()
	{
		if (Entities.Instance.me.CanAttack(entity))
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Combat);
		}
		else if (entity is NPC nPC && (nPC.npciatrigger?.CurrentApop != null || nPC.HasTalkObjective))
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Talk);
		}
		else
		{
			CursorManager.Instance.SetCursor(CursorManager.Icon.Interact);
		}
	}

	public void RetrieveEntity()
	{
		EntityController componentInParent = GetComponentInParent<EntityController>();
		if (componentInParent != null)
		{
			entity = componentInParent.Entity;
			if (entity.isMe)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				entity.ServerStateChanged += OnEntityDeath;
			}
		}
	}

	private void OnEntityDeath(Entity.State previousState, Entity.State newState)
	{
		if (this != null && GetComponent<Collider>() != null)
		{
			GetComponent<Collider>().enabled = newState != Entity.State.Dead;
		}
	}

	public bool IsInteractable(Game.InteractableRaycastResult raycastResult)
	{
		return true;
	}

	public int GetPriority()
	{
		return 0;
	}

	public void OnSecondTouchPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
	}
}
