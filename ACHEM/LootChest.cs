using Assets.Scripts.Game;
using UnityEngine;

public class LootChest : MonoBehaviour, IClickable, IInteractable
{
	private ComLoot loot;

	public void Awake()
	{
		LootBags.BagRemoved += OnLootDestroyed;
	}

	private void Start()
	{
		Ray ray = new Ray(base.transform.position, -Vector3.up);
		if (Physics.Raycast(ray, out var hitInfo, 8f, Layers.MASK_GROUNDTRACK_LOOT))
		{
			Vector3 point = ray.GetPoint(hitInfo.distance);
			base.transform.position = point;
		}
		Invoke("Expire", loot.Duration);
	}

	private void Expire()
	{
		LootBags.RemoveLoot(loot.ID);
	}

	public void Init(ComLoot loot)
	{
		this.loot = loot;
		loot.timeStamp = GameTime.realtimeSinceServerStartup;
		LootBags.AddLoot(loot);
	}

	public void OnClick(Vector3 hitpoint)
	{
		if ((base.transform.position - Entities.Instance.me.wrapper.transform.position).magnitude <= 15f)
		{
			UILoot.Load(loot);
		}
	}

	public void OnDoubleClick()
	{
	}

	public void OnPress(Game.InteractableRaycastResult raycastResult, bool state)
	{
	}

	public void OnHover()
	{
		CursorManager.Instance.SetCursor(CursorManager.Icon.Interact);
	}

	private void OnLootDestroyed(int lootID)
	{
		if (loot.ID == lootID)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void OnDestroy()
	{
		LootBags.BagRemoved -= OnLootDestroyed;
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
