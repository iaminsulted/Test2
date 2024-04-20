using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class UICharCustomize : UIStackingWindow
{
	public UIButton CreateButton;

	public GameObject LockedPanel;

	public UILabel TitleLabel;

	public UILabel RequirementsLabel;

	public GameObject HairLock;

	public GameObject BeardLock;

	public GameObject BraidLock;

	public GameObject StacheLock;

	public UISprite HairColor;

	public UISprite SkinColor;

	public UISprite EyeColor;

	public UISprite LipColor;

	private PlayerCustomizeAssetController assetController;

	private EntityAsset curAsset;

	private GameObject playerGO;

	private static UICharCustomize instance;

	public GameObject cameraGO;

	public Camera camMain;

	private int beardIndex;

	private int braidIndex;

	private int hairIndex;

	private int stacheIndex;

	private List<BeardStyle> BeardStyles;

	private List<BraidStyle> BraidStyles;

	private List<HairStyle> HairStyles;

	private List<StacheStyle> StacheStyles;

	public GameObject Indices;

	public UILabel[] labels;

	private bool ShowHelmet;

	public static void Show()
	{
		if (instance == null)
		{
			UIWindow.ClearWindows();
			instance = Object.Instantiate(Resources.Load<GameObject>("UIElements/CustomizeCharUI"), UIManager.Instance.transform).GetComponent<UICharCustomize>();
			instance.Init();
		}
	}

	protected override void Init()
	{
		base.Init();
		Player me = Entities.Instance.me;
		curAsset = new EntityAsset(me.baseAsset);
		BeardStyles = (from p in StylesUtil.BeardStyles
			where Session.MyPlayerData.IsStyleAvailable(p.Value) || p.Value.DontHideWhenLocked
			select p.Value).ToList();
		BraidStyles = (from p in StylesUtil.BraidStyles
			where Session.MyPlayerData.IsStyleAvailable(p.Value) || p.Value.DontHideWhenLocked
			select p.Value).ToList();
		HairStyles = (from p in StylesUtil.HairStyles
			where Session.MyPlayerData.IsStyleAvailable(p.Value) || p.Value.DontHideWhenLocked
			select p.Value).ToList();
		StacheStyles = (from p in StylesUtil.StacheStyles
			where Session.MyPlayerData.IsStyleAvailable(p.Value) || p.Value.DontHideWhenLocked
			select p.Value).ToList();
		SetBeard(BeardStyles.FindIndex((BeardStyle p) => p.ID == curAsset.Beard));
		SetBraid(BraidStyles.FindIndex((BraidStyle p) => p.ID == curAsset.Braid));
		SetHair(HairStyles.FindIndex((HairStyle p) => p.ID == curAsset.Hair));
		SetStache(StacheStyles.FindIndex((StacheStyle p) => p.ID == curAsset.Stache));
		curAsset.equips.Remove(EquipItemSlot.Helm);
		playerGO = new GameObject("playerCustomize");
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
		HairColor.color = StylesUtil.EntityHairColors[curAsset.ColorHair];
		SkinColor.color = StylesUtil.EntitySkinColors[curAsset.ColorSkin].B;
		EyeColor.color = StylesUtil.EntityEyeColors[curAsset.ColorEye];
		LipColor.color = StylesUtil.EntityLipColors[curAsset.ColorLip];
		UpdateLockedStyles();
		ShowIndices();
	}

	private void SetBeard(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		beardIndex = index;
		curAsset.Beard = BeardStyles[index].ID;
		ShowIndices();
	}

	private void SetBraid(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		braidIndex = index;
		curAsset.Braid = BraidStyles[index].ID;
		ShowIndices();
	}

	private void SetHair(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		hairIndex = index;
		curAsset.Hair = HairStyles[index].ID;
		ShowIndices();
	}

	private void SetStache(int index)
	{
		if (index < 0)
		{
			index = 0;
		}
		stacheIndex = index;
		curAsset.Stache = StacheStyles[index].ID;
		ShowIndices();
	}

	private void AssetReady(GameObject go)
	{
		EntityAssetData component = go.GetComponent<EntityAssetData>();
		iTween.MoveTo(cameraGO, iTween.Hash("position", component.CameraSpot.position, "easetype", iTween.EaseType.easeOutSine, "time", 1f));
		iTween.RotateTo(cameraGO, iTween.Hash("rotation", component.CameraSpot.rotation.eulerAngles, "easetype", iTween.EaseType.easeOutSine, "time", 1f));
	}

	public void randomizeLook()
	{
		if (curAsset.gender == "M")
		{
			SetBeard((Random.Range(0f, 3f) > 2f) ? Random.Range(0, BeardStyles.Count - 1) : 0);
			SetStache((Random.Range(0f, 3f) > 2f) ? Random.Range(0, StacheStyles.Count - 1) : 0);
			SetBraid(0);
			curAsset.ColorLip = 0;
			LipColor.color = StylesUtil.EntityLipColors[curAsset.ColorLip];
		}
		else
		{
			SetBraid((Random.Range(0f, 3f) > 2f) ? Random.Range(0, BraidStyles.Count - 1) : 0);
			SetBeard(0);
			SetStache(0);
			curAsset.ColorLip = Random.Range(0, StylesUtil.EntityLipColors.Count - 1);
			LipColor.color = StylesUtil.EntityLipColors[curAsset.ColorLip];
		}
		SetHair(Random.Range(0, HairStyles.Count - 1));
		curAsset.ColorHair = Random.Range(0, StylesUtil.EntityHairColors.Count - 1);
		HairColor.color = StylesUtil.EntityHairColors[curAsset.ColorHair];
		curAsset.ColorSkin = Random.Range(0, StylesUtil.EntitySkinColors.Count - 1);
		SkinColor.color = StylesUtil.EntitySkinColors[curAsset.ColorSkin].B;
		curAsset.ColorEye = Random.Range(0, StylesUtil.EntityEyeColors.Count - 1);
		EyeColor.color = StylesUtil.EntityEyeColors[curAsset.ColorEye];
		assetController.UpdateAll();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void OnHairLeft()
	{
		SetHair((hairIndex + HairStyles.Count - 1) % HairStyles.Count);
		assetController.UpdateHair();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void OnHairRight()
	{
		SetHair((hairIndex + 1) % HairStyles.Count);
		assetController.UpdateHair();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void ResetHair()
	{
		SetHair(14);
		assetController.UpdateHair();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void OnBraidLeft()
	{
		SetBraid((braidIndex + BraidStyles.Count - 1) % BraidStyles.Count);
		assetController.UpdateBraid();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void OnBraidRight()
	{
		SetBraid((braidIndex + 1) % BraidStyles.Count);
		assetController.UpdateBraid();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void ResetBraid()
	{
		SetBraid(0);
		assetController.UpdateBraid();
		UpdateLockedStyles();
	}

	public void OnBeardLeft()
	{
		SetBeard((beardIndex + BeardStyles.Count - 1) % BeardStyles.Count);
		assetController.UpdateBeard();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void OnBeardRight()
	{
		SetBeard((beardIndex + 1) % BeardStyles.Count);
		assetController.UpdateBeard();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void ResetBeard()
	{
		SetBeard(0);
		assetController.UpdateBeard();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void OnStacheLeft()
	{
		SetStache((stacheIndex + StacheStyles.Count - 1) % StacheStyles.Count);
		assetController.UpdateStache();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void OnStacheRight()
	{
		SetStache((stacheIndex + 1) % StacheStyles.Count);
		assetController.UpdateStache();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void ResetStache()
	{
		SetStache(0);
		assetController.UpdateStache();
		UpdateLockedStyles();
		ShowIndices();
	}

	public void OnHairColorLeft()
	{
		curAsset.ColorHair = (curAsset.ColorHair + StylesUtil.EntityHairColors.Count - 1) % StylesUtil.EntityHairColors.Count;
		assetController.UpdateHairSkinColor();
		HairColor.color = StylesUtil.EntityHairColors[curAsset.ColorHair];
		ShowIndices();
	}

	public void OnHairColorRight()
	{
		curAsset.ColorHair = (curAsset.ColorHair + 1) % StylesUtil.EntityHairColors.Count;
		assetController.UpdateHairSkinColor();
		HairColor.color = StylesUtil.EntityHairColors[curAsset.ColorHair];
		ShowIndices();
	}

	public void OnSkinColorLeft()
	{
		curAsset.ColorSkin = (curAsset.ColorSkin + StylesUtil.EntitySkinColors.Count - 1) % StylesUtil.EntitySkinColors.Count;
		assetController.UpdateHairSkinColor();
		SkinColor.color = StylesUtil.EntitySkinColors[curAsset.ColorSkin].B;
		ShowIndices();
	}

	public void OnSkinColorRight()
	{
		curAsset.ColorSkin = (curAsset.ColorSkin + 1) % StylesUtil.EntitySkinColors.Count;
		assetController.UpdateHairSkinColor();
		SkinColor.color = StylesUtil.EntitySkinColors[curAsset.ColorSkin].B;
		ShowIndices();
	}

	public void OnEyeColorLeft()
	{
		curAsset.ColorEye = (curAsset.ColorEye + StylesUtil.EntityEyeColors.Count - 1) % StylesUtil.EntityEyeColors.Count;
		assetController.UpdateEyeColor();
		EyeColor.color = StylesUtil.EntityEyeColors[curAsset.ColorEye];
		ShowIndices();
	}

	public void OnEyeColorRight()
	{
		curAsset.ColorEye = (curAsset.ColorEye + 1) % StylesUtil.EntityEyeColors.Count;
		assetController.UpdateEyeColor();
		EyeColor.color = StylesUtil.EntityEyeColors[curAsset.ColorEye];
		ShowIndices();
	}

	public void OnLipColorLeft()
	{
		curAsset.ColorLip = (curAsset.ColorLip + StylesUtil.EntityLipColors.Count - 1) % StylesUtil.EntityLipColors.Count;
		assetController.UpdateLipColor();
		LipColor.color = StylesUtil.EntityLipColors[curAsset.ColorLip];
		ShowIndices();
	}

	public void OnLipColorRight()
	{
		curAsset.ColorLip = (curAsset.ColorLip + 1) % StylesUtil.EntityLipColors.Count;
		assetController.UpdateLipColor();
		LipColor.color = StylesUtil.EntityLipColors[curAsset.ColorLip];
		ShowIndices();
	}

	public void UpdateLockedStyles()
	{
		bool flag = false;
		if (!Session.MyPlayerData.IsStyleAvailable(HairStyles[hairIndex]))
		{
			HairLock.SetActive(value: true);
			flag = true;
			TitleLabel.text = "Hair Locked";
			RequirementsLabel.text = StylesUtil.HairStyles[hairIndex].RequirementText;
		}
		else
		{
			HairLock.SetActive(value: false);
		}
		if (!Session.MyPlayerData.IsStyleAvailable(BeardStyles[beardIndex]))
		{
			BeardLock.SetActive(value: true);
			flag = true;
			TitleLabel.text = "Beard Locked";
			RequirementsLabel.text = StylesUtil.BeardStyles[beardIndex].RequirementText;
		}
		else
		{
			BeardLock.SetActive(value: false);
		}
		if (!Session.MyPlayerData.IsStyleAvailable(BraidStyles[braidIndex]))
		{
			BraidLock.SetActive(value: true);
			flag = true;
			TitleLabel.text = "Braid Locked";
			RequirementsLabel.text = StylesUtil.BraidStyles[braidIndex].RequirementText;
		}
		else
		{
			BraidLock.SetActive(value: false);
		}
		if (!Session.MyPlayerData.IsStyleAvailable(StacheStyles[stacheIndex]))
		{
			StacheLock.SetActive(value: true);
			flag = true;
			TitleLabel.text = "Stache Locked";
			RequirementsLabel.text = StylesUtil.StacheStyles[stacheIndex].RequirementText;
		}
		else
		{
			StacheLock.SetActive(value: false);
		}
		LockedPanel.SetActive(flag);
		CreateButton.isEnabled = !flag;
		ShowIndices();
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
		if (Session.MyPlayerData.Gold < 500)
		{
			Notification.ShowText("Insufficient gold");
			return;
		}
		RequestCustomize requestCustomize = new RequestCustomize();
		requestCustomize.haircolor = curAsset.ColorHair;
		requestCustomize.skincolor = curAsset.ColorSkin;
		requestCustomize.eyecolor = curAsset.ColorEye;
		requestCustomize.lipcolor = curAsset.ColorLip;
		requestCustomize.hair = curAsset.Hair;
		requestCustomize.braid = curAsset.Braid;
		requestCustomize.stache = curAsset.Stache;
		requestCustomize.beard = curAsset.Beard;
		Game.Instance.aec.sendRequest(requestCustomize);
		Destroy();
	}

	public void ShowHelmetToggle()
	{
		ShowHelmet = !ShowHelmet;
		EntityAsset entityAsset = new EntityAsset(Entities.Instance.me.baseAsset);
		entityAsset.ColorHair = curAsset.ColorHair;
		entityAsset.ColorSkin = curAsset.ColorSkin;
		entityAsset.ColorEye = curAsset.ColorEye;
		entityAsset.ColorLip = curAsset.ColorLip;
		entityAsset.Braid = curAsset.Braid;
		entityAsset.Hair = curAsset.Hair;
		entityAsset.Stache = curAsset.Stache;
		entityAsset.Beard = curAsset.Beard;
		if (!ShowHelmet)
		{
			entityAsset.equips.Remove(EquipItemSlot.Helm);
		}
		curAsset = entityAsset;
		assetController.Init(curAsset);
		assetController.Load();
	}

	private void ShowIndices()
	{
		if (Session.MyPlayerData.AccessLevel >= 50)
		{
			Indices.SetActive(value: true);
			labels[0].text = curAsset.Hair.ToString();
			labels[1].text = curAsset.Braid.ToString();
			labels[2].text = curAsset.Beard.ToString();
			labels[3].text = curAsset.Stache.ToString();
			labels[4].text = curAsset.ColorHair.ToString();
			labels[5].text = curAsset.ColorSkin.ToString();
			labels[6].text = curAsset.ColorEye.ToString();
			labels[7].text = curAsset.ColorLip.ToString();
		}
		else
		{
			Indices.SetActive(value: false);
		}
	}
}
