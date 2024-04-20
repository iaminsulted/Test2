using System;
using System.Collections;
using System.Collections.Generic;
using StatCurves;
using UnityEngine;

public abstract class AssetController : MonoBehaviour
{
	protected EntityAsset entityAsset;

	protected Entity entity;

	protected float entityScaleFactor = 1f;

	protected GameObject orbGO;

	protected GameObject defaultPrefab;

	public GameObject currentAsset;

	protected Transform assetTransform;

	protected string defaultPrefabName;

	protected BundleInfo defaultPrefabBundle;

	protected Vector3 weaponTipPosition = Vector3.zero;

	protected Vector3 weaponTipLastPosition = Vector3.zero;

	private Dictionary<Renderer, Material> matMap = new Dictionary<Renderer, Material>();

	private EffectTemplate.ShaderFxType currentShaderFxType;

	private Hashtable currentShaderFx;

	private Dictionary<string, GameObject> particleMap = new Dictionary<string, GameObject>();

	private List<string> currentParticleFxCollection = new List<string>();

	private bool isRandomized;

	public bool showorb = true;

	public string Error;

	public bool IsAssetLoadComplete;

	protected Dictionary<EquipItemSlot, EquipItem> equips = new Dictionary<EquipItemSlot, EquipItem>();

	protected Dictionary<string, AssetBundleLoader> loaders = new Dictionary<string, AssetBundleLoader>();

	private static BoneAdjustmentPresets boneRandomizerPreset;

	public EquipItemSlot ActiveSlot { get; protected set; } = EquipItemSlot.Weapon;


	public bool IsOrb => orbGO != null;

	private static BoneAdjustmentPresets BoneRandomizerPreset
	{
		get
		{
			if (boneRandomizerPreset == null)
			{
				boneRandomizerPreset = (BoneAdjustmentPresets)Resources.Load("RandomBones");
			}
			return boneRandomizerPreset;
		}
		set
		{
			boneRandomizerPreset = value;
		}
	}

	public event Action<GameObject> AssetUpdated;

	public event Action<GameObject> OrbCreated;

	protected AssetBundleLoader LoadAssetBundle(BundleInfo bundleinfo)
	{
		if (loaders.ContainsKey(bundleinfo.FileName))
		{
			return loaders[bundleinfo.FileName];
		}
		return loaders[bundleinfo.FileName] = AssetBundleManager.LoadAssetBundle(bundleinfo);
	}

	protected AssetBundleRequest LoadAssetFromBundle(EquipItem eItem)
	{
		bool flag = eItem.GenderBased == ItemGenderBasedType.Both || eItem.GenderBased == ItemGenderBasedType.Asset;
		return loaders[eItem.bundle.FileName].Asset.LoadAssetAsync<GameObject>((flag ? (entityAsset.gender + "_") : "") + eItem.AssetName);
	}

	public GameObject LoadCastSpotAsset()
	{
		if (!IsAssetLoadComplete)
		{
			return null;
		}
		if (equips.TryGetValue(EquipItemSlot.Weapon, out var value) && loaders.ContainsKey(value.bundle.FileName))
		{
			return loaders[value.bundle.FileName].Asset.LoadAsset<GameObject>(value.AssetName + "_Cast");
		}
		return null;
	}

	public Vector3 GetWeaponTipLocation()
	{
		return weaponTipPosition;
	}

	public Vector3 GetWeaponTipLastLocation()
	{
		return weaponTipLastPosition;
	}

	public void SetEntityScaleFactor(float entityScaleFactor)
	{
		this.entityScaleFactor = entityScaleFactor;
		if (!base.transform.localScale.y.ApproximatelyEquals(entityScaleFactor))
		{
			iTween.ScaleTo(base.gameObject, iTween.Hash("scale", Vector3.one * entityScaleFactor, "time", 3f, "easetype", iTween.EaseType.easeOutExpo));
		}
	}

	protected static void SetItemColors(GameObject go, string r, string g, string b, Transform probeAnchor, string skinB = "", string skinA = "")
	{
		Renderer[] componentsInChildren = go.GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!string.IsNullOrEmpty(r) && !string.IsNullOrEmpty(g))
			{
				Material material = componentsInChildren[i].material;
				material.SetColor("_RColor", r.ToColor32());
				material.SetColor("_GColor", g.ToColor32());
				if (!string.IsNullOrEmpty(b))
				{
					material.SetColor("_BColor", b.ToColor32());
				}
			}
			if (!string.IsNullOrEmpty(skinA) && !string.IsNullOrEmpty(skinB))
			{
				Material material2 = componentsInChildren[i].material;
				material2.SetColor("_R1Color", skinA.ToColor32());
				material2.SetColor("_R2Color", skinB.ToColor32());
			}
			if (probeAnchor != null)
			{
				componentsInChildren[i].probeAnchor = probeAnchor;
			}
		}
	}

	public void Init(EntityAsset entityAsset, Entity entity = null)
	{
		this.entityAsset = entityAsset;
		this.entity = entity;
	}

	public void Load()
	{
		Clear();
		StartCoroutine(LoadAsset());
	}

	public void LoadPet()
	{
		Clear();
		StartCoroutine(LoadAsset(disableColliders: true));
	}

	public void LoadWeapon()
	{
		StartCoroutine(LoadWeaponAsset());
	}

	public void LoadTool(EquipItemSlot slot)
	{
		StartCoroutine(LoadToolAsset(slot));
	}

	protected abstract void Clear();

	public void ClearBundle(EquipItemSlot slot)
	{
		foreach (string key in loaders.Keys)
		{
			if (equips.ContainsKey(slot) && equips[slot] != null && equips[slot].bundle.FileName == key)
			{
				loaders[key].Dispose();
				loaders.Remove(key);
				break;
			}
		}
	}

	public abstract IEnumerator LoadAsset(bool disableColliders = false);

	public abstract IEnumerator LoadWeaponAsset();

	public abstract IEnumerator LoadToolAsset(EquipItemSlot slot);

	protected virtual void OnAssetUpdated(GameObject go)
	{
		InitMaterialMap();
		ApplyShaderFx();
		ApplyParticleFx();
		go.SwitchLayerRecursively(Layers.OTHER_PLAYERS, base.gameObject.layer);
		go.transform.localScale = Vector3.one * entityAsset.ScaleFactor;
		IsAssetLoadComplete = true;
		SetRandomizedBones(isRandomized);
		this.AssetUpdated?.Invoke(go);
	}

	protected void OnAssetLoadFailed(string error)
	{
		Debug.LogWarning(error);
		Error = error;
		IsAssetLoadComplete = true;
	}

	protected void showOrb()
	{
		if (showorb && orbGO == null)
		{
			orbGO = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("OrbBlueFlame"), base.transform, worldPositionStays: false);
			orbGO.name = "orbGO";
			orbGO.SwitchLayerRecursively(Layers.OTHER_PLAYERS, base.gameObject.layer);
			if (this.OrbCreated != null)
			{
				this.OrbCreated(orbGO);
			}
		}
	}

	protected void hideOrb()
	{
		if (orbGO != null)
		{
			UnityEngine.Object.Destroy(orbGO);
			orbGO = null;
		}
	}

	protected virtual void Destroy()
	{
		foreach (GameObject value in particleMap.Values)
		{
			UnityEngine.Object.Destroy(value);
		}
		if (orbGO != null)
		{
			UnityEngine.Object.Destroy(orbGO);
		}
		if (currentAsset != null)
		{
			UnityEngine.Object.Destroy(currentAsset);
		}
		matMap.Clear();
	}

	public void OnDestroy()
	{
		Destroy();
		Clear();
	}

	private void InitMaterialMap()
	{
		matMap = new Dictionary<Renderer, Material>();
		Renderer[] componentsInChildren = GetComponentsInChildren<Renderer>();
		foreach (Renderer renderer in componentsInChildren)
		{
			if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
			{
				matMap[renderer] = renderer.sharedMaterial;
			}
		}
	}

	public void ApplyShaderFxAndParticles(Hashtable shaderFx, List<string> particleFxCollection)
	{
		EffectTemplate.ShaderFxType shaderFxType = EffectTemplate.ShaderFxType.None;
		if (shaderFx != null && shaderFx.ContainsKey("Type"))
		{
			shaderFxType = (EffectTemplate.ShaderFxType)Enum.Parse(typeof(EffectTemplate.ShaderFxType), (string)shaderFx["Type"]);
		}
		if (currentShaderFxType != shaderFxType)
		{
			currentShaderFxType = shaderFxType;
			currentShaderFx = shaderFx;
			ApplyShaderFx();
		}
		currentParticleFxCollection = particleFxCollection;
		ApplyParticleFx();
	}

	private void ApplyParticleFx()
	{
		foreach (string item in new List<string>(particleMap.Keys))
		{
			if (!currentParticleFxCollection.Contains(item))
			{
				UnityEngine.Object.Destroy(particleMap[item]);
				particleMap.Remove(item);
			}
		}
		foreach (string item2 in currentParticleFxCollection)
		{
			if (!string.IsNullOrEmpty(item2) && !particleMap.ContainsKey(item2))
			{
				GameObject gameObject = SpellFXContainer.mInstance.LoadAsset(item2);
				if (gameObject != null)
				{
					GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, base.transform.position, base.transform.rotation);
					gameObject2.SetLayerRecursively(Layers.OTHER_PLAYERS);
					gameObject2.transform.parent = base.transform;
					particleMap.Add(item2, gameObject2);
				}
			}
		}
	}

	private void ApplyShaderFx()
	{
		foreach (Renderer key in matMap.Keys)
		{
			if (key == null || !matMap.TryGetValue(key, out var value) || value == null || value.renderQueue >= 3000)
			{
				continue;
			}
			try
			{
				if (currentShaderFxType == EffectTemplate.ShaderFxType.None)
				{
					key.sharedMaterial = value;
				}
				else if (currentShaderFxType == EffectTemplate.ShaderFxType.Ghost)
				{
					Material material;
					if (value.HasProperty("_CMTex"))
					{
						material = new Material(Shader.Find("AE/AQ3D/Character (CM Ghost)"));
					}
					else if (value.shader.name == "AE/AQ3D/Character (Spec) Face")
					{
						material = new Material(Shader.Find("AE/AQ3D/Character (Player Ghost)"));
						material.SetColor("_EyeColor", value.GetColor("_EyeColor"));
						material.SetColor("_LipColor", value.GetColor("_LipColor"));
						material.SetTexture("_FaceTex", value.GetTexture("_FaceTex"));
						material.SetTexture("_MaskTex", value.GetTexture("_MaskTex"));
					}
					else
					{
						material = new Material(Shader.Find("AE/AQ3D/Character (Base Ghost)"));
					}
					if (value.HasProperty("_MainTex"))
					{
						material.SetTexture("_MainTex", value.GetTexture("_MainTex"));
					}
					if (currentShaderFx.ContainsKey("_Col"))
					{
						material.SetColor("_Col", ((string)currentShaderFx["_Col"]).ToColor32());
					}
					if (currentShaderFx.ContainsKey("_ACol"))
					{
						material.SetColor("_ACol", ((string)currentShaderFx["_ACol"]).ToColor32());
					}
					if (currentShaderFx.ContainsKey("_BlackCol"))
					{
						material.SetColor("_BlackCol", ((string)currentShaderFx["_BlackCol"]).ToColor32());
					}
					if (currentShaderFx.ContainsKey("_Max"))
					{
						material.SetFloat("_Max", (float)(double)currentShaderFx["_Max"]);
					}
					if (currentShaderFx.ContainsKey("_Alpha"))
					{
						material.SetFloat("_Alpha", (float)(double)currentShaderFx["_Alpha"]);
					}
					key.sharedMaterial = material;
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	public void SetRandomizedBones(bool isRandomized)
	{
		if (!this.isRandomized && !isRandomized)
		{
			return;
		}
		this.isRandomized = isRandomized;
		if (IsAssetLoadComplete)
		{
			if (isRandomized)
			{
				StartCoroutine(RandomizeBones());
			}
			else
			{
				ResetRandomBones();
			}
		}
	}

	protected IEnumerator RandomizeBones()
	{
		if (!currentAsset.GetComponent<Animator>() || !currentAsset.GetComponent<BoneCustomizer>())
		{
			yield break;
		}
		BoneCustomizer customizer = currentAsset.GetComponent<BoneCustomizer>();
		if (!customizer.boneAdjustmentPresets.Contains(BoneRandomizerPreset))
		{
			customizer.boneAdjustmentPresets.Add(BoneRandomizerPreset);
		}
		customizer.update = true;
		float[] array = new float[6] { 0f, 0.1f, 0.2f, 0.8f, 0.9f, 1f };
		foreach (BoneAdjust boneAdjustment in BoneRandomizerPreset.boneAdjustments)
		{
			customizer.ValueUpdate(array[UnityEngine.Random.Range(0, array.Length)], boneAdjustment);
		}
		customizer.apply = true;
		while (customizer.apply)
		{
			yield return null;
		}
	}

	protected void ResetRandomBones()
	{
		if ((bool)currentAsset.GetComponent<Animator>() && (bool)currentAsset.GetComponent<BoneCustomizer>())
		{
			currentAsset.GetComponent<BoneCustomizer>().RestoreDefaults();
		}
	}
}
