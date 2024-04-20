using Assets.Scripts.Game;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Triggers/Click Client Trigger")]
public class ClickClientTrigger : ClientTrigger, IClickable, IInteractable
{
	public string CastAnimation;

	public string CastMessage = "Interacting...";

	public float CastTime;

	public bool HideCastBar;

	private float currentCastTime;

	private bool doneCasting;

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
				doneCasting = true;
				Trigger(checkRequirements: true);
			}
		}
	}

	public override void Trigger(bool checkRequirements)
	{
		if (!CanInteract() || !IsInteractive())
		{
			return;
		}
		if (CastTime == 0f || (Session.MyPlayerData.CastMachineID == ID && doneCasting))
		{
			if (ChangeState)
			{
				SetState((State != 1) ? ((byte)1) : ((byte)0), ID);
			}
			foreach (ClientTriggerAction cTAction in CTActions)
			{
				cTAction.Execute();
			}
			if (TriggerAudioSource != null)
			{
				TriggerAudioSource.Play();
			}
			else if (!string.IsNullOrEmpty(SfxTrigger))
			{
				AudioManager.Play2DSFX(SfxTrigger);
			}
			if (!string.IsNullOrEmpty(Animation))
			{
				Entities.Instance.me.PlayAnimation(EntityAnimations.Get(Animation), 0f, isCancellableByMovement: true);
			}
			TryCancelCast();
		}
		else if (!Session.MyPlayerData.IsCastingMachine)
		{
			Entities.Instance.me.PlayAnimation(EntityAnimations.Get(CastAnimation), 0f, isCancellableByMovement: true);
			Session.MyPlayerData.CastMachineID = ID;
			Entities.Instance.me.moveController.Moved += OnMoved;
			Entities.Instance.me.moveController.Jumped += OnMoved;
			Entities.Instance.me.ServerStateChanged += OnServerStateChanged;
			currentCastTime = CastTime;
		}
	}

	public void OnClick(Vector3 hitpoint)
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

	public void OnSecondTouchPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
	}

	public void OnHover()
	{
		CursorManager.Instance.SetCursor(CursorManager.Icon.Interact);
	}

	private void OnMoved()
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
			}
			if (Session.MyPlayerData != null)
			{
				Session.MyPlayerData.CastMachineID = -1;
			}
			AEC.getInstance().sendRequest(new RequestMachineCast(-1));
			currentCastTime = 0f;
			doneCasting = false;
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
}
