using System;
using UnityEngine;

public class RotationGizmoHandle : GizmoHandle, IDraggable, IInteractable
{
	private Vector3 tangent;

	private Vector3 hitPos;

	public Action<float> onUpdate;

	private const float ROTATION_SPEED = 0.3f;

	private float inertia;

	public void OnDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult)
	{
		Vector2 vector = Game.Instance.cam.WorldToScreenPoint(hitPos);
		Vector2 normalized = ((Vector2)Game.Instance.cam.WorldToScreenPoint(hitPos + tangent) - vector).normalized;
		Vector2 lhs = raycastResult.ScreenPos - prevRaycastResult.ScreenPos;
		float num = Vector2.Dot(lhs, normalized) * 0.3f;
		num -= num * inertia;
		inertia -= 0.03f * lhs.magnitude;
		if (inertia < 0f)
		{
			inertia = 0f;
		}
		onUpdate?.Invoke(num);
	}

	protected override void OnUpdate()
	{
	}

	public override void OnPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
		if (isPressed)
		{
			inertia = 1f;
			Vector3 rhs = raycastResult.Hit.point - base.transform.position;
			Vector3 forward = base.transform.forward;
			tangent = Vector3.Cross(forward, rhs).normalized;
			hitPos = raycastResult.Hit.point;
		}
		base.OnPress(raycastResult, isPressed);
	}

	public override bool IsInteractable(Game.InteractableRaycastResult raycastResult)
	{
		if (Vector3.Dot(raycastResult.Hit.point - base.transform.position, base.transform.up) < 0f)
		{
			return false;
		}
		return true;
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
