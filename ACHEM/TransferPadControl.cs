using System.Collections.Generic;
using UnityEngine;

public class TransferPadControl : MonoBehaviour, ITrackable
{
	public delegate void onEvent(object sender);

	[ReadOnly]
	public string UniqueID;

	public int AreaID;

	public int CellID;

	public int SpawnID;

	public bool KillAllMonsters;

	public List<int> Areas = new List<int>();

	public GameObject waypointPrefab;

	public GameObject waypointGameObject;

	public ParticleSystem waypointParticles;

	private Transform waypoint;

	private GameObject secondaryRenderer;

	public Transform TrackedTransform => base.transform;

	public event onEvent onEnter;

	private void Awake()
	{
		waypoint = base.transform.parent.Find("Waypoint");
		if (waypoint == null)
		{
			waypoint = base.transform.Find("Waypoint");
		}
		if (waypoint == null)
		{
			waypoint = base.transform.parent.Find("Marker");
		}
		if (waypoint == null)
		{
			waypoint = base.transform.Find("Marker");
		}
		waypointPrefab = Resources.Load("Particles/IndicatorBeam_HD") as GameObject;
		if (!Areas.Contains(AreaID))
		{
			Areas.Add(AreaID);
		}
	}

	public void setKillAllMonsters(bool KillAllMonsters)
	{
		this.KillAllMonsters = KillAllMonsters;
		if (KillAllMonsters)
		{
			secondaryRenderer = Object.Instantiate(base.gameObject, base.transform.parent);
			Object.Destroy(secondaryRenderer.GetComponent<BoxCollider>());
			Object.Destroy(secondaryRenderer.GetComponent<TransferPadControl>());
			secondaryRenderer.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 0f, 0f, 0.25f));
			NPC.AnyNPCStateUpdated += CheckForAllMonstersKilledEventHandler;
			base.gameObject.SetActive(value: false);
		}
	}

	private void CheckForAllMonstersKilledEventHandler()
	{
		if (CheckForAllMonstersKilled())
		{
			base.gameObject.SetActive(value: true);
			if (secondaryRenderer != null)
			{
				Object.Destroy(secondaryRenderer);
			}
		}
	}

	private bool CheckForAllMonstersKilled()
	{
		foreach (NPC npc in Entities.Instance.NpcList)
		{
			if ((npc.react == Entity.Reaction.Hostile || npc.react == Entity.Reaction.Neutral) && npc.serverState != Entity.State.Dead)
			{
				return false;
			}
		}
		return true;
	}

	private void OnDestroy()
	{
		NPC.AnyNPCStateUpdated -= CheckForAllMonstersKilledEventHandler;
	}

	public void Track(IndicatorFX indicatorFX)
	{
		UpdateState(active: true);
	}

	public void Untrack()
	{
		UpdateState(active: false);
	}

	private void UpdateState(bool active)
	{
		if (active)
		{
			if (waypoint != null && waypointGameObject == null)
			{
				waypointGameObject = Object.Instantiate(waypointPrefab, waypoint.position, Quaternion.identity);
				waypointParticles = waypointGameObject.GetComponentInChildren<ParticleSystem>();
				waypointGameObject.transform.SetParent(base.transform);
				waypointParticles.Play();
			}
		}
		else if (waypointParticles != null)
		{
			waypointParticles.Clear();
			waypointParticles.Stop();
			Object.Destroy(waypointGameObject);
		}
	}

	public void OnTriggerEnter(Collider c)
	{
		if (!Game.Instance.TesterMode && (!KillAllMonsters || CheckForAllMonstersKilled()) && AreaID > 0 && c.gameObject.layer == Layers.PLAYER_ME)
		{
			if (this.onEnter != null)
			{
				this.onEnter(this);
			}
			Game.Instance.SendIATransferPadRequest(UniqueID, AreaID, CellID, SpawnID);
		}
	}
}
