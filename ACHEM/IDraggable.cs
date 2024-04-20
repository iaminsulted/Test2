public interface IDraggable : IInteractable
{
	bool IsDraggable(Game.InteractableRaycastResult raycastResult);

	void OnDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult);

	bool IsSecondTouchDraggable(Game.InteractableRaycastResult raycastResult);

	void OnSecondTouchDrag(Game.InteractableRaycastResult raycastResult, Game.InteractableRaycastResult prevRaycastResult);
}
