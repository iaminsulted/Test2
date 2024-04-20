using UnityEngine;

public class DevButton : MonoBehaviour, IClickable, IInteractable
{
	public Entity entity;

	public int ButtonType;

	public void OnClick(Vector3 hitpoint)
	{
		if (entity == null)
		{
			return;
		}
		if (ButtonType == 1)
		{
			if (((NPC)entity).ApopIDs != null)
			{
				AEC.getInstance().sendRequest(new RequestOpenApopAdmin(((NPC)entity).ApopIDs));
			}
		}
		else if (ButtonType == 2)
		{
			AEC.getInstance().sendRequest(new RequestOpenInAdmin(((NPC)entity).NPCID));
		}
	}

	public void OnDoubleClick()
	{
	}

	public void OnHover()
	{
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
