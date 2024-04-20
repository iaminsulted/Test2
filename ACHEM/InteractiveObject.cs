using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class InteractiveObject : MonoBehaviour, ITrackable
{
	public bool HasError;

	public int ID = -1;

	public bool lockID;

	public bool IsSingleUse;

	public string Animation;

	public string SfxTrigger;

	public AudioSource TriggerAudioSource;

	public List<InteractionRequirement> Requirements = new List<InteractionRequirement>();

	public float Distance = 7f;

	public bool CombatBlocked;

	public byte State = 1;

	public int RespawnTime;

	public bool ChangeState;

	private Collider iaCollider;

	private MyPlayerData myPlayerData;

	private bool isActive;

	public List<int> Areas = new List<int>();

	private Transform waypoint;

	private static GameObject beamPrefab;

	private static GameObject sparklesPrefab;

	private GameObject waypointGameObject;

	private ParticleSystem[] waypointParticles;

	public bool IsActive
	{
		get
		{
			return isActive;
		}
		set
		{
			if (isActive != value)
			{
				isActive = value;
				OnActiveUpdated(isActive);
			}
			UpdateCollider();
		}
	}

	public Transform TrackedTransform => base.transform;

	public event Action<bool> ActiveUpdated;

	public event Action<byte> StateUpdated;

	public event Action<byte> Initialized;

	public virtual void Awake()
	{
		waypoint = base.transform.parent.FindChildLike("Waypoint");
		if (waypoint == null)
		{
			waypoint = base.transform.FindChildRecursiveLike("Waypoint");
		}
		if (waypoint == null)
		{
			waypoint = base.transform.parent.FindChildLike("Marker");
		}
		if (waypoint == null)
		{
			waypoint = base.transform.FindChildRecursiveLike("Marker");
		}
		if (beamPrefab == null)
		{
			beamPrefab = Resources.Load("Particles/IndicatorBeam_HD") as GameObject;
		}
		if (sparklesPrefab == null)
		{
			sparklesPrefab = Resources.Load("Particles/IndicatorSparkles") as GameObject;
		}
		myPlayerData = Session.MyPlayerData;
		RegisterRequirementListeners();
		iaCollider = GetComponent<Collider>();
	}

	public void RegisterRequirementListeners()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			requirement.Updated -= CheckRequirements;
		}
		foreach (InteractionRequirement requirement2 in Requirements)
		{
			requirement2.Updated += CheckRequirements;
		}
	}

	public virtual void OnDestroy()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			requirement.Updated -= CheckRequirements;
		}
	}

	protected virtual void OnActiveUpdated(bool active)
	{
		this.ActiveUpdated?.Invoke(active);
	}

	protected virtual void OnInitialize(byte state)
	{
		this.Initialized?.Invoke(state);
	}

	protected void OnStateUpdated(byte state)
	{
		this.StateUpdated?.Invoke(state);
	}

	public virtual void Init(byte state, int ownerId, List<int> areas)
	{
		State = state;
		IsActive = IsInteractive();
		OnInitialize(state);
	}

	public virtual void SetState(byte state, int entityId)
	{
		State = state;
		IsActive = IsInteractive();
		OnStateUpdated(state);
	}

	public virtual void Trigger(bool checkRequirements)
	{
	}

	protected virtual bool CanInteract()
	{
		if (Entities.Instance.me.visualState == Entity.State.Dead)
		{
			Notification.ShowText("Cannot interact while dead");
			return false;
		}
		if (CombatBlocked && Entities.Instance.me.serverState == Entity.State.InCombat)
		{
			Notification.ShowText("Cannot interact while in combat");
			return false;
		}
		return true;
	}

	protected virtual bool IsRequirementMet()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			if (!requirement.IsRequirementMet(myPlayerData))
			{
				return false;
			}
		}
		return true;
	}

	protected virtual bool IsInteractive()
	{
		if (IsRequirementMet())
		{
			return State == 1;
		}
		return false;
	}

	public void Track(IndicatorFX indicatorFX)
	{
		UpdateState(active: true, indicatorFX);
	}

	public void Untrack()
	{
		UpdateState(active: false);
	}

	private void CheckRequirements()
	{
		IsActive = IsInteractive();
		UpdateCollider();
	}

	private void UpdateCollider()
	{
		if (iaCollider != null)
		{
			iaCollider.enabled = IsActive;
		}
	}

	private void UpdateState(bool active, IndicatorFX indicatorFX = IndicatorFX.None)
	{
		if (active)
		{
			if (waypointGameObject == null)
			{
				Vector3 position = ((waypoint == null) ? base.transform.position : waypoint.position);
				switch (indicatorFX)
				{
				case IndicatorFX.Beam:
					waypointGameObject = UnityEngine.Object.Instantiate(beamPrefab, position, Quaternion.identity);
					break;
				case IndicatorFX.Sparkles:
					waypointGameObject = UnityEngine.Object.Instantiate(sparklesPrefab, position, Quaternion.identity);
					break;
				case IndicatorFX.BeamAndSparkles:
					waypointGameObject = new GameObject();
					waypointGameObject.transform.position = position;
					waypointGameObject.transform.rotation = Quaternion.identity;
					UnityEngine.Object.Instantiate(beamPrefab, position, Quaternion.identity, waypointGameObject.transform);
					UnityEngine.Object.Instantiate(sparklesPrefab, position, Quaternion.identity, waypointGameObject.transform);
					break;
				case IndicatorFX.None:
					return;
				}
				waypointParticles = waypointGameObject.GetComponentsInChildren<ParticleSystem>();
				waypointGameObject.transform.SetParent(base.transform);
				waypointGameObject.SetLayerRecursively(Layers.CLICKIES);
				ParticleSystem[] array = waypointParticles;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].Play();
				}
			}
		}
		else if (waypointGameObject != null)
		{
			UnityEngine.Object.Destroy(waypointGameObject);
		}
	}
}
