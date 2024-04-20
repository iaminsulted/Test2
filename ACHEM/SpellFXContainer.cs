using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using UnityEngine;

public class SpellFXContainer : MonoBehaviour
{
	public static SpellFXContainer mInstance;

	private bool isReady;

	private AssetBundleLoader abl;

	private ParticleSystem animeSmack;

	private Camera mainCamera;

	private int SpellFxLimit
	{
		get
		{
			int num = SettingsManager.ParticleLimit;
			if (num != 5)
			{
				return (num + 1) * 5;
			}
			return int.MaxValue;
		}
	}

	private int SpellFxAALimit => Mathf.RoundToInt((float)SpellFxLimit * 0.6f);

	private void Awake()
	{
		mInstance = this;
		mainCamera = Game.Instance?.cam;
		StartCoroutine(Init());
	}

	private IEnumerator Init()
	{
		abl = AssetBundleManager.LoadAssetBundle(Session.Account.Characters_Bundle);
		yield return abl;
		LoadStandardImpact();
		isReady = true;
	}

	public GameObject LoadAsset(string assetName)
	{
		GameObject gameObject = null;
		if ((int)SettingsManager.ParticleQuality == 1 && abl.Asset.Contains(assetName + "_HD"))
		{
			gameObject = abl.Asset.LoadAsset<GameObject>(assetName + "_HD");
		}
		if (gameObject == null)
		{
			gameObject = abl.Asset.LoadAsset<GameObject>(assetName);
		}
		return gameObject;
	}

	public void LoadStandardImpact()
	{
		GameObject gameObject = LoadAsset("AnimeSmack");
		if (gameObject != null)
		{
			animeSmack = UnityEngine.Object.Instantiate(gameObject).GetComponent<ParticleSystem>();
			animeSmack.gameObject.SetLayerRecursively(Layers.OTHER_PLAYERS);
			animeSmack.transform.parent = base.transform;
		}
	}

	public void OnDestroy()
	{
		abl.Dispose();
		mInstance = null;
	}

	public void Reset()
	{
		foreach (Transform item in base.transform)
		{
			if (item != animeSmack.transform)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}

	public GameObject CreateCastSpotFX(Entity caster, SpellTemplate spellT, float chargeTime)
	{
		SpellAction firstCastableAction = spellT.GetFirstCastableAction(caster);
		if (!isReady || caster == null || caster.CastSpot == null || firstCastableAction == null || firstCastableAction.GetCastSpotFX() == "" || spellT.hideCastSpotFX)
		{
			return null;
		}
		GameObject gameObject = null;
		if (firstCastableAction.useWeaponCastPrefab)
		{
			gameObject = caster.assetController.LoadCastSpotAsset();
		}
		if (gameObject == null)
		{
			gameObject = LoadAsset(firstCastableAction.GetCastSpotFX());
		}
		if (gameObject == null)
		{
			return null;
		}
		Spellfx component = gameObject.GetComponent<Spellfx>();
		if (component == null)
		{
			return null;
		}
		GameObject result = UnityEngine.Object.Instantiate(gameObject, caster.CastSpot);
		result.SetLayerRecursively(Layers.OTHER_PLAYERS);
		component.LifeTime = chargeTime + 1f + 0.5f;
		return result;
	}

	public void CreateProjectiles(Entity caster, List<Entity> targets, KeyframeSpellData spellData, GameObject castSpotFX, float statDeltaPercent)
	{
		if (!isReady || caster.wrapper == null)
		{
			caster.HandleProjectileImpact(spellData, statDeltaPercent);
			return;
		}
		GameObject gameObject = LoadAsset(spellData.spellAction.projFX);
		if (!(gameObject == null))
		{
			if (targets.Count == 0)
			{
				CreateProjectileWithoutTarget(caster, spellData, gameObject, castSpotFX);
			}
			else
			{
				CreateProjectileWithTarget(caster, targets, spellData, gameObject, castSpotFX, statDeltaPercent);
			}
		}
	}

	private void CreateProjectileWithTarget(Entity caster, List<Entity> targets, KeyframeSpellData spellData, GameObject projectilePrefab, GameObject castSpotFX, float statDeltaPercent)
	{
		for (int i = 0; i < targets.Count; i++)
		{
			Entity entity = targets[i];
			if (entity == null)
			{
				continue;
			}
			if (entity.wrapper == null)
			{
				entity.HandleProjectileImpact(spellData, statDeltaPercent);
				continue;
			}
			if (!InCameraView(caster.wrapper.transform.position) && !InCameraView(entity.wrapper.transform.position))
			{
				entity.HandleProjectileImpact(spellData, statDeltaPercent);
				continue;
			}
			GameObject gameObject;
			if (castSpotFX != null && i == 0 && spellData.spellAction.ShouldReuseChargeProjectile)
			{
				gameObject = castSpotFX;
				gameObject.transform.position = caster.CastSpot.position;
			}
			else
			{
				Vector3 projectileStartPosition = spellData.spellAction.GetProjectileStartPosition(caster.CastSpot.position, entity.HitSpot.position);
				gameObject = UnityEngine.Object.Instantiate(projectilePrefab, projectileStartPosition, Quaternion.identity);
				gameObject.SetLayerRecursively(Layers.OTHER_PLAYERS);
			}
			AudioManager.PlayCombatSFX(spellData.spellAction.projSFX, caster.isMe || entity.isMe, gameObject.transform);
			gameObject.transform.parent = base.transform;
			Spellfx component = gameObject.GetComponent<Spellfx>();
			ProjectileFX componentInChildren = gameObject.GetComponentInChildren<ProjectileFX>();
			if (component == null)
			{
				Debug.LogError(new Exception("Spellfx not found on " + gameObject.name + " prefab"));
				break;
			}
			if (componentInChildren == null)
			{
				Debug.LogError(new Exception("ParticleFX not found on " + gameObject.name + " prefab"));
				break;
			}
			Transform attachPoint = ((spellData.spellAction.projType == CombatSolver.ProjectileType.Reverse) ? caster : entity).GetAttachPoint(component.spot);
			componentInChildren.InitWithTarget(entity, attachPoint, spellData, component, statDeltaPercent);
		}
	}

	private void CreateProjectileWithoutTarget(Entity caster, KeyframeSpellData spellData, GameObject projectilePrefab, GameObject castSpotFX)
	{
		if (caster == null || spellData == null)
		{
			return;
		}
		Vector3 vector = caster.wrapper.transform.rotation * Vector3.forward;
		vector.y = 0f;
		float num = spellData.spellAction.range;
		float stayTime = 0f;
		if (Physics.Raycast(new Ray(caster.CastSpot.position, vector), out var hitInfo, spellData.spellAction.range, Layers.MASK_GROUNDTRACK))
		{
			num = hitInfo.distance;
			stayTime = spellData.spellAction.projStayTime;
		}
		Vector3 vector2 = ((castSpotFX != null) ? castSpotFX.transform.position : caster.CastSpot.position);
		Vector3 vector3 = caster.CastSpot.position + vector * num;
		caster.HandleProjectileImpact(spellData, 0f);
		if (InCameraView(vector2) || InCameraView(vector3))
		{
			GameObject gameObject;
			if (castSpotFX != null && spellData.spellAction.ShouldReuseChargeProjectile)
			{
				gameObject = castSpotFX;
				gameObject.transform.position = vector2;
			}
			else
			{
				vector2 = spellData.spellAction.GetProjectileStartPosition(vector2, vector3);
				gameObject = UnityEngine.Object.Instantiate(projectilePrefab, vector2, Quaternion.identity);
				gameObject.SetLayerRecursively(Layers.OTHER_PLAYERS);
			}
			AudioManager.PlayCombatSFX(spellData.spellAction.projSFX, caster.isMe, gameObject.transform);
			gameObject.transform.parent = base.transform;
			Spellfx component = gameObject.GetComponent<Spellfx>();
			ProjectileFX projectileFX = gameObject.GetComponent<ProjectileFX>();
			if (projectileFX == null)
			{
				projectileFX = gameObject.GetComponentInChildren<ProjectileFX>();
			}
			if (component == null)
			{
				Debug.LogError(new Exception("Spellfx not found on " + gameObject.name + " prefab"));
			}
			else if (projectileFX == null)
			{
				Debug.LogError(new Exception("ParticleFX not found on " + gameObject.name + " prefab"));
			}
			else if (spellData.spellAction.projType == CombatSolver.ProjectileType.Reverse)
			{
				Transform attachPoint = caster.GetAttachPoint(component.spot);
				projectileFX.InitWithTarget(caster, attachPoint, spellData, component, 0f);
			}
			else
			{
				projectileFX.Init(vector3, spellData, component, stayTime);
			}
		}
	}

	private bool CanCreateSpellFx(Entity target, bool isCasterMe, SpellTemplate spellT = null)
	{
		if (!isReady || target == null || target.wrapper == null)
		{
			return false;
		}
		bool num = isCasterMe || target.isMe;
		int num2;
		int num3;
		if (spellT != null)
		{
			num2 = (spellT.isAA ? 1 : 0);
			if (num2 != 0)
			{
				num3 = ((Spellfx.Count < SpellFxAALimit) ? 1 : 0);
				goto IL_0046;
			}
		}
		else
		{
			num2 = 0;
		}
		num3 = 0;
		goto IL_0046;
		IL_0046:
		bool flag = (byte)num3 != 0;
		bool flag2 = num2 == 0 && Spellfx.Count < SpellFxLimit;
		return num || flag || flag2;
	}

	private bool InCameraView(Vector3 position)
	{
		if (mainCamera == null)
		{
			mainCamera = Game.Instance.cam;
		}
		float num = 40 + (int)SettingsManager.DrawDistance * 10;
		if ((position - mainCamera.transform.position).magnitude > num)
		{
			return false;
		}
		Vector3 vector = mainCamera.WorldToViewportPoint(position);
		if (vector.z > 0f && vector.x > 0f && vector.x < 1f && vector.y > 0f)
		{
			return vector.y < 1f;
		}
		return false;
	}

	public void HandleImpactFX(Entity target, string impactFx, bool isCasterMe, KeyframeSpellData spellData = null)
	{
		if (CanCreateSpellFx(target, isCasterMe, spellData?.spellT))
		{
			CreateEntityFX(target, isCasterMe, impactFx, spellData, spellData?.spellT, spellData?.spellAction);
		}
		else if (target != null && spellData != null && spellData.spellAction.usesFXImpacts)
		{
			target.HandleSpellImpact(spellData, Entity.ImpactSource.FX);
		}
	}

	private void ShowAnimeSmack(Vector3 position)
	{
		animeSmack.transform.position = position;
		animeSmack.Stop();
		animeSmack.Play();
	}

	public List<GameObject> CreateEntityFX(Entity entity, bool isCasterMe, string entityFX, KeyframeSpellData spellData = null, SpellTemplate spellT = null, SpellAction spellAction = null, bool hasTarget = true)
	{
		if (!CanCreateSpellFx(entity, isCasterMe, spellT))
		{
			return null;
		}
		if (!InCameraView(entity.wrapper.transform.position))
		{
			return null;
		}
		List<GameObject> list = new List<GameObject>();
		if (string.IsNullOrEmpty(entityFX))
		{
			return null;
		}
		if (entityFX == "AnimeSmack" && hasTarget)
		{
			ShowAnimeSmack(entity.GetAttachPoint(Spellfx.AttachSpot.Hit).position);
			return null;
		}
		GameObject gameObject = LoadAsset(entityFX);
		if (gameObject == null)
		{
			Debug.LogWarning("Asset could not be loaded: " + entityFX);
			return null;
		}
		Spellfx component = gameObject.GetComponent<Spellfx>();
		if (component == null)
		{
			return null;
		}
		Transform attachPoint = entity.GetAttachPoint(component.spot);
		Vector3 position = attachPoint.position;
		Quaternion rotation = (component.rotateWithSpot ? attachPoint.rotation : entity.wrapper.transform.rotation);
		if (!hasTarget)
		{
			if (component.spot != 0)
			{
				return null;
			}
			float num = ((spellAction != null && spellAction.range > 4f) ? 5 : 2);
			position += entity.wrapper.transform.rotation * Vector3.forward * num;
			rotation *= Quaternion.Euler(0f, 180f, 0f);
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, position, rotation);
		gameObject2.SetLayerRecursively(Layers.OTHER_PLAYERS);
		gameObject2.transform.parent = (component.isSticky ? entity.wrapper.transform : base.transform);
		gameObject2.GetComponent<Spellfx>().Init(entity, spellData, spellAction);
		list.Add(gameObject2);
		return list;
	}

	public GameObject CreateFXAtPosition(string fx, Vector3 worldPosition, Quaternion worldRotation, SpellAction spellAction)
	{
		if (string.IsNullOrEmpty(fx))
		{
			return null;
		}
		if (!InCameraView(worldPosition))
		{
			return null;
		}
		if (Spellfx.Count >= SpellFxLimit)
		{
			return null;
		}
		if (fx == "AnimeSmack")
		{
			ShowAnimeSmack(worldPosition);
			return null;
		}
		GameObject gameObject = LoadAsset(fx);
		if (gameObject == null)
		{
			return null;
		}
		GameObject obj = UnityEngine.Object.Instantiate(gameObject, worldPosition, worldRotation);
		obj.SetLayerRecursively(Layers.OTHER_PLAYERS);
		obj.transform.SetParent(base.transform, worldPositionStays: true);
		EffectEventTrigger component = obj.GetComponent<EffectEventTrigger>();
		if (component != null)
		{
			float duration = spellAction?.aura?.duration ?? 1f;
			component.Init(duration);
		}
		return obj;
	}
}
