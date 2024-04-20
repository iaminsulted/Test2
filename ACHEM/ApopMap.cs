using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StatCurves;
using UnityEngine;

public static class ApopMap
{
	private class EType
	{
		public const int NPCIAAction = 1;

		public const int NPCIATalk = 2;
	}

	private static Dictionary<int, NPCIA> npcias = new Dictionary<int, NPCIA>();

	private static Dictionary<int, List<NPCIA>> waitingForParent = new Dictionary<int, List<NPCIA>>();

	public static event Action Cleared;

	public static IEnumerable<NPCIA> GetAllApops()
	{
		return from kvp in npcias
			where kvp.Value != null
			select kvp.Value;
	}

	public static NPCIA GetApop(int id)
	{
		if (npcias.ContainsKey(id))
		{
			if (npcias[id] == null)
			{
				npcias.Remove(id);
				return null;
			}
			return npcias[id];
		}
		return null;
	}

	public static void AddApop(int id, int parentID, NPCIA npcia)
	{
		if (npcias.ContainsKey(id))
		{
			return;
		}
		if (waitingForParent.TryGetValue(id, out var value))
		{
			npcia.children.AddRange(value);
			waitingForParent.Remove(id);
		}
		if (parentID >= 0)
		{
			NPCIA apop = GetApop(parentID);
			if (apop == null)
			{
				if (waitingForParent.TryGetValue(parentID, out var value2))
				{
					if (!value2.Contains(npcia))
					{
						value2.Add(npcia);
					}
				}
				else
				{
					List<NPCIA> list = new List<NPCIA>();
					list.Add(npcia);
					waitingForParent.Add(parentID, list);
				}
			}
			else
			{
				apop.children.Add(npcia);
			}
		}
		npcias.Add(id, npcia);
	}

	public static void ClearApops(bool destroyNews = false)
	{
		npcias.Clear();
		waitingForParent.Clear();
		ApopMap.Cleared?.Invoke();
	}

	public static List<NPCIA> BuildApops(List<ApopData> datas)
	{
		List<NPCIA> list = new List<NPCIA>();
		if (datas.Count < 1)
		{
			return list;
		}
		foreach (ApopData data in datas)
		{
			if (GetApop(data.ID) != null)
			{
				continue;
			}
			if (data.Type == 2)
			{
				NPCIATalk nPCIATalk = new NPCIATalk();
				nPCIATalk.Title = data.NpcName;
				nPCIATalk.Subtitle = data.NpcTitle;
				nPCIATalk.SortingOrder = data.SortingOrder;
				nPCIATalk.icon = data.Icon;
				nPCIATalk.Label = data.Label;
				nPCIATalk.Text = data.Action;
				nPCIATalk.IsAutoTrigger = data.IsAutoTrigger;
				nPCIATalk.ImageUrl = data.ImageUrl;
				nPCIATalk.DontHideWhenLocked = data.DontHideWhenLocked;
				nPCIATalk.RequirementsText = data.RequirementsText;
				nPCIATalk.ID = data.ID;
				nPCIATalk.sagaID = data.sagaID;
				nPCIATalk.ImageTitle = data.ImageTitle;
				nPCIATalk.ImageDesc = data.ImageDescription;
				nPCIATalk.bStaff = data.bStaff;
				nPCIATalk.DateStart = data.DateStart;
				nPCIATalk.DateEnd = data.DateEnd;
				if (data.ParentID < 0 && ShowApop(data))
				{
					list.Add(nPCIATalk);
				}
				AddRequirements(nPCIATalk, data.Requirements);
				AddApop(data.ID, data.ParentID, nPCIATalk);
			}
			else if (data.Type == 1)
			{
				NPCIAAction nPCIAAction = new NPCIAAction();
				nPCIAAction.Title = data.NpcName;
				nPCIAAction.Subtitle = data.NpcTitle;
				nPCIAAction.SortingOrder = data.SortingOrder;
				nPCIAAction.icon = data.Icon;
				nPCIAAction.Label = data.Label;
				nPCIAAction.IsAutoTrigger = data.IsAutoTrigger;
				nPCIAAction.ImageUrl = data.ImageUrl;
				nPCIAAction.DontHideWhenLocked = data.DontHideWhenLocked;
				nPCIAAction.RequirementsText = data.RequirementsText;
				nPCIAAction.sagaID = data.sagaID;
				nPCIAAction.ImageTitle = data.ImageTitle;
				nPCIAAction.ImageDesc = data.ImageDescription;
				nPCIAAction.ID = data.ID;
				nPCIAAction.bStaff = data.bStaff;
				if (data.ParentID < 0 && ShowApop(data))
				{
					list.Add(nPCIAAction);
				}
				AddRequirements(nPCIAAction, data.Requirements);
				AddAction(nPCIAAction, data.Action);
				AddApop(data.ID, data.ParentID, nPCIAAction);
			}
		}
		foreach (NPCIA item in list)
		{
			SortChildrenRecursive(item);
			item.InitializeSelfAndChildren();
		}
		foreach (KeyValuePair<int, List<NPCIA>> item2 in waitingForParent)
		{
			foreach (NPCIA item3 in item2.Value)
			{
				list.Add(item3);
				SortChildrenRecursive(item3);
				item3.InitializeSelfAndChildren();
			}
		}
		return list;
	}

	private static bool ShowApop(ApopData data)
	{
		if (data.bStaff)
		{
			if (data.bStaff && Session.MyPlayerData.AccessLevel >= 100)
			{
				return Main.Environment == Environment.Content;
			}
			return false;
		}
		return true;
	}

	private static void SortChildrenRecursive(NPCIA rootApops)
	{
		rootApops.children = rootApops.children.OrderBy((NPCIA x) => x.SortingOrder).ToList();
		foreach (NPCIA child in rootApops.children)
		{
			SortChildrenRecursive(child);
		}
	}

	private static void AddRequirements(NPCIA npcia, string requirements)
	{
		requirements = Regex.Replace(requirements, "\\s+", "");
		if (string.IsNullOrEmpty(requirements))
		{
			return;
		}
		string[] array = requirements.Split('|');
		foreach (string text in array)
		{
			try
			{
				string[] array2 = text.Split('/');
				switch (array2[0])
				{
				case "IASpawnPadRequired":
				{
					IASpawnPadRequiredCore iASpawnPadRequiredCore = new IASpawnPadRequiredCore();
					int.TryParse(array2[1], out iASpawnPadRequiredCore.SpawnPadID);
					npcia.Requirements.Add(iASpawnPadRequiredCore);
					break;
				}
				case "IANPCRequired":
				{
					IANPCRequiredCore iANPCRequiredCore = new IANPCRequiredCore();
					int.TryParse(array2[1], out iANPCRequiredCore.NpcID);
					npcia.Requirements.Add(iANPCRequiredCore);
					break;
				}
				case "IABitFlagRequired":
				{
					IABitFlagRequiredCore iABitFlagRequiredCore = new IABitFlagRequiredCore();
					iABitFlagRequiredCore.BitFlagName = array2[1];
					byte.TryParse(array2[2], out iABitFlagRequiredCore.BitFlagIndex);
					npcia.Requirements.Add(iABitFlagRequiredCore);
					break;
				}
				case "IABitFlagValueRequired":
				{
					IABitFlagValueRequiredCore iABitFlagValueRequiredCore = new IABitFlagValueRequiredCore();
					iABitFlagValueRequiredCore.BitFlagName = array2[1];
					byte.TryParse(array2[2], out iABitFlagValueRequiredCore.BitFlagIndex);
					int.TryParse(array2[3], out var result4);
					iABitFlagValueRequiredCore.Value = result4 != 0;
					npcia.Requirements.Add(iABitFlagValueRequiredCore);
					break;
				}
				case "IAClassEquippedRequired":
				{
					IAClassEquippedRequiredCore iAClassEquippedRequiredCore = new IAClassEquippedRequiredCore();
					int.TryParse(array2[1], out iAClassEquippedRequiredCore.ClassID);
					bool.TryParse(array2[2], out iAClassEquippedRequiredCore.Not);
					npcia.Requirements.Add(iAClassEquippedRequiredCore);
					break;
				}
				case "IAItemEquippedRequired":
				{
					IAItemEquippedRequiredCore iAItemEquippedRequiredCore = new IAItemEquippedRequiredCore();
					int.TryParse(array2[1], out iAItemEquippedRequiredCore.ItemID);
					bool.TryParse(array2[2], out iAItemEquippedRequiredCore.Not);
					npcia.Requirements.Add(iAItemEquippedRequiredCore);
					break;
				}
				case "IAItemRequired":
				{
					IAItemRequiredCore iAItemRequiredCore = new IAItemRequiredCore();
					int.TryParse(array2[1], out iAItemRequiredCore.ItemID);
					int.TryParse(array2[2], out iAItemRequiredCore.Quantity);
					iAItemRequiredCore.Comparison = Util.GetComparison(array2[3]);
					npcia.Requirements.Add(iAItemRequiredCore);
					break;
				}
				case "IALevelRequired":
				{
					IALevelRequiredCore iALevelRequiredCore = new IALevelRequiredCore();
					int.TryParse(array2[1], out iALevelRequiredCore.Level);
					iALevelRequiredCore.Comparison = Util.GetComparison(array2[2]);
					npcia.Requirements.Add(iALevelRequiredCore);
					break;
				}
				case "IAMachineStateRequired":
				{
					BaseMachine[] array5 = UnityEngine.Object.FindObjectsOfType<BaseMachine>();
					int.TryParse(array2[1], out var result3);
					IAMachineStateRequiredCore iAMachineStateRequiredCore = null;
					BaseMachine[] array6 = array5;
					foreach (BaseMachine baseMachine in array6)
					{
						if (baseMachine.ID == result3)
						{
							iAMachineStateRequiredCore = new IAMachineStateRequiredCore(baseMachine);
							break;
						}
					}
					if (iAMachineStateRequiredCore.Machine == null)
					{
						Chat.Notify("Error: Machine with ID " + result3 + " was not found in the map - Fix in Admin for NPCIA ID " + npcia.ID);
						return;
					}
					byte.TryParse(array2[2], out iAMachineStateRequiredCore.State);
					npcia.Requirements.Add(iAMachineStateRequiredCore);
					break;
				}
				case "IAMapRequired":
				{
					IAMapRequiredCore iAMapRequiredCore = new IAMapRequiredCore();
					int.TryParse(array2[1], out iAMapRequiredCore.MapID);
					npcia.Requirements.Add(iAMapRequiredCore);
					break;
				}
				case "IANPCStateRequired":
				{
					IANPCStateRequiredCore iANPCStateRequiredCore = null;
					NPCSpawn[] array3 = UnityEngine.Object.FindObjectsOfType<NPCSpawn>();
					int.TryParse(array2[1], out var result2);
					NPCSpawn[] array4 = array3;
					foreach (NPCSpawn nPCSpawn in array4)
					{
						if (nPCSpawn.ID == result2)
						{
							iANPCStateRequiredCore = new IANPCStateRequiredCore(nPCSpawn);
							break;
						}
					}
					byte.TryParse(array2[2], out iANPCStateRequiredCore.State);
					npcia.Requirements.Add(iANPCStateRequiredCore);
					break;
				}
				case "IAQuestCompleted":
				{
					IAQuestCompletedCore iAQuestCompletedCore = new IAQuestCompletedCore();
					int.TryParse(array2[1], out iAQuestCompletedCore.QuestID);
					bool.TryParse(array2[2], out iAQuestCompletedCore.Not);
					npcia.Requirements.Add(iAQuestCompletedCore);
					break;
				}
				case "IAQuestObjectiveCompleted":
				{
					IAQuestObjectiveCompletedCore iAQuestObjectiveCompletedCore = new IAQuestObjectiveCompletedCore();
					int.TryParse(array2[1], out iAQuestObjectiveCompletedCore.QuestID);
					int.TryParse(array2[2], out iAQuestObjectiveCompletedCore.QOID);
					bool.TryParse(array2[3], out iAQuestObjectiveCompletedCore.Not);
					npcia.Requirements.Add(iAQuestObjectiveCompletedCore);
					break;
				}
				case "IAQuestObjectiveRequired":
				{
					IAQuestObjectiveRequiredCore iAQuestObjectiveRequiredCore = new IAQuestObjectiveRequiredCore();
					int.TryParse(array2[1], out iAQuestObjectiveRequiredCore.QuestID);
					int.TryParse(array2[2], out iAQuestObjectiveRequiredCore.QOID);
					bool.TryParse(array2[3], out iAQuestObjectiveRequiredCore.Not);
					npcia.Requirements.Add(iAQuestObjectiveRequiredCore);
					break;
				}
				case "IAQuestRequired":
				{
					IAQuestRequiredCore iAQuestRequiredCore = new IAQuestRequiredCore();
					int.TryParse(array2[1], out iAQuestRequiredCore.QuestID);
					bool.TryParse(array2[2], out iAQuestRequiredCore.Not);
					npcia.Requirements.Add(iAQuestRequiredCore);
					break;
				}
				case "IAQuestStringRequired":
				{
					IAQuestStringRequiredCore iAQuestStringRequiredCore = new IAQuestStringRequiredCore();
					int.TryParse(array2[1], out iAQuestStringRequiredCore.QSIndex);
					int.TryParse(array2[2], out iAQuestStringRequiredCore.QSValue);
					iAQuestStringRequiredCore.Comparison = Util.GetComparison(array2[3]);
					npcia.Requirements.Add(iAQuestStringRequiredCore);
					break;
				}
				case "IATradeSkillLevelRequired":
				{
					IATradeSkillLevelRequiredCore iATradeSkillLevelRequiredCore = new IATradeSkillLevelRequiredCore();
					int.TryParse(array2[1], out iATradeSkillLevelRequiredCore.level);
					int result = 0;
					int.TryParse(array2[2], out result);
					iATradeSkillLevelRequiredCore.tradeSkillType = (TradeSkillType)result;
					iATradeSkillLevelRequiredCore.comparison = Util.GetComparison(array2[3]);
					npcia.Requirements.Add(iATradeSkillLevelRequiredCore);
					break;
				}
				case "IAUserRequired":
				{
					IAUserRequiredCore iAUserRequiredCore = new IAUserRequiredCore();
					int.TryParse(array2[1], out iAUserRequiredCore.UserID);
					npcia.Requirements.Add(iAUserRequiredCore);
					break;
				}
				case "IAWarProgressRequired":
				{
					IAWarProgressRequiredCore iAWarProgressRequiredCore = new IAWarProgressRequiredCore();
					int.TryParse(array2[1], out iAWarProgressRequiredCore.WarID);
					float.TryParse(array2[2], out iAWarProgressRequiredCore.WarProgress);
					iAWarProgressRequiredCore.Comparison = Util.GetComparison(array2[3]);
					npcia.Requirements.Add(iAWarProgressRequiredCore);
					break;
				}
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}
	}

	private static void AddAction(NPCIAAction npcia, string action)
	{
		if (action != null && action.Split('/')[0] != "CTAChatCommand")
		{
			action = Regex.Replace(action, "\\s+", "");
		}
		if (string.IsNullOrEmpty(action))
		{
			return;
		}
		string[] array = action.Split('/');
		try
		{
			switch (array[0])
			{
			case "CTAOpenApop":
			{
				CTAOpenApopCore cTAOpenApopCore = new CTAOpenApopCore();
				int.TryParse(array[1], out cTAOpenApopCore.ApopID);
				npcia.Actions.Add(cTAOpenApopCore);
				npcia.Action = cTAOpenApopCore;
				break;
			}
			case "CTAChatCommand":
			{
				CTAChatCommandCore cTAChatCommandCore = new CTAChatCommandCore();
				cTAChatCommandCore.CommandText = array[1];
				npcia.Actions.Add(cTAChatCommandCore);
				npcia.Action = cTAChatCommandCore;
				break;
			}
			case "CTACinematic":
			{
				CTACinematicCore cTACinematicCore = new CTACinematicCore();
				cTACinematicCore.Name = array[1];
				npcia.Actions.Add(cTACinematicCore);
				npcia.Action = cTACinematicCore;
				break;
			}
			case "CTACraftShop":
			{
				CTACraftShopCore cTACraftShopCore = new CTACraftShopCore();
				int.TryParse(array[1], out cTACraftShopCore.ID);
				npcia.Actions.Add(cTACraftShopCore);
				npcia.Action = cTACraftShopCore;
				break;
			}
			case "CTADialogue":
			{
				CTADialogueCore cTADialogueCore = new CTADialogueCore();
				int.TryParse(array[1], out cTADialogueCore.ID);
				if (array.Length > 2)
				{
					bool.TryParse(array[2], out cTADialogueCore.SkipCompleteAction);
				}
				npcia.Actions.Add(cTADialogueCore);
				npcia.Action = cTADialogueCore;
				break;
			}
			case "CTADungeonLoad":
			{
				CTADungeonLoadCore cTADungeonLoadCore = new CTADungeonLoadCore();
				string[] array4 = array[1].Split(',');
				for (int k = 0; k < array4.Length; k++)
				{
					int.TryParse(array4[k], out var result3);
					cTADungeonLoadCore.DungeonIDs.Add(result3);
				}
				npcia.Actions.Add(cTADungeonLoadCore);
				npcia.Action = cTADungeonLoadCore;
				break;
			}
			case "CTAGender":
			{
				CTAGenderCore cTAGenderCore = new CTAGenderCore();
				npcia.Actions.Add(cTAGenderCore);
				npcia.Action = cTAGenderCore;
				break;
			}
			case "CTAIAPStore":
			{
				CTAIAPStoreCore cTAIAPStoreCore = new CTAIAPStoreCore();
				npcia.Actions.Add(cTAIAPStoreCore);
				npcia.Action = cTAIAPStoreCore;
				break;
			}
			case "CTALoadClassUI":
			{
				CTALoadClassUICore cTALoadClassUICore = new CTALoadClassUICore();
				int.TryParse(array[1], out cTALoadClassUICore.ClassID);
				npcia.Actions.Add(cTALoadClassUICore);
				npcia.Action = cTALoadClassUICore;
				break;
			}
			case "CTAPlayTutorial":
			{
				CTAPlayTutorialCore cTAPlayTutorialCore = new CTAPlayTutorialCore();
				int.TryParse(array[1], out cTAPlayTutorialCore.TutorialID);
				npcia.Actions.Add(cTAPlayTutorialCore);
				npcia.Action = cTAPlayTutorialCore;
				break;
			}
			case "CTAQuestLoad":
			{
				CTAQuestLoadCore cTAQuestLoadCore = new CTAQuestLoadCore();
				string[] array5 = array[1].Split(',');
				for (int l = 0; l < array5.Length; l++)
				{
					if (int.TryParse(array5[l], out var result4))
					{
						cTAQuestLoadCore.AcceptQuestIDs.Add(result4);
					}
				}
				if (array.Length > 2)
				{
					string[] array6 = array[2].Split(',');
					for (int m = 0; m < array6.Length; m++)
					{
						if (int.TryParse(array6[m], out var result5))
						{
							cTAQuestLoadCore.TurnInQuestIDs.Add(result5);
						}
					}
				}
				npcia.Actions.Add(cTAQuestLoadCore);
				npcia.Action = cTAQuestLoadCore;
				break;
			}
			case "CTARandomDialogue":
			{
				CTARandomDialogueCore cTARandomDialogueCore = new CTARandomDialogueCore();
				string[] array3 = array[1].Split(',');
				for (int j = 0; j < array3.Length; j++)
				{
					int.TryParse(array3[j], out var result2);
					cTARandomDialogueCore.IDs.Add(result2);
				}
				bool.TryParse(array[2], out cTARandomDialogueCore.SkipCompleteAction);
				npcia.Actions.Add(cTARandomDialogueCore);
				npcia.Action = cTARandomDialogueCore;
				break;
			}
			case "CTAShop":
			{
				CTAShopCore cTAShopCore = new CTAShopCore();
				int.TryParse(array[1], out cTAShopCore.ID);
				npcia.Actions.Add(cTAShopCore);
				npcia.Action = cTAShopCore;
				break;
			}
			case "CTADailyChest":
			{
				CTADailyChestCore cTADailyChestCore = new CTADailyChestCore();
				npcia.Actions.Add(cTADailyChestCore);
				npcia.Action = cTADailyChestCore;
				break;
			}
			case "CTASetBitFlag":
			{
				CTASetBitFlagCore cTASetBitFlagCore = new CTASetBitFlagCore();
				cTASetBitFlagCore.BitFlagName = array[1];
				byte.TryParse(array[2], out cTASetBitFlagCore.BitFlagIndex);
				npcia.Actions.Add(cTASetBitFlagCore);
				npcia.Action = cTASetBitFlagCore;
				break;
			}
			case "CTAInfusion":
			{
				CTAInfusionCore cTAInfusionCore = new CTAInfusionCore();
				npcia.Actions.Add(cTAInfusionCore);
				npcia.Action = cTAInfusionCore;
				break;
			}
			case "CTAAugmentReroll":
			{
				CTAAugmentRerollCore cTAAugmentRerollCore = new CTAAugmentRerollCore();
				npcia.Actions.Add(cTAAugmentRerollCore);
				npcia.Action = cTAAugmentRerollCore;
				break;
			}
			case "CTALeaderboard":
			{
				CTALeaderboardCore cTALeaderboardCore = new CTALeaderboardCore();
				int.TryParse(array[1], out cTALeaderboardCore.leaderboardType);
				npcia.Actions.Add(cTALeaderboardCore);
				npcia.Action = cTALeaderboardCore;
				break;
			}
			case "CTATransferMap":
			{
				CTATransferMapCore cTATransferMapCore = new CTATransferMapCore();
				int.TryParse(array[1], out cTATransferMapCore.MapID);
				int.TryParse(array[2], out cTATransferMapCore.CellID);
				int.TryParse(array[3], out cTATransferMapCore.SpawnID);
				cTATransferMapCore.UpdateTransferPadMapID();
				npcia.Actions.Add(cTATransferMapCore);
				npcia.Action = cTATransferMapCore;
				break;
			}
			case "CTATransferRandom":
			{
				CTATransferRandomCore cTATransferRandomCore = new CTATransferRandomCore();
				string[] array2 = array[1].Split(',');
				for (int i = 0; i < array2.Length; i++)
				{
					int.TryParse(array2[i], out var result);
					cTATransferRandomCore.MapIDs.Add(result);
				}
				npcia.Actions.Add(cTATransferRandomCore);
				npcia.Action = cTATransferRandomCore;
				break;
			}
			case "CTATriggerMachineByID":
			{
				CTATriggerMachineByIDCore cTATriggerMachineByIDCore = new CTATriggerMachineByIDCore();
				int.TryParse(array[1], out cTATriggerMachineByIDCore.machineID);
				npcia.Actions.Add(cTATriggerMachineByIDCore);
				npcia.Action = cTATriggerMachineByIDCore;
				break;
			}
			case "CTAVideo":
			{
				CTAVideoCore cTAVideoCore = new CTAVideoCore();
				cTAVideoCore.URL_Youtube = array[1];
				cTAVideoCore.URL = array[2];
				npcia.Actions.Add(cTAVideoCore);
				npcia.Action = cTAVideoCore;
				break;
			}
			case "CTAEnterQueue":
			{
				CTAEnterQueueCore cTAEnterQueueCore = new CTAEnterQueueCore();
				int.TryParse(array[1], out cTAEnterQueueCore.queueID);
				npcia.Actions.Add(cTAEnterQueueCore);
				npcia.Action = cTAEnterQueueCore;
				break;
			}
			case "CTAPrivateQueue":
			{
				CTAPrivateQueueCore cTAPrivateQueueCore = new CTAPrivateQueueCore();
				int.TryParse(array[1], out cTAPrivateQueueCore.queueID);
				npcia.Actions.Add(cTAPrivateQueueCore);
				npcia.Action = cTAPrivateQueueCore;
				break;
			}
			case "CTALeaveQueue":
			{
				CTALeaveQueueCore cTALeaveQueueCore = new CTALeaveQueueCore();
				npcia.Actions.Add(cTALeaveQueueCore);
				npcia.Action = cTALeaveQueueCore;
				break;
			}
			case "CTAExternalLink":
			{
				CTAExternalLinkCore cTAExternalLinkCore = new CTAExternalLinkCore();
				cTAExternalLinkCore.Url = string.Join("/", array.Skip(1).ToArray());
				npcia.Actions.Add(cTAExternalLinkCore);
				npcia.Action = cTAExternalLinkCore;
				break;
			}
			case "CTABank":
			{
				CTABankCore cTABankCore = new CTABankCore();
				npcia.Actions.Add(cTABankCore);
				npcia.Action = cTABankCore;
				break;
			}
			}
		}
		catch (Exception message)
		{
			Debug.LogError(message);
		}
	}
}
