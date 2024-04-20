using StatCurves;
using UnityEngine;

public class UICharGender : UIStackingWindow
{
	private static UICharGender instance;

	public UIButton CreateButton;

	public UILabel GenderLabel;

	public GameObject iconMale;

	public GameObject iconFemale;

	public GameObject cameraGO;

	public Camera camMain;

	private PlayerCustomizeAssetController assetController;

	private EntityAsset curAsset;

	private GameObject playerGO;

	private Vector3 cameraPosition;

	private Vector3 cameraEuler;

	private bool hasCameraData;

	private bool ShowHelmet;

	private string currentGender;

	public static void Show()
	{
		if (instance == null)
		{
			UIWindow.ClearWindows();
			instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/GenderCharUI"), UIManager.Instance.transform).GetComponent<UICharGender>();
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		Player me = Entities.Instance.me;
		curAsset = new EntityAsset(me.baseAsset);
		currentGender = curAsset.gender;
		UpdateGenderLabel();
		playerGO = new GameObject("playerGender");
		playerGO.layer = Layers.CUTSCENE;
		playerGO.transform.parent = base.transform;
		playerGO.transform.SetPositionAndRotation(me.wrapper.transform.position, me.wrapper.transform.rotation);
		assetController = playerGO.AddComponent<PlayerCustomizeAssetController>();
		assetController.Init(curAsset);
		assetController.Load();
		assetController.AssetUpdated += AssetReady;
		Game.Instance.camController.ResetToFront();
		camMain = Camera.main;
		cameraGO.transform.SetPositionAndRotation(camMain.transform.position, camMain.transform.rotation);
		camMain.enabled = false;
		UIGame.Instance.Visible = false;
		Game.Instance.DisableControls();
		playerGO.AddComponent<DragRotateSlowDown>();
		UpdateGenderAsset();
	}

	protected override void Destroy()
	{
		assetController.AssetUpdated -= AssetReady;
		UIGame.Instance.Visible = true;
		Game.Instance.EnableControls();
		camMain.enabled = true;
		base.Destroy();
	}

	public void OnCreateClick()
	{
		if (Entities.Instance.me.baseAsset.gender == currentGender)
		{
			Notification.ShowText("You are already this gender");
			return;
		}
		if (Session.MyPlayerData.MC < 500)
		{
			Notification.ShowText("Insufficient Dragon Crystals");
			return;
		}
		RequestEntityGender requestEntityGender = new RequestEntityGender();
		requestEntityGender.gender = currentGender;
		Game.Instance.aec.sendRequest(requestEntityGender);
		Destroy();
	}

	public void OnGenderRight()
	{
		if (currentGender == "M")
		{
			currentGender = "F";
		}
		else
		{
			currentGender = "M";
		}
		UpdateGenderLabel();
		UpdateGenderAsset();
	}

	public void OnGenderLeft()
	{
		if (currentGender == "M")
		{
			currentGender = "F";
		}
		else
		{
			currentGender = "M";
		}
		UpdateGenderLabel();
		UpdateGenderAsset();
	}

	public void ShowHelmetToggle()
	{
		ShowHelmet = !ShowHelmet;
		UpdateGenderAsset();
	}

	private void AssetReady(GameObject go)
	{
		if (!hasCameraData)
		{
			EntityAssetData component = go.GetComponent<EntityAssetData>();
			cameraPosition = component.CameraSpot.TransformPoint(new Vector3(0f, 0.65f, -2.25f));
			cameraEuler = component.CameraSpot.rotation.eulerAngles + new Vector3(20f, 0f, 0f);
			hasCameraData = true;
		}
		iTween.MoveTo(cameraGO, iTween.Hash("position", cameraPosition, "easetype", iTween.EaseType.easeOutSine, "time", 1f));
		iTween.RotateTo(cameraGO, iTween.Hash("rotation", cameraEuler, "easetype", iTween.EaseType.easeOutSine, "time", 1f));
	}

	private void UpdateGenderAsset()
	{
		EntityAsset entityAsset = new EntityAsset(Entities.Instance.me.baseAsset);
		entityAsset.gender = currentGender;
		if (!ShowHelmet)
		{
			entityAsset.equips.Remove(EquipItemSlot.Helm);
		}
		curAsset = entityAsset;
		assetController.Init(curAsset);
		assetController.Load();
	}

	private void UpdateGenderLabel()
	{
		if (currentGender == "M")
		{
			GenderLabel.text = "Male";
			iconMale.SetActive(value: true);
			iconFemale.SetActive(value: false);
		}
		else
		{
			GenderLabel.text = "Female";
			iconFemale.SetActive(value: true);
			iconMale.SetActive(value: false);
		}
	}
}
