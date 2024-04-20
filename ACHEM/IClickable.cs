using UnityEngine;

public interface IClickable : IInteractable
{
	void OnClick(Vector3 hitpoint);

	void OnDoubleClick();
}
