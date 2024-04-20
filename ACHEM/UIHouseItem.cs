using Assets.Scripts.Game;
using UnityEngine;
using UnityEngine.Rendering;

public class UIHouseItem : MonoBehaviour
{
	public UIWidget BackgroundWidget;

	public Transform ObjectMount;

	public UILabel Qty;

	public UILabel ItemName;

	public GameObject Orb;

	public InventoryItem iItem;

	private const float BOUNDING_CUBE_PERCENT = 0.55f;

	private const float SMALL_ITEM_SIZE_THRESHOLD = 3f;

	private const float MIN_OBJECT_SIZE_PERCENT = 0.4f;

	private void OnEnable()
	{
		Session.MyPlayerData.OnHouseItemAdded += OnHouseItemAdded;
		Session.MyPlayerData.OnHouseItemRemoved += OnHouseItemRemoved;
		Session.MyPlayerData.OnHouseItemClearAll += OnHouseItemClearAll;
	}

	private void OnDisable()
	{
		Session.MyPlayerData.OnHouseItemAdded -= OnHouseItemAdded;
		Session.MyPlayerData.OnHouseItemRemoved -= OnHouseItemRemoved;
		Session.MyPlayerData.OnHouseItemClearAll -= OnHouseItemClearAll;
	}

	private void OnDestroy()
	{
		Object.Destroy(ObjectMount.gameObject);
	}

	private void updateQtyText()
	{
		if (Session.MyPlayerData.HouseItemCounts != null && Session.MyPlayerData.HouseItemCounts.TryGetValue(iItem.ID, out var value))
		{
			Qty.text = $"{iItem.Qty - value} / {iItem.Qty}";
		}
		else
		{
			Qty.text = $"{iItem.Qty} / {iItem.Qty}";
		}
	}

	private void OnHouseItemAdded(ComHouseItem hItem)
	{
		if (hItem.ItemID == iItem.ID)
		{
			updateQtyText();
		}
	}

	private void OnHouseItemRemoved(int itemID, int houseItemID, int houseID)
	{
		if (itemID == iItem.ID)
		{
			updateQtyText();
		}
	}

	private void OnHouseItemClearAll()
	{
		updateQtyText();
	}

	public void Init(InventoryItem iItem, AssetLoader<GameObject> assetLoader, UIHouseItemList.UIHouseItemWrapper wrapper)
	{
		base.gameObject.SetActive(value: true);
		ItemName.text = "[" + iItem.RarityColor + "]" + iItem.Name + "[-]";
		this.iItem = iItem;
		updateQtyText();
		assetLoader.Get(iItem.AssetName, iItem.bundle, delegate(GameObject asset)
		{
			if (!(asset == null) && !(this == null))
			{
				GameObject gameObject = Object.Instantiate(asset);
				ProcessAsset(gameObject);
				InitObjectInstance(gameObject, wrapper);
				if (UIHouseItemList.instance != null && UIHouseItemList.instance.detail.isActiveAndEnabled && UIHouseItemList.instance.detail.iItem.ID == iItem.ID)
				{
					UIHouseItemList.instance.detail.SetAsset(gameObject);
				}
				asset = null;
			}
		});
	}

	private void ProcessAsset(GameObject go)
	{
		foreach (Transform item in go.transform)
		{
			ProcessAsset(item.gameObject);
		}
		go.layer = Layers.UIORTHO;
		Renderer component = go.GetComponent<Renderer>();
		if (component != null)
		{
			component.shadowCastingMode = ShadowCastingMode.Off;
			component.receiveShadows = false;
			component.lightProbeUsage = LightProbeUsage.Off;
			component.reflectionProbeUsage = ReflectionProbeUsage.Off;
		}
		Collider component2 = go.GetComponent<Collider>();
		if (component2 != null)
		{
			Object.Destroy(component2);
		}
		Rigidbody component3 = go.GetComponent<Rigidbody>();
		if (component3 != null)
		{
			Object.Destroy(component3);
		}
	}

	public void InitObjectInstance(GameObject obj, UIHouseItemList.UIHouseItemWrapper wrapper)
	{
		Bounds meshBounds = Util.GetMeshBounds(obj);
		float num = Mathf.Max(meshBounds.size.x, meshBounds.size.y, meshBounds.size.z);
		float num2 = Mathf.Lerp(0.4f, 1f, num / 3f);
		float num3 = (BackgroundWidget.worldCorners[1].y - BackgroundWidget.worldCorners[0].y) / num * num2 * 0.55f;
		_ = meshBounds.extents;
		meshBounds.size *= num3;
		_ = meshBounds.extents;
		obj.transform.localScale *= num3;
		meshBounds = Util.GetMeshBounds(obj);
		obj.transform.SetParent(wrapper.transform);
		obj.transform.rotation = ObjectMount.transform.rotation * obj.transform.rotation;
		Vector3 vector = meshBounds.center + Vector3.down * meshBounds.extents.y;
		obj.transform.position = obj.transform.position + (ObjectMount.transform.position - vector);
		obj.SetActive(value: true);
		Orb.SetActive(value: false);
	}
}
