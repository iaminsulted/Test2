public interface IInteractable
{
	void OnHover();

	bool IsInteractable(Game.InteractableRaycastResult raycastResult);

	int GetPriority();

	void OnPress(Game.InteractableRaycastResult raycastResult, bool isPressed);

	void OnSecondTouchPress(Game.InteractableRaycastResult raycastResult, bool isPressed);
}
