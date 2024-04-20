using UnityEngine;

public class PreviewGenerator : MonoBehaviour
{
	public enum TweenEffect
	{
		NONE,
		SCALEFROM
	}

	public TweenEffect tweenEffect;

	public Transform GOPosition;

	public Rect viewportSize;

	public GameObject GO;

	public Transform BottomLeftMarker;

	public Transform TopRightMarker;

	public Camera _cam;

	private Camera _uicamera;

	private bool IsPetOrTravelForm;

	private AssetController assetController;

	public void ShowWithMarkers(EntityAsset assetdata, bool isPetOrTravelform = false)
	{
		Init(assetdata, BottomLeftMarker, TopRightMarker, isPetOrTravelform);
	}

	private void Init(EntityAsset assetdata, Transform BottomLeft, Transform TopRight, bool isPetOrTravelform)
	{
		IsPetOrTravelForm = isPetOrTravelform;
		_uicamera = UICamera.currentCamera.GetComponent<Camera>();
		Vector3 vector = _uicamera.WorldToViewportPoint(BottomLeft.position);
		Vector3 vector2 = _uicamera.WorldToViewportPoint(TopRight.position);
		Rect rect = default(Rect);
		rect.x = vector.x;
		rect.width = vector2.x - vector.x;
		rect.y = vector.y;
		rect.height = vector2.y - vector.y;
		_cam.rect = rect;
		_cam.depth = 150f;
		createplayer(assetdata);
	}

	private void createplayer(EntityAsset assetdata)
	{
		if (GO != null)
		{
			Object.Destroy(GO);
		}
		GO = new GameObject();
		GO.layer = LayerMask.NameToLayer("NGUI");
		GO.transform.SetParent(GOPosition, worldPositionStays: false);
		GO.transform.localPosition = Vector3.zero;
		assetController = ((assetdata.gender == "N") ? ((AssetController)GO.AddComponent<NPCAssetController>()) : ((AssetController)GO.AddComponent<PlayerAssetController>()));
		assetController.Init(assetdata);
		assetController.Load();
		assetController.AssetUpdated += AssetReady;
	}

	private void AssetReady(GameObject go)
	{
		assetController.AssetUpdated -= AssetReady;
		MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
		Renderer[] componentsInChildren = GO.GetComponentsInChildren<Renderer>();
		Bounds bounds = default(Bounds);
		Renderer[] array = componentsInChildren;
		foreach (Renderer renderer in array)
		{
			bounds.Encapsulate(renderer.bounds);
			renderer.GetPropertyBlock(materialPropertyBlock);
			materialPropertyBlock.SetFloat("_Probe", 0f);
			renderer.SetPropertyBlock(materialPropertyBlock);
			renderer.gameObject.layer = Layers.NGUI;
		}
		float num = 1f;
		if (IsPetOrTravelForm)
		{
			num = 0.6f;
			if ((bounds.center.x + bounds.center.y + bounds.center.z) / 3f > 1.8f)
			{
				base.transform.GetChild(1).position = new Vector3(base.transform.GetChild(1).position.x, base.transform.GetChild(1).position.y - 0.5f, base.transform.GetChild(1).position.z);
			}
		}
		assetController.transform.parent.localScale *= num;
	}

	public void TweenScaleFrom(Vector3 scale, float time)
	{
		iTween.ScaleFrom(assetController.transform.parent.gameObject, scale, time);
	}
}
