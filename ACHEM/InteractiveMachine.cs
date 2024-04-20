using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public abstract class InteractiveMachine : BaseMachine
{
	private static JsonSerializerSettings SerializeSettings = new JsonSerializerSettings
	{
		TypeNameHandling = TypeNameHandling.Auto,
		Binder = new TypeNameSerializationBinder()
	};

	public List<MachineAction> Actions = new List<MachineAction>();

	public List<ClientTriggerAction> CTActions = new List<ClientTriggerAction>();

	public bool useDatabaseRecord;

	public int databaseID;

	public static event Action<InteractiveMachine> AnyMachineActiveUpdated;

	public override void Init(byte state, int ownerId, List<int> areas)
	{
		base.Init(state, ownerId, areas);
		Areas = areas;
		AddAreaIDs();
	}

	public virtual bool HasQuestObjective(int qoid)
	{
		foreach (MachineAction action in Actions)
		{
			if (action is MAQuestObjective mAQuestObjective)
			{
				if (mAQuestObjective.QOID == qoid)
				{
					return true;
				}
			}
			else if (action is MARandomAction mARandomAction)
			{
				foreach (MachineAction action2 in mARandomAction.Actions)
				{
					if (action2 is MAQuestObjective mAQuestObjective2 && mAQuestObjective2.QOID == qoid)
					{
						return true;
					}
				}
			}
			else
			{
				if (!(action is CompletionAction completionAction))
				{
					continue;
				}
				foreach (MachineAction completionAction2 in completionAction.completionActions)
				{
					if (completionAction2 is MAQuestObjective mAQuestObjective3 && mAQuestObjective3.QOID == qoid)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	private void AddAreaIDs()
	{
		foreach (ClientTriggerAction cTAction in CTActions)
		{
			if (cTAction is CTATransferMap)
			{
				int mapID = (cTAction as CTATransferMap).MapID;
				if (!Areas.Contains(mapID))
				{
					Areas.Add(mapID);
				}
			}
			if (cTAction is CTANPCIA)
			{
				(cTAction as CTANPCIA).ApopsLoaded += OnApopsLoaded;
			}
		}
		foreach (MachineAction action in Actions)
		{
			if (action is MATransferMap)
			{
				int mapID2 = (action as MATransferMap).MapID;
				if (!Areas.Contains(mapID2))
				{
					Areas.Add(mapID2);
				}
			}
		}
	}

	private void OnApopsLoaded(NPCIA root)
	{
		foreach (ClientTriggerAction cTAction in CTActions)
		{
			if (cTAction is CTANPCIA)
			{
				CTANPCIA cTANPCIA = cTAction as CTANPCIA;
				if (cTANPCIA.Apops != null && cTANPCIA.Apops.Count > 0 && cTANPCIA.Apops[0] == root)
				{
					cTANPCIA.ApopsLoaded -= OnApopsLoaded;
					RecursiveAddArea(root);
				}
			}
		}
	}

	private void RecursiveAddArea(NPCIA npcia)
	{
		if (npcia is NPCIAAction)
		{
			NPCIAAction nPCIAAction = npcia as NPCIAAction;
			if (nPCIAAction.Action is CTATransferMapCore)
			{
				int mapID = (nPCIAAction.Action as CTATransferMapCore).MapID;
				if (!Areas.Contains(mapID))
				{
					Areas.Add(mapID);
				}
			}
		}
		foreach (NPCIA child in npcia.children)
		{
			RecursiveAddArea(child);
		}
	}

	protected override void OnActiveUpdated(bool active)
	{
		base.OnActiveUpdated(active);
		if (InteractiveMachine.AnyMachineActiveUpdated != null)
		{
			InteractiveMachine.AnyMachineActiveUpdated(this);
		}
	}

	public override void Trigger(bool checkRequirements)
	{
		if (!CanInteract() || !IsInteractive())
		{
			return;
		}
		AEC.getInstance().sendRequest(new RequestMachineClick(ID));
		foreach (ClientTriggerAction cTAction in CTActions)
		{
			cTAction.Execute();
		}
		if (!string.IsNullOrEmpty(SfxTrigger))
		{
			AudioManager.Play2DSFX(SfxTrigger);
		}
	}

	public void LoadDBData(string jsonActions, string jsonCTActions, string jsonRequirements)
	{
		LoadActions(jsonActions, Actions);
		LoadCTActions(jsonCTActions, CTActions);
		LoadRequirements(jsonRequirements);
	}

	protected void LoadActions(string jsonActions, List<MachineAction> actionList)
	{
		if (string.IsNullOrEmpty(jsonActions))
		{
			return;
		}
		actionList.Clear();
		try
		{
			foreach (JToken item in JArray.Parse(jsonActions.Replace("Core", "")))
			{
				string text = item["$type"].ToString();
				if (text == "MATriggerMachine")
				{
					MATriggerMachine mATriggerMachine = base.gameObject.AddComponent<MATriggerMachine>();
					BaseMachine machineByMachineID = BaseMachine.GetMachineByMachineID(int.Parse(item["MachineID"].ToString()));
					mATriggerMachine.Machine = machineByMachineID;
					actionList.Add(mATriggerMachine);
				}
				if (text == "MASetMachineState")
				{
					MASetMachineState mASetMachineState = base.gameObject.AddComponent<MASetMachineState>();
					int targetMachineID = int.Parse(item["MachineID"].ToString());
					mASetMachineState.TargetMachineID = targetMachineID;
					actionList.Add(mASetMachineState);
				}
				switch (text)
				{
				case "MAAdjustScore":
				{
					MAAdjustScore mAAdjustScore = base.gameObject.AddComponent<MAAdjustScore>();
					mAAdjustScore.teamID = int.Parse(item["teamID"].ToString());
					mAAdjustScore.adjustment = int.Parse(item["adjustment"].ToString());
					if (item["MinDelay"] != null)
					{
						mAAdjustScore.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mAAdjustScore.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mAAdjustScore);
					break;
				}
				case "MAAdjustWarMeter":
				{
					MAAdjustWarMeter mAAdjustWarMeter = base.gameObject.AddComponent<MAAdjustWarMeter>();
					mAAdjustWarMeter.warID = int.Parse(item["warID"].ToString());
					mAAdjustWarMeter.adjustment = int.Parse(item["adjustment"].ToString());
					if (item["MinDelay"] != null)
					{
						mAAdjustWarMeter.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mAAdjustWarMeter.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mAAdjustWarMeter);
					break;
				}
				case "MACloseInstance maCloseInstance":
				{
					MACloseInstance mACloseInstance = base.gameObject.AddComponent<MACloseInstance>();
					mACloseInstance.MapID = int.Parse(item["MapID"].ToString());
					mACloseInstance.CellID = int.Parse(item["CellID"].ToString());
					mACloseInstance.SpawnID = int.Parse(item["SpawnID"].ToString());
					if (item["MinDelay"] != null)
					{
						mACloseInstance.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mACloseInstance.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mACloseInstance);
					break;
				}
				case "MACompleteCell":
				{
					MACompleteCell mACompleteCell = base.gameObject.AddComponent<MACompleteCell>();
					if (item["MinDelay"] != null)
					{
						mACompleteCell.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mACompleteCell.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mACompleteCell);
					break;
				}
				case "MAEquipItem":
				{
					MAEquipItem mAEquipItem = base.gameObject.AddComponent<MAEquipItem>();
					mAEquipItem.ItemID = int.Parse(item["ItemID"].ToString());
					if (item["MinDelay"] != null)
					{
						mAEquipItem.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mAEquipItem.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mAEquipItem);
					break;
				}
				case "MAExitInstance":
				{
					MAExitInstance mAExitInstance = base.gameObject.AddComponent<MAExitInstance>();
					if (item["MinDelay"] != null)
					{
						mAExitInstance.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mAExitInstance.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mAExitInstance);
					break;
				}
				case "MAGiveItem maGiveItem":
				{
					MAGiveItem mAGiveItem = base.gameObject.AddComponent<MAGiveItem>();
					mAGiveItem.ItemID = int.Parse(item["ItemID"].ToString());
					mAGiveItem.Quantity = int.Parse(item["Quantity"].ToString());
					mAGiveItem.AutoEquip = bool.Parse(item["maGiveItem.AutoEquip"].ToString());
					if (item["MinDelay"] != null)
					{
						mAGiveItem.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mAGiveItem.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mAGiveItem);
					break;
				}
				case "MANotifyCell":
				{
					MANotifyCell mANotifyCell = base.gameObject.AddComponent<MANotifyCell>();
					mANotifyCell.Message = item["Message"].ToString();
					Enum.TryParse<GameNotificationType>(item["NotificationType"].ToString(), out mANotifyCell.NotificationType);
					if (item["MinDelay"] != null)
					{
						mANotifyCell.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mANotifyCell.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mANotifyCell);
					break;
				}
				case "MANotifyTeamCell":
				{
					MANotifyTeamCell mANotifyTeamCell = base.gameObject.AddComponent<MANotifyTeamCell>();
					mANotifyTeamCell.Message = item["Message"].ToString();
					mANotifyTeamCell.NotificationType = (GameNotificationType)int.Parse(item["NotificationType"].ToString());
					mANotifyTeamCell.teamID = int.Parse(item["maNotifyTeamCell.teamID"].ToString());
					if (item["MinDelay"] != null)
					{
						mANotifyTeamCell.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mANotifyTeamCell.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mANotifyTeamCell);
					break;
				}
				case "MANpcNotifyCell":
				{
					NPCSpawn spawn = NPCSpawn.GetSpawn(int.Parse(item["NPCSpawnID"].ToString()));
					if (spawn == null)
					{
						SendDBErrorMessage();
						break;
					}
					MANpcNotifyCell mANpcNotifyCell = base.gameObject.AddComponent<MANpcNotifyCell>();
					mANpcNotifyCell.npc = spawn;
					mANpcNotifyCell.cellID = int.Parse(item["cellID"].ToString());
					mANpcNotifyCell.Message = item["Message"].ToString();
					mANpcNotifyCell.NotificationType = (GameNotificationType)int.Parse(item["NotificationType"].ToString());
					if (item["MinDelay"] != null)
					{
						mANpcNotifyCell.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mANpcNotifyCell.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mANpcNotifyCell);
					break;
				}
				case "MAQuestObjective":
				{
					MAQuestObjective mAQuestObjective = base.gameObject.AddComponent<MAQuestObjective>();
					mAQuestObjective.QuestID = int.Parse(item["QuestID"].ToString());
					mAQuestObjective.QOID = int.Parse(item["QOID"].ToString());
					if (item["MinDelay"] != null)
					{
						mAQuestObjective.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mAQuestObjective.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mAQuestObjective);
					break;
				}
				case "MARemoveItem":
				{
					MARemoveItem mARemoveItem = base.gameObject.AddComponent<MARemoveItem>();
					mARemoveItem.ItemID = int.Parse(item["ItemID"].ToString());
					mARemoveItem.Quantity = int.Parse(item["Quantity"].ToString());
					if (item["MinDelay"] != null)
					{
						mARemoveItem.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mARemoveItem.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mARemoveItem);
					break;
				}
				case "MASetAreaFlag":
				{
					MASetAreaFlag mASetAreaFlag = base.gameObject.AddComponent<MASetAreaFlag>();
					mASetAreaFlag.key = item["key"].ToString();
					mASetAreaFlag.value = item["value"].ToString();
					if (item["MinDelay"] != null)
					{
						mASetAreaFlag.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mASetAreaFlag.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mASetAreaFlag);
					break;
				}
				case "MASetAreaLock":
				{
					MASetAreaLock mASetAreaLock = base.gameObject.AddComponent<MASetAreaLock>();
					mASetAreaLock.lockState = bool.Parse(item["lockState"].ToString());
					if (item["MinDelay"] != null)
					{
						mASetAreaLock.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mASetAreaLock.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mASetAreaLock);
					break;
				}
				case "MASetNPCLevel":
				{
					NPCSpawn spawn7 = NPCSpawn.GetSpawn(int.Parse(item["NPCSpawnID"].ToString()));
					if (spawn7 == null)
					{
						SendDBErrorMessage();
						break;
					}
					MASetNPCLevel mASetNPCLevel = base.gameObject.AddComponent<MASetNPCLevel>();
					mASetNPCLevel.NPCSpawn = spawn7;
					mASetNPCLevel.level = int.Parse(item["level"].ToString());
					if (item["MinDelay"] != null)
					{
						mASetNPCLevel.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mASetNPCLevel.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mASetNPCLevel);
					break;
				}
				case "MASetNPCMovementBehavior":
				{
					NPCSpawn spawn5 = NPCSpawn.GetSpawn(int.Parse(item["NPCSpawnID"].ToString()));
					if (spawn5 == null)
					{
						SendDBErrorMessage();
						break;
					}
					MASetNPCMovementBehavior mASetNPCMovementBehavior = base.gameObject.AddComponent<MASetNPCMovementBehavior>();
					mASetNPCMovementBehavior.spawn = spawn5;
					mASetNPCMovementBehavior.behavior = (NPCMovementBehavior)int.Parse(item["behavior"].ToString());
					if (item["MinDelay"] != null)
					{
						mASetNPCMovementBehavior.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mASetNPCMovementBehavior.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mASetNPCMovementBehavior);
					break;
				}
				case "MASetNPCPathing":
				{
					NPCSpawn spawn3 = NPCSpawn.GetSpawn(int.Parse(item["NPCSpawnID"].ToString()));
					if (spawn3 == null)
					{
						SendDBErrorMessage();
						break;
					}
					MASetNPCPathing mASetNPCPathing = base.gameObject.AddComponent<MASetNPCPathing>();
					mASetNPCPathing.spawn = spawn3;
					mASetNPCPathing.pathingActive = bool.Parse(item["pathingActive"].ToString());
					mASetNPCPathing.duration = float.Parse(item["duration"].ToString());
					if (item["MinDelay"] != null)
					{
						mASetNPCPathing.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mASetNPCPathing.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mASetNPCPathing);
					break;
				}
				case "MASetNPCReaction":
				{
					NPCSpawn spawn6 = NPCSpawn.GetSpawn(int.Parse(item["NPCSpawnID"].ToString()));
					if (spawn6 == null)
					{
						SendDBErrorMessage();
						break;
					}
					MASetNPCReaction mASetNPCReaction = base.gameObject.AddComponent<MASetNPCReaction>();
					mASetNPCReaction.NPCSpawn = spawn6;
					mASetNPCReaction.Reaction = (Entity.Reaction)int.Parse(item["Reaction"].ToString());
					if (item["MinDelay"] != null)
					{
						mASetNPCReaction.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mASetNPCReaction.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mASetNPCReaction);
					break;
				}
				case "MASetNPCRegeneration":
				{
					NPCSpawn spawn4 = NPCSpawn.GetSpawn(int.Parse(item["NPCSpawnID"].ToString()));
					if (spawn4 == null)
					{
						SendDBErrorMessage();
						break;
					}
					MASetNPCRegeneration mASetNPCRegeneration = base.gameObject.AddComponent<MASetNPCRegeneration>();
					mASetNPCRegeneration.NPCSpawn = spawn4;
					mASetNPCRegeneration.allowRegeneration = bool.Parse(item["allowRegeneration"].ToString());
					if (item["MinDelay"] != null)
					{
						mASetNPCRegeneration.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mASetNPCRegeneration.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mASetNPCRegeneration);
					break;
				}
				case "MASetNPCState":
				{
					NPCSpawn spawn2 = NPCSpawn.GetSpawn(int.Parse(item["NPCSpawnID"].ToString()));
					if (spawn2 == null)
					{
						SendDBErrorMessage();
						break;
					}
					MASetNPCState mASetNPCState = base.gameObject.AddComponent<MASetNPCState>();
					mASetNPCState.NPCSpawn = spawn2;
					mASetNPCState.State = byte.Parse(item["State"].ToString());
					if (item["MinDelay"] != null)
					{
						mASetNPCState.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mASetNPCState.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mASetNPCState);
					break;
				}
				case "MAAbandonQuestCore":
				{
					MAAbandonQuest mAAbandonQuest = base.gameObject.AddComponent<MAAbandonQuest>();
					mAAbandonQuest.questID = int.Parse(item["questID"].ToString());
					actionList.Add(mAAbandonQuest);
					break;
				}
				case "MASetPlayerSpawn":
				{
					MASetPlayerSpawn mASetPlayerSpawn = base.gameObject.AddComponent<MASetPlayerSpawn>();
					mASetPlayerSpawn.spawnID = int.Parse(item["spawnID"].ToString());
					if (item["MinDelay"] != null)
					{
						mASetPlayerSpawn.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mASetPlayerSpawn.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mASetPlayerSpawn);
					break;
				}
				case "MATeleport":
				{
					MATeleport mATeleport = base.gameObject.AddComponent<MATeleport>();
					mATeleport.SpawnID = int.Parse(item["SpawnID"].ToString());
					if (item["MinDelay"] != null)
					{
						mATeleport.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mATeleport.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mATeleport);
					break;
				}
				case "MATransferCell":
				{
					MATransferCell mATransferCell = base.gameObject.AddComponent<MATransferCell>();
					mATransferCell.CellID = int.Parse(item["CellID"].ToString());
					mATransferCell.SpawnID = int.Parse(item["SpawnID"].ToString());
					if (item["MinDelay"] != null)
					{
						mATransferCell.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mATransferCell.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mATransferCell);
					break;
				}
				case "MATransferMap":
				{
					MATransferMap mATransferMap = base.gameObject.AddComponent<MATransferMap>();
					mATransferMap.MapID = int.Parse(item["MapID"].ToString());
					mATransferMap.CellID = int.Parse(item["CellID"].ToString());
					mATransferMap.SpawnID = int.Parse(item["SpawnID"].ToString());
					if (item["MinDelay"] != null)
					{
						mATransferMap.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mATransferMap.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mATransferMap);
					break;
				}
				case "MATransferTeamToMap":
				{
					MATransferTeamToMap mATransferTeamToMap = base.gameObject.AddComponent<MATransferTeamToMap>();
					mATransferTeamToMap.MapID = int.Parse(item["MapID"].ToString());
					mATransferTeamToMap.CellID = int.Parse(item["CellID"].ToString());
					mATransferTeamToMap.SpawnID = int.Parse(item["SpawnID"].ToString());
					mATransferTeamToMap.teamID = int.Parse(item["teamID"].ToString());
					if (item["MinDelay"] != null)
					{
						mATransferTeamToMap.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mATransferTeamToMap.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mATransferTeamToMap);
					break;
				}
				case "MACastSpell":
				{
					MACastSpell mACastSpell = base.gameObject.AddComponent<MACastSpell>();
					mACastSpell.SpellId = int.Parse(item["SpellId"].ToString());
					Enum.TryParse<MachineSpellMode>(item["SpellMode"].ToString(), out mACastSpell.SpellMode);
					mACastSpell.SpellModeValue = int.Parse(item["SpellModeValue"].ToString());
					mACastSpell.CastAsLevelMultiplier = float.Parse(item["CastAsLevelMultiplier"].ToString());
					mACastSpell.IsAOE = bool.Parse(item["IsAOE"].ToString());
					Enum.TryParse<CombatSolver.MachineTargetType>(item["TargetType"].ToString(), out mACastSpell.TargetType);
					if (item["MinDelay"] != null)
					{
						mACastSpell.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						mACastSpell.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					actionList.Add(mACastSpell);
					break;
				}
				}
			}
		}
		catch (Exception)
		{
			Chat.SendAdminMessage("DB Machine Error - Failed to parse Machine Actions for Database Machine ID " + databaseID + " . Please check admin");
		}
	}

	protected void LoadCTActions(string jsonCTActions, List<ClientTriggerAction> ctActionList)
	{
		if (string.IsNullOrEmpty(jsonCTActions))
		{
			return;
		}
		ctActionList.Clear();
		try
		{
			foreach (JToken item in JArray.Parse(jsonCTActions.Replace("Core", "")))
			{
				string text = item["$type"].ToString();
				if (text == "CTADialogue")
				{
					CTADialogue cTADialogue = base.gameObject.AddComponent<CTADialogue>();
					cTADialogue.ID = int.Parse(item["ID"].ToString());
					cTADialogue.SkipCompleteAction = bool.Parse(item["SkipCompleteAction"].ToString());
					if (item["MinDelay"] != null)
					{
						cTADialogue.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						cTADialogue.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					ctActionList.Add(cTADialogue);
				}
				if (text == "CTAOpenApop")
				{
					CTAOpenApop cTAOpenApop = base.gameObject.AddComponent<CTAOpenApop>();
					cTAOpenApop.ApopID = int.Parse(item["ApopID"].ToString());
					if (item["MinDelay"] != null)
					{
						cTAOpenApop.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						cTAOpenApop.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					ctActionList.Add(cTAOpenApop);
				}
				if (text == "CTATransferMap")
				{
					CTATransferMap cTATransferMap = base.gameObject.AddComponent<CTATransferMap>();
					cTATransferMap.MapID = int.Parse(item["MapID"].ToString());
					cTATransferMap.CellID = int.Parse(item["CellID"].ToString());
					cTATransferMap.SpawnID = int.Parse(item["SpawnID"].ToString());
					cTATransferMap.ShowConfirmation = bool.Parse(item["ShowConfirmation"].ToString());
					if (item["MinDelay"] != null)
					{
						cTATransferMap.MinDelay = float.Parse(item["MinDelay"].ToString());
					}
					if (item["MaxDelay"] != null)
					{
						cTATransferMap.MaxDelay = float.Parse(item["MaxDelay"].ToString());
					}
					ctActionList.Add(cTATransferMap);
				}
			}
		}
		catch (Exception)
		{
			Chat.SendAdminMessage("DB Machine Error - Failed to parse CT Actions for Database Machine ID " + databaseID + " . Please check admin");
		}
	}

	protected virtual void LoadRequirements(string jsonRequirements)
	{
		Requirements.Clear();
		if (!string.IsNullOrEmpty(jsonRequirements))
		{
			jsonRequirements = jsonRequirements.Replace(">=", "GreaterThanOrEqual");
			jsonRequirements = jsonRequirements.Replace("<=", "LessThanOrEqual");
			jsonRequirements = jsonRequirements.Replace("!=", "NotEqual");
			jsonRequirements = jsonRequirements.Replace("=", "Equal");
			jsonRequirements = jsonRequirements.Replace(">", "GreaterThan");
			jsonRequirements = jsonRequirements.Replace("<", "LessThan");
			try
			{
				Requirements = CreateRequirementsFromDeserializedRequirements(JsonConvert.DeserializeObject<List<InteractionRequirement>>(jsonRequirements.Replace("Core", ""), SerializeSettings));
			}
			catch (Exception)
			{
				Chat.SendAdminMessage("DB Machine Error - Failed to parse Requirements for Database Machine ID " + databaseID + " . Please check admin");
			}
		}
	}

	private List<InteractionRequirement> CreateRequirementsFromDeserializedRequirements(List<InteractionRequirement> deserializedRequirements)
	{
		List<InteractionRequirement> list = new List<InteractionRequirement>();
		foreach (InteractionRequirement deserializedRequirement in deserializedRequirements)
		{
			if (deserializedRequirement is IAAnd iAAnd)
			{
				IAAnd iAAnd2 = base.gameObject.AddComponent<IAAnd>();
				iAAnd2.Requirements = CreateRequirementsFromDeserializedRequirements(iAAnd.Requirements);
				iAAnd2.Not = iAAnd.Not;
				list.Add(iAAnd2);
			}
			else if (deserializedRequirement is IAAllMonstersCleared)
			{
				list.Add(base.gameObject.AddComponent<IAAllMonstersCleared>());
			}
			else if (deserializedRequirement is IAMatchStateRequired)
			{
				list.Add(base.gameObject.AddComponent<IAMatchStateRequired>());
			}
			else if (deserializedRequirement is IAAreaFlagRequired iAAreaFlagRequired)
			{
				IAAreaFlagRequired iAAreaFlagRequired2 = base.gameObject.AddComponent<IAAreaFlagRequired>();
				iAAreaFlagRequired2.key = iAAreaFlagRequired.key;
				iAAreaFlagRequired2.value = iAAreaFlagRequired.value;
				list.Add(iAAreaFlagRequired2);
			}
			else if (deserializedRequirement is IABagSlotRequired iABagSlotRequired)
			{
				IABagSlotRequired iABagSlotRequired2 = base.gameObject.AddComponent<IABagSlotRequired>();
				iABagSlotRequired2.Quantity = iABagSlotRequired.Quantity;
				iABagSlotRequired2.Comparison = iABagSlotRequired.Comparison;
				list.Add(iABagSlotRequired2);
			}
			else if (deserializedRequirement is IABitFlagRequired iABitFlagRequired)
			{
				IABitFlagRequired iABitFlagRequired2 = base.gameObject.AddComponent<IABitFlagRequired>();
				iABitFlagRequired2.BitFlagName = iABitFlagRequired.BitFlagName;
				iABitFlagRequired2.BitFlagIndex = iABitFlagRequired.BitFlagIndex;
				list.Add(iABitFlagRequired2);
			}
			else if (deserializedRequirement is IABitFlagValueRequired iABitFlagValueRequired)
			{
				IABitFlagValueRequired iABitFlagValueRequired2 = base.gameObject.AddComponent<IABitFlagValueRequired>();
				iABitFlagValueRequired2.BitFlagName = iABitFlagValueRequired.BitFlagName;
				iABitFlagValueRequired2.BitFlagIndex = iABitFlagValueRequired.BitFlagIndex;
				iABitFlagValueRequired2.Value = iABitFlagValueRequired.Value;
				list.Add(iABitFlagValueRequired2);
			}
			else if (deserializedRequirement is IAClassEquippedRequired iAClassEquippedRequired)
			{
				IAClassEquippedRequired iAClassEquippedRequired2 = base.gameObject.AddComponent<IAClassEquippedRequired>();
				iAClassEquippedRequired2.ClassID = iAClassEquippedRequired.ClassID;
				iAClassEquippedRequired2.Not = iAClassEquippedRequired.Not;
				list.Add(iAClassEquippedRequired2);
			}
			else if (deserializedRequirement is IADistanceRequired iADistanceRequired)
			{
				IADistanceRequired iADistanceRequired2 = base.gameObject.AddComponent<IADistanceRequired>();
				iADistanceRequired2.Distance = iADistanceRequired.Distance;
				list.Add(iADistanceRequired2);
			}
			else if (deserializedRequirement is IAEffectRequired iAEffectRequired)
			{
				IAEffectRequired iAEffectRequired2 = base.gameObject.AddComponent<IAEffectRequired>();
				iAEffectRequired2.EffectIDs = iAEffectRequired.EffectIDs;
				iAEffectRequired2.Not = iAEffectRequired.Not;
				list.Add(iAEffectRequired2);
			}
			else if (deserializedRequirement is IAItemEquippedRequired iAItemEquippedRequired)
			{
				IAItemEquippedRequired iAItemEquippedRequired2 = base.gameObject.AddComponent<IAItemEquippedRequired>();
				iAItemEquippedRequired2.ItemID = iAItemEquippedRequired.ItemID;
				iAItemEquippedRequired2.Not = iAItemEquippedRequired.Not;
				list.Add(iAItemEquippedRequired2);
			}
			else if (deserializedRequirement is IAItemRequired iAItemRequired)
			{
				IAItemRequired iAItemRequired2 = base.gameObject.AddComponent<IAItemRequired>();
				iAItemRequired2.ItemID = iAItemRequired.ItemID;
				iAItemRequired2.Quantity = iAItemRequired.Quantity;
				iAItemRequired2.Comparison = iAItemRequired.Comparison;
				list.Add(iAItemRequired2);
			}
			else if (deserializedRequirement is IALevelRequired iALevelRequired)
			{
				IALevelRequired iALevelRequired2 = base.gameObject.AddComponent<IALevelRequired>();
				iALevelRequired2.Level = iALevelRequired.Level;
				iALevelRequired2.Comparison = iALevelRequired.Comparison;
				list.Add(iALevelRequired2);
			}
			else if (deserializedRequirement is IAMapRequired iAMapRequired)
			{
				IAMapRequired iAMapRequired2 = base.gameObject.AddComponent<IAMapRequired>();
				iAMapRequired2.MapID = iAMapRequired.MapID;
				list.Add(iAMapRequired2);
			}
			else if (deserializedRequirement is IANot iANot)
			{
				IANot iANot2 = base.gameObject.AddComponent<IANot>();
				iANot2.Requirements = CreateRequirementsFromDeserializedRequirements(iANot.Requirements);
				list.Add(iANot2);
			}
			else if (deserializedRequirement is IANPCAggroRequired iANPCAggroRequired)
			{
				NPCSpawn spawn = NPCSpawn.GetSpawn(iANPCAggroRequired.NPCSpawnID);
				if (spawn == null)
				{
					SendDBErrorMessage();
					continue;
				}
				IANPCAggroRequired iANPCAggroRequired2 = base.gameObject.AddComponent<IANPCAggroRequired>();
				iANPCAggroRequired2.Spawn = spawn;
				iANPCAggroRequired2.HasAggro = iANPCAggroRequired.HasAggro;
				list.Add(iANPCAggroRequired2);
			}
			else if (deserializedRequirement is IANPCEffectRequired iANPCEffectRequired)
			{
				NPCSpawn spawn2 = NPCSpawn.GetSpawn(iANPCEffectRequired.NPCSpawnID);
				if (spawn2 == null)
				{
					SendDBErrorMessage();
					continue;
				}
				IANPCEffectRequired iANPCEffectRequired2 = base.gameObject.AddComponent<IANPCEffectRequired>();
				iANPCEffectRequired2.spawn = spawn2;
				iANPCEffectRequired2.effectID = iANPCEffectRequired.effectID;
				iANPCEffectRequired2.hasEffect = iANPCEffectRequired.hasEffect;
				list.Add(iANPCEffectRequired2);
			}
			else if (deserializedRequirement is IANPCHealthPercentRequired iANPCHealthPercentRequired)
			{
				NPCSpawn spawn3 = NPCSpawn.GetSpawn(iANPCHealthPercentRequired.NPCSpawnID);
				if (spawn3 == null)
				{
					SendDBErrorMessage();
					continue;
				}
				IANPCHealthPercentRequired iANPCHealthPercentRequired2 = base.gameObject.AddComponent<IANPCHealthPercentRequired>();
				iANPCHealthPercentRequired2.spawn = spawn3;
				iANPCHealthPercentRequired2.ComparisonType = iANPCHealthPercentRequired.ComparisonType;
				iANPCHealthPercentRequired2.percent = iANPCHealthPercentRequired.percent;
				list.Add(iANPCHealthPercentRequired2);
			}
			else if (deserializedRequirement is IANPCReactionRequired iANPCReactionRequired)
			{
				NPCSpawn spawn4 = NPCSpawn.GetSpawn(iANPCReactionRequired.NPCSpawnID);
				if (spawn4 == null)
				{
					SendDBErrorMessage();
					continue;
				}
				IANPCReactionRequired iANPCReactionRequired2 = base.gameObject.AddComponent<IANPCReactionRequired>();
				iANPCReactionRequired2.spawn = spawn4;
				iANPCReactionRequired2.reaction = iANPCReactionRequired.reaction;
				list.Add(iANPCReactionRequired2);
			}
			else if (deserializedRequirement is IANPCStateRequired iANPCStateRequired)
			{
				NPCSpawn spawn5 = NPCSpawn.GetSpawn(iANPCStateRequired.NPCSpawnID);
				if (spawn5 == null)
				{
					SendDBErrorMessage();
					continue;
				}
				IANPCStateRequired iANPCStateRequired2 = base.gameObject.AddComponent<IANPCStateRequired>();
				iANPCStateRequired2.Spawn = spawn5;
				iANPCStateRequired2.NPCSpawnID = iANPCStateRequired.NPCSpawnID;
				iANPCStateRequired2.State = iANPCStateRequired.State;
				iANPCStateRequired2.RegisterStateUpdateListener();
				list.Add(iANPCStateRequired2);
			}
			else if (deserializedRequirement is IAStateOnMultipleNPCsRequired iAStateOnMultipleNPCsRequired)
			{
				IAStateOnMultipleNPCsRequired iAStateOnMultipleNPCsRequired2 = base.gameObject.AddComponent<IAStateOnMultipleNPCsRequired>();
				iAStateOnMultipleNPCsRequired2.NPCSpawnIDs = iAStateOnMultipleNPCsRequired.NPCSpawnIDs;
				iAStateOnMultipleNPCsRequired2.Spawns = new NPCSpawn[iAStateOnMultipleNPCsRequired2.NPCSpawnIDs.Length];
				for (int i = 0; i < iAStateOnMultipleNPCsRequired2.NPCSpawnIDs.Length; i++)
				{
					NPCSpawn spawn6 = NPCSpawn.GetSpawn(iAStateOnMultipleNPCsRequired2.NPCSpawnIDs[i]);
					if (spawn6 == null)
					{
						SendDBErrorMessage();
					}
					else
					{
						iAStateOnMultipleNPCsRequired2.Spawns[i] = spawn6;
					}
				}
				iAStateOnMultipleNPCsRequired2.State = iAStateOnMultipleNPCsRequired.State;
				iAStateOnMultipleNPCsRequired2.RegisterStateUpdateListener();
				list.Add(iAStateOnMultipleNPCsRequired2);
			}
			else if (deserializedRequirement is IAOr iAOr)
			{
				IAOr iAOr2 = base.gameObject.AddComponent<IAOr>();
				iAOr2.Requirements = CreateRequirementsFromDeserializedRequirements(iAOr.Requirements);
				iAOr2.Not = iAOr.Not;
				list.Add(iAOr2);
			}
			else if (deserializedRequirement is IAPlayerCountRequired iAPlayerCountRequired)
			{
				IAPlayerCountRequired iAPlayerCountRequired2 = base.gameObject.AddComponent<IAPlayerCountRequired>();
				iAPlayerCountRequired2.TargetCount = iAPlayerCountRequired.TargetCount;
				iAPlayerCountRequired2.Comparison = iAPlayerCountRequired.Comparison;
				list.Add(iAPlayerCountRequired2);
			}
			else if (deserializedRequirement is IAQuestCompleted iAQuestCompleted)
			{
				IAQuestCompleted iAQuestCompleted2 = base.gameObject.AddComponent<IAQuestCompleted>();
				iAQuestCompleted2.QuestID = iAQuestCompleted.QuestID;
				iAQuestCompleted2.Not = iAQuestCompleted.Not;
				list.Add(iAQuestCompleted2);
			}
			else if (deserializedRequirement is IAQuestObjectiveCompleted iAQuestObjectiveCompleted)
			{
				IAQuestObjectiveCompleted iAQuestObjectiveCompleted2 = base.gameObject.AddComponent<IAQuestObjectiveCompleted>();
				iAQuestObjectiveCompleted2.QuestID = iAQuestObjectiveCompleted.QuestID;
				iAQuestObjectiveCompleted2.QOID = iAQuestObjectiveCompleted.QOID;
				iAQuestObjectiveCompleted2.Not = iAQuestObjectiveCompleted.Not;
				list.Add(iAQuestObjectiveCompleted2);
			}
			else if (deserializedRequirement is IAQuestObjectiveRequired iAQuestObjectiveRequired)
			{
				IAQuestObjectiveRequired iAQuestObjectiveRequired2 = base.gameObject.AddComponent<IAQuestObjectiveRequired>();
				iAQuestObjectiveRequired2.QuestID = iAQuestObjectiveRequired.QuestID;
				iAQuestObjectiveRequired2.QOID = iAQuestObjectiveRequired.QOID;
				iAQuestObjectiveRequired2.Not = iAQuestObjectiveRequired.Not;
				list.Add(iAQuestObjectiveRequired2);
			}
			else if (deserializedRequirement is IAQuestRequired iAQuestRequired)
			{
				IAQuestRequired iAQuestRequired2 = base.gameObject.AddComponent<IAQuestRequired>();
				iAQuestRequired2.QuestID = iAQuestRequired.QuestID;
				iAQuestRequired2.Not = iAQuestRequired.Not;
				list.Add(iAQuestRequired2);
			}
			else if (deserializedRequirement is IAQuestStringRequired iAQuestStringRequired)
			{
				IAQuestStringRequired iAQuestStringRequired2 = base.gameObject.AddComponent<IAQuestStringRequired>();
				iAQuestStringRequired2.QSIndex = iAQuestStringRequired.QSIndex;
				iAQuestStringRequired2.QSValue = iAQuestStringRequired.QSValue;
				iAQuestStringRequired2.Comparison = iAQuestStringRequired.Comparison;
				list.Add(iAQuestStringRequired2);
			}
			else if (deserializedRequirement is IASessionDataRequired iASessionDataRequired)
			{
				IASessionDataRequired iASessionDataRequired2 = base.gameObject.AddComponent<IASessionDataRequired>();
				iASessionDataRequired2.Key = iASessionDataRequired.Key;
				iASessionDataRequired2.Value = iASessionDataRequired.Value;
				list.Add(iASessionDataRequired2);
			}
			else if (deserializedRequirement is IATargetScoreRequired iATargetScoreRequired)
			{
				IATargetScoreRequired iATargetScoreRequired2 = base.gameObject.AddComponent<IATargetScoreRequired>();
				iATargetScoreRequired2.teamID = iATargetScoreRequired.teamID;
				iATargetScoreRequired2.targetScore = iATargetScoreRequired.targetScore;
				list.Add(iATargetScoreRequired2);
			}
			else if (deserializedRequirement is IATradeSkillLevelRequired iATradeSkillLevelRequired)
			{
				IATradeSkillLevelRequired iATradeSkillLevelRequired2 = base.gameObject.AddComponent<IATradeSkillLevelRequired>();
				iATradeSkillLevelRequired2.level = iATradeSkillLevelRequired.level;
				iATradeSkillLevelRequired2.tradeSkillType = iATradeSkillLevelRequired.tradeSkillType;
				iATradeSkillLevelRequired2.comparison = iATradeSkillLevelRequired.comparison;
				list.Add(iATradeSkillLevelRequired2);
			}
			else if (deserializedRequirement is IAWarProgressRequired iAWarProgressRequired)
			{
				IAWarProgressRequired iAWarProgressRequired2 = base.gameObject.AddComponent<IAWarProgressRequired>();
				iAWarProgressRequired2.WarID = iAWarProgressRequired.WarID;
				iAWarProgressRequired2.WarProgress = iAWarProgressRequired.WarProgress;
				iAWarProgressRequired2.Comparison = iAWarProgressRequired.Comparison;
				list.Add(iAWarProgressRequired2);
			}
			else if (deserializedRequirement is IAMachineStateRequired iAMachineStateRequired)
			{
				IAMachineStateRequired iAMachineStateRequired2 = base.gameObject.AddComponent<IAMachineStateRequired>();
				iAMachineStateRequired2.State = iAMachineStateRequired.State;
				iAMachineStateRequired2.MachineID = iAMachineStateRequired.MachineID;
				list.Add(iAMachineStateRequired2);
			}
		}
		return list;
	}

	private void SendDBErrorMessage()
	{
		if (AccessLevels.CanReceiveErrorMessages(Entities.Instance.me))
		{
			Chat.SendAdminMessage("NPC Spawn ID does not exist for the requirement on Database Machine " + databaseID + ". Please fix in admin");
		}
	}
}
