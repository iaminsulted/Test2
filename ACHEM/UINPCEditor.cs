using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCurves;
using UnityEngine;
using UnityEngine.Networking;

public class UINPCEditor : UIWindow
{
	public static int LastTabState = -1;

	private ComNPCMeta selectedNPC;

	private ComSpawnMeta selectedMeta;

	private UINPCItem selectedNPCItem;

	public int recentSpawnID;

	public int recentNpcID;

	private UINPCPathItem selectedPath;

	public GameObject SpawnTemplate;

	public GameObject npcListPrefab;

	private ObjectPool<GameObject> spawnPool;

	private List<GameObject> spawnCats;

	public GameObject ScrollView;

	public UITable SpawnTable;

	public UILabel ListSizeLabel;

	public UIInput SearchInput;

	private string searchText;

	public GameObject LeftPanel;

	public UILabel MonsterName;

	public UILabel PadIdLabel;

	public GameObject DetailsTab;

	public GameObject EditTab;

	public GameObject PathsTab;

	public GameObject ReqsTab;

	public GameObject CloseBtn;

	public GameObject Paths;

	public GameObject Details;

	public GameObject Edit;

	public GameObject AddNPC;

	public GameObject Requirements;

	public GameObject ExistingRequirements;

	public GameObject RequirementAdder;

	public UILabel D_Position;

	public GameObject D_Container;

	public GameObject D_UpdatePos;

	public UILabel D_UpdateLabel;

	public UILabel D_Signify;

	public GameObject D_ApopAdmin;

	public PreviewGenerator preview;

	public Camera previewCam;

	public GameObject modelShadow;

	public float initShadowPosX;

	public float initShadowPosY;

	public GameObject editListPrefab;

	private List<UINPCEditItem> editList;

	private ObjectPool<GameObject> editPool;

	private Dictionary<string, int> editItemProps = new Dictionary<string, int>
	{
		{ "Spawn ID", 0 },
		{ "NPC ID", 0 },
		{ "Name", 0 },
		{ "Apop ID", 0 },
		{ "Level", 0 },
		{ "Spawn Rate", 0 },
		{ "Respawn Time", 0 },
		{ "Despawn Time", 0 },
		{ "Aggro Radius", 0 },
		{ "Leash Radius", 0 },
		{ "Chain Radius", 0 },
		{ "Animations", 3 },
		{ "Min Time", 0 },
		{ "Max Time", 0 },
		{ "TeamID", 0 },
		{ "Animation Overrides", 3 },
		{ "Allow Health Regeneration", 1 },
		{ "AutoChain", 1 },
		{ "Use Rotation", 1 },
		{ "Sequential Animations", 1 },
		{ "Speed", 2 },
		{ "Move Override", 2 },
		{ "Reaction Override", 2 },
		{ "Position X", 0 },
		{ "Position Y", 0 },
		{ "Position Z", 0 },
		{ "Rotation Y", 0 },
		{ "Auto Spawn", 1 }
	};

	public GameObject P_TrashBtn;

	public UILabel P_SelectedLabel;

	public UIInput P_XPos;

	public UIInput P_YPos;

	public UIInput P_ZPos;

	public UIInput P_RotY;

	public UILabel P_UpdatePath;

	public GameObject pathListPrefab;

	private List<UINPCPathItem> pathList;

	private ObjectPool<GameObject> pathPool;

	public UIGrid PathsGrid;

	public UIInput A_SpawnID;

	public UIInput A_NpcID;

	public UIInput A_Name;

	public UIInput A_Level;

	public UIPopupList A_Reaction;

	public UIPopupList ExistingRequirementsDropdown;

	public GameObject ExistingRequirementsItemPrefab;

	public UITable ExistingRequirementsTable;

	public UILabel ExistingRequirementCountLabel;

	private List<UINPCEditItem> ExistingRequirmentsEditItemList;

	private ObjectPool<GameObject> ExistingRequirmentsObjectPool;

	public UIPopupList RequirementAdderDropdown;

	public GameObject RequirementAdderItemsPrefab;

	public UITable RequirementAdderTable;

	private List<UINPCEditItem> RequirementAdderEditItemList;

	private ObjectPool<GameObject> RequirementAdderObjectPool;

	private int latestSpawnId = -1;

	private int lastSpawnID = -1;

	private bool showPath;

	private string selectedExistingRequirementClassName = "";

	private int selectedExistingRequiermentIndex = int.MaxValue;

	private bool DetailsNeedSetup;

	private bool wasResponseUpdate;

	private int lastNPC = -1;

	private int lastTab = -1;

	public static UINPCEditor Instance { get; private set; }

	public static void Load()
	{
		if (Instance == null)
		{
			UIWindow.ClearWindows();
			Instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/NPCEditor"), UIManager.Instance.transform).GetComponent<UINPCEditor>();
			Instance.Init();
		}
		Instance.Refresh();
	}

	public static void Toggle()
	{
		if (Instance == null)
		{
			Load();
		}
		else
		{
			Instance.Close();
		}
	}

	public void GotoMapEditor()
	{
		Close();
		UIMapEditor.Toggle();
	}

	public static void DeleteSpawner(int SpawnID)
	{
		NPCSpawn.SwapToActive(SpawnID);
		List<GameObject> list = new List<GameObject>();
		foreach (KeyValuePair<GameObject, int> npcPathNode in Game.Instance.AreaData.npcPathNodes)
		{
			GameObject key = npcPathNode.Key;
			if (npcPathNode.Value == SpawnID)
			{
				list.Add(key);
				UnityEngine.Object.Destroy(key);
			}
		}
		foreach (GameObject item in list)
		{
			Game.Instance.AreaData.npcPathNodes.Remove(item);
		}
	}

	public static void DeletePathNode(int PathID)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (KeyValuePair<GameObject, int> npcPathNode in Game.Instance.AreaData.npcPathNodes)
		{
			GameObject key = npcPathNode.Key;
			if (key.GetComponent<SpawnEditorPathNode>().PathID == PathID)
			{
				list.Add(key);
				UnityEngine.Object.Destroy(key);
			}
		}
		foreach (GameObject item in list)
		{
			Game.Instance.AreaData.npcPathNodes.Remove(item);
		}
	}

	public static void CreateRequirements(int SpawnID, string Requirements)
	{
		if (string.IsNullOrEmpty(Requirements))
		{
			return;
		}
		NPCSpawn nPCSpawn = NPCSpawn.Map[SpawnID];
		foreach (JToken item in JArray.Parse(Requirements))
		{
			switch ((string?)item["$type"])
			{
			case "IABitFlagRequired":
			{
				IABitFlagRequired iABitFlagRequired = nPCSpawn.gameObject.AddComponent<IABitFlagRequired>();
				iABitFlagRequired.BitFlagName = (string?)item["BitFlagName"];
				byte.TryParse((string?)item["BitFlagIndex"], out iABitFlagRequired.BitFlagIndex);
				nPCSpawn.AddRequirement(iABitFlagRequired);
				break;
			}
			case "IAAllMonstersCleared":
			{
				IAAllMonstersCleared requirement = nPCSpawn.gameObject.AddComponent<IAAllMonstersCleared>();
				nPCSpawn.AddRequirement(requirement);
				break;
			}
			case "IABitFlagValueRequired":
			{
				IABitFlagValueRequired iABitFlagValueRequired = nPCSpawn.gameObject.AddComponent<IABitFlagValueRequired>();
				iABitFlagValueRequired.BitFlagName = (string?)item["BitFlagName"];
				byte.TryParse((string?)item["BitFlagIndex"], out iABitFlagValueRequired.BitFlagIndex);
				int.TryParse((string?)item["Value"], out var result9);
				iABitFlagValueRequired.Value = result9 != 0;
				nPCSpawn.AddRequirement(iABitFlagValueRequired);
				break;
			}
			case "IAClassEquippedRequired":
			{
				IAClassEquippedRequired iAClassEquippedRequired = nPCSpawn.gameObject.AddComponent<IAClassEquippedRequired>();
				int.TryParse((string?)item["ClassID"], out iAClassEquippedRequired.ClassID);
				bool.TryParse((string?)item["Not"], out iAClassEquippedRequired.Not);
				nPCSpawn.AddRequirement(iAClassEquippedRequired);
				break;
			}
			case "IAItemEquippedRequired":
			{
				IAItemEquippedRequired iAItemEquippedRequired = nPCSpawn.gameObject.AddComponent<IAItemEquippedRequired>();
				int.TryParse((string?)item["ItemID"], out iAItemEquippedRequired.ItemID);
				bool.TryParse((string?)item["Not"], out iAItemEquippedRequired.Not);
				nPCSpawn.AddRequirement(iAItemEquippedRequired);
				break;
			}
			case "IAItemRequired":
			{
				IAItemRequired iAItemRequired = nPCSpawn.gameObject.AddComponent<IAItemRequired>();
				int.TryParse((string?)item["ItemID"], out iAItemRequired.ItemID);
				int.TryParse((string?)item["Quantity"], out iAItemRequired.Quantity);
				iAItemRequired.Comparison = Util.GetComparison((string?)item["Comparison"]);
				nPCSpawn.AddRequirement(iAItemRequired);
				break;
			}
			case "IALevelRequired":
			{
				IALevelRequired iALevelRequired = nPCSpawn.gameObject.AddComponent<IALevelRequired>();
				int.TryParse((string?)item["Level"], out iALevelRequired.Level);
				iALevelRequired.Comparison = Util.GetComparison((string?)item["Comparison"]);
				nPCSpawn.AddRequirement(iALevelRequired);
				break;
			}
			case "IAMachineStateRequired":
			{
				IAMachineStateRequired iAMachineStateRequired = nPCSpawn.gameObject.AddComponent<IAMachineStateRequired>();
				byte.TryParse((string?)item["State"], out iAMachineStateRequired.State);
				iAMachineStateRequired.Not = int.Parse((string?)item["Not"]) > 0;
				BaseMachine[] array = UnityEngine.Object.FindObjectsOfType<BaseMachine>();
				int.TryParse((string?)item["MachineID"], out var result6);
				BaseMachine[] array2 = array;
				foreach (BaseMachine baseMachine in array2)
				{
					if (baseMachine.ID == result6)
					{
						iAMachineStateRequired.SetMachine(baseMachine);
						break;
					}
				}
				if (iAMachineStateRequired.Machine == null)
				{
					Chat.Notify("Error: Machine with ID " + result6 + " was not found in the map - Fix in Admin for npcSpawn ID " + nPCSpawn.ID);
					return;
				}
				nPCSpawn.AddRequirement(iAMachineStateRequired);
				break;
			}
			case "IAMapRequired":
			{
				IAMapRequired iAMapRequired = nPCSpawn.gameObject.AddComponent<IAMapRequired>();
				int.TryParse((string?)item["MapID"], out iAMapRequired.MapID);
				int result2 = 0;
				if (item["Not"] != null)
				{
					int.TryParse((string?)item["Not"], out result2);
				}
				iAMapRequired.Not = result2 > 0;
				nPCSpawn.AddRequirement(iAMapRequired);
				break;
			}
			case "IANPCStateRequired":
			{
				IANPCStateRequired iANPCStateRequired = nPCSpawn.gameObject.AddComponent<IANPCStateRequired>();
				NPCSpawn[] array3 = UnityEngine.Object.FindObjectsOfType<NPCSpawn>();
				int.TryParse((string?)item["SpawnID"], out var result7);
				int result8 = 0;
				if (item["Not"] != null)
				{
					int.TryParse((string?)item["Not"], out result8);
				}
				iANPCStateRequired.Not = result8 > 0;
				NPCSpawn[] array4 = array3;
				foreach (NPCSpawn nPCSpawn2 in array4)
				{
					if (nPCSpawn2.ID == result7)
					{
						iANPCStateRequired.SetSpawn(nPCSpawn2);
						break;
					}
				}
				byte.TryParse((string?)item["State"], out iANPCStateRequired.State);
				nPCSpawn.AddRequirement(iANPCStateRequired);
				break;
			}
			case "IAQuestCompleted":
			{
				IAQuestCompleted iAQuestCompleted = nPCSpawn.gameObject.AddComponent<IAQuestCompleted>();
				int.TryParse((string?)item["QuestID"], out iAQuestCompleted.QuestID);
				int result5 = 0;
				if (item["Not"] != null)
				{
					int.TryParse((string?)item["Not"], out result5);
				}
				iAQuestCompleted.Not = result5 > 0;
				nPCSpawn.AddRequirement(iAQuestCompleted);
				break;
			}
			case "IAQuestObjectiveCompleted":
			{
				IAQuestObjectiveCompleted iAQuestObjectiveCompleted = nPCSpawn.gameObject.AddComponent<IAQuestObjectiveCompleted>();
				int.TryParse((string?)item["QuestID"], out iAQuestObjectiveCompleted.QuestID);
				int.TryParse((string?)item["QOID"], out iAQuestObjectiveCompleted.QOID);
				int result4 = 0;
				if (item["Not"] != null)
				{
					int.TryParse((string?)item["Not"], out result4);
				}
				iAQuestObjectiveCompleted.Not = result4 > 0;
				nPCSpawn.AddRequirement(iAQuestObjectiveCompleted);
				break;
			}
			case "IAQuestObjectiveRequired":
			{
				IAQuestObjectiveRequired iAQuestObjectiveRequired = nPCSpawn.gameObject.AddComponent<IAQuestObjectiveRequired>();
				int.TryParse((string?)item["QuestID"], out iAQuestObjectiveRequired.QuestID);
				int.TryParse((string?)item["QOID"], out iAQuestObjectiveRequired.QOID);
				bool.TryParse((string?)item["Not"], out iAQuestObjectiveRequired.Not);
				nPCSpawn.AddRequirement(iAQuestObjectiveRequired);
				break;
			}
			case "IAQuestRequired":
			{
				IAQuestRequired iAQuestRequired = nPCSpawn.gameObject.AddComponent<IAQuestRequired>();
				int.TryParse((string?)item["QuestID"], out iAQuestRequired.QuestID);
				bool.TryParse((string?)item["Not"], out iAQuestRequired.Not);
				nPCSpawn.AddRequirement(iAQuestRequired);
				break;
			}
			case "IAQuestStringRequired":
			{
				IAQuestStringRequired iAQuestStringRequired = nPCSpawn.gameObject.AddComponent<IAQuestStringRequired>();
				int.TryParse((string?)item["QSIndex"], out iAQuestStringRequired.QSIndex);
				int.TryParse((string?)item["QSValue"], out iAQuestStringRequired.QSValue);
				iAQuestStringRequired.Comparison = Util.GetComparison((string?)item["Comparison"]);
				nPCSpawn.AddRequirement(iAQuestStringRequired);
				break;
			}
			case "IATradeSkillLevelRequired":
			{
				IATradeSkillLevelRequired iATradeSkillLevelRequired = nPCSpawn.gameObject.AddComponent<IATradeSkillLevelRequired>();
				int.TryParse((string?)item["Level"], out iATradeSkillLevelRequired.level);
				int result3 = 0;
				int.TryParse((string?)item["Type"], out result3);
				iATradeSkillLevelRequired.tradeSkillType = (TradeSkillType)result3;
				iATradeSkillLevelRequired.comparison = Util.GetComparison((string?)item["Comparison"]);
				nPCSpawn.AddRequirement(iATradeSkillLevelRequired);
				break;
			}
			case "IAUserRequired":
			{
				IAUserRequired iAUserRequired = nPCSpawn.gameObject.AddComponent<IAUserRequired>();
				int.TryParse((string?)item["UserID"], out iAUserRequired.UserID);
				int result = 0;
				if (item["Not"] != null)
				{
					int.TryParse((string?)item["Not"], out result);
				}
				iAUserRequired.Not = result > 0;
				nPCSpawn.AddRequirement(iAUserRequired);
				break;
			}
			case "IAWarProgressRequired":
			{
				IAWarProgressRequired iAWarProgressRequired = nPCSpawn.gameObject.AddComponent<IAWarProgressRequired>();
				int.TryParse((string?)item["WarID"], out iAWarProgressRequired.WarID);
				float.TryParse((string?)item["WarProgress"], out iAWarProgressRequired.WarProgress);
				iAWarProgressRequired.Comparison = Util.GetComparison((string?)item["Comparison"]);
				nPCSpawn.AddRequirement(iAWarProgressRequired);
				break;
			}
			}
		}
	}

	public static void AddNpcSpawnerPlatformMesh(GameObject parent)
	{
		UnityEngine.Object.Instantiate(Resources.Load("UIElements/NPCEditor/NPC_Spawner_mesh") as GameObject, parent.transform);
	}

	public static void CreateSpawner(int SpawnID, Vector3 Path, int RotationY, bool AutoSpawn)
	{
		NPCSpawn.SwapToInactive(SpawnID);
		GameObject obj = UnityEngine.Object.Instantiate(Resources.Load("UIElements/NPCEditor/NPC_Spawner") as GameObject, Game.Instance.AreaData.dbSpawns.transform, worldPositionStays: true);
		obj.layer = 20;
		NPCSpawn component = obj.GetComponent<NPCSpawn>();
		component.AutoSpawn = AutoSpawn;
		NPCSpawn.Map[SpawnID] = component;
		component.ID = SpawnID;
		component.TargetTransform.position = Path;
		component.TargetTransform.rotation = Quaternion.Euler(0f, RotationY, 0f);
	}

	public static void CreatePathNode(int SpawnID, int PathID, Vector3 Path)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UIElements/NPCEditor/NPC_Path_Node") as GameObject, Game.Instance.AreaData.dbSpawns.transform, worldPositionStays: true);
		gameObject.layer = 20;
		gameObject.transform.position = Path;
		gameObject.AddComponent<SpawnEditorPathNode>().PathID = PathID;
		TextMesh textMesh = new GameObject("ID Label").AddComponent<TextMesh>();
		textMesh.transform.SetParent(gameObject.transform);
		textMesh.text = "(" + SpawnID + "): " + PathID;
		textMesh.transform.localPosition = new Vector3(0f, 0f, 2f);
		textMesh.characterSize = 0.25f;
		textMesh.anchor = TextAnchor.MiddleCenter;
		textMesh.alignment = TextAlignment.Center;
		textMesh.fontStyle = FontStyle.Bold;
		textMesh.fontSize = 25;
		textMesh.gameObject.AddComponent<simpleBillboard>().Flip = true;
		textMesh.gameObject.layer = 20;
		MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
		meshCollider.convex = true;
		meshCollider.isTrigger = true;
		SpawnEditorPathNodes spawnEditorPathNodes = gameObject.AddComponent<SpawnEditorPathNodes>();
		spawnEditorPathNodes.SpawnID = SpawnID;
		spawnEditorPathNodes.MinDelay = -1f;
		spawnEditorPathNodes.MaxDelay = -1f;
		ClickMachine clickMachine = gameObject.AddComponent<ClickMachine>();
		clickMachine.Distance = 999f;
		clickMachine.CTActions.Add(spawnEditorPathNodes);
		Game.Instance.AreaData.npcPathNodes.Add(gameObject, SpawnID);
	}

	public static void LinkPaths(int SpawnID)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (KeyValuePair<GameObject, int> npcPathNode in Game.Instance.AreaData.npcPathNodes)
		{
			if (npcPathNode.Value == SpawnID)
			{
				list.Add(npcPathNode.Key);
			}
		}
		foreach (GameObject item in list)
		{
			if (item.GetComponent<LineRenderer>() != null)
			{
				UnityEngine.Object.Destroy(item.GetComponent<LineRenderer>());
			}
			item.AddComponent<LineRenderer>();
			LineRenderer component = item.GetComponent<LineRenderer>();
			component.material.SetColor("_Color", Color.blue);
			component.widthMultiplier = 0.2f;
			component.SetPosition(0, NPCSpawn.GetSpawn(SpawnID).TargetTransform.position);
			component.SetPosition(1, item.transform.position);
			int num = 2;
			foreach (GameObject item2 in list)
			{
				if (!(item2 == item))
				{
					component.positionCount++;
					component.SetPosition(num, item2.transform.position);
					num++;
				}
			}
		}
	}

	public void OpenSpawnID(int SpawnID)
	{
		lastSpawnID = SpawnID;
		ClearSearch();
	}

	public void OpenPath(int SpawnID)
	{
		lastSpawnID = SpawnID;
		showPath = true;
		ClearSearch();
	}

	public void HelloTest()
	{
		Chat.Notify("Hello!");
	}

	public void HideLeftPane()
	{
		LeftPanel.SetActive(value: false);
		Paths.SetActive(value: false);
		Details.SetActive(value: false);
		Edit.SetActive(value: false);
		AddNPC.SetActive(value: false);
		Requirements.SetActive(value: false);
		selectedNPC = null;
		selectedMeta = null;
		selectedPath = null;
	}

	private void ShowLeftPane(int tab)
	{
		LeftPanel.SetActive(value: true);
		Paths.SetActive(value: false);
		Details.SetActive(value: false);
		Edit.SetActive(value: false);
		AddNPC.SetActive(value: false);
		Requirements.SetActive(value: false);
		DetailsTab.SetActive(value: false);
		EditTab.SetActive(value: false);
		PathsTab.SetActive(value: false);
		ReqsTab.SetActive(value: false);
		switch (tab)
		{
		case 0:
			Details.SetActive(value: true);
			DetailsTab.SetActive(value: true);
			break;
		case 1:
			Edit.SetActive(value: true);
			EditTab.SetActive(value: true);
			break;
		case 2:
			Paths.SetActive(value: true);
			PathsTab.SetActive(value: true);
			break;
		case 3:
			LeftPanel.SetActive(value: false);
			AddNPC.SetActive(value: true);
			break;
		case 4:
			Requirements.SetActive(value: true);
			ExistingRequirements.SetActive(value: true);
			RequirementAdder.SetActive(value: false);
			ReqsTab.SetActive(value: true);
			break;
		}
	}

	public void showDetails()
	{
		ShowLeftPane(0);
		if (selectedMeta != null)
		{
			D_UpdatePos.SetActive(selectedMeta.IsDB);
			if (DetailsNeedSetup)
			{
				setupDetails();
				DetailsNeedSetup = false;
			}
		}
		LastTabState = 0;
	}

	public void showEdit()
	{
		if (!selectedMeta.IsDB)
		{
			return;
		}
		ShowLeftPane(1);
		EmptyEditList();
		if (selectedMeta != null)
		{
			foreach (KeyValuePair<string, int> editItemProp in editItemProps)
			{
				GameObject obj = editPool.Get();
				obj.transform.SetParent(editListPrefab.transform.parent, worldPositionStays: false);
				obj.SetActive(value: true);
				UINPCEditItem component = obj.GetComponent<UINPCEditItem>();
				List<string> dropOptions = new List<string>();
				float num = 0f;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				if (selectedMeta.Path.Count > 0)
				{
					num = selectedMeta.Path.First().Value.x;
					num2 = selectedMeta.Path.First().Value.y;
					num3 = selectedMeta.Path.First().Value.z;
				}
				if (selectedMeta.RotationY.Count > 0)
				{
					num4 = selectedMeta.RotationY.First().Value;
				}
				object value;
				switch (editItemProp.Key)
				{
				case "Spawn ID":
					value = selectedMeta.SpawnID.ToString();
					break;
				case "NPC ID":
					value = selectedNPC.NPCID.ToString();
					break;
				case "Name":
					value = (string.IsNullOrEmpty(selectedNPC.NameOverride) ? "" : selectedNPC.NameOverride.ToString());
					break;
				case "Apop ID":
					value = ((selectedNPC.ApopIDs != null) ? string.Join(",", selectedNPC.ApopIDs) : "");
					break;
				case "Level":
					value = selectedNPC.Level.ToString();
					break;
				case "Spawn Rate":
					value = selectedNPC.Rate.ToString();
					break;
				case "Respawn Time":
					value = selectedMeta.RespawnTime.ToString();
					break;
				case "Despawn Time":
					value = selectedMeta.DespawnTime.ToString();
					break;
				case "Aggro Radius":
					value = selectedMeta.AggroRadius.ToString();
					break;
				case "Leash Radius":
					value = selectedMeta.LeashRadius.ToString();
					break;
				case "Chain Radius":
					value = selectedMeta.ChainRadius.ToString();
					break;
				case "Animations":
					value = selectedMeta.Animations;
					break;
				case "Min Time":
					value = selectedMeta.MinTime.ToString();
					break;
				case "Max Time":
					value = selectedMeta.MaxTime.ToString();
					break;
				case "TeamID":
					value = selectedNPC.TeamID.ToString();
					break;
				case "Animation Overrides":
				{
					string text = "";
					if (selectedNPC.AnimationOverrides != null && selectedNPC.AnimationOverrides.Count > 0)
					{
						foreach (KeyValuePair<string, string> animationOverride in selectedNPC.AnimationOverrides)
						{
							text = text + animationOverride.Key + ":" + animationOverride.Value + ",";
						}
						text = text.Substring(0, text.Length - 1);
					}
					value = text;
					break;
				}
				case "Allow Health Regeneration":
					value = selectedMeta.AllowRegeneration;
					break;
				case "AutoChain":
					value = selectedMeta.AutoChain;
					break;
				case "Use Rotation":
					value = selectedMeta.UseRotation;
					break;
				case "Sequential Animations":
					value = selectedMeta.SequentialAnimations;
					break;
				case "Speed":
					dropOptions = new List<string> { "Walk", "Run" };
					_ = selectedMeta.Speed;
					value = selectedMeta.Speed;
					break;
				case "Move Override":
					dropOptions = new List<string> { "None", "Wander", "Loop", "Pingpong", "OneWay" };
					_ = selectedMeta.MoveOverride;
					value = selectedMeta.MoveOverride;
					break;
				case "Reaction Override":
					dropOptions = new List<string> { "None", "Friendly", "Hostile", "Neutral", "Passive", "PassiveAggressive", "AgroAll", "AgroOtherKind", "AgroIgnore" };
					value = ((!selectedNPC.ReactionOverride.HasValue) ? new int?(0) : (selectedNPC.ReactionOverride + 1));
					break;
				case "Position X":
					value = num.ToString();
					break;
				case "Position Y":
					value = num2.ToString();
					break;
				case "Position Z":
					value = num3.ToString();
					break;
				case "Rotation Y":
					value = num4.ToString();
					break;
				case "Auto Spawn":
					value = selectedMeta.AutoSpawn;
					break;
				default:
					if (editItemProp.Value == 0 || editItemProp.Value == 3)
					{
						value = "";
						break;
					}
					if (editItemProp.Value == 1)
					{
						value = false;
						break;
					}
					dropOptions = new List<string> { "None" };
					value = 0;
					break;
				}
				component.ConfigurePropertyInputUI(editItemProp.Key, editItemProp.Value, value, dropOptions);
				editList.Add(component);
			}
		}
		editListPrefab.transform.parent.parent.GetComponent<UIScrollView>().UpdatePosition();
		LastTabState = 1;
	}

	public void ClearEmptyTextEntry()
	{
		UIButton.current.transform.parent.GetComponent<UIInput>().text = "";
	}

	private void EmptyEditList()
	{
		foreach (UINPCEditItem edit in editList)
		{
			editPool.Release(edit.gameObject);
		}
		editList.Clear();
	}

	public void showPaths()
	{
		if (!selectedMeta.IsDB)
		{
			return;
		}
		ShowLeftPane(2);
		EmptyPathList();
		P_SelectedLabel.text = "Select a Path";
		if (selectedMeta != null)
		{
			bool flag = false;
			foreach (KeyValuePair<int, ComVector3> item in selectedMeta.Path)
			{
				if (!flag)
				{
					flag = true;
					continue;
				}
				GameObject obj = pathPool.Get();
				obj.transform.SetParent(pathListPrefab.transform.parent, worldPositionStays: false);
				obj.SetActive(value: true);
				UINPCPathItem component = obj.GetComponent<UINPCPathItem>();
				component.Load(item.Key, new Vector3(item.Value.x, item.Value.y, item.Value.z), selectedMeta.RotationY[item.Key]);
				pathList.Add(component);
			}
			PathsGrid.Reposition();
		}
		P_TrashBtn.GetComponent<UISprite>().gradientTop = Color.black;
		P_TrashBtn.GetComponent<UISprite>().gradientBottom = Color.black;
		selectedPath = null;
		P_UpdatePath.text = "Add Path";
		pathListPrefab.transform.parent.parent.parent.GetComponent<UIScrollView>().UpdatePosition();
		LastTabState = 2;
	}

	private void EmptyPathList()
	{
		foreach (UINPCPathItem path in pathList)
		{
			pathPool.Release(path.gameObject);
		}
		pathList.Clear();
		PathsGrid.Reposition();
	}

	public void showAddNPC()
	{
		A_SpawnID.text = latestSpawnId.ToString();
		selectedMeta = null;
		selectedNPC = null;
		selectedPath = null;
		ShowLeftPane(3);
	}

	public void showRequirements()
	{
		if (selectedMeta.IsDB)
		{
			ShowLeftPane(4);
			EmptyReqsMainList();
			PopulateExistingRequirementsDropdown();
			LastTabState = 4;
		}
	}

	private void PopulateExistingRequirementsDropdown()
	{
		ExistingRequirementsDropdown.Clear();
		ExistingRequirementCountLabel.text = "";
		if (selectedMeta != null && !string.IsNullOrEmpty(selectedMeta.Requirements))
		{
			foreach (JToken item in JArray.Parse(selectedMeta.Requirements))
			{
				JToken jToken = item["$type"];
				ExistingRequirementsDropdown.AddItem(GetMaskedName((string?)jToken));
			}
		}
		ExistingRequirementCountLabel.text = ((ExistingRequirementsDropdown.items.Count > 0) ? (ExistingRequirementsDropdown.items.Count + " REQUIREMENT(S)") : "NO REQUIREMENTS");
		ExistingRequirementsDropdown.AddItem("Add Requirement");
		ExistingRequirementsDropdown.value = ExistingRequirementsDropdown.items[0];
	}

	private Dictionary<string, int> GetRequirementProps(string type)
	{
		return type switch
		{
			"IABitFlagRequired" => new Dictionary<string, int>
			{
				{ "BitFlagName", 0 },
				{ "BitFlagIndex", 0 }
			}, 
			"IABitFlagValueRequired" => new Dictionary<string, int>
			{
				{ "BitFlagName", 0 },
				{ "BitFlagIndex", 0 },
				{ "Value", 1 }
			}, 
			"IAClassEquippedRequired" => new Dictionary<string, int>
			{
				{ "ClassID", 0 },
				{ "Not", 1 }
			}, 
			"IAItemEquippedRequired" => new Dictionary<string, int>
			{
				{ "ItemID", 0 },
				{ "Not", 1 }
			}, 
			"IAItemRequired" => new Dictionary<string, int>
			{
				{ "ItemID", 0 },
				{ "Comparison", 2 },
				{ "Quantity", 0 }
			}, 
			"IALevelRequired" => new Dictionary<string, int>
			{
				{ "Level", 0 },
				{ "Comparison", 2 }
			}, 
			"IAMachineStateRequired" => new Dictionary<string, int>
			{
				{ "MachineID", 0 },
				{ "State", 0 },
				{ "Not", 1 }
			}, 
			"IAMapRequired" => new Dictionary<string, int>
			{
				{ "MapID", 0 },
				{ "Not", 1 }
			}, 
			"IANPCStateRequired" => new Dictionary<string, int>
			{
				{ "SpawnID", 0 },
				{ "State", 0 },
				{ "Not", 1 }
			}, 
			"IAQuestCompleted" => new Dictionary<string, int>
			{
				{ "QuestID", 0 },
				{ "Not", 1 }
			}, 
			"IAQuestObjectiveCompleted" => new Dictionary<string, int>
			{
				{ "QuestID", 0 },
				{ "QOID", 0 },
				{ "Not", 1 }
			}, 
			"IAQuestObjectiveRequired" => new Dictionary<string, int>
			{
				{ "QuestID", 0 },
				{ "QOID", 0 },
				{ "Not", 1 }
			}, 
			"IAQuestRequired" => new Dictionary<string, int>
			{
				{ "QuestID", 0 },
				{ "Not", 1 }
			}, 
			"IAQuestStringRequired" => new Dictionary<string, int>
			{
				{ "QSIndex", 0 },
				{ "Comparison", 2 },
				{ "QSValue", 0 }
			}, 
			"IATradeSkillLevelRequired" => new Dictionary<string, int>
			{
				{ "Level", 0 },
				{ "Comparison", 2 },
				{ "Type", 0 }
			}, 
			"IAUserRequired" => new Dictionary<string, int>
			{
				{ "UserID", 0 },
				{ "Not", 1 }
			}, 
			"IAWarProgressRequired" => new Dictionary<string, int>
			{
				{ "WarID", 0 },
				{ "Comparison", 2 },
				{ "WarProgress", 0 }
			}, 
			_ => new Dictionary<string, int>(), 
		};
	}

	public void BuildDynamicReqs(JObject jData, Dictionary<string, int> props, int mode)
	{
		((mode == 0) ? ExistingRequirementsItemPrefab : RequirementAdderItemsPrefab).transform.parent.parent.GetComponent<UIScrollView>().ResetPosition();
		foreach (KeyValuePair<string, int> prop in props)
		{
			GameObject obj = ((mode == 0) ? ExistingRequirmentsObjectPool : RequirementAdderObjectPool).Get();
			obj.transform.SetParent(((mode == 0) ? ExistingRequirementsItemPrefab : RequirementAdderItemsPrefab).transform.parent, worldPositionStays: false);
			obj.SetActive(value: true);
			UINPCEditItem component = obj.GetComponent<UINPCEditItem>();
			List<string> list = new List<string>();
			object value;
			if (mode == 0 && jData != null)
			{
				switch (prop.Key)
				{
				case "BitFlagName":
				case "BitFlagIndex":
				case "ClassID":
				case "ItemID":
				case "Quantity":
				case "MachineID":
				case "State":
				case "MapID":
				case "SpawnID":
				case "QuestID":
				case "QOID":
				case "QSIndex":
				case "QSValue":
				case "Level":
				case "Type":
				case "UserID":
				case "WarID":
				case "WarProgress":
					value = ((jData[prop.Key] == null) ? "" : ((string?)jData[prop.Key]));
					break;
				case "Value":
				case "Not":
					value = ((jData[prop.Key] == null) ? ((object)false) : ((object)((string?)jData[prop.Key] == "1")));
					break;
				case "Comparison":
					list = new List<string> { "=", ">=", "<=", ">", "<" };
					value = ((jData[prop.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)jData[prop.Key])));
					break;
				default:
					if (prop.Value == 0 || prop.Value == 3)
					{
						value = "";
						break;
					}
					if (prop.Value == 1)
					{
						value = false;
						break;
					}
					list = new List<string> { "None" };
					value = 0;
					break;
				}
			}
			else if (prop.Value == 0 || prop.Value == 3)
			{
				value = "";
			}
			else if (prop.Value == 1)
			{
				value = false;
			}
			else
			{
				list = new List<string> { "=", ">=", "<=", ">", "<" };
				value = 0;
			}
			component.ConfigurePropertyInputUI(prop.Key, prop.Value, value, list);
			((mode == 0) ? ExistingRequirmentsEditItemList : RequirementAdderEditItemList).Add(component);
		}
		((mode == 0) ? ExistingRequirementsTable : RequirementAdderTable).Reposition();
		((mode == 0) ? ExistingRequirementsItemPrefab : RequirementAdderItemsPrefab).transform.parent.parent.GetComponent<UIScrollView>().ResetPosition();
	}

	public void OnMainReqPress()
	{
		if (ExistingRequirementsDropdown.items.Count == 1)
		{
			showReqsSecondary();
		}
	}

	public void OnExistingRequirementSelected()
	{
		EmptyReqsMainList();
		if (UIPopupList.current.value == "Add Requirement")
		{
			selectedExistingRequiermentIndex = int.MaxValue;
			showReqsSecondary();
		}
		else if (!string.IsNullOrEmpty(selectedMeta.Requirements))
		{
			JArray jArray = JArray.Parse(selectedMeta.Requirements);
			if (jArray.Count > UIPopupList.current.selectedIndex)
			{
				selectedExistingRequiermentIndex = UIPopupList.current.selectedIndex;
				selectedExistingRequirementClassName = GetRealName(UIPopupList.current.value);
				BuildDynamicReqs((JObject)jArray[UIPopupList.current.selectedIndex], GetRequirementProps(GetRealName(UIPopupList.current.value)), 0);
			}
		}
	}

	private void EmptyReqsMainList()
	{
		foreach (UINPCEditItem existingRequirmentsEditItem in ExistingRequirmentsEditItemList)
		{
			ExistingRequirmentsObjectPool.Release(existingRequirmentsEditItem.gameObject);
		}
		ExistingRequirmentsEditItemList.Clear();
	}

	public void showReqsSecondary()
	{
		ExistingRequirements.SetActive(value: false);
		RequirementAdder.SetActive(value: true);
		EmptyReqsSecondaryList();
		RequirementAdderDropdown.items = new List<string>
		{
			"Bit Flag Required", "Bit Flag Value Required", "Class Equipped Required", "Item Equipped Required", "Item Required", "Level Required", "Machine State Required", "Map Required", "NPC State Required", "Quest Completed",
			"Quest Objective Completed", "Quest Objective Required", "Quest Required", "Quest String Required", "Trade Skill Level Required", "User Required", "War Progress Required", "All Monsters Cleared"
		};
		RequirementAdderDropdown.value = null;
		RequirementAdderDropdown.value = RequirementAdderDropdown.items[13];
	}

	public void OnSecondaryReqChange()
	{
		EmptyReqsSecondaryList();
		BuildDynamicReqs(null, GetRequirementProps(GetRealName(UIPopupList.current.value)), 1);
	}

	private void EmptyReqsSecondaryList()
	{
		foreach (UINPCEditItem requirementAdderEditItem in RequirementAdderEditItemList)
		{
			RequirementAdderObjectPool.Release(requirementAdderEditItem.gameObject);
		}
		RequirementAdderEditItemList.Clear();
	}

	protected override void Init()
	{
		base.Init();
		EventDelegate.Add(SearchInput.onChange, UpdateSearch);
		SpawnTemplate.SetActive(value: false);
		npcListPrefab.SetActive(value: false);
		spawnCats = new List<GameObject>();
		spawnPool = new ObjectPool<GameObject>(SpawnTemplate);
		pathListPrefab.SetActive(value: false);
		pathList = new List<UINPCPathItem>();
		pathPool = new ObjectPool<GameObject>(pathListPrefab);
		editListPrefab.SetActive(value: false);
		editList = new List<UINPCEditItem>();
		editPool = new ObjectPool<GameObject>(editListPrefab);
		ExistingRequirementsItemPrefab.SetActive(value: false);
		ExistingRequirmentsEditItemList = new List<UINPCEditItem>();
		ExistingRequirmentsObjectPool = new ObjectPool<GameObject>(ExistingRequirementsItemPrefab);
		RequirementAdderItemsPrefab.SetActive(value: false);
		RequirementAdderEditItemList = new List<UINPCEditItem>();
		RequirementAdderObjectPool = new ObjectPool<GameObject>(RequirementAdderItemsPrefab);
		base.gameObject.SetActive(value: false);
		HideLeftPane();
		AudioManager.Play2DSFX("UI_Bag_Open");
		Refresh();
	}

	private void AddNPCs()
	{
		List<ComSpawnMeta> list = Game.Instance.AreaData.spawnMetas;
		if (!string.IsNullOrEmpty(searchText))
		{
			list = list.Where((ComSpawnMeta npc) => DoesNPCContainText(npc, searchText)).ToList();
		}
		foreach (ComSpawnMeta item in list)
		{
			GameObject gameObject = spawnPool.Get();
			gameObject.transform.SetParent(SpawnTemplate.transform.parent, worldPositionStays: false);
			gameObject.SetActive(value: true);
			SpawnDropDown component = gameObject.GetComponent<SpawnDropDown>();
			component.SpawnID = (latestSpawnId = item.SpawnID);
			component.IsDB = item.IsDB;
			component.BuildList(item.Spawns, searchText);
			spawnCats.Add(gameObject);
		}
		ListSizeLabel.text = "(" + spawnCats.Count + " Spawns)";
		if (lastSpawnID != -1)
		{
			foreach (GameObject spawnCat in spawnCats)
			{
				SpawnDropDown component2 = spawnCat.GetComponent<SpawnDropDown>();
				if (component2.SpawnID != lastSpawnID)
				{
					continue;
				}
				if (component2.Contents.Count < 1)
				{
					break;
				}
				int? num = (from x in Game.Instance.entities.NpcList
					where x.SpawnID == lastSpawnID
					select x.NPCID).FirstOrDefault();
				bool flag = false;
				if (num.HasValue && num > 0)
				{
					foreach (GameObject content in component2.Contents)
					{
						UINPCItem component3 = content.GetComponent<UINPCItem>();
						if (component3.npc.NPCID == num)
						{
							OnNpcClicked(component3);
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					OnNpcClicked(component2.Contents.FirstOrDefault().GetComponent<UINPCItem>());
				}
				break;
			}
			lastSpawnID = -1;
		}
		else
		{
			if (lastNPC == -1)
			{
				return;
			}
			bool flag2 = false;
			foreach (GameObject spawnCat2 in spawnCats)
			{
				foreach (GameObject content2 in spawnCat2.GetComponent<SpawnDropDown>().Contents)
				{
					UINPCItem component4 = content2.GetComponent<UINPCItem>();
					if (component4.npc.SpawnListNpcID == lastNPC)
					{
						OnNpcClicked(component4);
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					break;
				}
			}
			lastNPC = -1;
		}
	}

	public void ShowPosText()
	{
		D_Position.text = "X: " + selectedMeta.Path.First().Value.x + "\nY: " + selectedMeta.Path.First().Value.y + "\nZ: " + selectedMeta.Path.First().Value.z + "\nRotation Y: " + selectedMeta.RotationY.First().Value;
		D_Position.gameObject.SetActive(value: true);
	}

	public void OnNpcClickedInScene(NPC npc)
	{
		if (spawnCats == null)
		{
			return;
		}
		foreach (GameObject spawnCat in spawnCats)
		{
			SpawnDropDown component = spawnCat.GetComponent<SpawnDropDown>();
			if (component.SpawnID != npc.SpawnID)
			{
				continue;
			}
			foreach (GameObject content in component.Contents)
			{
				UINPCItem component2 = content.GetComponent<UINPCItem>();
				if (component2.npc.NPCID == npc.NPCID)
				{
					OnNpcClicked(component2);
				}
			}
		}
	}

	public void OnNpcClicked(UINPCItem selected)
	{
		if (selectedNPCItem != null)
		{
			selectedNPCItem.Highlight.SetActive(value: false);
		}
		_ = recentSpawnID;
		_ = recentNpcID;
		bool flag = false;
		foreach (GameObject spawnCat in spawnCats)
		{
			SpawnDropDown component = spawnCat.GetComponent<SpawnDropDown>();
			if (component.SpawnID == recentSpawnID)
			{
				foreach (GameObject content in component.Contents)
				{
					UINPCItem component2 = content.GetComponent<UINPCItem>();
					if (component2.npc.NPCID == recentNpcID)
					{
						component2.Highlight.SetActive(value: false);
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				break;
			}
		}
		selectedNPCItem = selected;
		recentNpcID = selected.npc.NPCID;
		recentSpawnID = selected.SpawnID;
		selectedNPC = selected.npc;
		selectedMeta = Game.Instance.AreaData.spawnMetas.Where((ComSpawnMeta x) => x.SpawnID == selected.SpawnID).FirstOrDefault();
		MonsterName.color = (selectedNPCItem.reqReload ? Color.red : Color.white);
		MonsterName.text = selectedNPCItem.npc.NameOverride;
		PadIdLabel.text = "ID #" + selected.SpawnID;
		if (lastTab > 0)
		{
			DetailsNeedSetup = true;
			switch (lastTab)
			{
			case 1:
				showEdit();
				break;
			case 2:
				showPaths();
				break;
			case 3:
				showRequirements();
				break;
			}
			lastTab = -1;
		}
		else if (showPath)
		{
			DetailsNeedSetup = true;
			showPath = false;
			showPaths();
		}
		else if (LastTabState > 0)
		{
			DetailsNeedSetup = true;
			if (!selectedMeta.IsDB)
			{
				showDetails();
				return;
			}
			switch (LastTabState)
			{
			case 1:
				showEdit();
				break;
			case 2:
				showPaths();
				break;
			case 4:
				showRequirements();
				break;
			case 3:
				break;
			}
		}
		else
		{
			lastTab = -1;
			ShowLeftPane(0);
			setupDetails();
		}
	}

	private void setupDetails()
	{
		D_ApopAdmin.SetActive(selectedNPC.ApopIDs != null && selectedNPC.ApopIDs.Count > 0);
		D_Signify.text = (selectedNPCItem.IsDB ? "Database" : "Map");
		D_UpdatePos.SetActive(selectedMeta.IsDB);
		D_UpdateLabel.text = (selectedNPCItem.IsDB ? "Remove from DB" : "Change Map to DB");
		if (selectedNPCItem.IsDB && NPCSpawn.InactiveMap.ContainsKey(selectedMeta.SpawnID))
		{
			D_UpdateLabel.text = "Change DB to Map";
		}
		selectedPath = null;
		D_Position.gameObject.SetActive(value: false);
		StartCoroutine(SetupModel(selectedNPCItem.npc.NPCID));
	}

	public IEnumerator SetupModel(int npcID)
	{
		initShadowPosX = modelShadow.transform.position.x;
		initShadowPosY = modelShadow.transform.position.y;
		using UnityWebRequest www = UnityWebRequest.Get(Main.WebServiceURL + "/Utilities/GetAllNPCAssetData?idtosplit=" + npcID);
		string errorTitle = "Failed to Load NPC";
		string friendlyMsg = "Unable to communicate with the server.";
		string customContext = "npcID: " + npcID;
		yield return www.SendWebRequest();
		customContext = UnityWebRequestHelper.AppendCloudFlareRay(www, customContext);
		if (www.isHttpError)
		{
			ErrorReporting.Instance.ReportHttpError(errorTitle, friendlyMsg, www.error, www.responseCode, null, customContext);
			ShowPosText();
			yield break;
		}
		if (www.isNetworkError)
		{
			ErrorReporting.Instance.ReportNetworkError(errorTitle, friendlyMsg, www.error, null, customContext);
			ShowPosText();
			yield break;
		}
		if (www.error != null)
		{
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, www.error, null, customContext);
			ShowPosText();
			yield break;
		}
		try
		{
			Dictionary<int, EntityAsset> dictionary = JsonConvert.DeserializeObject<Dictionary<int, EntityAsset>>(www.downloadHandler.text);
			if (dictionary.ContainsKey(npcID))
			{
				preview.ShowWithMarkers(dictionary[npcID]);
			}
			ShowPosText();
		}
		catch (Exception ex)
		{
			customContext = "Invalid NPC Data: " + customContext;
			friendlyMsg = "Unable to process response from the server.";
			ErrorReporting.Instance.ReportError(errorTitle, friendlyMsg, ex.Message, null, customContext);
			ShowPosText();
			yield break;
		}
	}

	public void UpdateAndRefresh(int SpawnID)
	{
		lastTab = -1;
		if (Details.activeSelf)
		{
			lastTab = 0;
		}
		if (Edit.activeSelf)
		{
			lastTab = 1;
		}
		if (Paths.activeSelf)
		{
			lastTab = 2;
		}
		if (Requirements.activeSelf)
		{
			lastTab = 3;
		}
		if (lastTab != -1)
		{
			lastNPC = selectedNPC.SpawnListNpcID;
		}
		if (selectedMeta != null && selectedMeta.SpawnID != SpawnID)
		{
			lastTab = -1;
			lastNPC = -1;
		}
		wasResponseUpdate = true;
		Refresh();
		base.gameObject.SetActive(value: true);
	}

	protected void Refresh()
	{
		EmptyList();
		AddNPCs();
		SpawnTable.Reposition();
		if (wasResponseUpdate)
		{
			wasResponseUpdate = false;
			ScrollView.GetComponent<UIScrollView>().UpdatePosition();
		}
		else
		{
			ScrollView.GetComponent<UIScrollView>().ResetPosition();
		}
		base.gameObject.SetActive(value: true);
	}

	public void ClearSearch()
	{
		searchText = "";
		SearchInput.value = "";
		Refresh();
	}

	private void UpdateSearch()
	{
		searchText = SearchInput.value.ToLower();
		Refresh();
	}

	private bool DoesNPCContainText(ComSpawnMeta npc, string text)
	{
		text = text.ToLower();
		if (text == "db" && npc.IsDB)
		{
			return true;
		}
		if (npc.Spawns == null)
		{
			return false;
		}
		foreach (ComNPCMeta spawn in npc.Spawns)
		{
			if (spawn.NameOverride.ToLower().Contains(text) || npc.SpawnID.ToString().Contains(text) || spawn.NPCID.ToString().Contains(text))
			{
				return true;
			}
		}
		return false;
	}

	private void EmptyList()
	{
		foreach (GameObject spawnCat in spawnCats)
		{
			spawnCat.GetComponent<SpawnDropDown>().Clear();
			spawnPool.Release(spawnCat);
		}
		spawnCats.Clear();
	}

	public void AddSpawn()
	{
		AEC.getInstance().sendRequest(new RequestAddSpawn());
	}

	public void TeleToSpawner()
	{
		AEC.getInstance().sendRequest(new RequestTeleport(selectedMeta.Path.First().Value.x, selectedMeta.Path.First().Value.y, selectedMeta.Path.First().Value.z, selectedMeta.RotationY.First().Value, GameTime.realtimeSinceServerStartup));
		Player me = Entities.Instance.me;
		me.position = new Vector3(selectedMeta.Path.First().Value.x, selectedMeta.Path.First().Value.y, selectedMeta.Path.First().Value.z);
		me.rotation = Quaternion.Euler(0f, selectedMeta.RotationY.First().Value, 0f);
		me.wrapper.transform.SetPositionAndRotation(me.position, me.rotation);
	}

	public void ReloadDB()
	{
		ChatCommands.ProcessCommand(new List<string> { "reloaddata" });
	}

	public void ReloadMap()
	{
		ChatCommands.ProcessCommand(new List<string> { "reloadmap" });
	}

	public void UpdateToPlayerPos()
	{
		if (!selectedMeta.IsDB || selectedMeta.Path.FirstOrDefault().Key < 0)
		{
			return;
		}
		foreach (ComNPCMeta spawn in selectedMeta.Spawns)
		{
			NPC nPC = Game.Instance.entities.NpcList.Where((NPC x) => x.NPCID == spawn.NPCID).FirstOrDefault();
			if (nPC != null)
			{
				nPC.wrapperTransform.position = Entities.Instance.me.position;
				Debug.LogError("spawn in selectedMeta was not in NpcList!");
			}
		}
		AEC.getInstance().sendRequest(new RequestUpdatePosSpawn(selectedMeta.SpawnID, new Vector3(Entities.Instance.me.position.x, Entities.Instance.me.position.y, Entities.Instance.me.position.z), (int)Entities.Instance.me.rotation.eulerAngles.y, selectedMeta.Path.FirstOrDefault().Key));
	}

	public void ToggleSpawnerVis()
	{
		Game.Instance.DeveloperModeToggle();
	}

	public void OpenInAdmin()
	{
		AEC.getInstance().sendRequest(new RequestOpenInAdmin(selectedNPC.NPCID));
	}

	public void ApopInAdmin()
	{
		AEC.getInstance().sendRequest(new RequestOpenApopAdmin(selectedNPC.ApopIDs));
	}

	public void Hijack()
	{
		if (!selectedMeta.IsDB)
		{
			Confirmation.Show("Spawn Editor", "Are you sure you want to Convert to DB on SpawnID#" + selectedMeta.SpawnID + "?", delegate(bool b)
			{
				if (b)
				{
					AEC.getInstance().sendRequest(new RequestHijackSpawn(selectedMeta.SpawnID, NPCSpawn.GetSpawn(selectedMeta.SpawnID).TargetTransform.position.x, NPCSpawn.GetSpawn(selectedMeta.SpawnID).TargetTransform.position.y, NPCSpawn.GetSpawn(selectedMeta.SpawnID).TargetTransform.position.z, (int)NPCSpawn.GetSpawn(selectedMeta.SpawnID).TargetTransform.rotation.eulerAngles.y));
				}
			});
			return;
		}
		string message = "Are you sure you want to Remove SpawnID#" + selectedMeta.SpawnID + "?";
		if (selectedMeta.IsDB && NPCSpawn.InactiveMap.ContainsKey(selectedMeta.SpawnID))
		{
			message = "Are you sure you want to Convert SpawnID#" + selectedMeta.SpawnID + " back to Map?";
		}
		Confirmation.Show("Spawn Editor", message, delegate(bool b)
		{
			if (b)
			{
				AEC.getInstance().sendRequest(new RequestDeleteSpawn(selectedMeta.SpawnID));
			}
		});
	}

	private Dictionary<string, string> formatToDictionary(string id)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		string textValue = editList.Where((UINPCEditItem x) => x.name.text == id).FirstOrDefault().GetTextValue();
		if (textValue.Contains(","))
		{
			string[] array = textValue.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Contains(":"))
				{
					string[] array2 = array[i].Split(':');
					dictionary.Add(array2[0], array2[1]);
				}
			}
		}
		else if (textValue.Contains(":"))
		{
			string[] array2 = textValue.Split(':');
			dictionary.Add(array2[0], array2[1]);
		}
		return dictionary;
	}

	private List<int> formatToIntList(string id)
	{
		string textValue = editList.Where((UINPCEditItem x) => x.name.text == id).FirstOrDefault().GetTextValue();
		if (string.IsNullOrEmpty(textValue))
		{
			return new List<int>();
		}
		return textValue.Split(',').Select(int.Parse).ToList();
	}

	private string getPropertyString(string id)
	{
		string textValue = editList.Where((UINPCEditItem x) => x.name.text == id).FirstOrDefault().GetTextValue();
		if (!string.IsNullOrEmpty(textValue))
		{
			return textValue;
		}
		return "";
	}

	private int toInt(string var, int def)
	{
		int result;
		return int.TryParse(var, out result) ? result : def;
	}

	private float toFloat(string var, float def)
	{
		float result;
		return float.TryParse(var, out result) ? result : def;
	}

	private bool getCheckValue(string id)
	{
		return editList.Where((UINPCEditItem x) => x.name.text == id).FirstOrDefault().GetCheckValue();
	}

	private int getDropValue(string id)
	{
		return editList.Where((UINPCEditItem x) => x.name.text == id).FirstOrDefault().GetDropIndex();
	}

	public void UpdateNPCProperties()
	{
		if (selectedMeta == null || selectedNPC == null)
		{
			return;
		}
		int num = toInt(getPropertyString("Spawn ID"), -1);
		int num2 = toInt(getPropertyString("NPC ID"), -1);
		float respawn = toFloat(getPropertyString("Respawn Time"), 15f);
		if (num == -1 || num2 == -1)
		{
			MessageBox.Show("Error", "NpcID must be valid", "OK", delegate
			{
			});
			return;
		}
		AEC.getInstance().sendRequest(new RequestEditNPC(selectedNPC.SpawnListNpcID, selectedMeta.SpawnID, selectedNPC.NPCID)
		{
			SpawnID = num,
			NpcID = num2,
			Name = getPropertyString("Name"),
			Apops = formatToIntList("Apop ID"),
			Level = toInt(getPropertyString("Level"), 1),
			Rate = toFloat(getPropertyString("Spawn Rate"), 100f),
			Respawn = respawn,
			Despawn = toFloat(getPropertyString("Despawn Time"), 6f),
			Aggro = toFloat(getPropertyString("Aggro Radius"), 1f),
			Leash = toFloat(getPropertyString("Leash Radius"), 40f),
			Chain = toFloat(getPropertyString("Chain Radius"), 60f),
			Animations = getPropertyString("Animations"),
			MinTime = toFloat(getPropertyString("Min Time"), 10f),
			MaxTime = toFloat(getPropertyString("Max Time"), 25f),
			TeamID = toInt(getPropertyString("TeamID"), -1),
			AnimationOverrides = formatToDictionary("Animation Overrides"),
			AllowRegen = getCheckValue("Allow Health Regeneration"),
			AutoChain = getCheckValue("AutoChain"),
			UseRotation = getCheckValue("Use Rotation"),
			SequentialAnimations = getCheckValue("Sequential Animations"),
			Speed = getDropValue("Speed"),
			MoveOverride = getDropValue("Move Override"),
			ReactionOverride = getDropValue("Reaction Override") - 1,
			AutoSpawn = getCheckValue("Auto Spawn")
		});
		selectedMeta.AutoSpawn = getCheckValue("Auto Spawn");
		if (selectedMeta.Path.Count > 0 && selectedMeta.RotationY.Count > 0)
		{
			AEC.getInstance().sendRequest(new RequestUpdatePosSpawn(selectedMeta.SpawnID, new Vector3(toFloat(getPropertyString("Position X"), 0f), toFloat(getPropertyString("Position Y"), 0f), toFloat(getPropertyString("Position Z"), 0f)), toInt(getPropertyString("Rotation Y"), 0), selectedMeta.Path.FirstOrDefault().Key));
		}
	}

	public void DeletePathEntry()
	{
		if (!(P_TrashBtn.GetComponent<UISprite>().gradientTop == Color.black))
		{
			AEC.getInstance().sendRequest(new RequestDeletePathNPC(selectedMeta.SpawnID, selectedPath.PathID));
		}
	}

	public void OnPathClicked(UINPCPathItem selected)
	{
		if (selectedPath == selected || selected.PathID < 0)
		{
			selectedPath = null;
			P_TrashBtn.GetComponent<UISprite>().gradientTop = Color.black;
			P_TrashBtn.GetComponent<UISprite>().gradientBottom = Color.black;
			P_SelectedLabel.text = "Select a Path";
			P_UpdatePath.text = "Add Path";
		}
		else
		{
			selectedPath = selected;
			P_TrashBtn.GetComponent<UISprite>().gradientTop = Color.white;
			P_TrashBtn.GetComponent<UISprite>().gradientBottom = Color.white;
			P_SelectedLabel.text = "Path ID: " + selected.PathID;
			P_XPos.text = selected.vector.x.ToString();
			P_YPos.text = selected.vector.y.ToString();
			P_ZPos.text = selected.vector.z.ToString();
			P_RotY.text = selected.RotationY.ToString();
			P_UpdatePath.text = "Update Path";
		}
	}

	public void GetPlayerPos()
	{
		P_XPos.text = Entities.Instance.me.position.x.ToString();
		P_YPos.text = Entities.Instance.me.position.y.ToString();
		P_ZPos.text = Entities.Instance.me.position.z.ToString();
		P_RotY.text = ((int)Entities.Instance.me.rotation.eulerAngles.y).ToString();
	}

	public void AddPath()
	{
		float result = (float.TryParse(P_XPos.text, out result) ? result : selectedMeta.Path.FirstOrDefault().Value.x);
		float result2 = (float.TryParse(P_YPos.text, out result2) ? result2 : selectedMeta.Path.FirstOrDefault().Value.y);
		float result3 = (float.TryParse(P_ZPos.text, out result3) ? result3 : selectedMeta.Path.FirstOrDefault().Value.z);
		int result4 = (int.TryParse(P_RotY.text, out result4) ? result4 : selectedMeta.RotationY.FirstOrDefault().Value);
		int pathID = ((selectedPath != null) ? selectedPath.PathID : (-1));
		AEC.getInstance().sendRequest(new RequestUpdatePathNPC(selectedMeta.SpawnID, pathID, new ComVector3(result, result2, result3), result4));
	}

	public void CreateNPC()
	{
		int result = (int.TryParse(A_SpawnID.text, out result) ? result : (-1));
		int result2 = ((!int.TryParse(A_Level.text, out result2)) ? 1 : result2);
		if (result2 < 0)
		{
			result2 = 1;
		}
		string[] array = A_NpcID.text.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			int result3 = (int.TryParse(array[i], out result3) ? result3 : (-1));
			if (result3 == -1)
			{
				MessageBox.Show("Error", "NpcID must be valid", "OK", delegate
				{
				});
				break;
			}
			AEC.getInstance().sendRequest(new RequestAddNPC(result, result3, result2, A_Name.text, A_Reaction.items.IndexOf(A_Reaction.value) - 1));
		}
	}

	public void OpenNPCs()
	{
		AEC.getInstance().sendRequest(new RequestOpenNPCs());
	}

	private string GetMaskedName(string name)
	{
		string text = "";
		int num = -1;
		for (int i = 0; i < name.Length; i++)
		{
			char c = name[i];
			num++;
			if (num >= 2)
			{
				if (num != 2 && char.IsUpper(c))
				{
					text += " ";
				}
				text += c;
			}
		}
		return text;
	}

	private string GetRealName(string name)
	{
		return "IA" + name.Replace(" ", "");
	}

	public void DeleteRequirementEntry()
	{
		if (string.IsNullOrEmpty(selectedMeta.Requirements) || (UIPopupList.current != null && UIPopupList.current.value == "Add Requirement"))
		{
			return;
		}
		JArray jArray = JArray.Parse(selectedMeta.Requirements);
		foreach (JToken item in jArray)
		{
			if ((string?)item["$type"] == selectedExistingRequirementClassName)
			{
				jArray.Remove(item);
				break;
			}
		}
		AEC.getInstance().sendRequest(new RequestUpdateRequirements(selectedMeta.SpawnID, jArray.ToString()));
	}

	public void AddRequirementEntry()
	{
		JArray jArray = new JArray();
		if (!string.IsNullOrEmpty(selectedMeta.Requirements))
		{
			jArray = JArray.Parse(selectedMeta.Requirements);
		}
		JObject jObject = new JObject(new JProperty("$type", GetRealName(RequirementAdderDropdown.value)));
		foreach (UINPCEditItem requirementAdderEditItem in RequirementAdderEditItemList)
		{
			string content = "";
			switch (requirementAdderEditItem.type)
			{
			case 0:
				content = requirementAdderEditItem.GetTextValue();
				break;
			case 1:
				content = (requirementAdderEditItem.GetCheckValue() ? "1" : "0");
				break;
			case 2:
				content = requirementAdderEditItem.GetDropValue();
				break;
			}
			jObject.Add(new JProperty(requirementAdderEditItem.name.text, content));
		}
		jArray.Add(jObject);
		AEC.getInstance().sendRequest(new RequestUpdateRequirements(selectedMeta.SpawnID, jArray.ToString()));
	}

	public void UpdateRequirementEntry()
	{
		JArray jArray = new JArray();
		if (!string.IsNullOrEmpty(selectedMeta.Requirements))
		{
			jArray = JArray.Parse(selectedMeta.Requirements);
		}
		if (jArray.Count <= selectedExistingRequiermentIndex)
		{
			Debug.LogError("Error: Attempting to update a requirement when none is selected!");
			return;
		}
		JObject jObject = (JObject)jArray[selectedExistingRequiermentIndex];
		foreach (UINPCEditItem existingRequirmentsEditItem in ExistingRequirmentsEditItemList)
		{
			string text = "";
			switch (existingRequirmentsEditItem.type)
			{
			case 0:
				text = existingRequirmentsEditItem.GetTextValue();
				break;
			case 1:
				text = (existingRequirmentsEditItem.GetCheckValue() ? "1" : "0");
				break;
			case 2:
				text = existingRequirmentsEditItem.GetDropValue();
				break;
			}
			jObject[existingRequirmentsEditItem.name.text] = text;
		}
		AEC.getInstance().sendRequest(new RequestUpdateRequirements(selectedMeta.SpawnID, jArray.ToString()));
	}

	public override void Close()
	{
		base.Close();
		EmptyList();
		EmptyPathList();
		AudioManager.Play2DSFX("UI_Bag_Close");
	}

	protected override void Destroy()
	{
		base.Destroy();
		EmptyList();
		EmptyPathList();
		Instance = null;
	}

	protected override void Resume()
	{
		base.Resume();
		Refresh();
	}
}
