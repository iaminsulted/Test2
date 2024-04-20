using System.Collections.Generic;
using Assets.Scripts.Game;
using StatCurves;
using UnityEngine;

public class PetTrigger : MonoBehaviour, IClickable, IInteractable
{
	private const float range = 7f;

	private const float warningDelay = 30f;

	private float clickDelay;

	private Entity owner;

	private List<string> animations;

	private bool hasClicked;

	private float elapsed;

	private float warningElapsed;

	private NPCIA itemActionApop;

	private ItemAction itemAction;

	private bool isFinishedLoading;

	private bool receivedCombatNotification;

	public void Init(Entity owner, List<string> animations)
	{
		this.owner = owner;
		this.animations = animations;
		Entity me = Entities.Instance.me;
		if (owner == me)
		{
			clickDelay = 0.5f;
		}
		else
		{
			clickDelay = 5f;
		}
	}

	private void Start()
	{
		base.gameObject.layer = Layers.CLICKIES;
	}

	private void Update()
	{
		if (hasClicked)
		{
			elapsed += Time.deltaTime;
			if (elapsed >= clickDelay)
			{
				hasClicked = false;
				elapsed = 0f;
			}
		}
		if (receivedCombatNotification)
		{
			warningElapsed += Time.deltaTime;
			if (warningElapsed >= 30f)
			{
				receivedCombatNotification = false;
				warningElapsed = 0f;
			}
		}
	}

	public void OnClick(Vector3 hitpoint)
	{
		if (hasClicked)
		{
			return;
		}
		Entity me = Entities.Instance.me;
		if (Vector3.Distance(me.wrapperTransform.position, base.transform.position) > 7f)
		{
			return;
		}
		if (me.serverState == Entity.State.InCombat && itemAction != null && !receivedCombatNotification)
		{
			Notification.ShowText("Cannot interact with pets during combat");
			receivedCombatNotification = true;
		}
		else if (me == owner)
		{
			InventoryItem equippedItem = Session.MyPlayerData.GetEquippedItem(EquipItemSlot.Pet);
			if (equippedItem != null)
			{
				itemAction = equippedItem.Action;
				Trigger();
			}
			SendPetInteractMessage();
		}
		else
		{
			SendPetInteractMessage();
			hasClicked = true;
		}
	}

	public void OnDoubleClick()
	{
	}

	public void OnPress(Game.InteractableRaycastResult raycastResult, bool state)
	{
	}

	public void OnHover()
	{
		CursorManager.Instance.SetCursor(CursorManager.Icon.Interact);
	}

	private void Trigger()
	{
		if (itemAction == null)
		{
			return;
		}
		switch (itemAction.Type)
		{
		case ItemActionType.Apop:
			if (itemActionApop == null)
			{
				ApopDownloader.GetApops(new List<int> { (itemAction as ItemActionApop).apopID }, OnLoadEndHandler);
			}
			else if (isFinishedLoading)
			{
				LoadApop();
			}
			break;
		case ItemActionType.Spell:
		case ItemActionType.TravelForm:
		case ItemActionType.Badge:
		case ItemActionType.Transfer:
			break;
		}
	}

	private void SendPetInteractMessage()
	{
		if (animations != null && animations.Count > 0)
		{
			AEC.getInstance().sendRequest(new RequestPetInteract(owner.ID, animations[Random.Range(0, animations.Count)]));
		}
	}

	private void OnLoadEndHandler(List<NPCIA> loadedApops)
	{
		if (loadedApops == null || loadedApops.Count == 0)
		{
			Debug.LogError("Fix in admin. Apop not found: " + (itemAction as ItemActionApop).apopID + " on item ID " + Session.MyPlayerData.GetEquippedItem(EquipItemSlot.Pet).ID);
			return;
		}
		NPCIA apop = ApopMap.GetApop((itemAction as ItemActionApop).apopID);
		if (apop != null)
		{
			itemActionApop = apop;
			isFinishedLoading = true;
			LoadApop();
		}
	}

	private void LoadApop()
	{
		UINPCDialog.Load(new List<NPCIA> { itemActionApop }, itemActionApop.Title, null, null);
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
