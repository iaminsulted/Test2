using System;
using UnityEngine;

public class AxisGizmoHandle : GizmoHandle, IDraggable, IInteractable, IClickable
{
	public Action<AxisGizmoHandle, Vector3> onUpdate;

	private Vector3 startPosition;

	public override void OnPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
		if (isPressed)
		{
			startPosition = base.transform.position;
		}
		base.OnPress(raycastResult, isPressed);
	}

	protected override void OnUpdate()
	{
		if (!dragging)
		{
			return;
		}
		Vector3 vector = base.transform.position + ClickOffset;
		Vector3 vector2 = Vector3.Project(Game.Instance.cam.transform.position - vector, base.transform.forward);
		Vector3 inNormal = Game.Instance.cam.transform.position - (vector2 + vector);
		Plane plane = new Plane(inNormal, vector);
		Ray ray = Game.Instance.cam.ScreenPointToRay(UICamera.lastEventPosition);
		if (plane.Raycast(ray, out var enter))
		{
			Vector3 vector3 = Vector3.Project(ray.GetPoint(enter) - vector, base.transform.forward);
			if (!((base.transform.position + vector3 - startPosition).magnitude > 100f))
			{
				onUpdate(this, vector3);
			}
		}
	}

	public void OnDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult)
	{
	}

	public void OnClick(Vector3 hitpoint)
	{
	}

	public void OnDoubleClick()
	{
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
