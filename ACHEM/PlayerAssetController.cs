using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAssetController : AssetController
{
	protected enum GloveHideState
	{
		None,
		Right,
		Both
	}

	protected static string[] ShirtMeshes = new string[31]
	{
		"Shirt_Skin", "Shirt_Collar", "Shirt_Robe", "Shirt_Armor", "Shirt_ShortSleeves", "Shirt_Jacket", "Shirt_Jacket02", "Shirt_CollarBMed", "Shirt_CollarBShort", "Shirt_CollarDUp",
		"Shirt_CollarDDown", "Shirt_CuffedA", "Shirt_SleevedA", "Shirt_ShoulderPads", "Shirt_CollarC", "Shirt_CollarC2", "Shirt_Armor02", "Shirt_JacketVest", "Shirt_GiNoSleeves", "Shirt_GiBaggySleeves",
		"Shirt_GiOpenSleeves", "Shirt_ComboLongCoat", "Shirt_Jacket03", "Shirt_TatteredRobe", "Shirt_SkinBaggySleeves", "Shirt_Jacket04", "Shirt_PlaceHolder01", "Shirt_PlaceHolder02", "Shirt_PlaceHolder03", "Shirt_PlaceHolder04",
		"Shirt_PlaceHolder05"
	};

	protected static string[] LegsMeshes = new string[27]
	{
		"Legs_Skin", "Legs_Robe", "Legs_Baggy", "Legs_WaistCoat", "Legs_MidSkirt", "Legs_LoinCloth", "Legs_Pants", "Legs_Harem", "Legs_WaistCoatSplit", "Legs_WaistCoatMid",
		"Legs_WaistCoatShort", "Legs_ShortSkirt", "Legs_LoinClothLong", "Legs_FancyMidRobe", "Legs_TrenchCoat", "Legs_ShortGi", "Legs_ShortGiSplit", "Legs_ComboLongCoat", "Legs_ComboLongCoatSplit", "Legs_MidGiSplit",
		"Legs_MidGi", "Legs_TatteredRobe", "Legs_PlaceHolder01", "Legs_PlaceHolder02", "Legs_PlaceHolder03", "Legs_PlaceHolder04", "Legs_PlaceHolder05"
	};

	protected static string[] BootsMeshes = new string[26]
	{
		"Boots_Bare", "Boots_Robe", "Boots_Skin", "Boots_Long", "Boots_Armor", "Boots_Cuff", "Boots_Heels", "Boots_Armor02", "Boots_PegL", "Boots_PegR",
		"Boots_PegDouble", "Boots_SkinnyCuff", "Boots_Short", "Boots_CuffShort", "Boots_Shoes", "Boots_ArmorBig01", "Boots_Spikey01", "Boots_LooseCloth", "Boots_NinjaFlops", "Boots_Barbarian01",
		"Boots_DemonHunter", "Boots_PlaceHolder01", "Boots_PlaceHolder02", "Boots_PlaceHolder03", "Boots_PlaceHolder04", "Boots_PlaceHolder05"
	};

	protected static string[] GlovesMeshes = new string[21]
	{
		"Gloves_Skin", "Gloves_Long", "Gloves_Short", "Gloves_CuffLong", "Gloves_Cuff_Short", "Gloves_Armor", "Gloves_Armor02", "Gloves_Bracer", "Gloves_ArmorBig01", "Gloves_Spikey01",
		"Gloves_Plated", "Gloves_Armor03", "Gloves_LooseCloth", "Gloves_BarbarianSpiked", "Gloves_DemonHunter", "Gloves_Armor04", "Gloves_PlaceHolder01", "Gloves_PlaceHolder02", "Gloves_PlaceHolder03", "Gloves_PlaceHolder04",
		"Gloves_PlaceHolder05"
	};

	public static string HUMAN_FEMALE_ASSETNAME = "p_female";

	public static string HUMAN_MALE_ASSETNAME = "p_male";

	protected Texture main_d;

	[SerializeField]
	protected Material matMain;

	protected Material hairMat;

	protected TransformCatelog boneCatelog;

	private Transform bobberLocation;

	private bool stopBobberLoad;

	private EquipItemSlot delayedToolSlot;

	private GameObject[] glovesR;

	private GameObject[] glovesL;

	private GloveHideState gloveHide;

	private bool isToolDelayed;

	private bool isWeaponDelayed;

	protected bool isBlinking = true;

	protected Vector4 face_BlinkOffset = new Vector4(1f, 2f, 0f, 0f);

	protected Vector4 face_DeadOffset = new Vector4(1f, 3f, 0f, 1f);

	protected Vector4 face_DefaultOffset = Vector4.zero;

	public List<GameObject> WeaponGameobjects { get; private set; }

	public GameObject weaponGO { get; private set; }

	public GameObject bobberGO { get; private set; }

	public bool IsBlinking
	{
		get
		{
			return isBlinking;
		}
		set
		{
			if (isBlinking != value)
			{
				isBlinking = value;
				SetFaceOffset(isBlinking ? face_DefaultOffset : face_DeadOffset);
				if (isBlinking)
				{
					StartBlink();
				}
				else
				{
					StopBlink();
				}
			}
		}
	}

	public event Action BobberDelayed;

	public event Action ToolDelayed;

	public event Action WeaponDelayed;

	public void Awake()
	{
		WeaponGameobjects = new List<GameObject>();
		main_d = new Texture2D(1024, 1024, TextureFormat.ARGB32, mipChain: true);
	}

	public void ToggleGloves(bool enabled)
	{
		GameObject[] array = glovesR;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(enabled);
		}
		array = glovesL;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(enabled);
		}
	}

	public void HideWeaponGos(bool enabled)
	{
		foreach (GameObject weaponGameobject in WeaponGameobjects)
		{
			if (weaponGameobject != null)
			{
				weaponGameobject.SetActive(enabled);
			}
		}
		switch (gloveHide)
		{
		case GloveHideState.Right:
		{
			GameObject[] array = glovesR;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(!enabled);
			}
			break;
		}
		case GloveHideState.Both:
		{
			GameObject[] array = glovesR;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(!enabled);
			}
			array = glovesL;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(!enabled);
			}
			break;
		}
		case GloveHideState.None:
			break;
		}
	}

	private void ClearLoaders()
	{
		foreach (AssetBundleLoader value in loaders.Values)
		{
			value.Dispose();
		}
		loaders.Clear();
	}

	protected override void Clear()
	{
		if (currentAsset != null)
		{
			UnityEngine.Object.Destroy(currentAsset);
		}
		ClearLoaders();
		StopAllCoroutines();
	}

	public void LoadBobber(Transform location)
	{
		bobberLocation = location;
		stopBobberLoad = false;
		StartCoroutine(LoadBobberAsset(bobberLocation));
	}

	public void DestroyBobber(bool immediate)
	{
		if (bobberGO == null)
		{
			stopBobberLoad = true;
		}
		else
		{
			StartCoroutine(RemoveBobber(immediate));
		}
	}

	private void DelayBobberLoad()
	{
		BobberDelayed -= DelayBobberLoad;
		StartCoroutine(LoadBobberAsset(bobberLocation));
	}

	private void DelayWeaponLoad()
	{
		WeaponDelayed -= DelayWeaponLoad;
		StartCoroutine(LoadWeaponAsset());
	}

	private void DelayToolLoad()
	{
		ToolDelayed -= DelayToolLoad;
		StartCoroutine(LoadToolAsset(delayedToolSlot));
	}

	public override IEnumerator LoadAsset(bool disableEntityTrigger = false)
	{
		Error = "";
		IsAssetLoadComplete = false;
		if (showorb)
		{
			showOrb();
		}
		defaultPrefabName = ((entityAsset.gender == "F") ? HUMAN_FEMALE_ASSETNAME : HUMAN_MALE_ASSETNAME);
		defaultPrefabBundle = Session.Account.Characters_Bundle;
		AssetBundleLoader request = LoadAssetBundle(defaultPrefabBundle);
		yield return request;
		if (!string.IsNullOrEmpty(request.Error))
		{
			OnAssetLoadFailed(request.Error);
			yield break;
		}
		equips.Clear();
		foreach (EquipItem value in entityAsset.equips.Values)
		{
			equips.Add(value.EquipSlot, value);
		}
		if (!equips.ContainsKey(EquipItemSlot.Armor))
		{
			equips.Add(EquipItemSlot.Armor, equips[EquipItemSlot.Class]);
		}
		equips.Remove(EquipItemSlot.Class);
		if (entityAsset.WeaponRequired != EquipItemSlot.Weapon && entityAsset.equips.ContainsKey(entityAsset.WeaponRequired))
		{
			equips[EquipItemSlot.Weapon] = entityAsset.equips[entityAsset.WeaponRequired];
			equips.Remove(entityAsset.WeaponRequired);
			entityAsset.Current = EquipItemSlot.Weapon;
		}
		foreach (EquipItem value2 in equips.Values)
		{
			Debug.Log(value2.EquipSlot.ToString() + " " + value2.bundle?.ToString() + " " + value2.AssetName);
			if (value2.bundle != null && !string.IsNullOrEmpty(value2.bundle.FileName))
			{
				AssetBundleLoader itemRequest = LoadAssetBundle(value2.bundle);
				yield return StartCoroutine(itemRequest);
				if (!string.IsNullOrEmpty(itemRequest.Error))
				{
					OnAssetLoadFailed(itemRequest.Error);
					yield break;
				}
			}
		}
		yield return StartCoroutine(CompositeRoutine());
		yield return StartCoroutine(Instantiate());
	}

	public override IEnumerator LoadWeaponAsset()
	{
		if (!IsAssetLoadComplete)
		{
			if (isToolDelayed)
			{
				ToolDelayed -= DelayToolLoad;
				isToolDelayed = false;
			}
			if (!isWeaponDelayed)
			{
				WeaponDelayed += DelayWeaponLoad;
				isWeaponDelayed = true;
			}
			yield break;
		}
		IsAssetLoadComplete = false;
		defaultPrefabBundle = Session.Account.Characters_Bundle;
		AssetBundleLoader request = LoadAssetBundle(defaultPrefabBundle);
		yield return request;
		if (!string.IsNullOrEmpty(request.Error))
		{
			OnAssetLoadFailed(request.Error);
			yield break;
		}
		if (entityAsset.WeaponRequired != EquipItemSlot.Weapon && entityAsset.equips.ContainsKey(entityAsset.WeaponRequired))
		{
			equips[EquipItemSlot.Weapon] = entityAsset.equips[entityAsset.WeaponRequired];
			equips.Remove(entityAsset.WeaponRequired);
		}
		else
		{
			equips[EquipItemSlot.Weapon] = entityAsset.equips[EquipItemSlot.Weapon];
		}
		if (equips[EquipItemSlot.Weapon].bundle != null && !string.IsNullOrEmpty(equips[EquipItemSlot.Weapon].bundle.FileName))
		{
			AssetBundleLoader itemRequest = LoadAssetBundle(equips[EquipItemSlot.Weapon].bundle);
			yield return StartCoroutine(itemRequest);
			if (!string.IsNullOrEmpty(itemRequest.Error))
			{
				OnAssetLoadFailed(itemRequest.Error);
				yield break;
			}
		}
		yield return StartCoroutine(InstantiateWeapon());
	}

	public override IEnumerator LoadToolAsset(EquipItemSlot slot)
	{
		if (!IsAssetLoadComplete)
		{
			delayedToolSlot = slot;
			if (isWeaponDelayed)
			{
				WeaponDelayed -= DelayWeaponLoad;
				isWeaponDelayed = false;
			}
			if (!isToolDelayed)
			{
				ToolDelayed += this.ToolDelayed;
				isToolDelayed = true;
			}
			yield break;
		}
		IsAssetLoadComplete = false;
		defaultPrefabBundle = Session.Account.Characters_Bundle;
		AssetBundleLoader request = LoadAssetBundle(defaultPrefabBundle);
		yield return request;
		if (!string.IsNullOrEmpty(request.Error))
		{
			OnAssetLoadFailed(request.Error);
			yield break;
		}
		equips[slot] = entityAsset.equips[slot];
		if (equips[slot].bundle != null && !string.IsNullOrEmpty(equips[slot].bundle.FileName))
		{
			AssetBundleLoader itemRequest = LoadAssetBundle(equips[slot].bundle);
			yield return StartCoroutine(itemRequest);
			if (!string.IsNullOrEmpty(itemRequest.Error))
			{
				OnAssetLoadFailed(itemRequest.Error);
				yield break;
			}
		}
		yield return StartCoroutine(InstantiateTool(slot));
	}

	private IEnumerator LoadBobberAsset(Transform castSpot)
	{
		if (!IsAssetLoadComplete)
		{
			BobberDelayed += DelayBobberLoad;
			yield break;
		}
		if (stopBobberLoad)
		{
			stopBobberLoad = false;
			yield break;
		}
		if (bobberGO != null)
		{
			yield return StartCoroutine(RemoveBobber(immediate: true));
		}
		if (equips.ContainsKey(EquipItemSlot.Bobber))
		{
			AssetBundleRequest abr = LoadAssetFromBundle(equips[EquipItemSlot.Bobber]);
			yield return abr;
			bobberGO = UnityEngine.Object.Instantiate(abr.asset as GameObject);
		}
		else
		{
			bobberGO = UnityEngine.Object.Instantiate(Resources.Load("TradeSkills/Prefabs/Fishing/Bobber") as GameObject);
		}
		if (weaponGO == null || castSpot == null)
		{
			StartCoroutine(RemoveBobber(immediate: true));
			yield break;
		}
		bobberGO.transform.SetParent(castSpot.gameObject.transform, worldPositionStays: false);
		LinkedLineRenderer componentInChildren = weaponGO.GetComponentInChildren<LinkedLineRenderer>();
		if (componentInChildren != null && componentInChildren.lineRendererPoints.Length > 1)
		{
			componentInChildren.SetFishingEndPoint(bobberGO.transform.GetChild(0));
		}
		yield return StartCoroutine(AddBobber());
		yield return StartCoroutine(MoveBobberToWater());
	}

	private IEnumerator AddBobber()
	{
		Vector3 targetScale = bobberGO.transform.localScale;
		float elapsed = 0f;
		while (bobberGO != null && elapsed <= 0.5f)
		{
			bobberGO.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / 0.5f);
			elapsed += Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator RemoveBobber(bool immediate)
	{
		LinkedLineRenderer lineRenderer = null;
		if (weaponGO != null)
		{
			lineRenderer = weaponGO.GetComponentInChildren<LinkedLineRenderer>();
		}
		if (!immediate)
		{
			Vector3 pos = bobberGO.transform.localPosition;
			float elapsed = 0f;
			while (bobberGO != null && elapsed <= 1.2f)
			{
				bobberGO.transform.localPosition = Vector3.Lerp(pos, Vector3.zero, elapsed / 0.8f);
				elapsed += Time.deltaTime;
				yield return null;
			}
			if (lineRenderer != null && bobberGO != null)
			{
				Transform parent = lineRenderer.lineRendererPoints[0];
				bobberGO.transform.SetParent(parent, worldPositionStays: true);
				pos = bobberGO.transform.localPosition;
				Vector3 scale = bobberGO.transform.localScale;
				elapsed = 0f;
				while (bobberGO != null && elapsed <= 0.4f)
				{
					bobberGO.transform.localPosition = Vector3.Lerp(pos, Vector3.zero, elapsed / 0.4f);
					bobberGO.transform.localScale = Vector3.Lerp(scale, Vector3.zero, elapsed / 0.4f);
					elapsed += Time.deltaTime;
					yield return null;
				}
			}
		}
		if (lineRenderer != null)
		{
			lineRenderer.ResetFishingEndPoint();
		}
		UnityEngine.Object.Destroy(bobberGO);
		bobberGO = null;
	}

	private IEnumerator MoveBobberToWater()
	{
		yield return new WaitForSeconds(1.7f);
		if (!(bobberGO != null))
		{
			yield break;
		}
		if (Physics.Raycast(bobberGO.transform.position, Vector3.up, out var hitInfo, 500f, 1 << Layers.WATER, QueryTriggerInteraction.Collide))
		{
			Vector3 start = bobberGO.transform.position;
			Vector3 end = bobberGO.transform.position + new Vector3(0f, hitInfo.distance, 0f);
			float elapsed = 0f;
			while (bobberGO != null && elapsed <= 0.5f)
			{
				elapsed += Time.deltaTime;
				bobberGO.transform.position = Vector3.Lerp(start, end, elapsed / 0.5f);
				yield return null;
			}
		}
		else
		{
			DestroyBobber(immediate: true);
		}
	}

	protected IEnumerator CompositeRoutine()
	{
		CompositorRequest cr = new CompositorRequest(main_d);
		string text = ((entityAsset.gender == "F") ? "Female_Skin_D" : "Male_Skin_D");
		AssetBundle ab = loaders[defaultPrefabBundle.FileName].Asset;
		AssetBundleRequest abr = ab.LoadAssetAsync(text);
		yield return abr;
		if (abr.asset == null)
		{
			OnAssetLoadFailed("AssetBundleRequest.asset is null");
			yield break;
		}
		cr.tasks.Add(new CompositorTaskSkin(new Rectangle(0, 0, 1024, 1024), abr.asset as Texture2D, StylesUtil.EntitySkinColors[entityAsset.ColorSkin].A, StylesUtil.EntitySkinColors[entityAsset.ColorSkin].B));
		EquipItem armor = equips[EquipItemSlot.Armor];
		string text2 = LegsMeshes[equips[EquipItemSlot.Armor].GetMeshType1(entityAsset.gender)];
		string text3 = ShirtMeshes[equips[EquipItemSlot.Armor].GetMeshType0(entityAsset.gender)];
		bool isRobeEquipped = false;
		bool isGloveOverride = false;
		if (equips.ContainsKey(EquipItemSlot.Armor))
		{
			if (string.IsNullOrEmpty(equips[EquipItemSlot.Armor].AssetName))
			{
				isRobeEquipped = text2 == "Legs_Robe" || text2 == "Legs_Pants" || text2 == "Legs_TatteredRobe";
				int num;
				switch (text3)
				{
				default:
					num = ((text3 == "Shirt_TatteredRobe") ? 1 : 0);
					break;
				case "Shirt_Robe":
				case "Shirt_Jacket":
				case "Shirt_Jacket02":
				case "Shirt_Jacket03":
				case "Shirt_Jacket04":
				case "Shirt_ComboLongCoat":
					num = 1;
					break;
				}
				isGloveOverride = (byte)num != 0;
			}
			else
			{
				if (equips[EquipItemSlot.Armor].Mesh0 == 1)
				{
					isRobeEquipped = true;
				}
				if (equips[EquipItemSlot.Armor].Mesh1 == 1)
				{
					isGloveOverride = true;
				}
			}
		}
		if (isRobeEquipped && equips.ContainsKey(EquipItemSlot.Boots))
		{
			yield return StartCoroutine(CreateCompositorTask(cr, equips[EquipItemSlot.Boots]));
		}
		if (isGloveOverride && equips.ContainsKey(EquipItemSlot.Gloves))
		{
			yield return StartCoroutine(CreateCompositorTask(cr, equips[EquipItemSlot.Gloves]));
		}
		if (equips.ContainsKey(EquipItemSlot.Armor))
		{
			yield return StartCoroutine(CreateCompositorTask(cr, armor));
		}
		if (!isRobeEquipped && equips.ContainsKey(EquipItemSlot.Boots))
		{
			yield return StartCoroutine(CreateCompositorTask(cr, equips[EquipItemSlot.Boots]));
		}
		if (!isGloveOverride && equips.ContainsKey(EquipItemSlot.Gloves))
		{
			yield return StartCoroutine(CreateCompositorTask(cr, equips[EquipItemSlot.Gloves]));
		}
		if (equips.ContainsKey(EquipItemSlot.Belt))
		{
			yield return StartCoroutine(CreateCompositorTask(cr, equips[EquipItemSlot.Belt]));
		}
		List<Texture2D> textures = new List<Texture2D>();
		List<Rectangle> vector4s = new List<Rectangle>();
		List<Color32> color32s = new List<Color32>();
		abr = ab.LoadAssetAsync<Texture2D>("Beard_Region_D");
		yield return abr;
		if (abr.asset == null)
		{
			OnAssetLoadFailed("AssetBundleRequest.asset is null.");
			yield break;
		}
		textures.Add(abr.asset as Texture2D);
		vector4s.Add(new Rectangle(640, 527, 384, 133));
		color32s.Add(StylesUtil.EntityHairColors[entityAsset.ColorHair]);
		if (entityAsset.Beard > 0 && StylesUtil.BeardStyles[entityAsset.Beard].Type == 1)
		{
			abr = ab.LoadAssetAsync<Texture2D>(StylesUtil.BeardStyles[entityAsset.Beard].BeardName);
			yield return abr;
			if (abr.asset == null)
			{
				OnAssetLoadFailed("AssetBundleRequest.asset is null.");
				yield break;
			}
			textures.Add(abr.asset as Texture2D);
			vector4s.Add(new Rectangle(832, 0, 192, 527));
			color32s.Add(StylesUtil.EntityHairColors[entityAsset.ColorHair]);
		}
		if (entityAsset.Stache > 0 && StylesUtil.StacheStyles[entityAsset.Stache].Type == 1)
		{
			abr = ab.LoadAssetAsync<Texture2D>(StylesUtil.StacheStyles[entityAsset.Stache].StacheName);
			yield return abr;
			if (abr.asset == null)
			{
				OnAssetLoadFailed("AssetBundleRequest.asset is null.");
				yield break;
			}
			textures.Add(abr.asset as Texture2D);
			vector4s.Add(new Rectangle(832, 0, 192, 527));
			color32s.Add(StylesUtil.EntityHairColors[entityAsset.ColorHair]);
		}
		cr.tasks.Add(new CompositorTaskColorTint(vector4s, textures, color32s));
		cr.Composite();
		yield return cr;
	}

	private IEnumerator CreateCompositorTask(CompositorRequest cr, EquipItem eItem)
	{
		Texture2D texFileCM = null;
		Texture2D texFileC = null;
		string genderPrefix = "";
		if (eItem.GenderBased == ItemGenderBasedType.Both || eItem.GenderBased == ItemGenderBasedType.Texture)
		{
			genderPrefix = entityAsset.gender + "_";
		}
		AssetBundleRequest abr = loaders[eItem.bundle.FileName].Asset.LoadAssetAsync<Texture2D>(genderPrefix + eItem.FileD);
		yield return abr;
		Texture2D texFileD = abr.asset as Texture2D;
		if (texFileD == null)
		{
			OnAssetLoadFailed("TextureNotFound - " + genderPrefix + eItem.FileD + " not found in " + eItem.bundle.FileName);
			yield break;
		}
		if (eItem.ColorType == 1 || eItem.ColorType == 2)
		{
			abr = loaders[eItem.bundle.FileName].Asset.LoadAssetAsync<Texture2D>(genderPrefix + eItem.FileCM);
			yield return abr;
			texFileCM = abr.asset as Texture2D;
			if (texFileCM == null)
			{
				OnAssetLoadFailed("TextureNotFound - " + genderPrefix + eItem.FileCM + " not found in " + eItem.bundle.FileName);
				yield break;
			}
		}
		if (eItem.ColorType == 0 || eItem.ColorType == 2)
		{
			abr = loaders[eItem.bundle.FileName].Asset.LoadAssetAsync<Texture2D>(genderPrefix + eItem.FileC);
			yield return abr;
			texFileC = abr.asset as Texture2D;
			if (texFileC == null)
			{
				OnAssetLoadFailed("TextureNotFound - " + genderPrefix + eItem.FileC + " not found in " + eItem.bundle.FileName);
				yield break;
			}
		}
		if (eItem.ColorType == 0)
		{
			cr.tasks.Add(new CompositorTaskDiffuse(eItem.Rect, texFileC, texFileD));
		}
		else if (eItem.ColorType == 1)
		{
			cr.tasks.Add(new CompositorTaskColorMask(eItem.Rect, texFileCM, texFileD, eItem.ArmorColor));
		}
		else if (eItem.ColorType == 2)
		{
			cr.tasks.Add(new CompositorTaskPartialColorMask(eItem.Rect, texFileCM, texFileD, texFileC, eItem.ArmorColor));
		}
	}

	private Transform[] TranslateTransforms(Transform[] sources, TransformCatelog transformCatelog)
	{
		Transform[] array = new Transform[sources.Length];
		for (int i = 0; i < sources.Length; i++)
		{
			array[i] = transformCatelog.Find(sources[i].name);
		}
		return array;
	}

	protected GameObject InstantiateMeshLinkBones(GameObject source)
	{
		SkinnedMeshRenderer skinnedMeshRenderer = source.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true)[0];
		GameObject obj = new GameObject(skinnedMeshRenderer.name);
		obj.transform.localRotation = skinnedMeshRenderer.transform.localRotation;
		obj.transform.SetParent(currentAsset.transform, worldPositionStays: false);
		SkinnedMeshRenderer skinnedMeshRenderer2 = obj.AddComponent<SkinnedMeshRenderer>();
		skinnedMeshRenderer2.sharedMesh = skinnedMeshRenderer.sharedMesh;
		skinnedMeshRenderer2.materials = skinnedMeshRenderer.sharedMaterials;
		skinnedMeshRenderer2.bones = TranslateTransforms(skinnedMeshRenderer.bones, boneCatelog);
		return obj;
	}

	protected GameObject[] InstantiateAllMeshLinkBones(GameObject source)
	{
		SkinnedMeshRenderer[] componentsInChildren = source.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		GameObject[] array = new GameObject[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			GameObject gameObject = new GameObject(componentsInChildren[i].name);
			gameObject.transform.localRotation = componentsInChildren[i].transform.localRotation;
			gameObject.transform.SetParent(currentAsset.transform, worldPositionStays: false);
			SkinnedMeshRenderer skinnedMeshRenderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			skinnedMeshRenderer.sharedMesh = componentsInChildren[i].sharedMesh;
			skinnedMeshRenderer.materials = componentsInChildren[i].sharedMaterials;
			skinnedMeshRenderer.bones = TranslateTransforms(componentsInChildren[i].bones, boneCatelog);
			skinnedMeshRenderer.rootBone = skinnedMeshRenderer.transform.parent;
			array[i] = gameObject;
		}
		return array;
	}

	protected GameObject[] InstantiateWithChildrenAndLinkBones(GameObject source, Transform parent)
	{
		Renderer[] componentsInChildren = source.GetComponentsInChildren<Renderer>(includeInactive: true);
		GameObject[] array = new GameObject[componentsInChildren.Length];
		List<string> list = new List<string>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Renderer renderer = componentsInChildren[i];
			if (renderer.GetType() != typeof(SkinnedMeshRenderer))
			{
				if (boneCatelog.ContainsKey(renderer.transform.parent.name))
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(renderer.gameObject);
					gameObject.transform.SetParent(boneCatelog[renderer.transform.parent.name], worldPositionStays: false);
					array[i] = gameObject;
				}
			}
			else
			{
				if (list.Contains(renderer.gameObject.name))
				{
					continue;
				}
				GameObject obj = UnityEngine.Object.Instantiate(renderer.gameObject);
				SkinnedMeshRenderer component = obj.GetComponent<SkinnedMeshRenderer>();
				component.bones = TranslateTransforms(component.bones, boneCatelog);
				component.updateWhenOffscreen = false;
				if (component.rootBone == null)
				{
					component.rootBone = parent;
				}
				else
				{
					component.rootBone = boneCatelog.Find(component.rootBone.name);
				}
				component.transform.SetParent(parent, worldPositionStays: false);
				component.quality = SkinQuality.Bone4;
				Renderer[] componentsInChildren2 = obj.GetComponentsInChildren<Renderer>();
				foreach (Renderer renderer2 in componentsInChildren2)
				{
					if (renderer2 != component)
					{
						renderer2.transform.localPosition = Vector3.zero;
						renderer2.transform.localRotation = Quaternion.identity;
						list.Add(renderer2.name);
					}
				}
				array[i] = component.gameObject;
			}
		}
		return array;
	}

	protected virtual IEnumerator Instantiate()
	{
		AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(defaultPrefabName);
		yield return abr;
		SetCurrentAsset(defaultPrefabName, defaultPrefabBundle.FileName, abr.asset as GameObject);
		currentAsset.SetActive(value: false);
		string prefix = ((entityAsset.gender == "F") ? "Female_" : "Male_");
		string text = LegsMeshes[equips[EquipItemSlot.Armor].GetMeshType1(entityAsset.gender)];
		string text2 = ShirtMeshes[equips[EquipItemSlot.Armor].GetMeshType0(entityAsset.gender)];
		bool isRobeEquipped = false;
		bool isGloveOverride = false;
		bool isHeadOverride = false;
		if (string.IsNullOrEmpty(equips[EquipItemSlot.Armor].AssetName))
		{
			isRobeEquipped = text == "Legs_Robe" || text == "Legs_Pants" || text == "Legs_TatteredRobe";
			int num;
			switch (text2)
			{
			default:
				num = ((text2 == "Shirt_TatteredRobe") ? 1 : 0);
				break;
			case "Shirt_Robe":
			case "Shirt_Jacket":
			case "Shirt_Jacket02":
			case "Shirt_Jacket03":
			case "Shirt_Jacket04":
			case "Shirt_ComboLongCoat":
				num = 1;
				break;
			}
			isGloveOverride = (byte)num != 0;
			abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(prefix + ShirtMeshes[equips[EquipItemSlot.Armor].GetMeshType0(entityAsset.gender)]);
			yield return abr;
			GameObject gameObject = abr.asset as GameObject;
			InstantiateWithChildrenAndLinkBones(gameObject, currentAsset.transform);
			matMain = new Material(gameObject.GetComponentsInChildren<SkinnedMeshRenderer>()[0].sharedMaterial);
			abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(prefix + LegsMeshes[equips[EquipItemSlot.Armor].GetMeshType1(entityAsset.gender)]);
			yield return abr;
			InstantiateWithChildrenAndLinkBones(abr.asset as GameObject, currentAsset.transform);
		}
		else
		{
			abr = loaders[equips[EquipItemSlot.Armor].bundle.FileName].Asset.LoadAssetAsync<GameObject>(entityAsset.gender + "_" + equips[EquipItemSlot.Armor].AssetName);
			yield return abr;
			GameObject gameObject2 = abr.asset as GameObject;
			if ((bool)gameObject2.GetComponent<EquipHelper>())
			{
				EquipHelper component = gameObject2.GetComponent<EquipHelper>();
				for (int i = 0; i < component.equipTags.Count; i++)
				{
					if (component.equipTags[i].equipTag.ToLower() == "head" && component.equipTags[i].gameObjects.Count > 0 && ((equips.ContainsKey(EquipItemSlot.Helm) && equips[EquipItemSlot.Helm].GetMeshType0(entityAsset.gender) != 2) || !equips.ContainsKey(EquipItemSlot.Helm)))
					{
						for (int j = 0; j < component.equipTags[i].gameObjects.Count; j++)
						{
							InstantiateWithChildrenAndLinkBones(component.equipTags[i].gameObjects[j], currentAsset.transform);
							isHeadOverride = true;
						}
					}
				}
			}
			InstantiateWithChildrenAndLinkBones(gameObject2, currentAsset.transform);
			matMain = new Material(gameObject2.GetComponentsInChildren<SkinnedMeshRenderer>()[0].sharedMaterial);
			if (equips[EquipItemSlot.Armor].Mesh0 == 1)
			{
				isRobeEquipped = true;
			}
			if (equips[EquipItemSlot.Armor].Mesh1 == 1)
			{
				isGloveOverride = true;
			}
		}
		if ((!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType0(entityAsset.gender) != 2) && !isHeadOverride)
		{
			abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(prefix + "Head_Human");
			yield return abr;
			InstantiateMeshLinkBones(abr.asset as GameObject);
		}
		int num2 = 0;
		if (equips.ContainsKey(EquipItemSlot.Boots))
		{
			num2 = ((!isRobeEquipped) ? equips[EquipItemSlot.Boots].GetMeshType0(entityAsset.gender) : 2);
		}
		BundleInfo bundleInfo;
		string assetName;
		if (equips.ContainsKey(EquipItemSlot.Boots) && !string.IsNullOrEmpty(equips[EquipItemSlot.Boots].AssetName) && !isRobeEquipped)
		{
			bundleInfo = equips[EquipItemSlot.Boots].bundle;
			assetName = entityAsset.gender + "_" + equips[EquipItemSlot.Boots].AssetName;
		}
		else
		{
			bundleInfo = defaultPrefabBundle;
			assetName = prefix + BootsMeshes[num2];
		}
		abr = loaders[bundleInfo.FileName].Asset.LoadAssetAsync<GameObject>(assetName);
		yield return abr;
		InstantiateAllMeshLinkBones(abr.asset as GameObject);
		int gauntletOverride = 0;
		if (equips.ContainsKey(EquipItemSlot.Weapon))
		{
			gauntletOverride = equips[EquipItemSlot.Weapon].GetMeshType1(entityAsset.gender);
		}
		int num3 = 0;
		if (!isGloveOverride && equips.ContainsKey(EquipItemSlot.Gloves))
		{
			num3 = equips[EquipItemSlot.Gloves].GetMeshType0(entityAsset.gender);
		}
		if (equips.ContainsKey(EquipItemSlot.Gloves) && !string.IsNullOrEmpty(equips[EquipItemSlot.Gloves].AssetName) && !isGloveOverride)
		{
			bundleInfo = equips[EquipItemSlot.Gloves].bundle;
			assetName = entityAsset.gender + "_" + equips[EquipItemSlot.Gloves].AssetName;
		}
		else
		{
			bundleInfo = defaultPrefabBundle;
			assetName = prefix + GlovesMeshes[num3];
		}
		abr = loaders[bundleInfo.FileName].Asset.LoadAssetAsync<GameObject>(assetName + "_L");
		yield return abr;
		glovesL = InstantiateAllMeshLinkBones(abr.asset as GameObject);
		abr = loaders[bundleInfo.FileName].Asset.LoadAssetAsync<GameObject>(assetName + "_R");
		yield return abr;
		glovesR = InstantiateAllMeshLinkBones(abr.asset as GameObject);
		GameObject[] array = glovesR;
		foreach (GameObject obj in array)
		{
			obj.SetLayerRecursively(Layers.OTHER_PLAYERS);
			obj.SetActive(value: false);
		}
		array = glovesL;
		foreach (GameObject obj2 in array)
		{
			obj2.SetLayerRecursively(Layers.OTHER_PLAYERS);
			obj2.SetActive(value: false);
		}
		if (equips.ContainsKey(EquipItemSlot.Belt))
		{
			abr = LoadAssetFromBundle(equips[EquipItemSlot.Belt]);
			yield return abr;
			InstantiateAllMeshLinkBones(abr.asset as GameObject);
		}
		if ((!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 1 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 2) && entityAsset.Braid > 0)
		{
			abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(prefix + StylesUtil.BraidStyles[entityAsset.Braid].BraidName);
			yield return abr;
			GameObject source = abr.asset as GameObject;
			InstantiateMeshLinkBones(source);
		}
		if (!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType0(entityAsset.gender) != 2 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) > 1)
		{
			if (entityAsset.Beard > 0 && StylesUtil.BeardStyles[entityAsset.Beard].Type == 0)
			{
				abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(prefix + StylesUtil.BeardStyles[entityAsset.Beard].BeardName);
				yield return abr;
				GameObject source2 = abr.asset as GameObject;
				InstantiateMeshLinkBones(source2);
			}
			if (entityAsset.Stache > 0 && StylesUtil.StacheStyles[entityAsset.Stache].Type == 0)
			{
				abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(prefix + StylesUtil.StacheStyles[entityAsset.Stache].StacheName);
				yield return abr;
				GameObject source3 = abr.asset as GameObject;
				InstantiateMeshLinkBones(source3);
			}
		}
		SkinnedMeshRenderer skinnedMeshRenderer = SkinnedMeshCombiner.CombineMeshes(currentAsset, (from p in currentAsset.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true)
			where p.sharedMesh.isReadable
			select p).ToArray());
		skinnedMeshRenderer.transform.gameObject.SetLayerRecursively(Layers.OTHER_PLAYERS);
		Transform probeAnchor = boneCatelog["Pelvis"];
		skinnedMeshRenderer.rootBone = boneCatelog["Pelvis"];
		skinnedMeshRenderer.probeAnchor = probeAnchor;
		skinnedMeshRenderer.material = matMain;
		matMain.mainTexture = main_d;
		Color value = StylesUtil.EntityEyeColors[entityAsset.ColorEye];
		Color value2 = StylesUtil.EntityLipColors[entityAsset.ColorLip];
		if (entityAsset.Face == 1)
		{
			matMain.SetTexture("_FaceTex", null);
		}
		if (equips[EquipItemSlot.Armor].RegionType == 1 || equips[EquipItemSlot.Armor].RegionType == 3)
		{
			value.a = 0f;
		}
		if (equips[EquipItemSlot.Armor].RegionType == 1 || equips[EquipItemSlot.Armor].RegionType == 2)
		{
			value2.a = 0f;
		}
		matMain.SetColor("_EyeColor", value);
		matMain.SetColor("_LipColor", value2);
		SetFaceOffset(isBlinking ? face_DefaultOffset : face_DeadOffset);
		currentAsset.SetActive(value: false);
		array = glovesR;
		foreach (GameObject obj3 in array)
		{
			SkinnedMeshRenderer component2 = obj3.GetComponent<SkinnedMeshRenderer>();
			component2.material = matMain;
			component2.shadowCastingMode = ShadowCastingMode.On;
			component2.receiveShadows = true;
			component2.quality = SkinQuality.Bone4;
			component2.rootBone = boneCatelog["Neck"];
			obj3.transform.SetParent(boneCatelog["R_ArmHand_Mount"]);
		}
		array = glovesL;
		foreach (GameObject obj4 in array)
		{
			SkinnedMeshRenderer component3 = obj4.GetComponent<SkinnedMeshRenderer>();
			component3.material = matMain;
			component3.shadowCastingMode = ShadowCastingMode.On;
			component3.receiveShadows = true;
			component3.quality = SkinQuality.Bone4;
			component3.rootBone = boneCatelog["Neck"];
			obj4.transform.SetParent(boneCatelog["L_ArmHand_Mount"]);
		}
		switch (gauntletOverride)
		{
		case 0:
		{
			array = glovesR;
			for (int k = 0; k < array.Length; k++)
			{
				array[k].SetActive(value: true);
			}
			array = glovesL;
			for (int k = 0; k < array.Length; k++)
			{
				array[k].SetActive(value: true);
			}
			gloveHide = GloveHideState.None;
			break;
		}
		case 1:
		{
			array = glovesL;
			for (int k = 0; k < array.Length; k++)
			{
				array[k].SetActive(value: true);
			}
			gloveHide = GloveHideState.Right;
			break;
		}
		default:
			gloveHide = GloveHideState.Both;
			break;
		}
		Transform parent = boneCatelog["Helmet_Mount"];
		if (equips.ContainsKey(EquipItemSlot.Helm))
		{
			abr = LoadAssetFromBundle(equips[EquipItemSlot.Helm]);
			yield return abr;
			if (!(abr.asset != null))
			{
				Debug.LogError("This asset was null!");
				yield break;
			}
			GameObject gameObject3 = UnityEngine.Object.Instantiate(abr.asset as GameObject);
			gameObject3.SetLayerRecursively(Layers.OTHER_PLAYERS);
			SetHelmColors(gameObject3, equips[EquipItemSlot.Helm].ColorR, equips[EquipItemSlot.Helm].ColorG, equips[EquipItemSlot.Helm].ColorB, StylesUtil.EntityHairColors[entityAsset.ColorHair], StylesUtil.EntitySkinColors[entityAsset.ColorSkin].A, StylesUtil.EntitySkinColors[entityAsset.ColorSkin].B, StylesUtil.EntityEyeColors[entityAsset.ColorEye], StylesUtil.EntityLipColors[entityAsset.ColorLip], probeAnchor);
			if (equips[EquipItemSlot.Helm].GetMeshType0(entityAsset.gender) != 0 && (bool)gameObject3.GetComponentInChildren<SkinnedMeshRenderer>())
			{
				InstantiateWithChildrenAndLinkBones(gameObject3, parent);
				UnityEngine.Object.Destroy(gameObject3);
			}
			else
			{
				gameObject3.transform.SetParent(parent, worldPositionStays: false);
			}
		}
		string text3 = "";
		if (!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 1 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 2)
		{
			text3 = ((entityAsset.gender == "M") ? StylesUtil.HairStyles[entityAsset.Hair].MaleHairName : StylesUtil.HairStyles[entityAsset.Hair].FemaleHairName);
		}
		if (text3 != "")
		{
			abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(text3);
			yield return abr;
			GameObject source4 = abr.asset as GameObject;
			GameObject obj5 = InstantiateMeshLinkBones(source4);
			obj5.layer = Layers.OTHER_PLAYERS;
			SkinnedMeshRenderer component4 = obj5.GetComponent<SkinnedMeshRenderer>();
			component4.rootBone = parent;
			component4.probeAnchor = probeAnchor;
			hairMat = component4.material;
			hairMat.color = StylesUtil.EntityHairColors[entityAsset.ColorHair];
		}
		if (equips.ContainsKey(EquipItemSlot.Shoulders))
		{
			abr = LoadAssetFromBundle(equips[EquipItemSlot.Shoulders]);
			yield return abr;
			if (!(abr.asset != null))
			{
				Debug.LogError("This asset was null!");
				yield break;
			}
			GameObject gameObject4 = UnityEngine.Object.Instantiate(abr.asset as GameObject);
			gameObject4.SetLayerRecursively(Layers.OTHER_PLAYERS);
			AssetController.SetItemColors(gameObject4, equips[EquipItemSlot.Shoulders].ColorR, equips[EquipItemSlot.Shoulders].ColorG, equips[EquipItemSlot.Shoulders].ColorB, probeAnchor);
			int meshType = equips[EquipItemSlot.Shoulders].GetMeshType0(entityAsset.gender);
			if (meshType == 1)
			{
				InstantiateWithChildrenAndLinkBones(gameObject4, currentAsset.transform);
			}
			else
			{
				if (meshType == 0 || meshType == 2)
				{
					gameObject4.transform.SetParent(boneCatelog["R_Shoulder_Mount"], worldPositionStays: false);
				}
				if (meshType == 0 || meshType == 3)
				{
					UnityEngine.Object.Instantiate(gameObject4).transform.SetParent(boneCatelog["L_Shoulder_Mount"], worldPositionStays: false);
				}
			}
			if (meshType == 1 || meshType == 3)
			{
				UnityEngine.Object.Destroy(gameObject4);
			}
		}
		if (equips.ContainsKey(entityAsset.Current))
		{
			WeaponGameobjects.Clear();
			if (equips[entityAsset.Current].GetMeshType0(entityAsset.gender) != 1)
			{
				abr = LoadAssetFromBundle(equips[entityAsset.Current]);
				yield return abr;
				if (abr.asset == null)
				{
					Debug.LogError(equips[entityAsset.Current].AssetName + " could not be loaded. The asset was null!");
					yield break;
				}
				weaponGO = UnityEngine.Object.Instantiate(abr.asset as GameObject);
				weaponGO.SetLayerRecursively(Layers.OTHER_PLAYERS);
				AssetController.SetItemColors(weaponGO, equips[entityAsset.Current].ColorR, equips[entityAsset.Current].ColorG, equips[entityAsset.Current].ColorB, probeAnchor);
				if (entityAsset.WeaponRequired == EquipItemSlot.Bow)
				{
					weaponGO.transform.SetParent(boneCatelog["L_ArmHand_Mount"], worldPositionStays: false);
				}
				else
				{
					weaponGO.transform.SetParent(boneCatelog["R_ArmHand_Mount"], worldPositionStays: false);
				}
				WeaponGameobjects.Add(weaponGO);
				if ((!equips[entityAsset.Current].IsTool && entityAsset.DualWield) || equips[entityAsset.Current].GetMeshType0(entityAsset.gender) == 2)
				{
					GameObject gameObject5 = UnityEngine.Object.Instantiate(weaponGO);
					gameObject5.transform.SetParent(boneCatelog["L_ArmHand_Mount"], worldPositionStays: false);
					WeaponGameobjects.Add(gameObject5);
				}
				if (entity != null)
				{
					SetWeaponsMounted(entity.IsSheathed);
				}
			}
			else
			{
				if (entityAsset.DualWield)
				{
					EquipItem equipItem = new EquipItem(equips[entityAsset.Current]);
					equipItem.AssetName += "_Dual";
					abr = LoadAssetFromBundle(equipItem);
					yield return abr;
					if (abr.asset == null)
					{
						abr = LoadAssetFromBundle(equips[entityAsset.Current]);
						yield return abr;
					}
				}
				else
				{
					abr = LoadAssetFromBundle(equips[entityAsset.Current]);
					yield return abr;
				}
				if (!(abr.asset != null))
				{
					Debug.LogError("This asset was null!");
					yield break;
				}
				weaponGO = UnityEngine.Object.Instantiate(abr.asset as GameObject);
				weaponGO.SetLayerRecursively(Layers.OTHER_PLAYERS);
				AssetController.SetItemColors(weaponGO, equips[entityAsset.Current].ColorR, equips[entityAsset.Current].ColorG, equips[entityAsset.Current].ColorB, probeAnchor);
				WeaponGameobjects.AddRange(InstantiateWithChildrenAndLinkBones(weaponGO, currentAsset.transform));
				UnityEngine.Object.Destroy(weaponGO);
			}
			base.ActiveSlot = entityAsset.Current;
		}
		if (equips.ContainsKey(EquipItemSlot.Back))
		{
			parent = boneCatelog["Back_Mount"];
			abr = LoadAssetFromBundle(equips[EquipItemSlot.Back]);
			yield return abr;
			if (!(abr.asset != null))
			{
				Debug.LogError("This asset was null!");
				yield break;
			}
			GameObject gameObject6 = UnityEngine.Object.Instantiate(abr.asset as GameObject);
			gameObject6.SetLayerRecursively(Layers.OTHER_PLAYERS);
			AssetController.SetItemColors(gameObject6, equips[EquipItemSlot.Back].ColorR, equips[EquipItemSlot.Back].ColorG, equips[EquipItemSlot.Back].ColorB, probeAnchor);
			if (equips[EquipItemSlot.Back].GetMeshType0(entityAsset.gender) == 0)
			{
				gameObject6.transform.SetParent(parent, worldPositionStays: false);
			}
			else
			{
				InstantiateWithChildrenAndLinkBones(gameObject6, parent);
				UnityEngine.Object.Destroy(gameObject6);
			}
		}
		hideOrb();
		currentAsset.SetActive(value: true);
		StartBlink();
		OnAssetUpdated(currentAsset);
	}

	protected IEnumerator InstantiateWeapon()
	{
		if (!equips.ContainsKey(EquipItemSlot.Weapon))
		{
			yield break;
		}
		List<GameObject> old = new List<GameObject>(WeaponGameobjects);
		WeaponGameobjects.Clear();
		AssetBundleRequest abr = LoadAssetFromBundle(equips[EquipItemSlot.Weapon]);
		if (boneCatelog == null)
		{
			yield return StartCoroutine(Instantiate());
		}
		Transform probeAnchor = boneCatelog["Pelvis"];
		switch (equips[EquipItemSlot.Weapon].GetMeshType1(entityAsset.gender))
		{
		case 0:
		{
			GameObject[] array = glovesR;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: true);
			}
			array = glovesL;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: true);
			}
			break;
		}
		case 1:
		{
			GameObject[] array = glovesR;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			array = glovesL;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: true);
			}
			break;
		}
		case 2:
		{
			GameObject[] array = glovesR;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			array = glovesL;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetActive(value: false);
			}
			break;
		}
		}
		if (equips[EquipItemSlot.Weapon].GetMeshType0(entityAsset.gender) != 1)
		{
			yield return abr;
			if (!(abr.asset != null))
			{
				Debug.LogError("This asset was null!");
				yield break;
			}
			weaponGO = UnityEngine.Object.Instantiate(abr.asset as GameObject);
			weaponGO.SetLayerRecursively(Layers.OTHER_PLAYERS);
			AssetController.SetItemColors(weaponGO, equips[EquipItemSlot.Weapon].ColorR, equips[EquipItemSlot.Weapon].ColorG, equips[EquipItemSlot.Weapon].ColorB, probeAnchor);
			if (entityAsset.WeaponRequired == EquipItemSlot.Bow)
			{
				weaponGO.transform.SetParent(boneCatelog["L_ArmHand_Mount"], worldPositionStays: false);
			}
			else
			{
				weaponGO.transform.SetParent(boneCatelog["R_ArmHand_Mount"], worldPositionStays: false);
			}
			WeaponGameobjects.Add(weaponGO);
			if (entityAsset.DualWield || equips[entityAsset.Current].GetMeshType0(entityAsset.gender) == 2)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(weaponGO);
				gameObject.transform.SetParent(boneCatelog["L_ArmHand_Mount"], worldPositionStays: false);
				WeaponGameobjects.Add(gameObject);
			}
			base.ActiveSlot = EquipItemSlot.Weapon;
			if (entity != null)
			{
				SetWeaponsMounted(entity.IsSheathed);
			}
		}
		else
		{
			if (entityAsset.DualWield)
			{
				EquipItem equipItem = new EquipItem(equips[EquipItemSlot.Weapon]);
				equipItem.AssetName += "_Dual";
				abr = LoadAssetFromBundle(equipItem);
				yield return abr;
				if (abr.asset == null)
				{
					abr = LoadAssetFromBundle(equips[EquipItemSlot.Weapon]);
					yield return abr;
				}
			}
			else
			{
				abr = LoadAssetFromBundle(equips[EquipItemSlot.Weapon]);
				yield return abr;
			}
			if (!(abr.asset != null))
			{
				Debug.LogError("This asset was null!");
				yield break;
			}
			weaponGO = UnityEngine.Object.Instantiate(abr.asset as GameObject);
			weaponGO.SetLayerRecursively(Layers.OTHER_PLAYERS);
			AssetController.SetItemColors(weaponGO, equips[EquipItemSlot.Weapon].ColorR, equips[EquipItemSlot.Weapon].ColorG, equips[EquipItemSlot.Weapon].ColorB, probeAnchor);
			WeaponGameobjects.AddRange(InstantiateWithChildrenAndLinkBones(weaponGO, currentAsset.transform));
			UnityEngine.Object.Destroy(weaponGO);
			base.ActiveSlot = EquipItemSlot.Weapon;
		}
		foreach (GameObject item in old)
		{
			UnityEngine.Object.Destroy(item);
		}
		OnAssetUpdated(currentAsset);
	}

	protected IEnumerator InstantiateTool(EquipItemSlot slot)
	{
		if (!equips.ContainsKey(slot))
		{
			yield break;
		}
		List<GameObject> old = new List<GameObject>(WeaponGameobjects);
		WeaponGameobjects.Clear();
		AssetBundleRequest abr = LoadAssetFromBundle(equips[slot]);
		if (boneCatelog == null)
		{
			yield return StartCoroutine(Instantiate());
		}
		Transform probeAnchor = boneCatelog["Pelvis"];
		GameObject[] array = glovesR;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
		array = glovesL;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].SetActive(value: true);
		}
		if (equips[slot].GetMeshType0(entityAsset.gender) == 0)
		{
			yield return abr;
			if (!(abr.asset != null))
			{
				Debug.LogError("This asset was null!");
				yield break;
			}
			weaponGO = UnityEngine.Object.Instantiate(abr.asset as GameObject);
			weaponGO.SetLayerRecursively(Layers.OTHER_PLAYERS);
			AssetController.SetItemColors(weaponGO, equips[slot].ColorR, equips[slot].ColorG, equips[slot].ColorB, probeAnchor);
			weaponGO.transform.SetParent(boneCatelog["R_ArmHand_Mount"], worldPositionStays: false);
			WeaponGameobjects.Add(weaponGO);
		}
		else
		{
			abr = LoadAssetFromBundle(equips[slot]);
			yield return abr;
			if (!(abr.asset != null))
			{
				Debug.LogError("This asset was null!");
				yield break;
			}
			weaponGO = UnityEngine.Object.Instantiate(abr.asset as GameObject);
			weaponGO.SetLayerRecursively(Layers.OTHER_PLAYERS);
			AssetController.SetItemColors(weaponGO, equips[slot].ColorR, equips[slot].ColorG, equips[slot].ColorB, probeAnchor);
			WeaponGameobjects.AddRange(InstantiateWithChildrenAndLinkBones(weaponGO, currentAsset.transform));
			UnityEngine.Object.Destroy(weaponGO);
		}
		foreach (GameObject item in old)
		{
			UnityEngine.Object.Destroy(item);
		}
		base.ActiveSlot = slot;
		OnAssetUpdated(currentAsset);
	}

	protected void SetCurrentAsset(string newAssetName, string newAssetBundleName, GameObject newAsset)
	{
		Debug.Log("setCurrentAsset( " + newAssetName + ", " + newAssetBundleName + " ) for '" + base.name + "'");
		if (newAssetName == defaultPrefabName && defaultPrefab == null)
		{
			defaultPrefab = newAsset;
		}
		currentAsset = UnityEngine.Object.Instantiate(newAsset);
		currentAsset.name = "charGO";
		currentAsset.transform.SetParent(base.transform, worldPositionStays: false);
		assetTransform = currentAsset.transform;
		boneCatelog = new TransformCatelog(assetTransform);
	}

	protected static void SetHelmColors(GameObject go, string r, string g, string b, Color32 haircolor, Color32 skincolorA, Color32 skincolorB, Color32 eyecolor, Color32 lipcolor, Transform probeAnchor)
	{
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Material material = componentsInChildren[i].material;
			if (!string.IsNullOrEmpty(r) && !string.IsNullOrEmpty(g) && !string.IsNullOrEmpty(b))
			{
				material.SetColor("_RColor", r.ToColor32());
				material.SetColor("_GColor", g.ToColor32());
				material.SetColor("_BColor", b.ToColor32());
			}
			material.SetColor("_HairColor", haircolor);
			material.SetColor("_R1Color", skincolorB);
			material.SetColor("_R2Color", skincolorA);
			material.SetColor("_EyeColor", eyecolor);
			material.SetColor("_LipColor", lipcolor);
			componentsInChildren[i].probeAnchor = probeAnchor;
		}
	}

	public void SetFaceOffset(Vector4 face)
	{
		if (matMain != null && matMain.HasProperty("_FaceOffset"))
		{
			matMain.SetVector("_FaceOffset", face);
		}
	}

	public void StartBlink()
	{
		if (isBlinking)
		{
			StartCoroutine("Blink");
		}
	}

	public void StopBlink()
	{
		StopCoroutine("Blink");
	}

	private IEnumerator Blink()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f, 6f));
		SetFaceOffset(face_BlinkOffset);
		yield return new WaitForSeconds(0.05f);
		SetFaceOffset(face_DefaultOffset);
		StartCoroutine("Blink");
	}

	protected override void Destroy()
	{
		UnityEngine.Object.Destroy(main_d);
		UnityEngine.Object.Destroy(matMain);
		UnityEngine.Object.Destroy(hairMat);
		base.Destroy();
	}

	protected override void OnAssetUpdated(GameObject go)
	{
		base.OnAssetUpdated(go);
		this.BobberDelayed?.Invoke();
		this.ToolDelayed?.Invoke();
		this.WeaponDelayed?.Invoke();
	}

	public Sheathing.Type GetMountingType()
	{
		try
		{
			if (base.ActiveSlot != entityAsset.Current)
			{
				return Sheathing.Type.None;
			}
			if (equips.TryGetValue(entityAsset.WeaponRequired, out var value))
			{
				if (value.DisableSheathing)
				{
					return Sheathing.Type.None;
				}
				if (value.GetMeshType0(entityAsset.gender) == 1 || value.IsTool)
				{
					return Sheathing.Type.None;
				}
			}
			if (entityAsset.WeaponRequired == EquipItemSlot.Bow)
			{
				return Sheathing.Type.None;
			}
			if (entityAsset.WeaponRequired == EquipItemSlot.Pistol)
			{
				return Sheathing.Type.Guns;
			}
			if (entityAsset.WeaponRequired == EquipItemSlot.Weapon)
			{
				if (WeaponGameobjects.Count > 1)
				{
					return Sheathing.Type.DualWield;
				}
				return Sheathing.Type.OneHanded;
			}
			return Sheathing.Type.None;
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
			return Sheathing.Type.None;
		}
	}

	public void SetWeaponsMounted(bool toSheathed)
	{
		Sheathing.Type mountingType = GetMountingType();
		if (weaponGO == null || entity == null || mountingType == Sheathing.Type.None)
		{
			return;
		}
		try
		{
			switch (mountingType)
			{
			case Sheathing.Type.OneHanded:
				if (toSheathed)
				{
					weaponGO.transform.SetParent(boneCatelog["R_MeleeMount"], worldPositionStays: false);
				}
				else
				{
					weaponGO.transform.SetParent(boneCatelog["R_ArmHand_Mount"], worldPositionStays: false);
				}
				break;
			case Sheathing.Type.DualWield:
				if (WeaponGameobjects.Count() == 2)
				{
					if (toSheathed)
					{
						WeaponGameobjects[0].transform.SetParent(boneCatelog["R_MeleeMount"], worldPositionStays: false);
						WeaponGameobjects[1].transform.SetParent(boneCatelog["L_MeleeMount"], worldPositionStays: false);
					}
					else
					{
						WeaponGameobjects[0].transform.SetParent(boneCatelog["R_ArmHand_Mount"], worldPositionStays: false);
						WeaponGameobjects[1].transform.SetParent(boneCatelog["L_ArmHand_Mount"], worldPositionStays: false);
					}
				}
				break;
			case Sheathing.Type.Bow:
				if (toSheathed)
				{
					weaponGO.transform.SetParent(boneCatelog["L_MeleeMount"], worldPositionStays: false);
				}
				else
				{
					weaponGO.transform.SetParent(boneCatelog["L_ArmHand_Mount"], worldPositionStays: false);
				}
				break;
			case Sheathing.Type.Guns:
				if (WeaponGameobjects.Count() == 2)
				{
					if (toSheathed)
					{
						WeaponGameobjects[0].transform.SetParent(boneCatelog["R_HandgunMount"], worldPositionStays: false);
						WeaponGameobjects[1].transform.SetParent(boneCatelog["L_HandgunMount"], worldPositionStays: false);
					}
					else
					{
						WeaponGameobjects[0].transform.SetParent(boneCatelog["R_ArmHand_Mount"], worldPositionStays: false);
						WeaponGameobjects[1].transform.SetParent(boneCatelog["L_ArmHand_Mount"], worldPositionStays: false);
					}
				}
				break;
			}
		}
		catch (Exception exception)
		{
			Debug.LogException(exception);
		}
	}
}
