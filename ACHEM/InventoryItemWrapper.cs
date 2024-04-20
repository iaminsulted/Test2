using UnityEngine;

public class InventoryItemWrapper
{
	public InventoryItem InventoryItem;

	public GameObject GameObject;

	public float Position;

	public bool Locked;

	public bool HasObject => GameObject != null;

	public InventoryItemWrapper(InventoryItem inventoryItem = null, GameObject gameObject = null, float position = 0f)
	{
		GameObject = gameObject;
		Position = position;
		InventoryItem = inventoryItem;
	}
}
