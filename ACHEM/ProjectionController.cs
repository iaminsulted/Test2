using System.Collections.Generic;
using Assets.Scripts.Game;
using UnityEngine;

public class ProjectionController : MonoBehaviour
{
	public enum TelegraphType
	{
		SpellCharge,
		SpellHover,
		SpellCast
	}

	private static readonly SuperObjectPool<ProjectionController> projectorPool;

	private const float MaxTelegraphDistance = 60f;

	private static Transform projectorsPoolTransform;

	private static bool isInitialized;

	private Entity caster;

	private Entity aoeSource;

	private int aoeLocationId;

	private bool isAura;

	private bool isChargeFinished;

	private float chargeTime;

	private float startTime;

	private TelegraphType type;

	private Projector Projector;

	private Transform SourceTransform;

	private Transform CasterTransform;

	private Vector3 OffsetPosition;

	private Vector3 OffsetRotation;

	private bool isFixedPosition;

	private static Transform getProjectorsPoolTransform
	{
		get
		{
			if (projectorsPoolTransform == null)
			{
				projectorsPoolTransform = new GameObject("ProjectorPool").transform;
				Object.DontDestroyOnLoad(projectorsPoolTransform.gameObject);
			}
			return projectorsPoolTransform;
		}
	}

	static ProjectionController()
	{
		projectorPool = new SuperObjectPool<ProjectionController>(0, 100, 20).SetGenerate(delegate
		{
			ProjectionController projectionController = new GameObject("pivot").AddComponent<ProjectionController>();
			projectionController.transform.SetParent(getProjectorsPoolTransform);
			Projector projector2 = new GameObject("projector").AddComponent<Projector>();
			projector2.transform.SetParent(projectionController.transform);
			projector2.nearClipPlane = 0.05f;
			projector2.farClipPlane = 10f;
			projector2.aspectRatio = 1f;
			projector2.orthographic = true;
			projector2.orthographicSize = 1f;
			projector2.ignoreLayers = ~((1 << Layers.DEFAULT) | (1 << Layers.WATER) | (1 << Layers.TERRAIN) | (1 << Layers.CLOSE) | (1 << Layers.MIDDLE) | (1 << Layers.MIDDLEFAR) | (1 << Layers.FAR) | (1 << Layers.SNOW) | (1 << Layers.BG));
			projectionController.Projector = projector2;
			return projectionController;
		}).SetDispose(delegate(ProjectionController projector)
		{
			if (projector != null)
			{
				Object.Destroy(projector.gameObject);
			}
		}).SetOnBorrow(delegate(ProjectionController projector)
		{
			projector.gameObject.SetActive(value: true);
			projector.Projector.aspectRatio = 1f;
			projector.Projector.transform.localPosition = Vector3.zero;
			projector.Projector.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
		})
			.SetOnReturn(delegate(ProjectionController projector)
			{
				if (projector != null)
				{
					projector.gameObject.SetActive(value: false);
				}
			});
	}

	public static void Init()
	{
		UIGame.Instance.ActionBar.ActionHovered += ActionBar_ActionHovered1;
		isInitialized = true;
	}

	public static void Close()
	{
		if (isInitialized)
		{
			UIGame.Instance.ActionBar.ActionHovered -= ActionBar_ActionHovered1;
		}
		isInitialized = false;
	}

	private static void ActionBar_ActionHovered1(InputAction action, bool show)
	{
		if (show)
		{
			SpellTemplate mySpellTemplate = Game.Instance.combat.GetMySpellTemplate(action);
			ShowSpellProjections(Entities.Instance.me, mySpellTemplate, TelegraphType.SpellHover, -1f);
		}
	}

	public static void ShowSpellProjections(Entity caster, SpellTemplate spellT, TelegraphType type, float chargeTime)
	{
		if (spellT == null || !Game.Instance.IsUIVisible || caster.wrapper == null || Entities.Instance.me.wrapper == null)
		{
			return;
		}
		foreach (SpellAction castableAction in spellT.GetCastableActions(caster))
		{
			ShowActionProjections(caster, spellT, type, castableAction, chargeTime);
		}
	}

	private static void ShowActionProjections(Entity caster, SpellTemplate spellT, TelegraphType type, SpellAction spellAction, float chargeTime)
	{
		if (!ShouldShowProjection(caster, spellAction) || (type == TelegraphType.SpellCast && spellAction.aura == null))
		{
			return;
		}
		List<AoeLocation> list = new List<AoeLocation>();
		if ((type == TelegraphType.SpellCharge || type == TelegraphType.SpellCast) && caster.spellCastData != null)
		{
			list = caster.spellCastData.aoeLocations;
		}
		else if (type == TelegraphType.SpellHover)
		{
			list = Game.Instance.combat.GetActionAoeLocations(caster, spellT, spellAction);
		}
		foreach (AoeLocation item in list)
		{
			ShowProjectionByLocation(caster, spellT, spellAction, type, item, chargeTime);
		}
	}

	private static void ShowProjectionByLocation(Entity caster, SpellTemplate spellT, SpellAction spellAction, TelegraphType type, AoeLocation aoeLocation, float chargeTime)
	{
		if (spellAction.ID != aoeLocation.actionId || (type == TelegraphType.SpellCast && !aoeLocation.isAura))
		{
			return;
		}
		Entity entity = Entities.Instance.GetEntity(aoeLocation.aoeSourceType, aoeLocation.aoeSourceId);
		if (AoeInVisibilityRange(entity.position))
		{
			AoeShape aoeById = spellAction.GetAoeById(aoeLocation.aoeId, aoeLocation.isAura);
			if (aoeById.isFixed && type != TelegraphType.SpellHover)
			{
				ShowFixedProjection(caster, spellT, spellAction, entity, aoeById, type, aoeLocation.ID, aoeLocation.position + aoeLocation.randomOffset, aoeLocation.rotation, chargeTime);
			}
			else
			{
				ShowFollowProjection(caster, spellT, spellAction, entity, aoeById, type, aoeLocation.ID, chargeTime);
			}
		}
	}

	private static void ShowFixedProjection(Entity caster, SpellTemplate spellT, SpellAction spellAction, Entity aoeSource, AoeShape aoeShape, TelegraphType type, int aoeLocationId, Vector3 position, Quaternion rotation, float chargeTime)
	{
		BaseTelegraph telegraph = aoeShape.GetTelegraph(caster, aoeSource);
		if (telegraph != null)
		{
			Color color = InterfaceColors.Telegraphs.GetColor(spellT.IsAoeHarmful(spellAction), caster.CanAttack(Entities.Instance.me), caster.isMe);
			projectorPool.Borrow().InitFixedProjection(telegraph, caster, aoeSource, color, type, position, rotation, aoeShape.height, aoeLocationId, aoeShape.isAura, chargeTime);
		}
	}

	private static void ShowFollowProjection(Entity caster, SpellTemplate spellT, SpellAction spellAction, Entity aoeSource, AoeShape aoeShape, TelegraphType type, int aoeLocationId, float chargeTime)
	{
		BaseTelegraph telegraph = aoeShape.GetTelegraph(caster, aoeSource);
		if (telegraph != null)
		{
			Color color = InterfaceColors.Telegraphs.GetColor(spellT.IsAoeHarmful(spellAction), caster.CanAttack(Entities.Instance.me), caster.isMe);
			projectorPool.Borrow().InitFollowProjection(telegraph, caster, aoeSource, color, type, aoeShape.height, aoeLocationId, aoeShape.isAura, chargeTime);
		}
	}

	private static bool ShouldShowProjection(Entity caster, SpellAction spellAction)
	{
		if ((!spellAction.isAoe && !spellAction.makesAura) || spellAction.hideTelegraphs)
		{
			return false;
		}
		if (spellAction.isHarmful && !caster.CanAttack(Entities.Instance.me) && !caster.isMe)
		{
			return false;
		}
		return true;
	}

	private static bool AoeInVisibilityRange(Vector3 aoePosition)
	{
		Entity me = Entities.Instance.me;
		if (me == null || me.wrapperTransform == null)
		{
			return false;
		}
		if ((aoePosition - me.wrapperTransform.position).magnitude > 60f)
		{
			return false;
		}
		return true;
	}

	private void InitFixedProjection(BaseTelegraph telegraph, Entity caster, Entity aoeSource, Color color, TelegraphType type, Vector3 position, Quaternion rotation, float height, int aoeLocationId, bool isAura, float chargeTime)
	{
		InitProjection(telegraph, caster, aoeSource, color, type, aoeLocationId, isAura, chargeTime);
		position.y += height / 2f;
		Projector.farClipPlane = height;
		isFixedPosition = false;
		base.transform.position = position;
		base.transform.rotation = rotation;
	}

	private void InitFollowProjection(BaseTelegraph telegraph, Entity caster, Entity aoeSource, Color color, TelegraphType type, float height, int aoeLocationId, bool isAura, float chargeTime)
	{
		InitProjection(telegraph, caster, aoeSource, color, type, aoeLocationId, isAura, chargeTime);
		isFixedPosition = true;
		CasterTransform = caster.wrapper.transform;
		SourceTransform = aoeSource.wrapper.transform;
		OffsetPosition = new Vector3(telegraph.OffsetPosition.x, height / 2f, telegraph.OffsetPosition.z);
		OffsetRotation = new Vector3(0f, telegraph.OffsetRotation, 0f);
		Projector.farClipPlane = height;
		if (aoeSource != caster)
		{
			aoeSource.Destroyed += OnAoeSourceDestroyed;
		}
	}

	private void InitProjection(BaseTelegraph telegraph, Entity caster, Entity aoeSource, Color color, TelegraphType type, int aoeLocationId, bool isAura, float chargeTime)
	{
		this.aoeLocationId = aoeLocationId;
		this.caster = caster;
		this.aoeSource = aoeSource;
		this.type = type;
		this.isAura = isAura;
		isChargeFinished = false;
		this.chargeTime = chargeTime;
		startTime = GameTime.realtimeSinceServerStartup;
		telegraph.Draw(Projector, color);
		float value = ((chargeTime == -1f) ? 0f : 1f);
		Projector.material.SetFloat("_FillTime", value);
		caster.Destroyed += OnAoeSourceDestroyed;
		switch (type)
		{
		case TelegraphType.SpellHover:
			UIGame.Instance.ActionBar.ActionHovered += ActionBar_ActionHovered;
			break;
		case TelegraphType.SpellCharge:
			caster.ChargeEnded += OnChargeEnded;
			caster.ChannelEnded += OnChannelEnded;
			caster.CastCanceled += OnCastCanceled;
			break;
		}
		if (isAura)
		{
			caster.AoeDestroyed += OnAoeDestroyed;
		}
	}

	private void OnChargeEnded()
	{
		isChargeFinished = true;
		if (!isAura)
		{
			Dispose();
		}
	}

	private void OnChannelEnded()
	{
		if (!isAura)
		{
			Dispose();
		}
	}

	private void OnCastCanceled()
	{
		if (!isAura || !isChargeFinished)
		{
			Dispose();
		}
	}

	private void OnAoeSourceDestroyed()
	{
		Dispose();
	}

	private void ActionBar_ActionHovered(InputAction _, bool show)
	{
		if (!show)
		{
			Dispose();
		}
	}

	private void OnAoeDestroyed(int aoeLocationId)
	{
		if (this.aoeLocationId == aoeLocationId)
		{
			Dispose();
		}
	}

	private void Dispose()
	{
		caster.Destroyed -= OnAoeSourceDestroyed;
		if (aoeSource != caster)
		{
			aoeSource.Destroyed -= OnAoeSourceDestroyed;
		}
		if (type == TelegraphType.SpellHover)
		{
			UIGame.Instance.ActionBar.ActionHovered -= ActionBar_ActionHovered;
		}
		else if (type == TelegraphType.SpellCharge)
		{
			caster.ChargeEnded -= OnChargeEnded;
			caster.ChannelEnded -= OnChannelEnded;
			caster.CastCanceled -= OnCastCanceled;
		}
		if (isAura)
		{
			caster.AoeDestroyed -= OnAoeDestroyed;
		}
		SourceTransform = null;
		CasterTransform = null;
		projectorPool.Return(this);
	}

	private void Update()
	{
		if (!(chargeTime <= 0f))
		{
			float num = Mathf.Clamp01((GameTime.realtimeSinceServerStartup - startTime) / chargeTime);
			Projector.material.SetFloat("_FillTime", 1f - num);
		}
	}

	private void LateUpdate()
	{
		if (CasterTransform == null || (type == TelegraphType.SpellCharge && !isFixedPosition))
		{
			return;
		}
		Quaternion quaternion = CasterTransform.rotation;
		if (CasterTransform != SourceTransform)
		{
			Vector3 forward = SourceTransform.position - CasterTransform.position;
			if (forward.x != 0f || forward.z != 0f)
			{
				forward.y = 0f;
				quaternion = Quaternion.LookRotation(forward);
			}
		}
		base.transform.position = SourceTransform.position + quaternion * OffsetPosition;
		base.transform.rotation = quaternion * Quaternion.Euler(OffsetRotation);
	}
}
