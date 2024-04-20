using System.Collections;
using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public class PlayerCustomizeAssetController : PlayerAssetController
{
	private GameObject braidGO;

	private GameObject beardGO;

	private GameObject stacheGO;

	private GameObject hairGO;

	private List<GameObject> HelmetGameobjects { get; set; }

	protected override void Destroy()
	{
		if (HelmetGameobjects != null && HelmetGameobjects.Count > 0)
		{
			HelmetGameobjects.Clear();
		}
	}

	protected override IEnumerator Instantiate()
	{
		AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(defaultPrefabName);
		yield return abr;
		SetCurrentAsset(defaultPrefabName, defaultPrefabBundle.FileName, abr.asset as GameObject);
		currentAsset.SetActive(value: false);
		string prefix = ((entityAsset.gender == "F") ? "Female_" : "Male_");
		string text = PlayerAssetController.LegsMeshes[equips[EquipItemSlot.Armor].GetMeshType1(entityAsset.gender)];
		string text2 = PlayerAssetController.ShirtMeshes[equips[EquipItemSlot.Armor].GetMeshType0(entityAsset.gender)];
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
			abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(prefix + PlayerAssetController.ShirtMeshes[equips[EquipItemSlot.Armor].GetMeshType0(entityAsset.gender)]);
			yield return abr;
			GameObject gameObject = abr.asset as GameObject;
			InstantiateWithChildrenAndLinkBones(gameObject, currentAsset.transform);
			matMain = new Material(gameObject.GetComponentsInChildren<SkinnedMeshRenderer>()[0].sharedMaterial);
			abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(prefix + PlayerAssetController.LegsMeshes[equips[EquipItemSlot.Armor].GetMeshType1(entityAsset.gender)]);
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
			assetName = prefix + PlayerAssetController.BootsMeshes[num2];
		}
		abr = loaders[bundleInfo.FileName].Asset.LoadAssetAsync<GameObject>(assetName);
		yield return abr;
		InstantiateAllMeshLinkBones(abr.asset as GameObject);
		int num3 = 0;
		if (equips.ContainsKey(EquipItemSlot.Weapon))
		{
			num3 = equips[EquipItemSlot.Weapon].GetMeshType1(entityAsset.gender);
		}
		int num4 = 0;
		if (!isGloveOverride && equips.ContainsKey(EquipItemSlot.Gloves))
		{
			num4 = equips[EquipItemSlot.Gloves].GetMeshType0(entityAsset.gender);
		}
		if (equips.ContainsKey(EquipItemSlot.Gloves) && !string.IsNullOrEmpty(equips[EquipItemSlot.Gloves].AssetName) && !isGloveOverride)
		{
			bundleInfo = equips[EquipItemSlot.Gloves].bundle;
			assetName = entityAsset.gender + "_" + equips[EquipItemSlot.Gloves].AssetName;
		}
		else
		{
			bundleInfo = defaultPrefabBundle;
			assetName = prefix + PlayerAssetController.GlovesMeshes[num4];
		}
		switch (num3)
		{
		case 0:
			abr = loaders[bundleInfo.FileName].Asset.LoadAssetAsync<GameObject>(assetName + "_L");
			yield return abr;
			InstantiateAllMeshLinkBones(abr.asset as GameObject);
			abr = loaders[bundleInfo.FileName].Asset.LoadAssetAsync<GameObject>(assetName + "_R");
			yield return abr;
			InstantiateAllMeshLinkBones(abr.asset as GameObject);
			break;
		case 1:
			abr = loaders[bundleInfo.FileName].Asset.LoadAssetAsync<GameObject>(assetName + "_L");
			yield return abr;
			InstantiateAllMeshLinkBones(abr.asset as GameObject);
			break;
		}
		if (equips.ContainsKey(EquipItemSlot.Belt))
		{
			abr = LoadAssetFromBundle(equips[EquipItemSlot.Belt]);
			yield return abr;
			GameObject source = abr.asset as GameObject;
			InstantiateAllMeshLinkBones(source);
		}
		currentAsset.gameObject.SetLayerRecursively(Layers.OTHER_PLAYERS);
		Transform probeAnchor = boneCatelog["Pelvis"];
		SkinnedMeshRenderer[] componentsInChildren = currentAsset.GetComponentsInChildren<SkinnedMeshRenderer>(includeInactive: true);
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].rootBone = boneCatelog["Pelvis"];
			componentsInChildren[k].probeAnchor = probeAnchor;
			componentsInChildren[k].material = matMain;
		}
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
		Transform parent = boneCatelog["Helmet_Mount"];
		if (equips.ContainsKey(EquipItemSlot.Helm))
		{
			if (HelmetGameobjects == null)
			{
				HelmetGameobjects = new List<GameObject>();
			}
			else if (HelmetGameobjects.Count > 0)
			{
				HelmetGameobjects.Clear();
			}
			abr = LoadAssetFromBundle(equips[EquipItemSlot.Helm]);
			yield return abr;
			GameObject gameObject3 = Object.Instantiate(abr.asset as GameObject);
			gameObject3.SetLayerRecursively(Layers.OTHER_PLAYERS);
			PlayerAssetController.SetHelmColors(gameObject3, equips[EquipItemSlot.Helm].ColorR, equips[EquipItemSlot.Helm].ColorG, equips[EquipItemSlot.Helm].ColorB, StylesUtil.EntityHairColors[entityAsset.ColorHair], StylesUtil.EntitySkinColors[entityAsset.ColorSkin].A, StylesUtil.EntitySkinColors[entityAsset.ColorSkin].B, StylesUtil.EntityEyeColors[entityAsset.ColorEye], StylesUtil.EntityLipColors[entityAsset.ColorLip], probeAnchor);
			if (equips[EquipItemSlot.Helm].GetMeshType0(entityAsset.gender) != 0 && (bool)gameObject3.GetComponentInChildren<SkinnedMeshRenderer>())
			{
				HelmetGameobjects.AddRange(InstantiateWithChildrenAndLinkBones(gameObject3, parent));
				Object.Destroy(gameObject3);
			}
			else
			{
				HelmetGameobjects.Add(gameObject3);
				gameObject3.transform.SetParent(parent, worldPositionStays: false);
			}
		}
		if (equips.ContainsKey(EquipItemSlot.Shoulders))
		{
			abr = LoadAssetFromBundle(equips[EquipItemSlot.Shoulders]);
			yield return abr;
			GameObject gameObject4 = Object.Instantiate(abr.asset as GameObject);
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
					Object.Instantiate(gameObject4).transform.SetParent(boneCatelog["L_Shoulder_Mount"], worldPositionStays: false);
				}
			}
			if (meshType == 1 || meshType == 3)
			{
				Object.Destroy(gameObject4);
			}
		}
		if (equips.ContainsKey(EquipItemSlot.Weapon))
		{
			if (equips[EquipItemSlot.Weapon].GetMeshType0(entityAsset.gender) != 1)
			{
				abr = LoadAssetFromBundle(equips[EquipItemSlot.Weapon]);
				yield return abr;
				GameObject gameObject5 = Object.Instantiate(abr.asset as GameObject);
				gameObject5.SetLayerRecursively(Layers.OTHER_PLAYERS);
				AssetController.SetItemColors(gameObject5, equips[EquipItemSlot.Weapon].ColorR, equips[EquipItemSlot.Weapon].ColorG, equips[EquipItemSlot.Weapon].ColorB, probeAnchor);
				if (entityAsset.WeaponRequired == EquipItemSlot.Bow)
				{
					gameObject5.transform.SetParent(boneCatelog["L_ArmHand_Mount"], worldPositionStays: false);
				}
				else
				{
					gameObject5.transform.SetParent(boneCatelog["R_ArmHand_Mount"], worldPositionStays: false);
				}
				if (entityAsset.DualWield || equips[entityAsset.Current].GetMeshType0(entityAsset.gender) == 2)
				{
					Object.Instantiate(gameObject5).transform.SetParent(boneCatelog["L_ArmHand_Mount"], worldPositionStays: false);
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
				GameObject gameObject6 = Object.Instantiate(abr.asset as GameObject);
				gameObject6.SetLayerRecursively(Layers.OTHER_PLAYERS);
				AssetController.SetItemColors(gameObject6, equips[EquipItemSlot.Weapon].ColorR, equips[EquipItemSlot.Weapon].ColorG, equips[EquipItemSlot.Weapon].ColorB, probeAnchor);
				InstantiateWithChildrenAndLinkBones(gameObject6, currentAsset.transform);
				Object.Destroy(gameObject6);
			}
		}
		if (equips.ContainsKey(EquipItemSlot.Back))
		{
			parent = boneCatelog["Back_Mount"];
			abr = LoadAssetFromBundle(equips[EquipItemSlot.Back]);
			yield return abr;
			GameObject gameObject7 = Object.Instantiate(abr.asset as GameObject);
			gameObject7.SetLayerRecursively(Layers.OTHER_PLAYERS);
			AssetController.SetItemColors(gameObject7, equips[EquipItemSlot.Back].ColorR, equips[EquipItemSlot.Back].ColorG, equips[EquipItemSlot.Back].ColorB, probeAnchor);
			if (equips[EquipItemSlot.Back].GetMeshType0(entityAsset.gender) == 0)
			{
				gameObject7.transform.SetParent(parent, worldPositionStays: false);
			}
			else
			{
				InstantiateWithChildrenAndLinkBones(gameObject7, parent);
				Object.Destroy(gameObject7);
			}
		}
		yield return UpdateHairCoroutine();
		yield return UpdateBraidCoroutine();
		yield return UpdateStacheCoroutine();
		yield return UpdateBeardCoroutine();
		hideOrb();
		StartBlink();
		currentAsset.SetActive(value: true);
		OnAssetUpdated(currentAsset);
	}

	public void UpdateAll()
	{
		StopAllCoroutines();
		StartCoroutine(UpdateAllRoutine());
		StartBlink();
	}

	private void StopControllerRoutines()
	{
		StopCoroutine(Instantiate());
		StopCoroutine(UpdateAllRoutine());
		StopCoroutine(UpdateSkinColorRoutine());
		StopCoroutine(UpdateBraidCoroutine());
		StopCoroutine(UpdateBeardCoroutine());
		StopCoroutine(UpdateStacheCoroutine());
		StopCoroutine(CompositeRoutine());
	}

	private IEnumerator UpdateAllRoutine()
	{
		GameObject hair = null;
		GameObject braid = null;
		GameObject stache = null;
		GameObject beard = null;
		string text = "";
		if (!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 1 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 2)
		{
			text = ((entityAsset.gender == "M") ? StylesUtil.HairStyles[entityAsset.Hair].MaleHairName : StylesUtil.HairStyles[entityAsset.Hair].FemaleHairName);
		}
		if (text != "")
		{
			AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(text);
			yield return abr;
			hair = abr.asset as GameObject;
		}
		if (entityAsset.Braid > 0)
		{
			string text2 = ((entityAsset.gender == "F") ? "Female_" : "Male_");
			AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(text2 + StylesUtil.BraidStyles[entityAsset.Braid].BraidName);
			yield return abr;
			braid = abr.asset as GameObject;
		}
		if (entityAsset.Stache > 0 && StylesUtil.StacheStyles[entityAsset.Stache].Type == 0)
		{
			string text3 = ((entityAsset.gender == "F") ? "Female_" : "Male_");
			AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(text3 + StylesUtil.StacheStyles[entityAsset.Stache].StacheName);
			yield return abr;
			stache = abr.asset as GameObject;
		}
		if (entityAsset.Beard > 0 && StylesUtil.BeardStyles[entityAsset.Beard].Type == 0)
		{
			string text4 = ((entityAsset.gender == "F") ? "Female_" : "Male_");
			AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(text4 + StylesUtil.BeardStyles[entityAsset.Beard].BeardName);
			yield return abr;
			beard = abr.asset as GameObject;
		}
		yield return CompositeRoutine();
		Object.Destroy(hairGO);
		Object.Destroy(braidGO);
		Object.Destroy(stacheGO);
		Object.Destroy(beardGO);
		if (hair != null)
		{
			hairGO = InstantiateMeshLinkBones(hair);
			hairMat = hairGO.GetComponent<Renderer>().material;
			hairMat.color = StylesUtil.EntityHairColors[entityAsset.ColorHair];
		}
		if ((!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 1 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 2) && braid != null)
		{
			braidGO = InstantiateMeshLinkBones(braid);
			braidGO.GetComponent<SkinnedMeshRenderer>().sharedMaterial = matMain;
		}
		if (!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType0(entityAsset.gender) != 2 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) > 1)
		{
			if (stache != null)
			{
				stacheGO = InstantiateMeshLinkBones(stache);
				stacheGO.GetComponent<SkinnedMeshRenderer>().sharedMaterial = matMain;
			}
			if (beard != null)
			{
				beardGO = InstantiateMeshLinkBones(beard);
				beardGO.GetComponent<SkinnedMeshRenderer>().sharedMaterial = matMain;
			}
		}
		if (hairMat != null)
		{
			hairMat.color = StylesUtil.EntityHairColors[entityAsset.ColorHair];
		}
		if (matMain != null)
		{
			matMain.SetColor("_EyeColor", StylesUtil.EntityEyeColors[entityAsset.ColorEye]);
			matMain.SetColor("_LipColor", StylesUtil.EntityLipColors[entityAsset.ColorLip]);
		}
	}

	public void UpdateHairSkinColor()
	{
		StopAllCoroutines();
		StartCoroutine(UpdateSkinColorRoutine());
		if (equips.ContainsKey(EquipItemSlot.Helm))
		{
			Transform probeAnchor = boneCatelog["Pelvis"];
			foreach (GameObject helmetGameobject in HelmetGameobjects)
			{
				PlayerAssetController.SetHelmColors(helmetGameobject, equips[EquipItemSlot.Helm].ColorR, equips[EquipItemSlot.Helm].ColorG, equips[EquipItemSlot.Helm].ColorB, StylesUtil.EntityHairColors[entityAsset.ColorHair], StylesUtil.EntitySkinColors[entityAsset.ColorSkin].A, StylesUtil.EntitySkinColors[entityAsset.ColorSkin].B, StylesUtil.EntityEyeColors[entityAsset.ColorEye], StylesUtil.EntityLipColors[entityAsset.ColorLip], probeAnchor);
			}
		}
		StartBlink();
	}

	private IEnumerator UpdateSkinColorRoutine()
	{
		yield return StartCoroutine(CompositeRoutine());
		if (hairMat != null)
		{
			hairMat.color = StylesUtil.EntityHairColors[entityAsset.ColorHair];
		}
	}

	public void UpdateLipColor()
	{
		if (!(matMain != null))
		{
			return;
		}
		matMain.SetColor("_LipColor", StylesUtil.EntityLipColors[entityAsset.ColorLip]);
		if (!equips.ContainsKey(EquipItemSlot.Helm))
		{
			return;
		}
		Transform probeAnchor = boneCatelog["Pelvis"];
		foreach (GameObject helmetGameobject in HelmetGameobjects)
		{
			PlayerAssetController.SetHelmColors(helmetGameobject, equips[EquipItemSlot.Helm].ColorR, equips[EquipItemSlot.Helm].ColorG, equips[EquipItemSlot.Helm].ColorB, StylesUtil.EntityHairColors[entityAsset.ColorHair], StylesUtil.EntitySkinColors[entityAsset.ColorSkin].A, StylesUtil.EntitySkinColors[entityAsset.ColorSkin].B, StylesUtil.EntityEyeColors[entityAsset.ColorEye], StylesUtil.EntityLipColors[entityAsset.ColorLip], probeAnchor);
		}
	}

	public void UpdateEyeColor()
	{
		if (!(matMain != null))
		{
			return;
		}
		matMain.SetColor("_EyeColor", StylesUtil.EntityEyeColors[entityAsset.ColorEye]);
		if (!equips.ContainsKey(EquipItemSlot.Helm))
		{
			return;
		}
		Transform probeAnchor = boneCatelog["Pelvis"];
		foreach (GameObject helmetGameobject in HelmetGameobjects)
		{
			PlayerAssetController.SetHelmColors(helmetGameobject, equips[EquipItemSlot.Helm].ColorR, equips[EquipItemSlot.Helm].ColorG, equips[EquipItemSlot.Helm].ColorB, StylesUtil.EntityHairColors[entityAsset.ColorHair], StylesUtil.EntitySkinColors[entityAsset.ColorSkin].A, StylesUtil.EntitySkinColors[entityAsset.ColorSkin].B, StylesUtil.EntityEyeColors[entityAsset.ColorEye], StylesUtil.EntityLipColors[entityAsset.ColorLip], probeAnchor);
		}
	}

	public void UpdateBraid()
	{
		StartCoroutine(UpdateBraidCoroutine());
	}

	private IEnumerator UpdateBraidCoroutine()
	{
		if (!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 1 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 2)
		{
			if (entityAsset.Braid > 0)
			{
				string text = ((entityAsset.gender == "F") ? "Female_" : "Male_");
				AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(text + StylesUtil.BraidStyles[entityAsset.Braid].BraidName);
				yield return abr;
				Object.Destroy(braidGO);
				braidGO = InstantiateMeshLinkBones(abr.asset as GameObject);
				braidGO.GetComponent<SkinnedMeshRenderer>().sharedMaterial = matMain;
			}
			else
			{
				Object.Destroy(braidGO);
			}
		}
	}

	public void UpdateBeard()
	{
		UpdateHairSkinColor();
		StartCoroutine(UpdateBeardCoroutine());
	}

	private IEnumerator UpdateBeardCoroutine()
	{
		if (!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType0(entityAsset.gender) != 2 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) > 1)
		{
			if (entityAsset.Beard > 0 && StylesUtil.BeardStyles[entityAsset.Beard].Type == 0)
			{
				string text = ((entityAsset.gender == "F") ? "Female_" : "Male_");
				AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(text + StylesUtil.BeardStyles[entityAsset.Beard].BeardName);
				yield return abr;
				Object.Destroy(beardGO);
				beardGO = InstantiateMeshLinkBones(abr.asset as GameObject);
				beardGO.GetComponent<SkinnedMeshRenderer>().sharedMaterial = matMain;
			}
			else
			{
				Object.Destroy(beardGO);
			}
		}
	}

	public void UpdateStache()
	{
		UpdateHairSkinColor();
		StartCoroutine(UpdateStacheCoroutine());
	}

	private IEnumerator UpdateStacheCoroutine()
	{
		if (!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType0(entityAsset.gender) != 2 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) > 1)
		{
			if (entityAsset.Stache > 0 && StylesUtil.StacheStyles[entityAsset.Stache].Type == 0)
			{
				string text = ((entityAsset.gender == "F") ? "Female_" : "Male_");
				AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(text + StylesUtil.StacheStyles[entityAsset.Stache].StacheName);
				yield return abr;
				Object.Destroy(stacheGO);
				stacheGO = InstantiateMeshLinkBones(abr.asset as GameObject);
				stacheGO.GetComponent<SkinnedMeshRenderer>().sharedMaterial = matMain;
			}
			else
			{
				Object.Destroy(stacheGO);
			}
		}
	}

	public void UpdateHair()
	{
		StartCoroutine(UpdateHairCoroutine());
	}

	private IEnumerator UpdateHairCoroutine()
	{
		string text = "";
		if (!equips.ContainsKey(EquipItemSlot.Helm) || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 1 || equips[EquipItemSlot.Helm].GetMeshType1(entityAsset.gender) == 2)
		{
			text = ((entityAsset.gender == "M") ? StylesUtil.HairStyles[entityAsset.Hair].MaleHairName : StylesUtil.HairStyles[entityAsset.Hair].FemaleHairName);
		}
		if (text != "")
		{
			AssetBundleRequest abr = loaders[defaultPrefabBundle.FileName].Asset.LoadAssetAsync<GameObject>(text);
			yield return abr;
			GameObject source = abr.asset as GameObject;
			Object.Destroy(hairGO);
			hairGO = InstantiateMeshLinkBones(source);
			hairMat = hairGO.GetComponent<Renderer>().material;
			hairMat.color = StylesUtil.EntityHairColors[entityAsset.ColorHair];
		}
		else
		{
			Object.Destroy(hairGO);
		}
	}
}
