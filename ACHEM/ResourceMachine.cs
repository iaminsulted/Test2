using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using StatCurves;
using UnityEngine;

public class ResourceMachine : ClickMachine
{
	private const float DegreeOfFreedom = 45f;

	public bool randomizeRotationY;

	public TradeSkillType tradeSkillType;

	public ItemType itemType;

	public int tradeSkillLevel;

	public EquipItemSlot equipItemSlot;

	public int power;

	public List<InteractionRequirement> reqBitFlags;

	public List<InteractionRequirement> reqEquippedItems;

	public List<InteractionRequirement> reqItems;

	public List<InteractionRequirement> reqLevels;

	public List<InteractionRequirement> reqQuests;

	public List<InteractionRequirement> reqWars;

	private Collider trigger;

	private GameObject node;

	private InventoryItem item;

	private ResourceNode resourceNode;

	private bool isUsingResource;

	private int primaryItemID;

	private int rareItemID;

	private List<int> dropIDs;

	private Dictionary<int, GameObject> playerFish;

	private Player me;

	public override void Awake()
	{
		base.Awake();
		playerFish = new Dictionary<int, GameObject>();
		dropIDs = new List<int>();
		trigger = GetComponent<Collider>();
		if (trigger == null)
		{
			Debug.LogError("Resource Machine with ID " + ID + " is missing a collider. Please add a collider in the map. A new sphere collider has been temporarily added.");
			trigger = base.gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
			trigger.isTrigger = true;
			(trigger as SphereCollider).radius = 5f;
		}
	}

	public void Start()
	{
		if (resourceNode == null)
		{
			trigger.enabled = false;
		}
		me = Entities.Instance.me;
	}

	public override void OnClick(Vector3 hitpoint)
	{
		if ((base.transform.position - me.wrapper.transform.position).magnitude <= Game.Max_Click_Distance)
		{
			me.TargetNode = resourceNode;
		}
	}

	public override void Trigger(bool checkRequirements)
	{
		if ((base.transform.position - me.wrapper.transform.position).magnitude > Distance || isUsingResource)
		{
			return;
		}
		if (!me.assetController.IsAssetLoadComplete)
		{
			SendMessageLoadingAsset();
			return;
		}
		item = Session.MyPlayerData.GetEquippedItem(equipItemSlot);
		if (item == null)
		{
			SendMessageIncorrectItemType();
			return;
		}
		if (item.GetTradeSkillPower() < power)
		{
			SendMessageItemPowerTooLow();
			return;
		}
		Effect effect = me.effects.FirstOrDefault((Effect p) => p.template.isTravelForm);
		if (effect != null)
		{
			Game.Instance.SendRequestEffectRemove(effect.ID);
		}
		else if (IsRequirementMet())
		{
			AEC.getInstance().sendRequest(new RequestMachineResourceTrigger(ID));
			me.TurnTo(Vector3.Normalize(base.transform.position - Entities.Instance.me.wrapperTransform.position), 45f);
			isUsingResource = true;
			trigger.enabled = false;
		}
	}

	public void DespawnNode()
	{
		if (Entities.Instance.me.TargetNode == resourceNode)
		{
			Entities.Instance.me.TargetNode = null;
		}
		if (resourceNode != null)
		{
			resourceNode.Destroy();
			resourceNode = null;
		}
		UnityEngine.Object.Destroy(node);
		node = null;
		trigger.enabled = false;
		dropIDs.Clear();
	}

	public void SpawnNode(int totalUsages, int usages, int nodeID, RarityType rarity, int tradeSkillLevel, string requirements, List<int> dropIDs)
	{
		if (node != null)
		{
			return;
		}
		this.tradeSkillLevel = tradeSkillLevel;
		power = tradeSkillLevel * 20;
		this.dropIDs.AddRange(dropIDs);
		LoadRequirements(requirements);
		string path = ((tradeSkillType != 0) ? ("TradeSkills/Prefabs/" + Enum.GetName(typeof(TradeSkillType), tradeSkillType) + "/Node" + nodeID + "/" + Enum.GetName(typeof(RarityType), rarity)) : ("TradeSkills/Prefabs/Fishing/" + Enum.GetName(typeof(RarityType), rarity)));
		GameObject gameObject = Resources.Load(path) as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("Resource node prefab not found!");
			return;
		}
		node = base.gameObject.AddChild(gameObject);
		if (randomizeRotationY)
		{
			node.transform.rotation = node.transform.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range(0f, 360f), 0f);
		}
		resourceNode = node.GetComponent<ResourceNode>();
		if (resourceNode == null)
		{
			resourceNode = node.AddComponent(typeof(ResourceNode)) as ResourceNode;
		}
		resourceNode.Init(this, totalUsages, usages);
		trigger.enabled = true;
	}

	public void CollectResource()
	{
		AEC.getInstance().sendRequest(new RequestMachineResourceCollect(ID, primaryItemID, rareItemID));
		StartCoroutine(StopUsingResource());
	}

	public void DropResource()
	{
		AEC.getInstance().sendRequest(new RequestMachineResourceDrop(ID));
		StartCoroutine(StopUsingResource());
	}

	public void Interrupt()
	{
		if (tradeSkillType == TradeSkillType.Fishing)
		{
			UIGame.Instance.fishing.Close();
			ReleaseFish(me.ID, immediate: true);
			DespawnBobber(me.ID, immediate: true);
		}
		TradeSkillCastBar.Instance.Hide();
		StartCoroutine(StopUsingResource());
	}

	public bool ContainsItem(int itemID)
	{
		return dropIDs.Contains(itemID);
	}

	public void UpdateUsage(int remainingUsages)
	{
		if (resourceNode != null)
		{
			resourceNode.UpdateUsage(remainingUsages);
		}
	}

	public void Channel(float time)
	{
		if (isUsingResource)
		{
			TradeSkillCastBar.Instance.Show(tradeSkillType, time);
		}
	}

	public void Interact(int primaryItemID, int rareItemID, RarityType rarity)
	{
		if (isUsingResource)
		{
			TradeSkillCastBar.Instance.Hide();
			this.primaryItemID = primaryItemID;
			this.rareItemID = rareItemID;
			if (tradeSkillType == TradeSkillType.Fishing)
			{
				UIGame.Instance.fishing.Init(this, item.Rarity, rarity);
			}
		}
	}

	public void DespawnBobber(int playerID, bool immediate)
	{
		Entities.Instance.GetPlayerById(playerID)?.DespawnBobber(immediate);
	}

	public void SpawnBobber(int playerID)
	{
		Entities.Instance.GetPlayerById(playerID)?.SpawnBobber();
	}

	public void CatchFish(int playerID)
	{
		if (playerFish.ContainsKey(playerID))
		{
			StartCoroutine(RemoveFish(playerID, immediate: false));
		}
		else
		{
			DestroyFish(playerID);
		}
	}

	public void HookFish(int playerID, RarityType rarity)
	{
		Player playerById = Entities.Instance.GetPlayerById(playerID);
		if (playerById != null)
		{
			DestroyFish(playerID);
			playerFish.Add(playerID, playerById.CastSpot.gameObject.AddChild(Resources.Load("TradeSkills/Prefabs/Fishing/" + Enum.GetName(typeof(RarityType), rarity) + "Fish") as GameObject));
			StartCoroutine(SpawnFish(playerID));
		}
		else
		{
			DestroyFish(playerID);
		}
	}

	public void ReleaseFish(int playerID, bool immediate)
	{
		if (playerFish.ContainsKey(playerID) && playerFish[playerID] != null)
		{
			playerFish[playerID].transform.SetParent(null);
			Animator componentInChildren = playerFish[playerID].GetComponentInChildren<Animator>();
			if (componentInChildren != null && componentInChildren.HasAnimatorState("fastswim"))
			{
				componentInChildren.CrossFadeInFixedTime("fastswim", 0.25f, -1, 0f);
			}
			StartCoroutine(UnhookFish(playerID));
			StartCoroutine(RemoveFish(playerID, immediate));
		}
		else
		{
			DestroyFish(playerID);
		}
	}

	private void DestroyFish(int playerID)
	{
		if (playerFish.ContainsKey(playerID))
		{
			UnityEngine.Object.Destroy(playerFish[playerID]);
			playerFish[playerID] = null;
			playerFish.Remove(playerID);
		}
	}

	protected override bool IsRequirementMet()
	{
		if (reqBitFlags.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			Chat.Notify("You do not meet the requirements.", InterfaceColors.Chat.Yellow.ToBBCode());
			return false;
		}
		if (reqEquippedItems.Count > 0 && !reqEquippedItems.Exists((InteractionRequirement x) => x.IsRequirementMet(Session.MyPlayerData)))
		{
			Chat.Notify("You do not have the correct items equipped.", InterfaceColors.Chat.Yellow.ToBBCode());
			return false;
		}
		if (reqItems.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			Chat.Notify("You do not have the correct items in your inventory.", InterfaceColors.Chat.Yellow.ToBBCode());
			return false;
		}
		if (reqLevels.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			Chat.Notify("You do not meet the level requirements.", InterfaceColors.Chat.Yellow.ToBBCode());
			return false;
		}
		if (reqQuests.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			Chat.Notify("You do not meet the quest requirements.", InterfaceColors.Chat.Yellow.ToBBCode());
			return false;
		}
		if (reqWars.Exists((InteractionRequirement x) => !x.IsRequirementMet(Session.MyPlayerData)))
		{
			Chat.Notify("The war requirements are not met yet.", InterfaceColors.Chat.Yellow.ToBBCode());
			return false;
		}
		return true;
	}

	protected override void LoadRequirements(string jsonRequirements)
	{
		base.LoadRequirements(jsonRequirements);
		reqBitFlags = Requirements.Where((InteractionRequirement x) => x is IABitFlagRequired).ToList();
		reqEquippedItems = Requirements.Where((InteractionRequirement x) => x is IAItemEquippedRequired).ToList();
		reqItems = Requirements.Where((InteractionRequirement x) => x is IAItemRequired).ToList();
		reqLevels = Requirements.Where((InteractionRequirement x) => x is IALevelRequired).ToList();
		reqQuests = Requirements.Where((InteractionRequirement x) => x is IAQuestRequired || x is IAQuestCompleted || x is IAQuestObjectiveRequired || x is IAWarProgressRequired).ToList();
		reqWars = Requirements.Where((InteractionRequirement x) => x is IAWarProgressRequired).ToList();
	}

	private void SendMessageIncorrectItemType()
	{
		switch (itemType)
		{
		case ItemType.FishingRod:
		{
			string text = "Requires a Fishing Rod equipped.";
			if (Session.MyPlayerData.tradeSkillXP[TradeSkillType.Fishing] == 0 && Entities.Instance.me.tradeSkillLevel[TradeSkillType.Fishing] == 1)
			{
				text += " Talk to Faith in Battleon to learn how to fish!";
			}
			Chat.Notify(text, InterfaceColors.Chat.Yellow.ToBBCode());
			break;
		}
		case ItemType.PickAxe:
			Chat.Notify("Requires a Mining Pickaxe equipped.", InterfaceColors.Chat.Yellow.ToBBCode());
			break;
		}
	}

	private void SendMessageItemPowerTooLow()
	{
		switch (itemType)
		{
		case ItemType.FishingRod:
			Chat.Notify("Requires a Fishing Power " + power + " Rod or higher.", InterfaceColors.Chat.Yellow.ToBBCode());
			break;
		case ItemType.PickAxe:
			Chat.Notify("Requires a Mining Power " + power + " Pickaxe or higher.", InterfaceColors.Chat.Yellow.ToBBCode());
			break;
		}
	}

	private void SendMessageLoadingAsset()
	{
		Chat.Notify("Your character is still loading.", InterfaceColors.Chat.Yellow.ToBBCode());
	}

	private IEnumerator StopUsingResource()
	{
		float elapsed = 0f;
		while (elapsed < 1.5f)
		{
			elapsed += Time.deltaTime;
			yield return null;
		}
		isUsingResource = false;
		trigger.enabled = true;
	}

	private IEnumerator RemoveFish(int playerID, bool immediate)
	{
		GameObject fish = playerFish[playerID];
		if (fish != null && !immediate)
		{
			fish.transform.localPosition = Vector3.zero;
			float elapsed = 0f;
			while (fish != null && elapsed < 1.825f)
			{
				elapsed += Time.deltaTime;
				yield return null;
			}
		}
		DestroyFish(playerID);
	}

	private IEnumerator UnhookFish(int playerID)
	{
		GameObject fish = playerFish[playerID];
		if (fish != null)
		{
			Vector3 initialScale = fish.transform.lossyScale;
			float elapsed = 0f;
			while (fish != null && elapsed <= 1f)
			{
				fish.transform.Translate(base.transform.forward * 5f * Time.deltaTime);
				fish.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, elapsed);
				elapsed += Time.deltaTime;
				yield return null;
			}
			if (fish == null)
			{
				DestroyFish(playerID);
			}
		}
		else
		{
			DestroyFish(playerID);
		}
	}

	private IEnumerator SpawnFish(int playerID)
	{
		GameObject fish = playerFish[playerID];
		if (fish != null)
		{
			if (Physics.Raycast(fish.transform.position, fish.transform.TransformDirection(Vector3.up), out var hitInfo, 15f, 1 << Layers.WATER, QueryTriggerInteraction.Collide))
			{
				fish.transform.position += Vector3.up * (hitInfo.distance - 0.5f);
				fish.transform.localPosition += new Vector3(0f, 0f, 0.35f);
				Vector3 targetScale = fish.transform.localScale;
				float elapsed = 0f;
				while (fish != null && elapsed <= 1f)
				{
					fish.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed);
					elapsed += Time.deltaTime;
					yield return null;
				}
				if (fish == null)
				{
					DestroyFish(playerID);
				}
			}
			else
			{
				RemoveFish(playerID, immediate: true);
			}
		}
		else
		{
			DestroyFish(playerID);
		}
	}
}
