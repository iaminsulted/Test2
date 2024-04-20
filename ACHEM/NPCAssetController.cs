using System;
using System.Collections;
using System.Linq;
using StatCurves;
using UnityEngine;

public class NPCAssetController : AssetController
{
	private AssetBundleLoader request;

	protected override void Clear()
	{
		if (currentAsset != null)
		{
			UnityEngine.Object.Destroy(currentAsset);
		}
		if (request != null)
		{
			request.Dispose();
			request = null;
		}
		StopAllCoroutines();
	}

	public override IEnumerator LoadAsset(bool disableColliders)
	{
		Error = "";
		IsAssetLoadComplete = false;
		if (showorb)
		{
			showOrb();
		}
		defaultPrefabName = entityAsset.prefab;
		defaultPrefabBundle = entityAsset.bundle;
		request = AssetBundleManager.LoadAssetBundle(defaultPrefabBundle);
		yield return request;
		if (!string.IsNullOrEmpty(request.Error))
		{
			OnAssetLoadFailed(request.Error);
			yield break;
		}
		AssetBundleRequest abr = request.Asset.LoadAssetAsync<GameObject>(defaultPrefabName);
		yield return abr;
		if (abr.asset == null)
		{
			OnAssetLoadFailed(defaultPrefabName + " not found in " + defaultPrefabBundle.FileName);
			Debug.LogException(new Exception(defaultPrefabName + " not found in " + defaultPrefabBundle.FileName));
			yield break;
		}
		currentAsset = UnityEngine.Object.Instantiate(abr.asset) as GameObject;
		if (disableColliders)
		{
			entityAsset.ScaleFactor = currentAsset.transform.localScale.x;
		}
		MeshCombine component = currentAsset.GetComponent<MeshCombine>();
		if (component != null)
		{
			try
			{
				MeshUtil.CombineMesh(currentAsset, (from p in currentAsset.GetComponentsInChildren<SkinnedMeshRenderer>()
					where p.sharedMesh.isReadable
					select p).ToArray());
				currentAsset.transform.Find("Model").GetComponent<SkinnedMeshRenderer>().rootBone = component.RootBone;
			}
			catch (Exception innerException)
			{
				Debug.LogException(new Exception("AQ3DException-> [Error combining mesh: " + defaultPrefabName + " - " + defaultPrefabBundle.FileName + "]", innerException), base.gameObject);
			}
			currentAsset.SetActive(value: false);
			UnityEngine.Object.Destroy(component);
		}
		currentAsset.name = "charGO";
		EntityAssetData entityAssetData = currentAsset.GetComponent<EntityAssetData>();
		Transform probeAnchor = ((entityAssetData != null) ? entityAssetData.ProbeAnchor : null);
		AssetController.SetItemColors(currentAsset, entityAsset.ColorR, entityAsset.ColorG, entityAsset.ColorB, probeAnchor, entityAsset.ColorSkinA, entityAsset.ColorSkinB);
		if (entityAssetData != null)
		{
			equips.Clear();
			foreach (EquipItem value in entityAsset.equips.Values)
			{
				if (value.Mesh0 == 0 && ((value.EquipSlot == EquipItemSlot.Weapon && entityAssetData.RightWeaponMount != null) || (value.EquipSlot == EquipItemSlot.Weapon && entityAssetData.LeftWeaponMount != null) || (value.EquipSlot == EquipItemSlot.Shoulders && entityAssetData.RightShoulderMount != null) || (value.EquipSlot == EquipItemSlot.Shoulders && entityAssetData.LeftShoulderMount != null) || (value.EquipSlot == EquipItemSlot.Helm && entityAssetData.HelmetMount != null)))
				{
					equips.Add(value.EquipSlot, value);
				}
			}
			foreach (EquipItem value2 in equips.Values)
			{
				if (value2.bundle != null && !string.IsNullOrEmpty(value2.bundle.FileName))
				{
					AssetBundleLoader itemrequest = LoadAssetBundle(value2.bundle);
					yield return StartCoroutine(itemrequest);
					if (!string.IsNullOrEmpty(itemrequest.Error))
					{
						OnAssetLoadFailed(itemrequest.Error);
						yield break;
					}
				}
			}
			if (equips.ContainsKey(EquipItemSlot.Weapon) && (entityAssetData.RightWeaponMount != null || entityAssetData.LeftWeaponMount != null))
			{
				abr = LoadAssetFromBundle(equips[EquipItemSlot.Weapon]);
				yield return abr;
				if (entityAssetData.RightWeaponMount != null)
				{
					GameObject obj = UnityEngine.Object.Instantiate(abr.asset as GameObject);
					AssetController.SetItemColors(obj, equips[EquipItemSlot.Weapon].ColorR, equips[EquipItemSlot.Weapon].ColorG, equips[EquipItemSlot.Weapon].ColorB, null);
					obj.transform.SetParent(entityAssetData.RightWeaponMount, worldPositionStays: false);
				}
				if (entityAssetData.LeftWeaponMount != null && entityAsset.DualWield)
				{
					GameObject obj2 = UnityEngine.Object.Instantiate(abr.asset as GameObject);
					AssetController.SetItemColors(obj2, equips[EquipItemSlot.Weapon].ColorR, equips[EquipItemSlot.Weapon].ColorG, equips[EquipItemSlot.Weapon].ColorB, null);
					obj2.transform.SetParent(entityAssetData.LeftWeaponMount, worldPositionStays: false);
				}
			}
			if (equips.ContainsKey(EquipItemSlot.Shoulders) && (entityAssetData.RightShoulderMount != null || entityAssetData.LeftShoulderMount != null))
			{
				abr = LoadAssetFromBundle(equips[EquipItemSlot.Shoulders]);
				yield return abr;
				if (entityAssetData.RightShoulderMount != null)
				{
					GameObject obj3 = UnityEngine.Object.Instantiate(abr.asset as GameObject);
					AssetController.SetItemColors(obj3, equips[EquipItemSlot.Shoulders].ColorR, equips[EquipItemSlot.Shoulders].ColorG, equips[EquipItemSlot.Shoulders].ColorB, null);
					obj3.transform.SetParent(entityAssetData.RightShoulderMount, worldPositionStays: false);
				}
				if (entityAssetData.LeftShoulderMount != null)
				{
					GameObject obj4 = UnityEngine.Object.Instantiate(abr.asset as GameObject);
					AssetController.SetItemColors(obj4, equips[EquipItemSlot.Shoulders].ColorR, equips[EquipItemSlot.Shoulders].ColorG, equips[EquipItemSlot.Shoulders].ColorB, null);
					obj4.transform.SetParent(entityAssetData.LeftShoulderMount, worldPositionStays: false);
				}
			}
			if (equips.ContainsKey(EquipItemSlot.Helm) && entityAssetData.HelmetMount != null)
			{
				abr = LoadAssetFromBundle(equips[EquipItemSlot.Helm]);
				yield return abr;
				GameObject obj5 = UnityEngine.Object.Instantiate(abr.asset as GameObject);
				AssetController.SetItemColors(obj5, equips[EquipItemSlot.Helm].ColorR, equips[EquipItemSlot.Helm].ColorG, equips[EquipItemSlot.Helm].ColorB, null);
				obj5.transform.SetParent(entityAssetData.HelmetMount, worldPositionStays: false);
			}
		}
		Vector3 localPosition = currentAsset.transform.localPosition;
		Quaternion localRotation = currentAsset.transform.localRotation;
		Vector3 localScale = currentAsset.transform.localScale;
		currentAsset.transform.parent = base.transform;
		currentAsset.transform.localPosition = localPosition;
		currentAsset.transform.localRotation = localRotation;
		currentAsset.transform.localScale = localScale;
		assetTransform = currentAsset.transform;
		currentAsset.SetActive(value: true);
		hideOrb();
		if (disableColliders)
		{
			Collider[] componentsInChildren = currentAsset.GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren)
			{
				if (collider.gameObject.layer == Layers.CLICKIES)
				{
					collider.enabled = false;
				}
			}
		}
		EntityTrigger componentInChildren = GetComponentInChildren<EntityTrigger>();
		if (componentInChildren != null)
		{
			componentInChildren.RetrieveEntity();
		}
		OnAssetUpdated(currentAsset);
	}

	public override IEnumerator LoadWeaponAsset()
	{
		yield return null;
	}

	public override IEnumerator LoadToolAsset(EquipItemSlot slot)
	{
		yield return null;
	}
}
