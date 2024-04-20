using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Interactivity/Machines/Owner Machine")]
public class OwnerMachine : SwitchMachine
{
	protected static Vector3 defaultPosition = new Vector3(-0.039f, 0.281f, 1.385f);

	public Transform DisownPosition;

	public Transform OwnPosition;

	protected Vector3 disownPosition;

	protected Quaternion disownRotation;

	protected Vector3 ownPosition;

	protected Quaternion ownRotation;

	private int ownerID;

	private Entity owner;

	protected Action OwnLocally;

	protected Action DisownLocally;

	public Entity Owner => owner;

	public virtual void Start()
	{
		SetMount();
		SetDismount();
		SetOwner();
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		if (owner != null && owner.wrapper != null)
		{
			owner.assetController.AssetUpdated -= OnAssetUpdated;
			owner.entitycontroller.CharacterController.enabled = true;
			owner.entitycontroller.CancelAction();
			owner.OwnedMachine = null;
		}
	}

	public override void Init(byte state, int ownerID, List<int> areas)
	{
		this.ownerID = ownerID;
		base.Init(state, ownerID, areas);
	}

	public override void SetState(byte state, int entityID = 0)
	{
		if (state == 1)
		{
			Disown();
		}
		else
		{
			Own(Entities.Instance.GetEntity(Entity.Type.Player, entityID));
		}
		base.SetState(state, entityID);
	}

	protected void OnAssetUpdated(GameObject obj)
	{
		owner.assetController.AssetUpdated -= OnAssetUpdated;
		owner.PlayAnimation(EntityAnimations.Get(Animation));
		owner.entitycontroller.CharacterController.enabled = false;
		owner.wrapper.transform.position = ownPosition;
		owner.wrapper.transform.rotation = ownRotation;
		owner.OwnedMachine = this;
		if (owner.isMe)
		{
			OwnLocally?.Invoke();
		}
	}

	protected override bool IsInteractive()
	{
		if (owner == null && !isOnCooldown)
		{
			return IsRequirementMet();
		}
		return false;
	}

	protected override bool CanInteract()
	{
		if (Entities.Instance.me.IsInTransform)
		{
			Notification.ShowWarning("Cannot interact with this while transformed.");
			return false;
		}
		return base.CanInteract();
	}

	private void Disown()
	{
		if (owner != null && owner.wrapper != null)
		{
			owner.assetController.AssetUpdated -= OnAssetUpdated;
			owner.entitycontroller.CharacterController.enabled = true;
			owner.entitycontroller.CancelAction();
			owner.entitycontroller.StopDash();
			owner.wrapper.transform.position = disownPosition;
			owner.wrapper.transform.rotation = disownRotation;
			owner.OwnedMachine = null;
			if (owner.isMe)
			{
				DisownLocally?.Invoke();
			}
		}
		owner = null;
	}

	private void Own(Entity entity)
	{
		owner = entity;
		if (owner != null && owner.wrapper != null)
		{
			owner.assetController.AssetUpdated += OnAssetUpdated;
			owner.PlayAnimation(EntityAnimations.Get(Animation));
			owner.entitycontroller.StopDash();
			owner.entitycontroller.CharacterController.enabled = false;
			owner.wrapper.transform.position = ownPosition;
			owner.wrapper.transform.rotation = ownRotation;
			owner.OwnedMachine = this;
			if (owner.isMe)
			{
				OwnLocally?.Invoke();
			}
		}
	}

	private void SetDismount()
	{
		if (DisownPosition == null)
		{
			disownPosition = defaultPosition;
			disownRotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
		}
		else
		{
			disownPosition = DisownPosition.position;
			disownRotation = Quaternion.Euler(0f, DisownPosition.rotation.eulerAngles.y, 0f);
		}
	}

	private void SetMount()
	{
		if (OwnPosition == null)
		{
			ownPosition = base.transform.position;
			ownRotation = Quaternion.Euler(0f, base.transform.rotation.eulerAngles.y, 0f);
		}
		else
		{
			ownPosition = OwnPosition.position;
			ownRotation = Quaternion.Euler(0f, OwnPosition.rotation.eulerAngles.y, 0f);
		}
	}

	private void SetOwner()
	{
		if (ownerID > 0)
		{
			owner = Entities.Instance.GetEntity(Entity.Type.Player, ownerID);
			if (owner != null)
			{
				owner.assetController.AssetUpdated += OnAssetUpdated;
			}
		}
	}
}
