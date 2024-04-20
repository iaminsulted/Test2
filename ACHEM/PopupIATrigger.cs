using Assets.Scripts.Game;
using UnityEngine;

public class PopupIATrigger : MonoBehaviour, IClickable, IInteractable
{
	public InteractiveObject IA;

	public void OnClick(Vector3 hitpoint)
	{
		IA.Trigger(checkRequirements: true);
	}

	public void OnDoubleClick()
	{
	}

	public void OnHover()
	{
		CursorManager.Instance.SetCursor(CursorManager.Icon.Interact);
	}

	private void Start()
	{
		base.gameObject.SetActive(IA.IsActive);
		IA.ActiveUpdated += OnMachineActiveUpdated;
	}

	private void OnMachineActiveUpdated(bool active)
	{
		base.gameObject.SetActive(IA.IsActive);
	}

	public void OnPress(Game.InteractableRaycastResult raycastResult, bool state)
	{
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
