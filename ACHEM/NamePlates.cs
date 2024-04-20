using UnityEngine;

public class NamePlates : MonoBehaviour
{
	public static GameObject namePlateObject;

	public Camera mainCamera;

	private ObjectPool<GameObject> NamePlatePool;

	private static NamePlates mInstance;

	private void Awake()
	{
		mInstance = this;
	}

	public void OnDestroy()
	{
		mInstance = null;
	}

	public static NamePlate Create(Entity entity)
	{
		if (mInstance == null)
		{
			return null;
		}
		return mInstance.CreateNamePlate(entity);
	}

	public static void Destroy(NamePlate namePlate)
	{
		namePlate.RemoveNamePlate();
		mInstance.NamePlatePool.Release(namePlate.gameObject);
	}

	private NamePlate CreateNamePlate(Entity entity)
	{
		NamePlate namePlate = GetNamePlate(entity);
		namePlate.gameObject.transform.SetParent(base.transform, worldPositionStays: false);
		return namePlate;
	}

	private NamePlate GetNamePlate(Entity entity)
	{
		if (namePlateObject == null || NamePlatePool == null)
		{
			namePlateObject = Resources.Load<GameObject>("NamePlate_MainCam");
			NamePlatePool = new ObjectPool<GameObject>(namePlateObject);
		}
		NamePlate component = NamePlatePool.Get().GetComponent<NamePlate>();
		component.Init(entity, mainCamera);
		return component;
	}
}
