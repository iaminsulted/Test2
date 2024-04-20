using System.Collections.Generic;
using Assets.Scripts.Game;
using UnityEngine;

[AddComponentMenu("Interactivity/Machines/Click Machine")]
public class ClickMachine : InteractiveMachine, IClickable, IInteractable
{
	public List<string> CastAnimations = new List<string>();

	public string CastAnimation;

	public string CastMessage = "Interacting...";

	public float CastTime;

	public bool HideCastBar;

	private float currentCastTime;

	private bool isCasting;

	public override void OnDestroy()
	{
		if (isCasting)
		{
			TryCancelCast();
		}
		base.OnDestroy();
	}

	private void Update()
	{
		if (currentCastTime > 0f)
		{
			currentCastTime -= Time.deltaTime;
			if (!HideCastBar)
			{
				ProgressBar.Show((CastTime - currentCastTime) / CastTime, CastMessage);
			}
			if (currentCastTime <= 0f)
			{
				Trigger(checkRequirements: true);
			}
		}
	}

	public string GetCastAnimation()
	{
		if (CastAnimations.Count > 1)
		{
			return CastAnimations[Random.Range(0, CastAnimations.Count)];
		}
		return CastAnimation;
	}

	public override void Trigger(bool checkRequirements)
	{
		if (!CanInteract() || !IsInteractive())
		{
			return;
		}
		playSfx();
		if (CastTime == 0f || (Session.MyPlayerData.CastMachineID == ID && isCasting && currentCastTime <= 0f))
		{
			AEC.getInstance().sendRequest(new RequestMachineClick(ID));
			foreach (ClientTriggerAction cTAction in CTActions)
			{
				cTAction.Execute();
			}
			if (!string.IsNullOrEmpty(SfxTrigger))
			{
				AudioManager.Play2DSFX(SfxTrigger);
			}
			TryCancelCast();
		}
		else if (!Session.MyPlayerData.IsCastingMachine)
		{
			Entities.Instance.me.PlayAnimation(EntityAnimations.Get(GetCastAnimation()), 0f, isCancellableByMovement: true);
			Entities.Instance.me.Target = null;
			Session.MyPlayerData.CastMachineID = ID;
			AEC.getInstance().sendRequest(new RequestMachineCast(ID));
			Entities.Instance.me.moveController.Moved += OnMoved;
			Entities.Instance.me.moveController.Jumped += OnMoved;
			Entities.Instance.me.ServerStateChanged += OnServerStateChanged;
			Game.Instance.combat.ClearQueuedSpell();
			Game.Instance.combat.CastRequest += OnCast;
			currentCastTime = CastTime;
			isCasting = true;
		}
	}

	public virtual void OnClick(Vector3 hitpoint)
	{
		if ((base.transform.position - Entities.Instance.me.wrapper.transform.position).magnitude <= Distance)
		{
			Trigger(checkRequirements: true);
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

	protected override void OnActiveUpdated(bool active)
	{
		base.OnActiveUpdated(active);
		if (!active && isCasting)
		{
			TryCancelCast();
			Notification.ShowText(CastMessage + " failed.");
		}
	}

	private void OnMoved()
	{
		TryCancelCast();
	}

	private void OnCast(SpellTemplate template)
	{
		TryCancelCast();
	}

	private void OnServerStateChanged(Entity.State prevState, Entity.State newState)
	{
		if (newState == Entity.State.Dead || newState == Entity.State.InCombat)
		{
			TryCancelCast();
		}
	}

	private void TryCancelCast()
	{
		if (CastTime != 0f)
		{
			ProgressBar.Hide();
			if (Entities.Instance.me != null)
			{
				Entities.Instance.me.entitycontroller.CancelAction();
				Entities.Instance.me.moveController.Moved -= OnMoved;
				Entities.Instance.me.moveController.Jumped -= OnMoved;
				Entities.Instance.me.ServerStateChanged -= OnServerStateChanged;
				Game.Instance.combat.CastRequest -= OnCast;
			}
			if (Session.MyPlayerData != null)
			{
				Session.MyPlayerData.CastMachineID = -1;
			}
			AEC.getInstance().sendRequest(new RequestMachineCast(-1));
			currentCastTime = 0f;
			isCasting = false;
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
