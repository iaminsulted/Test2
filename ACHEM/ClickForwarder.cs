using UnityEngine;

public class ClickForwarder : MonoBehaviour, IClickable, IInteractable, IDraggable
{
	public IInteractable iaParent;

	public GameObject clickParentGO;

	public bool IsInteractable(Game.InteractableRaycastResult raycastResult)
	{
		if (iaParent != null)
		{
			return iaParent.IsInteractable(raycastResult);
		}
		return false;
	}

	private void Start()
	{
		if (clickParentGO != null)
		{
			iaParent = clickParentGO.GetComponent<IInteractable>();
		}
	}

	public void OnClick(Vector3 hitpoint)
	{
		(iaParent as IClickable)?.OnClick(hitpoint);
	}

	public void OnDoubleClick()
	{
		(iaParent as IClickable)?.OnDoubleClick();
	}

	public void OnPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
		iaParent?.OnPress(raycastResult, isPressed);
	}

	public void OnHover()
	{
		iaParent?.OnHover();
	}

	public int GetPriority()
	{
		return iaParent?.GetPriority() ?? 0;
	}

	public void OnDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult)
	{
		(iaParent as IDraggable)?.OnDrag(raycastResult, prevRaycastResult);
	}

	public bool IsDraggable(Game.InteractableRaycastResult raycastResult)
	{
		return (iaParent as IDraggable)?.IsDraggable(raycastResult) ?? false;
	}

	public void OnSecondTouchPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
		iaParent?.OnSecondTouchPress(raycastResult, isPressed);
	}

	public bool IsSecondTouchDraggable(Game.InteractableRaycastResult raycastResult)
	{
		return (iaParent as IDraggable)?.IsSecondTouchDraggable(raycastResult) ?? false;
	}

	public void OnSecondTouchDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult)
	{
		(iaParent as IDraggable)?.OnSecondTouchDrag(raycastResult, prevRaycastResult);
	}
}
