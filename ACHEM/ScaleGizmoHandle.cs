using System;
using UnityEngine;

public class ScaleGizmoHandle : GizmoHandle, IDraggable, IInteractable, IClickable
{
	private bool initialized;

	private Vector3 startScale;

	private float scaleAmount;

	private Vector3 forward;

	private Vector3 hitPos;

	public Action<ScaleGizmoHandle> onUpdate;

	private const float SCALE_SPEED = 0.003f;

	public Vector3 GetNewTargetGlobalScale(Transform target)
	{
		if (!initialized)
		{
			startScale = target.localScale;
			initialized = true;
		}
		return startScale + startScale * scaleAmount;
	}

	public Vector3 GetNewTargetLocalScale(Transform target)
	{
		if (!initialized)
		{
			startScale = target.localScale;
			initialized = true;
		}
		return startScale + target.InverseTransformDirection(base.transform.forward * scaleAmount);
	}

	public void OnDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult)
	{
		Vector2 vector = Game.Instance.cam.WorldToScreenPoint(hitPos);
		Vector2 normalized = ((Vector2)Game.Instance.cam.WorldToScreenPoint(hitPos + forward) - vector).normalized;
		Vector2 lhs = raycastResult.ScreenPos - prevRaycastResult.ScreenPos;
		scaleAmount += Vector2.Dot(lhs, normalized) * 0.003f;
		onUpdate?.Invoke(this);
	}

	protected override void OnUpdate()
	{
	}

	public override bool IsInteractable(Game.InteractableRaycastResult raycastResult)
	{
		return true;
	}

	public void OnClick(Vector3 hitpoint)
	{
	}

	public void OnDoubleClick()
	{
	}

	public override void OnPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
		if (isPressed)
		{
			forward = base.transform.forward;
			hitPos = raycastResult.Hit.point;
			initialized = false;
			scaleAmount = 0f;
		}
		base.OnPress(raycastResult, isPressed);
	}

	public bool IsDraggable(Game.InteractableRaycastResult raycastResult)
	{
		return true;
	}

	public bool IsSecondTouchDraggable(Game.InteractableRaycastResult raycastResult)
	{
		return false;
	}

	public void OnSecondTouchDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult)
	{
	}
}
