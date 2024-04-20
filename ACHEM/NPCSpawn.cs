using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCSpawn : NPCPathNode
{
	[Serializable]
	public struct AnimationOverride
	{
		public string animationName;

		public string overrideName;
	}

	private static Dictionary<int, NPCSpawn> inactiveMap = new Dictionary<int, NPCSpawn>();

	private static Dictionary<int, NPCSpawn> map = new Dictionary<int, NPCSpawn>();

	public int ID;

	public bool AutoSpawn = true;

	public bool RandomSpawn;

	public float RespawnTime = 30f;

	public float DespawnTime = 6f;

	public float AggroRadius = 6f;

	public float LeashRadius = 50f;

	public NPCPathingType MoveOverride;

	public List<Transform> pathList;

	public List<InteractionRequirement> Requirements = new List<InteractionRequirement>();

	public List<InteractiveObject> InteractiveObjects = new List<InteractiveObject>();

	public byte State;

	public Transform HeadSpot;

	public Transform CameraSpot;

	public static Dictionary<int, NPCSpawn> Map => map;

	public static Dictionary<int, NPCSpawn> InactiveMap => inactiveMap;

	public Transform TargetTransform => base.transform;

	public event Action<byte> StateUpdated;

	public event Action<bool> RequirementUpdated;

	public event Action BaitUpdated;

	public static NPCSpawn GetSpawn(int id)
	{
		if (map.ContainsKey(id))
		{
			return map[id];
		}
		return null;
	}

	public static NPCSpawn GetInactiveSpawn(int id)
	{
		if (inactiveMap.ContainsKey(id))
		{
			return inactiveMap[id];
		}
		return null;
	}

	public static void Init(NPCSpawn[] spawns)
	{
		map.Clear();
		inactiveMap.Clear();
		foreach (NPCSpawn nPCSpawn in spawns)
		{
			if (!map.ContainsKey(nPCSpawn.ID))
			{
				map.Add(nPCSpawn.ID, nPCSpawn);
			}
		}
	}

	public static void SwapToInactive(int id)
	{
		NPCSpawn spawn = GetSpawn(id);
		if (spawn != null)
		{
			spawn.TargetTransform.gameObject.SetActive(value: false);
			if (inactiveMap.ContainsKey(id))
			{
				UnityEngine.Object.Destroy(inactiveMap[id].TargetTransform.gameObject);
			}
			inactiveMap[id] = spawn;
			map.Remove(id);
		}
	}

	public static void SwapToActive(int id)
	{
		NPCSpawn inactiveSpawn = GetInactiveSpawn(id);
		if (inactiveSpawn != null)
		{
			inactiveSpawn.TargetTransform.gameObject.SetActive(value: true);
			if (map.ContainsKey(id))
			{
				UnityEngine.Object.Destroy(map[id].TargetTransform.gameObject);
			}
			map[id] = inactiveSpawn;
			inactiveMap.Remove(id);
		}
		else
		{
			if (map.ContainsKey(id))
			{
				UnityEngine.Object.Destroy(map[id].TargetTransform.gameObject);
			}
			map.Remove(id);
		}
	}

	private void Awake()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			requirement.Updated += OnRequirementUpdated;
		}
	}

	public void AddRequirement(InteractionRequirement requirement)
	{
		Requirements.Add(requirement);
		requirement.Updated += OnRequirementUpdated;
	}

	private void Start()
	{
		base.transform.gameObject.layer = 20;
		if (Entities.Instance.me.AccessLevel > 50)
		{
			TextMesh textMesh = new GameObject("ID Label").AddComponent<TextMesh>();
			textMesh.transform.SetParent(base.transform);
			ComSpawnMeta comSpawnMeta = Game.Instance.AreaData.spawnMetas.Where((ComSpawnMeta nm) => nm.SpawnID == ID).FirstOrDefault();
			textMesh.text = ((comSpawnMeta != null && comSpawnMeta.IsDB) ? "(DB) " : "") + ID;
			textMesh.transform.localPosition = new Vector3(0f, -0.5f, 0f);
			textMesh.characterSize = 0.1f;
			textMesh.fontStyle = FontStyle.Bold;
			textMesh.fontSize = 40;
			textMesh.anchor = TextAnchor.MiddleCenter;
			textMesh.alignment = TextAlignment.Center;
			textMesh.color = new Color(1f, 66f / 85f, 0f, 1f);
			textMesh.gameObject.AddComponent<simpleBillboard>().Flip = true;
			textMesh.gameObject.layer = 20;
			BoxCollider boxCollider = base.transform.gameObject.AddComponent<BoxCollider>();
			boxCollider.size = new Vector3(2f, 0.3f, 3f);
			boxCollider.center = new Vector3(0f, 0.1f, 0.5f);
			SpawnEditorSpawner spawnEditorSpawner = base.transform.gameObject.AddComponent<SpawnEditorSpawner>();
			spawnEditorSpawner.SpawnID = ID;
			ClickClientTrigger clickClientTrigger = base.transform.gameObject.AddComponent<ClickClientTrigger>();
			clickClientTrigger.Distance = 999f;
			clickClientTrigger.Requirements = new List<InteractionRequirement>();
			clickClientTrigger.CTActions.Add(spawnEditorSpawner);
		}
	}

	private void OnDestroy()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			requirement.Updated -= OnRequirementUpdated;
		}
	}

	public bool IsRequirementMet()
	{
		foreach (InteractionRequirement requirement in Requirements)
		{
			if (!requirement.IsRequirementMet(Session.MyPlayerData))
			{
				return false;
			}
		}
		return true;
	}

	public void SetState(byte value)
	{
		if (State != value)
		{
			State = value;
			this.StateUpdated?.Invoke(State);
		}
	}

	public void UpdateBaitState()
	{
		this.BaitUpdated?.Invoke();
	}

	private void OnRequirementUpdated()
	{
		this.RequirementUpdated?.Invoke(IsRequirementMet());
	}
}
