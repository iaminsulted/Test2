using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StatCurves;
using UnityEngine;

public class UIMapEditor : UIWindow
{
	public static Dictionary<string, string> retainer = new Dictionary<string, string>();

	private Dictionary<string, string> ActionLabelAndClassnameDictionary = new Dictionary<string, string>();

	private Dictionary<string, string> RequirementLabelAndClassnameDictionary = new Dictionary<string, string>();

	public GameObject EntityTemplate;

	public UITable EntityTable;

	public GameObject entityListPrefab;

	private ObjectPool<GameObject> entityPool;

	private List<GameObject> entityCategories;

	public GameObject ScrollView;

	public UIInput SearchInput;

	private string searchText;

	private List<MapAsset> favoriteMapAssets = new List<MapAsset>();

	public GameObject TransferPadAdderWindow;

	public GameObject TPEditor;

	public GameObject PSEditor;

	public GameObject MachineAdderWindow;

	public GameObject MEditor;

	public GameObject DetailsTabHighlight;

	public GameObject RequirementsTabHighlight;

	public GameObject ActionsTabHighlight;

	public GameObject DetailsScreen;

	public UILabel MachineEditorTitle;

	public UIPopupList MachineTypeDropDownList;

	public UIPopupList EditMachineAssetDropDownList;

	public UIPopupList EditMachineFavoriteAssetDropDownList;

	public UIPopupList ToggleModeDropDownList;

	public UIPopupList CollisionModeDropDownList;

	public UINPCEditItem MachineNameInput;

	public UINPCEditItem AssetBundleInput;

	private string EditMachineChosenAssetName;

	public UINPCEditItem RespawnTimeInput;

	public UINPCEditItem CombatBlockedInput;

	public UINPCEditItem ChangeStateInput;

	public UIInput EditMachineScaleX;

	public UIInput EditMachineScaleY;

	public UIInput EditMachineScaleZ;

	public UIInput EditMachinePosX;

	public UIInput EditMachinePosY;

	public UIInput EditMachinePosZ;

	public UIInput EditMachineRotY;

	public UIInput EditMachineAnimation;

	public UIInput EditMachineSFX;

	public UIInput EditMachineCastAnimation;

	public UIInput EditMachineCastMessage;

	public UIInput EditMachineCastTime;

	public UINPCEditItem EditMachineHideCastBarInput;

	public UIInput EditMachineDismountX;

	public UIInput EditMachineDismountY;

	public UIInput EditMachineDismountZ;

	public UIInput EditMachineDismountRotation;

	public UINPCEditItem EditMachineStateInput;

	public GameObject ResourceMachinePropertiesWindow;

	public GameObject CurrentRequirementsScreenV2;

	public GameObject AddNewRequirementScreen;

	public GameObject CurrentActionsScreenV2;

	public GameObject AddActionsScreen;

	public GameObject ExistingRequirementPrefab;

	public GameObject ExistingRequirementPropertyItemPrefab;

	public GameObject ExistingRequirementsTableObject;

	public UITable ExistingRequirementsTable;

	public UIScrollView ExistingRequirementsScrollView;

	private List<GameObject> ExistingRequirementItems = new List<GameObject>();

	public UILabel CurrentRequirementCountLabel;

	public UIScrollView AddNewRequirementPropertyScrollView;

	public UIPopupList AddRequirementDropDownList;

	public GameObject AddRequirementPropertyItemPrefab;

	public UITable AddRequirementTable;

	private List<UINPCEditItem> AddRequirementPropertyEditItemList;

	private ObjectPool<GameObject> NewRequirementEditItemPool;

	public GameObject ExistingActionPrefab;

	public GameObject ExistingActionPropertyItemPrefab;

	public GameObject ExistingActionsTableObject;

	public UITable ExistingActionsTable;

	public UIScrollView ExistingActionsScrollView;

	private List<GameObject> ExistingActionItems = new List<GameObject>();

	public UILabel CurrentActionCountLabel;

	public UIScrollView AddActionPropertiesScrollView;

	public UIPopupList AddActionDropDownList;

	public GameObject AddActionPropertyItemPrefab;

	public UITable A_S_Table;

	private List<UINPCEditItem> AddActionPropertyEditItemList;

	private ObjectPool<GameObject> A_S_Pool;

	public UIToggle TPA_ShowConfirmation;

	public UIInput TPA_AssetBundle;

	public UIInput TPA_AssetName;

	public UIInput TPA_SpawnID;

	public UIInput TPA_AreaID;

	public UIInput TPA_CellID;

	public UIInput TPA_ScaleX;

	public UIInput TPA_ScaleY;

	public UIInput TPA_ScaleZ;

	public UIInput NewMachineNameInput;

	public UIInput NewMachineAssetBundleIdInput;

	private string NewMachineChosenAssetName;

	public UIPopupList NewMachineAssetDropDownList;

	public UIPopupList NewMachineFavoriteAssetDropDownList;

	public UIInput NewMachineScaleX;

	public UIInput NewMachineScaleY;

	public UIInput NewMachineScaleZ;

	public UIPopupList NewMachineTypeDropDownList;

	public UIToggle TPE_ShowConfirmation;

	public UIInput TPE_AssetBundle;

	public UIInput TPE_AssetName;

	public UIInput TPE_SpawnID;

	public UIInput TPE_AreaID;

	public UIInput TPE_CellID;

	public UIInput TPE_ScaleX;

	public UIInput TPE_ScaleY;

	public UIInput TPE_ScaleZ;

	public UIToggle TPE_KillAllMonsters;

	public UIPopupList ResourceMachineTradeSkillTypeDropDownList;

	public UIPopupList ResourceMachineItemTypeDropDownList;

	public UIPopupList ResourceMachineEquipItemSlotDropDownList;

	public UIInput ResourceMachinePowerInput;

	public UIInput ResourceMachineTradeSkillLevelInput;

	public UIToggle ResourceMachineRandomizeYToggle;

	private UIMapEntityItem selectedMapEntityItem;

	private int lastSelectedMapEntityID = -1;

	public static int nextMachineId = 1;

	private int uiPopupPageNumber = 1;

	private bool shiftHeld;

	public static Dictionary<int, GameObject> go_dict = new Dictionary<int, GameObject>();

	private string selectedExistingRequirementClassName = "";

	private bool wasResponseUpdate;

	public static UIMapEditor Instance { get; private set; }

	private void Update()
	{
		if (shiftHeld && Input.GetKeyDown(KeyCode.Alpha1))
		{
			uiPopupPageNumber = 1;
			Refresh();
		}
		if (shiftHeld && Input.GetKeyDown(KeyCode.Alpha2))
		{
			uiPopupPageNumber = 2;
			Refresh();
		}
		if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
		{
			shiftHeld = true;
		}
		if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
		{
			shiftHeld = false;
		}
	}

	public static void Load()
	{
		if (Instance == null)
		{
			UIWindow.ClearWindows();
			Instance = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("UIElements/MapEditor"), UIManager.Instance.transform).GetComponent<UIMapEditor>();
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

	public static void AddPlayerSpawnerPlatformMesh(GameObject parent)
	{
		UnityEngine.Object.Instantiate(Resources.Load("UIElements/MapEditor/PlayerSpawnerMesh") as GameObject, parent.transform);
	}

	public void TeleToMachine()
	{
		if (!(selectedMapEntityItem == null))
		{
			AEC.getInstance().sendRequest(new RequestTeleport(selectedMapEntityItem.entity.Position.x, selectedMapEntityItem.entity.Position.y, selectedMapEntityItem.entity.Position.z, selectedMapEntityItem.entity.RotationY, GameTime.realtimeSinceServerStartup));
			Player me = Entities.Instance.me;
			me.position = new Vector3(selectedMapEntityItem.entity.Position.x, selectedMapEntityItem.entity.Position.y, selectedMapEntityItem.entity.Position.z);
			me.rotation = Quaternion.Euler(0f, selectedMapEntityItem.entity.RotationY, 0f);
			me.wrapper.transform.SetPositionAndRotation(me.position, me.rotation);
		}
	}

	public void MoveDismountToPlayerPosition()
	{
		if (!(selectedMapEntityItem == null))
		{
			Player me = Entities.Instance.me;
			dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
			if (val["DismountX"] == null)
			{
				val["DismountX"] = new JValue("");
			}
			((JValue)val["DismountX"]).Value = me.position.x;
			if (val["DismountY"] == null)
			{
				val["DismountY"] = new JValue("");
			}
			((JValue)val["DismountY"]).Value = me.position.y;
			if (val["DismountZ"] == null)
			{
				val["DismountZ"] = new JValue("");
			}
			((JValue)val["DismountZ"]).Value = me.position.z;
			if (val["DismountRotation"] == null)
			{
				val["DismountRotation"] = new JValue("");
			}
			((JValue)val["DismountRotation"]).Value = me.rotation.eulerAngles.y;
			string data = JsonConvert.SerializeObject(val);
			AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: false, selectedMapEntityItem.entity));
		}
	}

	public void DuplicateMachine()
	{
		if (!(selectedMapEntityItem == null) && !(MachineTypeDropDownList.value.Trim() == ""))
		{
			int num = UnityEngine.Random.Range(1073741823, int.MaxValue);
			retainer["UniqueID"] = num.ToString(CultureInfo.InvariantCulture);
			dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
			((JValue)val["MachineID"]).Value = nextMachineId.ToString(CultureInfo.InvariantCulture);
			((JValue)val["UniqueID"]).Value = num;
			((JValue)val["AssetBundle"]).Value = AssetBundleInput.textField.value;
			((JValue)val["AssetName"]).Value = EditMachineChosenAssetName;
			((JValue)val["MachineName"]).Value = MachineNameInput.textField.value;
			((JValue)val["machineType"]).Value = MachineTypeDropDownList.value;
			((JValue)val["respawnTime"]).Value = Convert.ToInt32(RespawnTimeInput.GetTextValue());
			((JValue)val["combatBlocked"]).Value = CombatBlockedInput.GetCheckValue();
			((JValue)val["changeState"]).Value = ChangeStateInput.GetCheckValue();
			((JValue)val["ScaleX"]).Value = float.Parse(EditMachineScaleX.value);
			((JValue)val["ScaleY"]).Value = float.Parse(EditMachineScaleY.value);
			((JValue)val["ScaleZ"]).Value = float.Parse(EditMachineScaleZ.value);
			if (val["Animation"] == null)
			{
				val["Animation"] = new JValue("");
			}
			((JValue)val["Animation"]).Value = EditMachineAnimation.value.Trim();
			if (val["SFX"] == null)
			{
				val["SFX"] = new JValue("");
			}
			((JValue)val["SFX"]).Value = EditMachineSFX.value.Trim();
			if (val["toggleMode"] == null)
			{
				val["toggleMode"] = new JValue("");
			}
			((JValue)val["toggleMode"]).Value = ToggleModeDropDownList.value;
			if (val["collisionMode"] == null)
			{
				val["collisionMode"] = new JValue("");
			}
			((JValue)val["collisionMode"]).Value = CollisionModeDropDownList.value;
			if (val["CastAnimation"] == null)
			{
				val["CastAnimation"] = new JValue("");
			}
			((JValue)val["CastAnimation"]).Value = EditMachineCastAnimation.value.Trim();
			if (val["CastMessage"] == null)
			{
				val["CastMessage"] = new JValue("");
			}
			((JValue)val["CastMessage"]).Value = EditMachineCastMessage.value.Trim();
			if (val["CastTime"] == null)
			{
				val["CastTime"] = new JValue("");
			}
			((JValue)val["CastTime"]).Value = EditMachineCastTime.value.Trim();
			if (val["HideCastBar"] == null)
			{
				val["HideCastBar"] = new JValue("");
			}
			((JValue)val["HideCastBar"]).Value = EditMachineHideCastBarInput.GetCheckValue();
			if (val["DismountX"] == null)
			{
				val["DismountX"] = new JValue("");
			}
			((JValue)val["DismountX"]).Value = EditMachineDismountX.value.Trim();
			if (val["DismountY"] == null)
			{
				val["DismountY"] = new JValue("");
			}
			((JValue)val["DismountY"]).Value = EditMachineDismountY.value.Trim();
			if (val["DismountZ"] == null)
			{
				val["DismountZ"] = new JValue("");
			}
			((JValue)val["DismountZ"]).Value = EditMachineDismountZ.value.Trim();
			if (val["DismountRotation"] == null)
			{
				val["DismountRotation"] = new JValue("");
			}
			((JValue)val["DismountRotation"]).Value = EditMachineDismountRotation.value.Trim();
			if (val["State"] == null)
			{
				val["State"] = new JValue("");
			}
			((JValue)val["State"]).Value = EditMachineStateInput.GetCheckValue();
			selectedMapEntityItem.entity.Position.x = float.Parse(EditMachinePosX.value);
			selectedMapEntityItem.entity.Position.y = float.Parse(EditMachinePosY.value);
			selectedMapEntityItem.entity.Position.z = float.Parse(EditMachinePosZ.value);
			selectedMapEntityItem.entity.RotationY = int.Parse(EditMachineRotY.value);
			string data = JsonConvert.SerializeObject(val);
			AEC.getInstance().sendRequest(new RequestAddMapEntity(MapEntityTypes.Machine, data));
			HideEM();
		}
	}

	public static void CreatePlayerSpawnerPlatformGO(GameObject parent, ComMapEntity cmEntity)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load("UIElements/MapEditor/p_hexagonaltile") as GameObject, parent.transform, worldPositionStays: true);
		gameObject.layer = 10;
		JObject jObject = JObject.Parse(cmEntity.Data);
		TextMesh textMesh = new GameObject("ID Label").AddComponent<TextMesh>();
		PlayerSpawn playerSpawn = gameObject.AddComponent<PlayerSpawn>();
		playerSpawn.display = false;
		playerSpawn.ID = (int)jObject["SpawnID"];
		textMesh.transform.SetParent(gameObject.transform);
		textMesh.text = "Player Spawner (" + ((int)jObject["SpawnID"]).ToString(CultureInfo.InvariantCulture) + ")";
		textMesh.transform.localPosition = new Vector3(0f, -0.03f, 0f);
		textMesh.characterSize = 0.1f;
		textMesh.anchor = TextAnchor.MiddleCenter;
		textMesh.alignment = TextAlignment.Center;
		textMesh.fontStyle = FontStyle.Bold;
		textMesh.fontSize = 30;
		textMesh.gameObject.AddComponent<simpleBillboard>().Flip = true;
		textMesh.gameObject.layer = 10;
		gameObject.transform.position = new Vector3(cmEntity.Position.x, cmEntity.Position.y, cmEntity.Position.z);
		gameObject.transform.rotation = Quaternion.Euler(0f, cmEntity.RotationY, 0f);
		go_dict.Add(cmEntity.ID, gameObject);
	}

	public static IEnumerator LoadEntityAsset(GameObject parent, ComMapEntity cmEntity)
	{
		JObject data = JObject.Parse(cmEntity.Data);
		AssetBundleLoader assetBundleLoader = AssetBundleManager.LoadAssetBundle(cmEntity.Bundle);
		yield return assetBundleLoader;
		if (!string.IsNullOrEmpty(assetBundleLoader.Error) || parent == null)
		{
			Debug.LogError("LoadEntityAsset error!");
			yield break;
		}
		if (data["AssetName"] == null || ((string?)data["AssetName"]).Trim().Length < 1)
		{
			Debug.LogError("ERROR: AssetName cannot be null or empty!");
			yield break;
		}
		AssetBundleRequest abr = assetBundleLoader.Asset.LoadAssetAsync<GameObject>((string?)data["AssetName"]);
		yield return abr;
		GameObject gameObject = abr.asset as GameObject;
		if (gameObject == null)
		{
			Debug.LogError("LoadEntityAsset error! entity was null");
			yield break;
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate(gameObject, parent.transform);
		gameObject2.transform.localPosition = Vector3.zero;
		switch (cmEntity.Type)
		{
		case MapEntityTypes.TransferPad:
			ConfigureTransferPadAsset(gameObject2, data, cmEntity);
			break;
		case MapEntityTypes.Machine:
			ConfigureMachineAsset(gameObject2, data, cmEntity);
			break;
		}
		if (!go_dict.ContainsKey(cmEntity.ID))
		{
			go_dict.Add(cmEntity.ID, gameObject2);
		}
		else
		{
			Debug.LogError("ATTEMPTING TO LOAD THE SAME ENTITY ID TWICE");
		}
		if (Instance != null)
		{
			Instance.BuildDetailsScreen();
		}
	}

	private static void ConfigureTransferPadAsset(GameObject transferPadGameObject, JObject data, ComMapEntity cmEntity)
	{
		transferPadGameObject.transform.localScale = new Vector3((float)data["ScaleX"], (float)data["ScaleY"], (float)data["ScaleZ"]);
		TextMesh textMesh = new GameObject("ID Label").AddComponent<TextMesh>();
		CTATransferMap componentInChildren = transferPadGameObject.GetComponentInChildren<CTATransferMap>();
		if (componentInChildren != null)
		{
			componentInChildren.UniqueID = (string?)data["UniqueID"];
			componentInChildren.MapID = (int)data["AreaID"];
			componentInChildren.CellID = (int)data["CellID"];
			componentInChildren.SpawnID = (int)data["SpawnID"];
			componentInChildren.ShowConfirmation = (bool)data["ShowConfirmation"];
		}
		TransferPadControl componentInChildren2 = transferPadGameObject.GetComponentInChildren<TransferPadControl>();
		if (componentInChildren2 != null)
		{
			componentInChildren2.UniqueID = (string?)data["UniqueID"];
			componentInChildren2.AreaID = (int)data["AreaID"];
			componentInChildren2.CellID = (int)data["CellID"];
			componentInChildren2.SpawnID = (int)data["SpawnID"];
			componentInChildren2.setKillAllMonsters(data["KillAllMonsters"] != null && (bool)data["KillAllMonsters"]);
		}
		textMesh.transform.SetParent(transferPadGameObject.transform);
		textMesh.text = "Transfer Pad (" + cmEntity.ID.ToString(CultureInfo.InvariantCulture) + ")";
		textMesh.transform.localPosition = new Vector3(0f, 2f, 0f);
		textMesh.characterSize = 0.1f;
		textMesh.anchor = TextAnchor.MiddleCenter;
		textMesh.alignment = TextAlignment.Center;
		textMesh.fontStyle = FontStyle.Bold;
		textMesh.fontSize = 40;
		textMesh.gameObject.AddComponent<simpleBillboard>().Flip = true;
		textMesh.gameObject.layer = 10;
		transferPadGameObject.transform.position = new Vector3(cmEntity.Position.x, cmEntity.Position.y, cmEntity.Position.z);
		transferPadGameObject.transform.rotation = Quaternion.Euler(0f, cmEntity.RotationY, 0f);
	}

	private static void ConfigureMachineAsset(GameObject machineGameObject, JObject data, ComMapEntity cmEntity)
	{
		string text = ((string?)data["machineType"]).Replace(" ", "");
		machineGameObject.name = "Machine (" + text + ") - " + cmEntity.ID;
		BaseMachine machine = null;
		SphereCollider sphereCollider = null;
		BoxCollider boxCollider = null;
		CapsuleCollider capsuleCollider = null;
		MeshCollider meshCollider = null;
		Type type = Type.GetType(text);
		if (typeof(BaseMachine).IsAssignableFrom(type))
		{
			machine = (BaseMachine)machineGameObject.AddComponent(type);
			if (machine is CollideMachine || machine is ClickMachine)
			{
				sphereCollider = machine.gameObject.GetComponent<SphereCollider>();
				boxCollider = machine.gameObject.GetComponent<BoxCollider>();
				capsuleCollider = machine.gameObject.GetComponent<CapsuleCollider>();
				meshCollider = machine.gameObject.GetComponent<MeshCollider>();
			}
		}
		if (machine == null)
		{
			return;
		}
		machine.ID = (int)data["UniqueID"];
		machine.MachineID = (int)data["MachineID"];
		machine.RespawnTime = (int)data["respawnTime"];
		machine.CombatBlocked = (bool)data["combatBlocked"];
		machine.ChangeState = (bool)data["changeState"];
		machine.AddMachineToMap();
		if (machine is InteractiveMachine interactiveMachine)
		{
			interactiveMachine.useDatabaseRecord = true;
			interactiveMachine.LoadDBData(data.GetValue("actions").ToString(), data.GetValue("ctaActions").ToString(), data.GetValue("requirements").ToString());
			machine.RegisterRequirementListeners();
			if (data["toggleMode"] != null && ((string?)data["toggleMode"]).Equals("Object Switch"))
			{
				MachineActiveGameObjectSwitch machineActiveGameObjectSwitch = machineGameObject.AddComponent<MachineActiveGameObjectSwitch>();
				machineActiveGameObjectSwitch.InteractiveObject = machine;
				Transform transform = machine.transform.Find("Inactive");
				if (transform != null)
				{
					machineActiveGameObjectSwitch.GameObjectToShowWhenMachineIsInactive = transform.gameObject;
				}
				Transform transform2 = machine.transform.Find("Active");
				if (transform2 != null)
				{
					machineActiveGameObjectSwitch.GameObjectToShowWhenMachineIsActive = transform2.gameObject;
				}
				machineActiveGameObjectSwitch.SetIdAndAddListenerToMap(machine.ID);
			}
			else if (interactiveMachine.Requirements.Count > 0 || interactiveMachine.ChangeState)
			{
				MachineActiveRenderersAndCollidersActive machineActiveRenderersAndCollidersActive = machineGameObject.AddComponent<MachineActiveRenderersAndCollidersActive>();
				machineActiveRenderersAndCollidersActive.InteractiveObject = machine;
				machineActiveRenderersAndCollidersActive.SetIdAndAddListenerToMap(machine.ID);
			}
			if (machine is CollideMachine collideMachine)
			{
				collideMachine.isDb = true;
				if (machine is CollideTriggerMachine collideTriggerMachine)
				{
					machine.gameObject.layer = 10;
					Collider component = machine.gameObject.GetComponent<Collider>();
					if (component != null)
					{
						component.isTrigger = true;
					}
					if (data["collisionMode"] != null)
					{
						Enum.TryParse<CollisionMode>((string?)data["collisionMode"], out collideTriggerMachine.collisionMode);
					}
					if (machine is PressureMachine pressureMachine)
					{
						string jsonExitCTActions = ((data["exitCTActions"] == null) ? null : data.GetValue("exitCTActions").ToString());
						string jsonExitActions = ((data["exitActions"] == null) ? null : data.GetValue("exitActions").ToString());
						string jsonStayCTActions = ((data["stayCTActions"] == null) ? null : data.GetValue("stayCTActions").ToString());
						string jsonStayActions = ((data["stayActions"] == null) ? null : data.GetValue("stayActions").ToString());
						pressureMachine.exitActions = new List<MachineAction>();
						pressureMachine.exitCTActions = new List<ClientTriggerAction>();
						pressureMachine.stayActions = new List<MachineAction>();
						pressureMachine.stayCTActions = new List<ClientTriggerAction>();
						pressureMachine.LoadExitAndStayActionDBData(jsonExitActions, jsonExitCTActions, jsonStayActions, jsonStayCTActions);
					}
				}
			}
			else if (machine is ClickMachine)
			{
				machine.gameObject.layer = 13;
				try
				{
					if (data["CastAnimation"] != null)
					{
						string[] array = ((string?)data["CastAnimation"]).Split(',');
						((ClickMachine)machine).CastAnimation = array[UnityEngine.Random.Range(0, array.Length)];
						((ClickMachine)machine).CastAnimations = array.ToList();
					}
					if (data["CastMessage"] != null)
					{
						((ClickMachine)machine).CastMessage = (string?)data["CastMessage"];
					}
					if (data["CastTime"] != null)
					{
						float result = 0f;
						float.TryParse((string?)data["CastTime"], out result);
						((ClickMachine)machine).CastTime = result;
					}
					if (data["HideCastBar"] != null)
					{
						((ClickMachine)machine).HideCastBar = (bool)data["HideCastBar"];
					}
				}
				catch (Exception ex)
				{
					Debug.LogError(ex.ToString());
				}
				if (machine is OwnerMachine)
				{
					MachineStateGOActive[] componentsInChildren = machine.gameObject.GetComponentsInChildren<MachineStateGOActive>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].InteractiveObject = machine;
					}
					if (data["DismountX"] != null && data["DismountY"] != null && data["DismountZ"] != null && data["DismountRotation"] != null)
					{
						GameObject gameObject = new GameObject();
						gameObject.transform.parent = machine.transform.parent;
						gameObject.transform.position = new Vector3((float)data["DismountX"], (float)data["DismountY"], (float)data["DismountZ"]);
						gameObject.transform.rotation = Quaternion.Euler(0f, (float)data["DismountRotation"], 0f);
						((OwnerMachine)machine).DisownPosition = gameObject.transform;
					}
					GameObject gameObject2 = new GameObject();
					gameObject2.transform.parent = machine.transform;
					gameObject2.transform.position = machine.transform.position + new Vector3(0f, -0.681f, 0f);
					gameObject2.transform.rotation = machine.transform.rotation;
					((OwnerMachine)machine).OwnPosition = gameObject2.transform;
				}
				if (machine is ResourceMachine resourceMachine)
				{
					if (Enum.TryParse<TradeSkillType>((string?)data["TradeSkillType"], out var result2))
					{
						resourceMachine.tradeSkillType = result2;
					}
					if (Enum.TryParse<EquipItemSlot>((string?)data["ResourceMachineEquipItemSlot"], out var result3))
					{
						resourceMachine.equipItemSlot = result3;
					}
					if (Enum.TryParse<ItemType>((string?)data["ResourceMachineItemType"], out var result4))
					{
						resourceMachine.itemType = result4;
					}
					if (data["ResourceMachineRandomizeY"] != null)
					{
						resourceMachine.randomizeRotationY = (bool)data["ResourceMachineRandomizeY"];
					}
					if (data["ResourceMachinePower"] != null)
					{
						resourceMachine.power = (int)data["ResourceMachinePower"];
					}
					if (data["ResourceMachineTradeSkillLevel"] != null)
					{
						resourceMachine.tradeSkillLevel = (int)data["ResourceMachineTradeSkillLevel"];
					}
				}
			}
			machine.Animation = (string?)data["Animation"];
			machine.SfxTrigger = (string?)data["SFX"];
		}
		machineGameObject.transform.localScale = new Vector3((float)data["ScaleX"], (float)data["ScaleY"], (float)data["ScaleZ"]);
		machineGameObject.transform.position = new Vector3(cmEntity.Position.x, cmEntity.Position.y, cmEntity.Position.z);
		machineGameObject.transform.rotation = Quaternion.Euler(0f, cmEntity.RotationY, 0f);
		TextMesh textMesh = new GameObject("ID Label").AddComponent<TextMesh>();
		textMesh.transform.SetParent(machineGameObject.transform);
		textMesh.text = (string?)data["MachineName"] + " : " + (string?)data["MachineID"];
		textMesh.transform.localPosition = new Vector3(0f, -0.03f, 0f);
		textMesh.characterSize = 0.1f;
		textMesh.anchor = TextAnchor.MiddleCenter;
		textMesh.alignment = TextAlignment.Center;
		textMesh.fontStyle = FontStyle.Bold;
		textMesh.fontSize = 30;
		textMesh.gameObject.AddComponent<simpleBillboard>().Flip = true;
		textMesh.gameObject.layer = 10;
		ComMachine comMachine = Game.Instance.ComMachines.FirstOrDefault((ComMachine x) => x.ID == machine.ID);
		if (comMachine != null)
		{
			machine.Init(comMachine.State, comMachine.OwnerID, comMachine.Areas);
			GameObject gameObject3 = null;
			if (sphereCollider != null)
			{
				gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("UIElements/MapEditor/p_temp_machine_visualizer") as GameObject, machineGameObject.transform, worldPositionStays: true);
				gameObject3.transform.localScale = Vector3.one * sphereCollider.radius * 2f;
			}
			if (boxCollider != null)
			{
				gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("UIElements/MapEditor/p_temp_machine_visualizer_box") as GameObject, machineGameObject.transform, worldPositionStays: true);
				gameObject3.transform.localScale = boxCollider.size;
			}
			if (capsuleCollider != null)
			{
				gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("UIElements/MapEditor/p_temp_machine_visualizer_capsule") as GameObject, machineGameObject.transform, worldPositionStays: true);
				gameObject3.transform.localScale = new Vector3(capsuleCollider.radius * 2f, capsuleCollider.height / 2f, capsuleCollider.radius * 2f);
			}
			if (meshCollider != null)
			{
				gameObject3 = UnityEngine.Object.Instantiate(Resources.Load("UIElements/MapEditor/p_temp_machine_visualizer_mesh") as GameObject, machineGameObject.transform, worldPositionStays: true);
				gameObject3.GetComponent<MeshFilter>().mesh = meshCollider.sharedMesh;
				gameObject3.transform.localScale = Vector3.one;
			}
			if (gameObject3 != null)
			{
				gameObject3.transform.localPosition = Vector3.zero;
				gameObject3.transform.localRotation = Quaternion.identity;
				gameObject3.layer = 10;
			}
			if (machine is ResourceMachine resourceMachine2)
			{
				resourceMachine2.SpawnNode(comMachine.TotalUsages, comMachine.Usages, comMachine.NodeID, comMachine.Rarity, comMachine.TradeSkillLevel, comMachine.JsonRequirements, comMachine.DropIDs);
			}
			if (gameObject3 != null && gameObject3.GetComponent<MeshRenderer>() != null)
			{
				if (machine is OwnerMachine || machine is PressureMachine || machine is ResourceMachine)
				{
					gameObject3.GetComponent<MeshRenderer>().material = (Material)Resources.Load("UIElements/MapEditor/temp_machine_visualizer-Other");
				}
				else if (machine is CollideMachine)
				{
					gameObject3.GetComponent<MeshRenderer>().material = (Material)Resources.Load("UIElements/MapEditor/temp_machine_visualizer-Collide");
				}
			}
		}
		else
		{
			Debug.LogWarning("ComMachine not found for db-loaded machine!");
		}
	}

	public void RefreshMachineLabels()
	{
		foreach (KeyValuePair<int, GameObject> item in go_dict)
		{
			Transform transform = item.Value.transform.Find("ID Label");
			if (transform != null)
			{
				transform.SetParent(null);
				transform.localScale = Vector3.one;
				transform.SetParent(item.Value.transform);
			}
		}
	}

	public static void DeleteGO(int ID)
	{
		if (go_dict.ContainsKey(ID))
		{
			UnityEngine.Object.Destroy(go_dict[ID]);
			go_dict.Remove(ID);
		}
	}

	public void ClearEmptyTextEntry()
	{
		UIButton.current.transform.parent.GetComponent<UIInput>().value = "";
	}

	public void AddMachine()
	{
		if (NewMachineChosenAssetName == null || NewMachineChosenAssetName.Trim().Length < 1 || NewMachineAssetBundleIdInput.value == null || NewMachineAssetBundleIdInput.value.Trim().Length < 1)
		{
			Debug.LogError("ERROR: Asset Name cannot be empty!");
			return;
		}
		Player me = Entities.Instance.me;
		int num = UnityEngine.Random.Range(1073741823, int.MaxValue);
		retainer["UniqueID"] = num.ToString(CultureInfo.InvariantCulture);
		AEC.getInstance().sendRequest(new RequestAddMapEntity(MapEntityTypes.Machine, "{\"AreaID\":" + Game.CurrentAreaID.ToString(CultureInfo.InvariantCulture) + ", \"MachineID\":" + nextMachineId.ToString(CultureInfo.InvariantCulture) + ", \"MachineName\":\"" + NewMachineNameInput.value + "\", \"UniqueID\":" + num.ToString(CultureInfo.InvariantCulture) + ", \"AssetName\":\"" + NewMachineChosenAssetName + "\",\"AssetBundle\":" + NewMachineAssetBundleIdInput.value + ", \"combatBlocked\":false,\"changeState\":false, \"respawnTime\":20, \"machineType\":\"" + NewMachineTypeDropDownList.value.Replace(" ", "") + "\",\"ScaleX\":" + NewMachineScaleX.value + ",\"ScaleY\":" + NewMachineScaleY.value + ",\"ScaleZ\":" + NewMachineScaleZ.value + ", \"toggleMode\":\"Visibility\", \"collisionMode\":\"Enter\", \"CastAnimation\":\"\", \"CastMessage\":\"\", \"CastTime\":0, \"HideCastBar\":false, \"Animation\":" + (NewMachineTypeDropDownList.value.Equals("Owner Machine") ? "\"Seated\"" : "\"\"") + ", \"DismountX\":" + me.position.x.ToString(CultureInfo.InvariantCulture) + ", \"DismountY\":" + me.position.y.ToString(CultureInfo.InvariantCulture) + ", \"DismountZ\":" + me.position.z.ToString(CultureInfo.InvariantCulture) + ", \"DismountRotation\":" + me.rotation.eulerAngles.y.ToString(CultureInfo.InvariantCulture) + ", \"requirements\": [], \"actions\": [], \"ctaActions\": [] }"));
		MachineAdderWindow.SetActive(value: false);
	}

	public void ShowAddTransferPadWindow()
	{
		if (retainer.ContainsKey("AssetBundle"))
		{
			TPA_AssetBundle.value = retainer["AssetBundle"];
			TPA_AssetName.value = retainer["AssetName"];
			TPA_SpawnID.value = retainer["SpawnID"];
			TPA_AreaID.value = retainer["AreaID"];
			TPA_CellID.value = retainer["CellID"];
			TPA_ShowConfirmation.value = bool.Parse(retainer["ShowConfirmation"]);
			TPA_ScaleX.value = retainer["ScaleX"];
			TPA_ScaleY.value = retainer["ScaleY"];
			TPA_ScaleZ.value = retainer["ScaleZ"];
		}
		TransferPadAdderWindow.SetActive(value: true);
	}

	public void ShowMachineAdder()
	{
		HideEM();
		resetMachineTypeDropdowns();
		NewMachineAssetDropDownList.Clear();
		NewMachineTypeDropDownList.value = NewMachineTypeDropDownList.items[0];
		MachineAdderWindow.SetActive(value: true);
		NewMachineAssetBundleIdInput.value = "";
		NewMachineFavoriteAssetDropDownList.items = new List<string> { "Favorite Assets" };
		NewMachineFavoriteAssetDropDownList.items.AddRange(favoriteMapAssets.Select((MapAsset x) => x.PrefabName).ToList());
		NewMachineFavoriteAssetDropDownList.SetToIndex(0);
	}

	public void OpenAssetBundlesInAdmin()
	{
		Application.OpenURL("https://admin.aq3d.com/content/MapAssets/");
	}

	public void OnAssetBundleIdChanged()
	{
		int result = 0;
		NewMachineAssetDropDownList.items = new List<string> { "" };
		NewMachineAssetDropDownList.SetToIndex(0);
		EditMachineAssetDropDownList.items = new List<string> { "" };
		EditMachineAssetDropDownList.SetToIndex(0);
		if (int.TryParse(NewMachineAssetBundleIdInput.value.Trim(), out result))
		{
			AEC.getInstance().sendRequest(new RequestMapAssets(result));
		}
	}

	public void OnEditAssetBundleIdChanged()
	{
		int result = 0;
		NewMachineAssetDropDownList.items = new List<string> { "" };
		NewMachineAssetDropDownList.SetToIndex(0);
		EditMachineAssetDropDownList.items = new List<string> { "" };
		EditMachineAssetDropDownList.SetToIndex(0);
		if (int.TryParse(AssetBundleInput.textField.value.Trim(), out result))
		{
			AEC.getInstance().sendRequest(new RequestMapAssets(result));
		}
	}

	public void SetMapAssetChoices(List<MapAsset> mapAssets)
	{
		NewMachineAssetDropDownList.items = new List<string> { "Choose Asset" };
		NewMachineAssetDropDownList.items.AddRange(mapAssets.Select((MapAsset x) => x.PrefabName).ToList());
		if (NewMachineAssetDropDownList.items.Contains(NewMachineChosenAssetName))
		{
			NewMachineAssetDropDownList.value = NewMachineChosenAssetName;
		}
		else
		{
			NewMachineAssetDropDownList.SetToIndex(0);
		}
		EditMachineAssetDropDownList.items = new List<string> { "Choose Asset" };
		EditMachineAssetDropDownList.items.AddRange(mapAssets.Select((MapAsset x) => x.PrefabName).ToList());
		StartCoroutine(WaitThenUpdateEditMachineAssetName());
	}

	public void SetMapAssetFavorites(List<MapAsset> mapAssets)
	{
		favoriteMapAssets = new List<MapAsset>();
		favoriteMapAssets.AddRange(mapAssets);
	}

	private IEnumerator WaitThenUpdateEditMachineAssetName()
	{
		yield return new WaitForSeconds(0.1f);
		if (EditMachineAssetDropDownList.items.Contains(EditMachineChosenAssetName))
		{
			EditMachineAssetDropDownList.value = EditMachineChosenAssetName;
		}
		else
		{
			EditMachineAssetDropDownList.SetToIndex(0);
		}
	}

	public void NewMachineMapAssetChosen()
	{
		if (NewMachineAssetDropDownList.selectedIndex > 0 && NewMachineAssetDropDownList.value.Trim().Length > 0)
		{
			NewMachineChosenAssetName = NewMachineAssetDropDownList.value;
		}
	}

	public void EditMachineMapAssetChosen()
	{
		if (EditMachineAssetDropDownList.selectedIndex > 0 && EditMachineAssetDropDownList.value.Trim().Length > 0)
		{
			EditMachineChosenAssetName = EditMachineAssetDropDownList.value;
		}
	}

	public void NewMachineFavoriteMapAssetChosen()
	{
		try
		{
			if (NewMachineFavoriteAssetDropDownList.selectedIndex > 0 && NewMachineFavoriteAssetDropDownList.value.Trim().Length > 0)
			{
				MapAsset mapAsset = favoriteMapAssets.FirstOrDefault((MapAsset x) => x.PrefabName == NewMachineFavoriteAssetDropDownList.value);
				NewMachineAssetBundleIdInput.value = mapAsset.AssetBundleID.ToString(CultureInfo.InvariantCulture);
				NewMachineChosenAssetName = mapAsset.PrefabName;
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("ERROR CHOOSING FAVORITE MAP ASSET: " + ex.ToString());
		}
	}

	public void EditMachineFavoriteMapAssetChosen()
	{
		if (EditMachineFavoriteAssetDropDownList.selectedIndex > 0 && EditMachineFavoriteAssetDropDownList.value.Trim().Length > 0)
		{
			MapAsset mapAsset = favoriteMapAssets.FirstOrDefault((MapAsset x) => x.PrefabName == EditMachineFavoriteAssetDropDownList.value);
			AssetBundleInput.textField.value = mapAsset.AssetBundleID.ToString(CultureInfo.InvariantCulture);
			EditMachineChosenAssetName = mapAsset.PrefabName;
		}
	}

	public void HideATP()
	{
		TransferPadAdderWindow.SetActive(value: false);
	}

	public void HideMachineAdderWindow()
	{
		MachineAdderWindow.SetActive(value: false);
	}

	public void GotoSpawnEditor()
	{
		Close();
		UINPCEditor.Toggle();
	}

	public void ShowEditPlayerSpawner()
	{
		PSEditor.SetActive(value: true);
	}

	public void HideEditPlayerSpawner()
	{
		PSEditor.SetActive(value: false);
	}

	public void ShowMachineEditor()
	{
		MEditor.SetActive(value: true);
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		MachineEditorTitle.text = (string)val["MachineName"] + " (" + (string)val["MachineID"] + ")";
		ShowDetailsPage();
		EditMachineFavoriteAssetDropDownList.items = new List<string> { "Favorite Assets" };
		EditMachineFavoriteAssetDropDownList.items.AddRange(favoriteMapAssets.Select((MapAsset x) => x.PrefabName).ToList());
		EditMachineFavoriteAssetDropDownList.SetToIndex(0);
	}

	private void HideMachineEditorPages()
	{
		DetailsTabHighlight.SetActive(value: false);
		RequirementsTabHighlight.SetActive(value: false);
		ActionsTabHighlight.SetActive(value: false);
		ResourceMachinePropertiesWindow.SetActive(value: false);
		DetailsScreen.SetActive(value: false);
		AddNewRequirementScreen.SetActive(value: false);
		CurrentActionsScreenV2.SetActive(value: false);
		AddActionsScreen.SetActive(value: false);
		CurrentRequirementsScreenV2.SetActive(value: false);
	}

	public void ShowCurrentRequirementsPage()
	{
		HideMachineEditorPages();
		RequirementsTabHighlight.SetActive(value: true);
		CurrentRequirementsScreenV2.SetActive(value: true);
		StartCoroutine(FixRequirementsScrollView());
	}

	public void ShowDetailsPage()
	{
		HideMachineEditorPages();
		DetailsTabHighlight.SetActive(value: true);
		DetailsScreen.SetActive(value: true);
	}

	public void ShowActionsPage()
	{
		HideMachineEditorPages();
		ActionsTabHighlight.SetActive(value: true);
		CurrentActionsScreenV2.SetActive(value: true);
		StartCoroutine(FixActionsScrollView());
	}

	public void ShowAddNewRequirementPage()
	{
		HideMachineEditorPages();
		RequirementsTabHighlight.SetActive(value: false);
		AddNewRequirementScreen.SetActive(value: true);
		EmptyAddRequirementPropertyEditItemList();
		List<string> list = new List<string>(RequirementLabelAndClassnameDictionary.Keys);
		list.Sort();
		AddRequirementDropDownList.items = list;
		AddRequirementDropDownList.value = null;
		AddRequirementDropDownList.value = AddRequirementDropDownList.items[13];
	}

	private void EmptyAddRequirementPropertyEditItemList()
	{
		foreach (UINPCEditItem addRequirementPropertyEditItem in AddRequirementPropertyEditItemList)
		{
			NewRequirementEditItemPool.Release(addRequirementPropertyEditItem.gameObject);
		}
		AddRequirementPropertyEditItemList.Clear();
	}

	public void DeleteSelectedRequirementEntry()
	{
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		if (val["requirements"].Count == 0)
		{
			return;
		}
		foreach (dynamic item in val["requirements"])
		{
			dynamic val2 = item["$type"];
			if ((string)val2 == selectedExistingRequirementClassName)
			{
				((JArray)val["requirements"]).Remove(item);
				break;
			}
		}
		string data = JsonConvert.SerializeObject(val);
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: false));
	}

	public void DeleteRequirementEntry(string deletedRequirementClassName, int index)
	{
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		if (val["requirements"].Count == 0)
		{
			return;
		}
		int num = 0;
		foreach (dynamic item in val["requirements"])
		{
			num++;
			dynamic val2 = item["$type"];
			if ((string)val2 == deletedRequirementClassName && num == index)
			{
				((JArray)val["requirements"]).Remove(item);
				break;
			}
		}
		string data = JsonConvert.SerializeObject(val);
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: false));
	}

	public void DeleteActionEntry(string deletedActionClassName, int index)
	{
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		dynamic val2 = val["exitCTActions"];
		dynamic val3 = val["exitActions"];
		dynamic val4 = val["stayCTActions"];
		dynamic val5 = val["stayActions"];
		int num = 0;
		foreach (dynamic item in val["actions"])
		{
			num++;
			dynamic val6 = item["$type"];
			if ((string)val6 == deletedActionClassName && num == index)
			{
				((JArray)val["actions"]).Remove(item);
				break;
			}
		}
		foreach (dynamic item2 in val["ctaActions"])
		{
			num++;
			dynamic val7 = item2["$type"];
			if ((string)val7 == deletedActionClassName && num == index)
			{
				((JArray)val["ctaActions"]).Remove(item2);
				break;
			}
		}
		if (val2 != null)
		{
			foreach (dynamic item3 in val2)
			{
				num++;
				dynamic val8 = item3["$type"];
				if ((string)val8 == deletedActionClassName && num == index)
				{
					((JArray)val2).Remove(item3);
					break;
				}
			}
		}
		if (val3 != null)
		{
			foreach (dynamic item4 in val3)
			{
				num++;
				dynamic val9 = item4["$type"];
				if ((string)val9 == deletedActionClassName && num == index)
				{
					((JArray)val3).Remove(item4);
					break;
				}
			}
		}
		if (val4 != null)
		{
			foreach (dynamic item5 in val4)
			{
				num++;
				dynamic val10 = item5["$type"];
				if ((string)val10 == deletedActionClassName && num == index)
				{
					((JArray)val4).Remove(item5);
					break;
				}
			}
		}
		if (val5 != null)
		{
			foreach (dynamic item6 in val5)
			{
				num++;
				dynamic val11 = item6["$type"];
				if ((string)val11 == deletedActionClassName && num == index)
				{
					((JArray)val5).Remove(item6);
					break;
				}
			}
		}
		string data = JsonConvert.SerializeObject(val);
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: false));
	}

	public void OnAddRequirementDropdownListItemSelected()
	{
		EmptyAddRequirementPropertyEditItemList();
		string text = RequirementLabelAndClassnameDictionary[UIPopupList.current.value.Trim()];
		BuildEditItemsForNewRequirementProperties(null, GetRequirementProperites(text), text);
	}

	private Dictionary<string, int> GetRequirementProperites(string type)
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
				{ "State", 0 }
			}, 
			"IAPlayerStateRequired" => new Dictionary<string, int> { { "State", 0 } }, 
			"IAMapRequired" => new Dictionary<string, int> { { "MapID", 0 } }, 
			"IANPCStateRequired" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "State", 0 }
			}, 
			"IAStateOnMultipleNPCsRequired" => new Dictionary<string, int>
			{
				{ "NPCSpawnIDs", 3 },
				{ "State", 0 }
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
			"IAUserRequired" => new Dictionary<string, int> { { "UserID", 0 } }, 
			"IAWarProgressRequired" => new Dictionary<string, int>
			{
				{ "WarID", 0 },
				{ "Comparison", 2 },
				{ "WarProgress", 0 }
			}, 
			"IAPlayerCountRequired" => new Dictionary<string, int>
			{
				{ "TargetCount", 0 },
				{ "Comparison", 2 }
			}, 
			_ => new Dictionary<string, int>(), 
		};
	}

	public void BuildInlinePropertyEditItemsForRequirement(JObject requirementData, Dictionary<string, int> properties, string requirementType, List<UINPCEditItem> propertyItemList)
	{
		foreach (KeyValuePair<string, int> property in properties)
		{
			GameObject gameObject = null;
			gameObject = UnityEngine.Object.Instantiate(ExistingRequirementPropertyItemPrefab);
			ExistingRequirementItems.Add(gameObject);
			gameObject.transform.SetParent(ExistingRequirementPropertyItemPrefab.transform.parent, worldPositionStays: false);
			gameObject.SetActive(value: true);
			UINPCEditItem component = gameObject.GetComponent<UINPCEditItem>();
			propertyItemList.Add(component);
			List<string> list = new List<string>();
			if ((requirementType.Equals("IANPCStateRequired") || requirementType.Equals("IAStateOnMultipleNPCsRequired")) && property.Key.Equals("State"))
			{
				list = new List<string> { "Dead", "Alive" };
				component.ConfigurePropertyInputUI(value: (requirementData == null || requirementData[property.Key] == null) ? ((object)0) : ((object)(int)requirementData[property.Key]), name: property.Key, type: 2, dropOptions: list);
				continue;
			}
			if (requirementType.Equals("IAMachineStateRequired") && property.Key.Equals("State"))
			{
				list = new List<string> { "Off", "On" };
				component.ConfigurePropertyInputUI(value: (requirementData == null || requirementData[property.Key] == null) ? ((object)0) : ((object)(int)requirementData[property.Key]), name: property.Key, type: 2, dropOptions: list);
				continue;
			}
			object obj;
			if (requirementData != null)
			{
				switch (property.Key)
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
				case "NPCSpawnID":
				case "QuestID":
				case "questID":
				case "QOID":
				case "QSIndex":
				case "QSValue":
				case "Level":
				case "Type":
				case "UserID":
				case "WarID":
				case "TargetCount":
				case "WarProgress":
					obj = ((requirementData[property.Key] == null) ? "" : ((string?)requirementData[property.Key]));
					break;
				case "Value":
				case "Not":
					obj = ((requirementData[property.Key] == null) ? ((object)false) : ((object)bool.Parse((string?)requirementData[property.Key])));
					break;
				case "Comparison":
					list = new List<string> { "=", ">=", "<=", ">", "<" };
					obj = ((requirementData[property.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)requirementData[property.Key])));
					break;
				case "NPCSpawnIDs":
				{
					obj = "";
					JArray jArray = (JArray)requirementData[property.Key];
					for (int i = 0; i < jArray.Count; i++)
					{
						obj = obj?.ToString() + jArray[i].ToString();
						if (i < jArray.Count - 1)
						{
							obj = obj?.ToString() + ",";
						}
					}
					break;
				}
				default:
					if (property.Value == 0 || property.Value == 3)
					{
						obj = "";
						break;
					}
					if (property.Value == 1)
					{
						obj = false;
						break;
					}
					list = new List<string> { "None" };
					obj = 0;
					break;
				}
			}
			else if (property.Value == 0 || property.Value == 3)
			{
				obj = "";
			}
			else if (property.Value == 1)
			{
				obj = false;
			}
			else
			{
				list = new List<string> { "=", ">=", "<=", ">", "<" };
				obj = 0;
			}
			component.ConfigurePropertyInputUI(property.Key, property.Value, obj, list);
		}
		ExistingRequirementsTable.Reposition();
		ExistingRequirementsScrollView.ResetPosition();
	}

	public void BuildEditItemsForNewRequirementProperties(JObject jData, Dictionary<string, int> properties, string requirementType)
	{
		AddNewRequirementPropertyScrollView.ResetPosition();
		foreach (KeyValuePair<string, int> property in properties)
		{
			GameObject obj = NewRequirementEditItemPool.Get();
			obj.transform.SetParent(AddRequirementPropertyItemPrefab.transform.parent, worldPositionStays: false);
			obj.SetActive(value: true);
			UINPCEditItem component = obj.GetComponent<UINPCEditItem>();
			List<string> list = new List<string>();
			if ((requirementType.Equals("IANPCStateRequired") || requirementType.Equals("IAStateOnMultipleNPCsRequired")) && property.Key.Equals("State"))
			{
				list = new List<string> { "Dead", "Alive" };
				component.ConfigurePropertyInputUI(value: (jData == null || jData[property.Key] == null) ? ((object)0) : ((object)(int)jData[property.Key]), name: property.Key, type: 2, dropOptions: list);
			}
			else if (requirementType.Equals("IAMachineStateRequired") && property.Key.Equals("State"))
			{
				list = new List<string> { "Off", "On" };
				component.ConfigurePropertyInputUI(value: (jData == null || jData[property.Key] == null) ? ((object)0) : ((object)(int)jData[property.Key]), name: property.Key, type: 2, dropOptions: list);
			}
			else
			{
				object obj2;
				if (jData != null)
				{
					switch (property.Key)
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
					case "NPCSpawnID":
					case "QuestID":
					case "questID":
					case "QOID":
					case "QSIndex":
					case "QSValue":
					case "Level":
					case "Type":
					case "UserID":
					case "WarID":
					case "TargetCount":
					case "WarProgress":
						obj2 = ((jData[property.Key] == null) ? "" : ((string?)jData[property.Key]));
						break;
					case "Value":
					case "Not":
						obj2 = ((jData[property.Key] == null) ? ((object)false) : ((object)bool.Parse((string?)jData[property.Key])));
						break;
					case "Comparison":
						list = new List<string> { "=", ">=", "<=", ">", "<" };
						obj2 = ((jData[property.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)jData[property.Key])));
						break;
					case "NPCSpawnIDs":
					{
						obj2 = "";
						JArray jArray = (JArray)jData[property.Key];
						for (int i = 0; i < jArray.Count; i++)
						{
							obj2 = obj2?.ToString() + jArray[i].ToString();
							if (i < jArray.Count - 1)
							{
								obj2 = obj2?.ToString() + ",";
							}
						}
						break;
					}
					default:
						if (property.Value == 0 || property.Value == 3)
						{
							obj2 = "";
							break;
						}
						if (property.Value == 1)
						{
							obj2 = false;
							break;
						}
						list = new List<string> { "None" };
						obj2 = 0;
						break;
					}
				}
				else if (property.Value == 0 || property.Value == 3)
				{
					obj2 = "";
				}
				else if (property.Value == 1)
				{
					obj2 = false;
				}
				else
				{
					list = new List<string> { "=", ">=", "<=", ">", "<" };
					obj2 = 0;
				}
				component.ConfigurePropertyInputUI(property.Key, property.Value, obj2, list);
			}
			AddRequirementPropertyEditItemList.Add(component);
		}
		AddRequirementTable.Reposition();
		AddNewRequirementPropertyScrollView.ResetPosition();
	}

	public void AddRequirementEntry()
	{
		if (selectedMapEntityItem == null)
		{
			return;
		}
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		string text = RequirementLabelAndClassnameDictionary[AddRequirementDropDownList.value];
		JObject jObject = new JObject(new JProperty("$type", text));
		foreach (UINPCEditItem addRequirementPropertyEditItem in AddRequirementPropertyEditItemList)
		{
			object obj;
			switch (addRequirementPropertyEditItem.type)
			{
			case 0:
			case 3:
				obj = addRequirementPropertyEditItem.GetTextValue();
				if (obj.ToString().Trim().Length == 0)
				{
					Debug.LogError("Textfield cannot be blank!");
					return;
				}
				break;
			case 1:
				obj = (addRequirementPropertyEditItem.GetCheckValue() ? true : false);
				break;
			case 2:
				obj = addRequirementPropertyEditItem.GetDropValue();
				break;
			default:
				obj = "";
				break;
			}
			if (addRequirementPropertyEditItem.name.text.Equals("NPCSpawnIDs"))
			{
				string[] array = ((string)obj).Split(',');
				JArray jArray = new JArray();
				string[] array2 = array;
				foreach (string s in array2)
				{
					jArray.Add(int.Parse(s));
				}
				obj = jArray;
			}
			if ((text.Equals("IANPCStateRequired") || text.Equals("IAStateOnMultipleNPCsRequired")) && addRequirementPropertyEditItem.name.text.Equals("State"))
			{
				obj = ((!addRequirementPropertyEditItem.GetDropValue().Equals("Dead")) ? 1 : 0);
			}
			if (text.Equals("IAMachineStateRequired") && addRequirementPropertyEditItem.name.text.Equals("State"))
			{
				obj = ((!addRequirementPropertyEditItem.GetDropValue().Equals("Off")) ? 1 : 0);
			}
			jObject.Add(new JProperty(addRequirementPropertyEditItem.name.text, obj));
		}
		val["requirements"].Add(jObject);
		string data = JsonConvert.SerializeObject(val);
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: false));
		ShowCurrentRequirementsPage();
	}

	public void UpdateRequirementEntry(string requirementClassName, List<UINPCEditItem> editItems, int index)
	{
		if (selectedMapEntityItem == null)
		{
			return;
		}
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		JObject jObject = null;
		int num = 0;
		foreach (dynamic item in val["requirements"])
		{
			num++;
			dynamic val2 = item["$type"];
			if ((string)val2 == requirementClassName && num == index)
			{
				jObject = item;
				break;
			}
		}
		foreach (UINPCEditItem editItem in editItems)
		{
			string text;
			switch (editItem.type)
			{
			case 0:
			case 3:
				text = editItem.GetTextValue();
				break;
			case 1:
				text = (editItem.GetCheckValue() ? "true" : "false");
				break;
			case 2:
				text = editItem.GetDropValue();
				break;
			default:
				text = "";
				break;
			}
			if ((requirementClassName.Equals("IANPCStateRequired") || requirementClassName.Equals("IAStateOnMultipleNPCsRequired")) && editItem.name.text.Equals("State"))
			{
				text = (editItem.GetDropValue().Equals("Dead") ? "0" : "1");
			}
			if (requirementClassName.Equals("IAMachineStateRequired") && editItem.name.text.Equals("State"))
			{
				text = (editItem.GetDropValue().Equals("Off") ? "0" : "1");
			}
			jObject[editItem.name.text] = text;
			if (editItem.name.text.Equals("NPCSpawnIDs"))
			{
				string[] array = text.Split(',');
				JArray jArray = new JArray();
				string[] array2 = array;
				foreach (string s in array2)
				{
					jArray.Add(int.Parse(s));
				}
				jObject[editItem.name.text] = jArray;
			}
		}
		string data = JsonConvert.SerializeObject(val);
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: false));
		ShowCurrentRequirementsPage();
	}

	public void UpdateActionEntry(string actionClassName, List<UINPCEditItem> editItems, int index)
	{
		if (selectedMapEntityItem == null)
		{
			return;
		}
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		JObject jObject = null;
		int num = 0;
		foreach (dynamic item in val["actions"])
		{
			num++;
			dynamic val2 = item["$type"];
			if ((string)val2 == actionClassName && num == index)
			{
				jObject = item;
				break;
			}
		}
		foreach (dynamic item2 in val["ctaActions"])
		{
			num++;
			dynamic val3 = item2["$type"];
			if ((string)val3 == actionClassName && num == index)
			{
				jObject = item2;
				break;
			}
		}
		if (val["exitCTActions"] != null)
		{
			foreach (dynamic item3 in val["exitCTActions"])
			{
				num++;
				dynamic val4 = item3["$type"];
				if ((string)val4 == actionClassName && num == index)
				{
					jObject = item3;
					break;
				}
			}
		}
		if (val["exitActions"] != null)
		{
			foreach (dynamic item4 in val["exitActions"])
			{
				num++;
				dynamic val5 = item4["$type"];
				if ((string)val5 == actionClassName && num == index)
				{
					jObject = item4;
					break;
				}
			}
		}
		if (val["stayCTActions"] != null)
		{
			foreach (dynamic item5 in val["stayCTActions"])
			{
				num++;
				dynamic val6 = item5["$type"];
				if ((string)val6 == actionClassName && num == index)
				{
					jObject = item5;
					break;
				}
			}
		}
		if (val["stayActions"] != null)
		{
			foreach (dynamic item6 in val["stayActions"])
			{
				num++;
				dynamic val7 = item6["$type"];
				if ((string)val7 == actionClassName && num == index)
				{
					jObject = item6;
					break;
				}
			}
		}
		foreach (UINPCEditItem editItem in editItems)
		{
			string text;
			switch (editItem.type)
			{
			case 0:
			case 3:
				text = editItem.GetTextValue();
				break;
			case 1:
				text = (editItem.GetCheckValue() ? "true" : "false");
				break;
			case 2:
				text = editItem.GetDropValue();
				break;
			default:
				text = "";
				break;
			}
			jObject[editItem.name.text] = text;
		}
		string data = JsonConvert.SerializeObject(val);
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: false));
	}

	public void UpdateMachineDetails()
	{
		if (selectedMapEntityItem == null || MachineTypeDropDownList.value.Trim() == "")
		{
			return;
		}
		if (EditMachineChosenAssetName == null || EditMachineChosenAssetName.Trim().Length < 1 || AssetBundleInput.textField.value == null || AssetBundleInput.textField.value.Trim().Length < 1)
		{
			Debug.LogError("ERROR: Asset Name and Asset Bundle cannot be empty!");
			return;
		}
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		((JValue)val["UniqueID"]).Value = int.Parse(retainer["UniqueID"]);
		((JValue)val["AssetBundle"]).Value = AssetBundleInput.textField.value;
		((JValue)val["AssetName"]).Value = EditMachineChosenAssetName;
		((JValue)val["MachineName"]).Value = MachineNameInput.largeTextField.value;
		((JValue)val["machineType"]).Value = MachineTypeDropDownList.value;
		((JValue)val["respawnTime"]).Value = Convert.ToInt32(RespawnTimeInput.GetTextValue());
		((JValue)val["combatBlocked"]).Value = CombatBlockedInput.GetCheckValue();
		((JValue)val["changeState"]).Value = ChangeStateInput.GetCheckValue();
		((JValue)val["ScaleX"]).Value = float.Parse(EditMachineScaleX.value);
		((JValue)val["ScaleY"]).Value = float.Parse(EditMachineScaleY.value);
		((JValue)val["ScaleZ"]).Value = float.Parse(EditMachineScaleZ.value);
		if (val["Animation"] == null)
		{
			val["Animation"] = new JValue("");
		}
		((JValue)val["Animation"]).Value = EditMachineAnimation.value.Trim();
		if (val["SFX"] == null)
		{
			val["SFX"] = new JValue("");
		}
		((JValue)val["SFX"]).Value = EditMachineSFX.value.Trim();
		if (val["toggleMode"] == null)
		{
			val["toggleMode"] = new JValue("");
		}
		((JValue)val["toggleMode"]).Value = ToggleModeDropDownList.value;
		if (val["collisionMode"] == null)
		{
			val["collisionMode"] = new JValue("");
		}
		((JValue)val["collisionMode"]).Value = CollisionModeDropDownList.value;
		if (val["CastAnimation"] == null)
		{
			val["CastAnimation"] = new JValue("");
		}
		((JValue)val["CastAnimation"]).Value = EditMachineCastAnimation.value.Trim();
		if (val["CastMessage"] == null)
		{
			val["CastMessage"] = new JValue("");
		}
		((JValue)val["CastMessage"]).Value = EditMachineCastMessage.value.Trim();
		if (val["CastTime"] == null)
		{
			val["CastTime"] = new JValue("");
		}
		((JValue)val["CastTime"]).Value = EditMachineCastTime.value.Trim();
		if (val["HideCastBar"] == null)
		{
			val["HideCastBar"] = new JValue("");
		}
		((JValue)val["HideCastBar"]).Value = EditMachineHideCastBarInput.GetCheckValue();
		if (val["DismountX"] == null)
		{
			val["DismountX"] = new JValue("");
		}
		((JValue)val["DismountX"]).Value = EditMachineDismountX.value.Trim();
		if (val["DismountY"] == null)
		{
			val["DismountY"] = new JValue("");
		}
		((JValue)val["DismountY"]).Value = EditMachineDismountY.value.Trim();
		if (val["DismountZ"] == null)
		{
			val["DismountZ"] = new JValue("");
		}
		((JValue)val["DismountZ"]).Value = EditMachineDismountZ.value.Trim();
		if (val["DismountRotation"] == null)
		{
			val["DismountRotation"] = new JValue("");
		}
		((JValue)val["DismountRotation"]).Value = EditMachineDismountRotation.value.Trim();
		if (val["State"] == null)
		{
			val["State"] = new JValue("");
		}
		((JValue)val["State"]).Value = EditMachineStateInput.GetCheckValue();
		selectedMapEntityItem.entity.Position.x = float.Parse(EditMachinePosX.value);
		selectedMapEntityItem.entity.Position.y = float.Parse(EditMachinePosY.value);
		selectedMapEntityItem.entity.Position.z = float.Parse(EditMachinePosZ.value);
		selectedMapEntityItem.entity.RotationY = int.Parse(EditMachineRotY.value);
		if (((string)val["machineType"]).Replace(" ", "").Equals("ResourceMachine"))
		{
			if (val["TradeSkillType"] == null)
			{
				val["TradeSkillType"] = new JValue("");
			}
			((JValue)val["TradeSkillType"]).Value = ResourceMachineTradeSkillTypeDropDownList.value;
			if (val["ResourceMachineItemType"] == null)
			{
				val["ResourceMachineItemType"] = new JValue("");
			}
			((JValue)val["ResourceMachineItemType"]).Value = ResourceMachineItemTypeDropDownList.value;
			if (val["ResourceMachineEquipItemSlot"] == null)
			{
				val["ResourceMachineEquipItemSlot"] = new JValue("");
			}
			((JValue)val["ResourceMachineEquipItemSlot"]).Value = ResourceMachineEquipItemSlotDropDownList.value;
			if (val["ResourceMachineRandomizeY"] == null)
			{
				val["ResourceMachineRandomizeY"] = new JValue("");
			}
			((JValue)val["ResourceMachineRandomizeY"]).Value = ResourceMachineRandomizeYToggle.value;
			if (val["ResourceMachinePower"] == null)
			{
				val["ResourceMachinePower"] = new JValue("");
			}
			((JValue)val["ResourceMachinePower"]).Value = ResourceMachinePowerInput.value.Trim();
			if (val["ResourceMachineTradeSkillLevel"] == null)
			{
				val["ResourceMachineTradeSkillLevel"] = new JValue("");
			}
			((JValue)val["ResourceMachineTradeSkillLevel"]).Value = ResourceMachineTradeSkillLevelInput.value.Trim();
		}
		string data = JsonConvert.SerializeObject(val);
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: false, selectedMapEntityItem.entity));
	}

	public void MoveMachineToMe()
	{
		if (!(selectedMapEntityItem == null) && !(MachineTypeDropDownList.value.Trim() == ""))
		{
			dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
			string data = JsonConvert.SerializeObject(val);
			AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: true));
		}
	}

	public void HideEM()
	{
		MEditor.SetActive(value: false);
	}

	public void ShowETP()
	{
		if (retainer.ContainsKey("AssetBundle"))
		{
			TPE_AssetBundle.value = retainer["AssetBundle"];
			TPE_AssetName.value = retainer["AssetName"];
			TPE_SpawnID.value = retainer["SpawnID"];
			TPE_AreaID.value = retainer["AreaID"];
			TPE_CellID.value = retainer["CellID"];
			TPE_ShowConfirmation.value = bool.Parse(retainer["ShowConfirmation"]);
			TPE_ScaleX.value = retainer["ScaleX"];
			TPE_ScaleY.value = retainer["ScaleY"];
			TPE_ScaleZ.value = retainer["ScaleZ"];
			TPE_KillAllMonsters.value = bool.Parse(retainer["KillAllMonsters"]);
		}
		TPEditor.SetActive(value: true);
	}

	public void HideETP()
	{
		TPEditor.SetActive(value: false);
	}

	public void RefreshRequirements()
	{
		BuildCurrentRequirementsPage();
	}

	private void resetMachineTypeDropdowns()
	{
		MachineTypeDropDownList.Clear();
		MachineTypeDropDownList.AddItem("Click Machine");
		MachineTypeDropDownList.AddItem("Collide Trigger Machine");
		MachineTypeDropDownList.AddItem("Owner Machine");
		MachineTypeDropDownList.AddItem("Pressure Machine");
		MachineTypeDropDownList.AddItem("Resource Machine");
		NewMachineTypeDropDownList.Clear();
		NewMachineTypeDropDownList.AddItem("Click Machine");
		NewMachineTypeDropDownList.AddItem("Collide Trigger Machine");
		NewMachineTypeDropDownList.AddItem("Owner Machine");
		NewMachineTypeDropDownList.AddItem("Pressure Machine");
		NewMachineTypeDropDownList.AddItem("Resource Machine");
	}

	private void BuildDetailsScreen()
	{
		if (selectedMapEntityItem == null)
		{
			return;
		}
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		string text = ((JValue)val["machineType"]).ToString().Replace(" ", "");
		MachineNameInput.largeTextField.value = ((JValue)val["MachineName"]).ToString();
		AssetBundleInput.textField.value = ((JValue)val["AssetBundle"]).ToString();
		EditMachineChosenAssetName = ((JValue)val["AssetName"]).ToString(CultureInfo.InvariantCulture);
		if (EditMachineAssetDropDownList.items.Contains(EditMachineChosenAssetName))
		{
			EditMachineAssetDropDownList.value = EditMachineChosenAssetName;
		}
		RespawnTimeInput.textField.value = ((JValue)val["respawnTime"]).ToString();
		ChangeStateInput.checkField.value = (bool)((JValue)val["changeState"]).Value;
		CombatBlockedInput.checkField.value = (bool)((JValue)val["combatBlocked"]).Value;
		EditMachineScaleX.value = ((JValue)val["ScaleX"]).Value.ToString();
		EditMachineScaleY.value = ((JValue)val["ScaleY"]).Value.ToString();
		EditMachineScaleZ.value = ((JValue)val["ScaleZ"]).Value.ToString();
		EditMachinePosX.value = selectedMapEntityItem.entity.Position.x.ToString();
		EditMachinePosY.value = selectedMapEntityItem.entity.Position.y.ToString();
		EditMachinePosZ.value = selectedMapEntityItem.entity.Position.z.ToString();
		EditMachineRotY.value = selectedMapEntityItem.entity.RotationY.ToString();
		EditMachineAnimation.value = ((val["Animation"] != null) ? ((JValue)val["Animation"]).Value.ToString() : "");
		EditMachineSFX.value = ((val["SFX"] != null) ? ((JValue)val["SFX"]).Value.ToString() : "");
		resetMachineTypeDropdowns();
		ToggleModeDropDownList.Clear();
		ToggleModeDropDownList.AddItem("Visibility");
		ToggleModeDropDownList.AddItem("Object Switch");
		CollisionModeDropDownList.Clear();
		CollisionModeDropDownList.AddItem("Enter");
		CollisionModeDropDownList.AddItem("Exit");
		CollisionModeDropDownList.AddItem("Stay");
		EditMachineCastAnimation.value = ((val["CastAnimation"] != null) ? ((JValue)val["CastAnimation"]).Value.ToString() : "");
		EditMachineCastMessage.value = ((val["CastMessage"] != null) ? ((JValue)val["CastMessage"]).Value.ToString() : "");
		EditMachineCastTime.value = ((val["CastTime"] != null) ? ((JValue)val["CastTime"]).Value.ToString() : "");
		EditMachineHideCastBarInput.checkField.value = (val["HideCastBar"] != null) && (bool)((JValue)val["HideCastBar"]).Value;
		EditMachineDismountX.value = ((val["DismountX"] != null) ? ((JValue)val["DismountX"]).Value.ToString() : "");
		EditMachineDismountY.value = ((val["DismountY"] != null) ? ((JValue)val["DismountY"]).Value.ToString() : "");
		EditMachineDismountZ.value = ((val["DismountZ"] != null) ? ((JValue)val["DismountZ"]).Value.ToString() : "");
		EditMachineDismountRotation.value = ((val["DismountRotation"] != null) ? ((JValue)val["DismountRotation"]).Value.ToString() : "");
		EditMachineStateInput.checkField.value = !((val["State"] != null) ? true : false) || (bool)((JValue)val["State"]).Value;
		for (int i = 0; i < MachineTypeDropDownList.items.Count; i++)
		{
			string pattern = "\\s";
			string replacement = "";
			if (Regex.Replace(MachineTypeDropDownList.items[i], pattern, replacement) == text)
			{
				MachineTypeDropDownList.value = MachineTypeDropDownList.items[i];
				break;
			}
		}
		if (val["toggleMode"] == null)
		{
			val["toggleMode"] = "Visibility";
		}
		ToggleModeDropDownList.value = (string)val["toggleMode"];
		if (val["collisionMode"] == null)
		{
			val["collisionMode"] = "Enter";
		}
		CollisionModeDropDownList.value = (string)val["collisionMode"];
		ResourceMachineTradeSkillTypeDropDownList.value = ((val["TradeSkillType"] != null) ? ((JValue)val["TradeSkillType"]).Value.ToString() : ResourceMachineTradeSkillTypeDropDownList.items[0]);
		ResourceMachineItemTypeDropDownList.value = ((val["ResourceMachineItemType"] != null) ? ((JValue)val["ResourceMachineItemType"]).Value.ToString() : ResourceMachineItemTypeDropDownList.items[0]);
		ResourceMachineEquipItemSlotDropDownList.value = ((val["ResourceMachineEquipItemSlot"] != null) ? ((JValue)val["ResourceMachineEquipItemSlot"]).Value.ToString() : ResourceMachineEquipItemSlotDropDownList.items[0]);
		ResourceMachineRandomizeYToggle.value = (val["ResourceMachineRandomizeY"] != null) && (bool)((JValue)val["ResourceMachineRandomizeY"]).Value;
		ResourceMachinePowerInput.value = ((val["ResourceMachinePower"] != null) ? ((JValue)val["ResourceMachinePower"]).Value.ToString() : "1");
		ResourceMachineTradeSkillLevelInput.value = ((val["ResourceMachineTradeSkillLevel"] != null) ? ((JValue)val["ResourceMachineTradeSkillLevel"]).Value.ToString() : "1");
		AEC.getInstance().sendRequest(new RequestMapAssets((int)(JToken)(JValue)val["AssetBundle"]));
	}

	private void BuildCurrentRequirementsPage()
	{
		if (selectedMapEntityItem == null)
		{
			return;
		}
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		dynamic val2 = val["requirements"];
		int num = 0;
		foreach (GameObject existingRequirementItem in ExistingRequirementItems)
		{
			if (existingRequirementItem != null)
			{
				UnityEngine.Object.Destroy(existingRequirementItem);
			}
		}
		ExistingRequirementItems.Clear();
		foreach (dynamic item in val2)
		{
			num++;
			GameObject gameObject = UnityEngine.Object.Instantiate(ExistingRequirementPrefab, ExistingRequirementsTableObject.transform);
			gameObject.SetActive(value: true);
			UIMapEditorExistingRequirementItem component = gameObject.GetComponent<UIMapEditorExistingRequirementItem>();
			component.index = num;
			string requirementType = (string)item["$type"];
			component.LabelTitle.text = RequirementLabelAndClassnameDictionary.FirstOrDefault((KeyValuePair<string, string> x) => x.Value == requirementType).Key;
			component.RequirementClassName = requirementType;
			ExistingRequirementItems.Add(gameObject);
			BuildInlinePropertyEditItemsForRequirement(item, GetRequirementProperites(requirementType), requirementType, component.PropertyEditItems);
		}
		CurrentRequirementCountLabel.text = num.ToString(CultureInfo.InvariantCulture) + " REQUIREMENT(S)";
		StartCoroutine(FixRequirementsScrollView());
	}

	private IEnumerator FixRequirementsScrollView()
	{
		ExistingRequirementsTable.Reposition();
		ExistingRequirementsScrollView.contentPivot = UIWidget.Pivot.TopLeft;
		ExistingRequirementsScrollView.ResetPosition();
		yield return null;
		ExistingRequirementsTable.Reposition();
		ExistingRequirementsScrollView.contentPivot = UIWidget.Pivot.TopLeft;
		ExistingRequirementsScrollView.ResetPosition();
	}

	private IEnumerator FixActionsScrollView()
	{
		ExistingActionsTable.Reposition();
		ExistingActionsScrollView.contentPivot = UIWidget.Pivot.TopLeft;
		ExistingActionsScrollView.ResetPosition();
		yield return null;
		ExistingActionsTable.Reposition();
		ExistingActionsScrollView.contentPivot = UIWidget.Pivot.TopLeft;
		ExistingActionsScrollView.ResetPosition();
	}

	protected override void Init()
	{
		base.Init();
		HideEM();
		HideMachineAdderWindow();
		EventDelegate.Add(SearchInput.onChange, UpdateSearch);
		ActionLabelAndClassnameDictionary.Add("Quest Objective", "MAQuestObjectiveCore");
		ActionLabelAndClassnameDictionary.Add("Exit Instance", "MAExitInstanceCore");
		ActionLabelAndClassnameDictionary.Add("Set NPC State", "MASetNPCStateCore");
		ActionLabelAndClassnameDictionary.Add("Set NPC Reaction", "MASetNPCReactionCore");
		ActionLabelAndClassnameDictionary.Add("Close Instance", "MACloseInstanceCore");
		ActionLabelAndClassnameDictionary.Add("Complete Cell", "MACompleteCellCore");
		ActionLabelAndClassnameDictionary.Add("Notify Cell", "MANotifyCellCore");
		ActionLabelAndClassnameDictionary.Add("Npc Notify Cell", "MANpcNotifyCellCore");
		ActionLabelAndClassnameDictionary.Add("Transfer Cell", "MATransferCellCore");
		ActionLabelAndClassnameDictionary.Add("Cast Spell", "MACastSpellCore");
		ActionLabelAndClassnameDictionary.Add("Dialogue", "CTADialogue");
		ActionLabelAndClassnameDictionary.Add("OpenApop", "CTAOpenApop");
		ActionLabelAndClassnameDictionary.Add("CTA Transfer Map", "CTATransferMap");
		ActionLabelAndClassnameDictionary.Add("Set Machine State", "MASetMachineStateCore");
		ActionLabelAndClassnameDictionary.Add("Trigger Machine", "MATriggerMachineCore");
		ActionLabelAndClassnameDictionary.Add("Abandon Quest", "MAAbandonQuestCore");
		RequirementLabelAndClassnameDictionary.Add("Bit Flag Required", "IABitFlagRequired");
		RequirementLabelAndClassnameDictionary.Add("Bit Flag Value Required", "IABitFlagValueRequired");
		RequirementLabelAndClassnameDictionary.Add("Class Equipped Required", "IAClassEquippedRequired");
		RequirementLabelAndClassnameDictionary.Add("Item Equipped Required", "IAItemEquippedRequired");
		RequirementLabelAndClassnameDictionary.Add("Item Required", "IAItemRequired");
		RequirementLabelAndClassnameDictionary.Add("Level Required", "IALevelRequired");
		RequirementLabelAndClassnameDictionary.Add("Machine State Required", "IAMachineStateRequired");
		RequirementLabelAndClassnameDictionary.Add("Map Required", "IAMapRequired");
		RequirementLabelAndClassnameDictionary.Add("NPC State Required", "IANPCStateRequired");
		RequirementLabelAndClassnameDictionary.Add("Player State Required", "IAPlayerStateRequired");
		RequirementLabelAndClassnameDictionary.Add("Quest Completed", "IAQuestCompleted");
		RequirementLabelAndClassnameDictionary.Add("Quest Objective Completed", "IAQuestObjectiveCompleted");
		RequirementLabelAndClassnameDictionary.Add("Quest Objective Required", "IAQuestObjectiveRequired");
		RequirementLabelAndClassnameDictionary.Add("Quest Required", "IAQuestRequired");
		RequirementLabelAndClassnameDictionary.Add("Quest String Required", "IAQuestStringRequired");
		RequirementLabelAndClassnameDictionary.Add("Trade Skill Level Required", "IATradeSkillLevelRequired");
		RequirementLabelAndClassnameDictionary.Add("User Required", "IAUserRequired");
		RequirementLabelAndClassnameDictionary.Add("War Progress Required", "IAWarProgressRequired");
		RequirementLabelAndClassnameDictionary.Add("All Monsters Cleared", "IAAllMonstersCleared");
		RequirementLabelAndClassnameDictionary.Add("Player Count Required", "IAPlayerCountRequired");
		RequirementLabelAndClassnameDictionary.Add("Multiple Monsters Dead", "IAStateOnMultipleNPCsRequired");
		EntityTemplate.SetActive(value: false);
		entityListPrefab.SetActive(value: false);
		entityCategories = new List<GameObject>();
		entityPool = new ObjectPool<GameObject>(EntityTemplate);
		ExistingRequirementPrefab.SetActive(value: false);
		ExistingRequirementPropertyItemPrefab.SetActive(value: false);
		ExistingActionPrefab.SetActive(value: false);
		ExistingActionPropertyItemPrefab.SetActive(value: false);
		AddRequirementPropertyItemPrefab.SetActive(value: false);
		AddRequirementPropertyEditItemList = new List<UINPCEditItem>();
		NewRequirementEditItemPool = new ObjectPool<GameObject>(AddRequirementPropertyItemPrefab);
		AddActionPropertyItemPrefab.SetActive(value: false);
		AddActionPropertyEditItemList = new List<UINPCEditItem>();
		A_S_Pool = new ObjectPool<GameObject>(AddActionPropertyItemPrefab);
		TransferPadAdderWindow.SetActive(value: false);
		base.gameObject.SetActive(value: false);
		AudioManager.Play2DSFX("UI_Bag_Open");
		Refresh();
		AEC.getInstance().sendRequest(new RequestFavoriteMapAssets());
	}

	private void PopulateMapEntities()
	{
		List<ComMapEntity> list = Game.Instance.AreaData.mapEntityPlayerSpawners;
		List<ComMapEntity> mapEntityTransferPads = Game.Instance.AreaData.mapEntityTransferPads;
		List<ComMapEntity> mapEntityMachines = Game.Instance.AreaData.mapEntityMachines;
		if (!string.IsNullOrEmpty(searchText))
		{
			list = list.Where((ComMapEntity x) => DoesEntityContainText(x, searchText)).ToList();
			mapEntityTransferPads = mapEntityTransferPads.Where((ComMapEntity x) => DoesEntityContainText(x, searchText)).ToList();
		}
		GameObject gameObject = entityPool.Get();
		gameObject.transform.SetParent(EntityTemplate.transform.parent, worldPositionStays: false);
		gameObject.SetActive(value: true);
		MapEntityDropDown component = gameObject.GetComponent<MapEntityDropDown>();
		component.Type = "Player Spawners";
		component.BuildList(list, searchText);
		entityCategories.Add(gameObject);
		gameObject = entityPool.Get();
		gameObject.transform.SetParent(EntityTemplate.transform.parent, worldPositionStays: false);
		gameObject.SetActive(value: true);
		nextMachineId = 1;
		MapEntityDropDown component2 = gameObject.GetComponent<MapEntityDropDown>();
		component2.Type = "Machines";
		component2.BuildList(mapEntityMachines, searchText, (selectedMapEntityItem != null) ? selectedMapEntityItem.entity : null);
		entityCategories.Add(gameObject);
	}

	public void checkIfEntityWasSelected(UIMapEntityItem uiMapEntityItem)
	{
		if (uiMapEntityItem.entity.ID == lastSelectedMapEntityID)
		{
			selectedMapEntityItem = uiMapEntityItem;
		}
	}

	public void UpdateAndRefresh(ComMapEntity updatedMapEntity = null, bool machineAdded = false)
	{
		if (updatedMapEntity != null && selectedMapEntityItem != null)
		{
			selectedMapEntityItem.entity = updatedMapEntity;
		}
		wasResponseUpdate = true;
		Refresh();
		base.gameObject.SetActive(value: true);
	}

	protected void Refresh()
	{
		EmptyList();
		PopulateMapEntities();
		EntityTable.Reposition();
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
		RefreshRequirements();
		RefreshActions();
		if (Game.Instance.TesterMode)
		{
			RefreshMachineLabels();
		}
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

	private bool DoesEntityContainText(ComMapEntity entity, string text)
	{
		text = text.ToLower();
		if (!entity.MapID.ToString(CultureInfo.InvariantCulture).Contains(text))
		{
			return entity.CellID.ToString(CultureInfo.InvariantCulture).Contains(text);
		}
		return true;
	}

	private void EmptyList()
	{
		foreach (GameObject entityCategory in entityCategories)
		{
			entityCategory.GetComponent<MapEntityDropDown>().Clear();
			entityPool.Release(entityCategory);
		}
		entityCategories.Clear();
	}

	public void AddPlayerSpawner()
	{
		AEC.getInstance().sendRequest(new RequestAddMapEntity(MapEntityTypes.PlayerSpawner, ""));
	}

	public void AddTransferPad()
	{
		if (string.IsNullOrEmpty(TPA_AssetBundle.value))
		{
			TPA_AssetBundle.value = "2240";
		}
		if (string.IsNullOrEmpty(TPA_AssetName.value))
		{
			TPA_AssetName.value = "TransferPad";
		}
		if (string.IsNullOrEmpty(TPA_ScaleX.value))
		{
			TPA_ScaleX.value = "1";
		}
		if (string.IsNullOrEmpty(TPA_ScaleY.value))
		{
			TPA_ScaleY.value = "1";
		}
		if (string.IsNullOrEmpty(TPA_ScaleZ.value))
		{
			TPA_ScaleZ.value = "1";
		}
		string text = Guid.NewGuid().ToString();
		JObject jObject = new JObject(new JProperty("AssetBundle", int.Parse(TPA_AssetBundle.value)), new JProperty("AssetName", TPA_AssetName.value), new JProperty("UniqueID", text), new JProperty("SpawnID", int.Parse(TPA_SpawnID.value)), new JProperty("AreaID", int.Parse(TPA_AreaID.value)), new JProperty("CellID", int.Parse(TPA_CellID.value)), new JProperty("ShowConfirmation", TPA_ShowConfirmation.value), new JProperty("ScaleX", int.Parse(TPA_ScaleX.value)), new JProperty("ScaleY", int.Parse(TPA_ScaleY.value)), new JProperty("ScaleZ", int.Parse(TPA_ScaleZ.value)), new JProperty("KillAllMonsters", false));
		retainer["AssetBundle"] = TPA_AssetBundle.value;
		retainer["AssetName"] = TPA_AssetName.value;
		retainer["UniqueID"] = text;
		retainer["SpawnID"] = TPA_SpawnID.value;
		retainer["AreaID"] = TPA_AreaID.value;
		retainer["CellID"] = TPA_CellID.value;
		retainer["ShowConfirmation"] = TPA_ShowConfirmation.value.ToString(CultureInfo.InvariantCulture);
		retainer["ScaleX"] = TPA_ScaleX.value;
		retainer["ScaleY"] = TPA_ScaleY.value;
		retainer["ScaleZ"] = TPA_ScaleZ.value;
		retainer["KillAllMonsters"] = false.ToString(CultureInfo.InvariantCulture);
		AEC.getInstance().sendRequest(new RequestAddMapEntity(MapEntityTypes.TransferPad, jObject.ToString()));
	}

	public void UpdatePlayerSpawner()
	{
		HideEditPlayerSpawner();
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.PlayerSpawner, selectedMapEntityItem.entity.Data, selectedMapEntityItem.entity.ID, MoveToMe: true));
	}

	public void UpdateTransferPad()
	{
		if (!retainer.ContainsKey("UniqueID"))
		{
			Debug.LogError("No uniqueID found. Update transfer pad failed");
			return;
		}
		if (string.IsNullOrEmpty(TPE_AssetBundle.value))
		{
			TPE_AssetBundle.value = "2240";
		}
		if (string.IsNullOrEmpty(TPE_AssetName.value))
		{
			TPE_AssetName.value = "TransferPad";
		}
		if (string.IsNullOrEmpty(TPE_ScaleX.value))
		{
			TPE_ScaleX.value = "1";
		}
		if (string.IsNullOrEmpty(TPE_ScaleY.value))
		{
			TPE_ScaleY.value = "1";
		}
		if (string.IsNullOrEmpty(TPE_ScaleZ.value))
		{
			TPE_ScaleZ.value = "1";
		}
		JObject jObject = new JObject(new JProperty("AssetBundle", int.Parse(TPE_AssetBundle.value)), new JProperty("MapEntityID", selectedMapEntityItem.entity.ID), new JProperty("AssetName", TPE_AssetName.value), new JProperty("UniqueID", retainer["UniqueID"]), new JProperty("SpawnID", int.Parse(TPE_SpawnID.value)), new JProperty("AreaID", int.Parse(TPE_AreaID.value)), new JProperty("CellID", int.Parse(TPE_CellID.value)), new JProperty("ShowConfirmation", TPE_ShowConfirmation.value), new JProperty("ScaleX", int.Parse(TPE_ScaleX.value)), new JProperty("ScaleY", int.Parse(TPE_ScaleY.value)), new JProperty("ScaleZ", int.Parse(TPE_ScaleZ.value)), new JProperty("KillAllMonsters", TPE_KillAllMonsters.value));
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.TransferPad, jObject.ToString(), selectedMapEntityItem.entity.ID, MoveToMe: false));
	}

	public void MoveTransferPadToPlayerLocation()
	{
		if (string.IsNullOrEmpty(TPE_AssetBundle.value))
		{
			TPE_AssetBundle.value = "2240";
		}
		if (string.IsNullOrEmpty(TPE_AssetName.value))
		{
			TPE_AssetName.value = "TransferPad";
		}
		if (string.IsNullOrEmpty(TPE_ScaleX.value))
		{
			TPE_ScaleX.value = "1";
		}
		if (string.IsNullOrEmpty(TPE_ScaleY.value))
		{
			TPE_ScaleY.value = "1";
		}
		if (string.IsNullOrEmpty(TPE_ScaleZ.value))
		{
			TPE_ScaleZ.value = "1";
		}
		JObject jObject = new JObject(new JProperty("AssetBundle", int.Parse(TPE_AssetBundle.value)), new JProperty("MapEntityID", selectedMapEntityItem.entity.ID), new JProperty("AssetName", TPE_AssetName.value), new JProperty("UniqueID", Guid.NewGuid().ToString()), new JProperty("SpawnID", int.Parse(TPE_SpawnID.value)), new JProperty("AreaID", int.Parse(TPE_AreaID.value)), new JProperty("CellID", int.Parse(TPE_CellID.value)), new JProperty("ShowConfirmation", TPE_ShowConfirmation.value), new JProperty("ScaleX", int.Parse(TPE_ScaleX.value)), new JProperty("ScaleY", int.Parse(TPE_ScaleY.value)), new JProperty("ScaleZ", int.Parse(TPE_ScaleZ.value)), new JProperty("KillAllMonsters", TPE_KillAllMonsters.value));
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.TransferPad, jObject.ToString(), selectedMapEntityItem.entity.ID, MoveToMe: true));
	}

	public void OnMapEntityClicked(UIMapEntityItem selected)
	{
		if (selected == null)
		{
			return;
		}
		selectedMapEntityItem = selected;
		lastSelectedMapEntityID = selected.entity.ID;
		foreach (GameObject entityCategory in entityCategories)
		{
			foreach (GameObject content in entityCategory.GetComponent<MapEntityDropDown>().Contents)
			{
				UIMapEntityItem component = content.GetComponent<UIMapEntityItem>();
				if (component.entity == selected.entity)
				{
					component.Highlight.SetActive(value: true);
				}
				else
				{
					component.Highlight.SetActive(value: false);
				}
			}
		}
		if (selected.entity.Type == MapEntityTypes.TransferPad)
		{
			ShowETP();
			dynamic val = JsonConvert.DeserializeObject<object>(selected.entity.Data);
			TPE_ShowConfirmation.value = (bool)val["ShowConfirmation"];
			retainer["ShowConfirmation"] = TPE_ShowConfirmation.value.ToString(CultureInfo.InvariantCulture);
			Dictionary<string, string> dictionary = retainer;
			string value = (TPE_AssetBundle.value = (string)val["AssetBundle"]);
			dictionary["AssetBundle"] = value;
			Dictionary<string, string> dictionary2 = retainer;
			value = (TPE_AssetName.value = (string)val["AssetName"]);
			dictionary2["AssetName"] = value;
			retainer["UniqueID"] = (string)val["UniqueID"];
			Dictionary<string, string> dictionary3 = retainer;
			value = (TPE_SpawnID.value = (string)val["SpawnID"]);
			dictionary3["SpawnID"] = value;
			Dictionary<string, string> dictionary4 = retainer;
			value = (TPE_AreaID.value = (string)val["AreaID"]);
			dictionary4["AreaID"] = value;
			Dictionary<string, string> dictionary5 = retainer;
			value = (TPE_CellID.value = (string)val["CellID"]);
			dictionary5["CellID"] = value;
			Dictionary<string, string> dictionary6 = retainer;
			value = (TPE_ScaleX.value = (string)val["ScaleX"]);
			dictionary6["ScaleX"] = value;
			Dictionary<string, string> dictionary7 = retainer;
			value = (TPE_ScaleY.value = (string)val["ScaleY"]);
			dictionary7["ScaleY"] = value;
			Dictionary<string, string> dictionary8 = retainer;
			value = (TPE_ScaleZ.value = (string)val["ScaleZ"]);
			dictionary8["ScaleZ"] = value;
			TPE_KillAllMonsters.value = (bool)val["KillAllMonsters"];
			retainer["KillAllMonsters"] = TPE_KillAllMonsters.value.ToString(CultureInfo.InvariantCulture);
		}
		else if (selected.entity.Type == MapEntityTypes.PlayerSpawner)
		{
			ShowEditPlayerSpawner();
		}
		else
		{
			if (selected.entity.Type != MapEntityTypes.Machine)
			{
				return;
			}
			dynamic val2 = JsonConvert.DeserializeObject<object>(selected.entity.Data);
			retainer["UniqueID"] = ((int)val2["UniqueID"]).ToString(CultureInfo.InvariantCulture);
			ShowMachineEditor();
			BuildDetailsScreen();
			RefreshRequirements();
			RefreshActions();
			ShowDetailsPage();
			if (((string)val2["machineType"]).Replace(" ", "").Equals("ResourceMachine"))
			{
				ResourceMachinePropertiesWindow.SetActive(value: true);
				if (ResourceMachineTradeSkillTypeDropDownList.items.Count < 2)
				{
					ResourceMachineTradeSkillTypeDropDownList.items.AddRange(Enum.GetNames(typeof(TradeSkillType)).ToList());
				}
				if (ResourceMachineItemTypeDropDownList.items.Count < 2)
				{
					ResourceMachineItemTypeDropDownList.items.AddRange(Enum.GetNames(typeof(ItemType)).ToList());
				}
				if (ResourceMachineEquipItemSlotDropDownList.items.Count < 2)
				{
					ResourceMachineEquipItemSlotDropDownList.items.AddRange(Enum.GetNames(typeof(EquipItemSlot)).ToList());
				}
			}
		}
	}

	private void BuildCurrentActionsPage()
	{
		if (selectedMapEntityItem == null)
		{
			return;
		}
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		dynamic val2 = val["actions"];
		dynamic val3 = val["ctaActions"];
		dynamic val4 = val["exitCTActions"];
		dynamic val5 = val["exitActions"];
		dynamic val6 = val["stayCTActions"];
		dynamic val7 = val["stayActions"];
		int num = 0;
		foreach (GameObject existingActionItem in ExistingActionItems)
		{
			if (existingActionItem != null)
			{
				UnityEngine.Object.Destroy(existingActionItem);
			}
		}
		ExistingActionItems.Clear();
		foreach (dynamic item in val2)
		{
			num++;
			GameObject gameObject = UnityEngine.Object.Instantiate(ExistingActionPrefab, ExistingActionsTableObject.transform);
			gameObject.SetActive(value: true);
			UIMapEditorExistingActionItem component = gameObject.GetComponent<UIMapEditorExistingActionItem>();
			component.index = num;
			string actionType = (string)item["$type"];
			component.LabelTitle.text = ActionLabelAndClassnameDictionary.FirstOrDefault((KeyValuePair<string, string> x) => x.Value == actionType).Key;
			component.ActionClassName = actionType;
			ExistingActionItems.Add(gameObject);
			BuildInlinePropertyEditItemsForAction(item, GetActionProperties(actionType), actionType, component.PropertyEditItems);
		}
		foreach (dynamic item2 in val3)
		{
			num++;
			GameObject gameObject2 = UnityEngine.Object.Instantiate(ExistingActionPrefab, ExistingActionsTableObject.transform);
			gameObject2.SetActive(value: true);
			UIMapEditorExistingActionItem component2 = gameObject2.GetComponent<UIMapEditorExistingActionItem>();
			component2.index = num;
			string actionType = (string)item2["$type"];
			component2.LabelTitle.text = ActionLabelAndClassnameDictionary.FirstOrDefault((KeyValuePair<string, string> x) => x.Value == actionType).Key;
			component2.ActionClassName = actionType;
			ExistingActionItems.Add(gameObject2);
			BuildInlinePropertyEditItemsForAction(item2, GetActionProperties(actionType), actionType, component2.PropertyEditItems);
		}
		if (val4 != null)
		{
			foreach (dynamic item3 in val4)
			{
				num++;
				GameObject gameObject3 = UnityEngine.Object.Instantiate(ExistingActionPrefab, ExistingActionsTableObject.transform);
				gameObject3.SetActive(value: true);
				UIMapEditorExistingActionItem component3 = gameObject3.GetComponent<UIMapEditorExistingActionItem>();
				component3.index = num;
				string actionType = (string)item3["$type"];
				component3.LabelTitle.text = ActionLabelAndClassnameDictionary.FirstOrDefault((KeyValuePair<string, string> x) => x.Value == actionType).Key;
				component3.ActionClassName = actionType;
				ExistingActionItems.Add(gameObject3);
				BuildInlinePropertyEditItemsForAction(item3, GetActionProperties(actionType), actionType, component3.PropertyEditItems, "Exit");
			}
		}
		if (val5 != null)
		{
			foreach (dynamic item4 in val5)
			{
				num++;
				GameObject gameObject4 = UnityEngine.Object.Instantiate(ExistingActionPrefab, ExistingActionsTableObject.transform);
				gameObject4.SetActive(value: true);
				UIMapEditorExistingActionItem component4 = gameObject4.GetComponent<UIMapEditorExistingActionItem>();
				component4.index = num;
				string actionType = (string)item4["$type"];
				component4.LabelTitle.text = ActionLabelAndClassnameDictionary.FirstOrDefault((KeyValuePair<string, string> x) => x.Value == actionType).Key;
				component4.ActionClassName = actionType;
				ExistingActionItems.Add(gameObject4);
				BuildInlinePropertyEditItemsForAction(item4, GetActionProperties(actionType), actionType, component4.PropertyEditItems, "Exit");
			}
		}
		if (val6 != null)
		{
			foreach (dynamic item5 in val6)
			{
				num++;
				GameObject gameObject5 = UnityEngine.Object.Instantiate(ExistingActionPrefab, ExistingActionsTableObject.transform);
				gameObject5.SetActive(value: true);
				UIMapEditorExistingActionItem component5 = gameObject5.GetComponent<UIMapEditorExistingActionItem>();
				component5.index = num;
				string actionType = (string)item5["$type"];
				component5.LabelTitle.text = ActionLabelAndClassnameDictionary.FirstOrDefault((KeyValuePair<string, string> x) => x.Value == actionType).Key;
				component5.ActionClassName = actionType;
				ExistingActionItems.Add(gameObject5);
				BuildInlinePropertyEditItemsForAction(item5, GetActionProperties(actionType), actionType, component5.PropertyEditItems, "Stay");
			}
		}
		if (val7 != null)
		{
			foreach (dynamic item6 in val7)
			{
				num++;
				GameObject gameObject6 = UnityEngine.Object.Instantiate(ExistingActionPrefab, ExistingActionsTableObject.transform);
				gameObject6.SetActive(value: true);
				UIMapEditorExistingActionItem component6 = gameObject6.GetComponent<UIMapEditorExistingActionItem>();
				component6.index = num;
				string actionType = (string)item6["$type"];
				component6.LabelTitle.text = ActionLabelAndClassnameDictionary.FirstOrDefault((KeyValuePair<string, string> x) => x.Value == actionType).Key;
				component6.ActionClassName = actionType;
				ExistingActionItems.Add(gameObject6);
				BuildInlinePropertyEditItemsForAction(item6, GetActionProperties(actionType), actionType, component6.PropertyEditItems, "Stay");
			}
		}
		CurrentActionCountLabel.text = num.ToString(CultureInfo.InvariantCulture) + " ACTION(S)";
		StartCoroutine(FixActionsScrollView());
	}

	public void BuildInlinePropertyEditItemsForAction(JObject actionData, Dictionary<string, int> properties, string actionType, List<UINPCEditItem> propertyItemList, string pressureMachineCollisionMode = "Enter")
	{
		foreach (KeyValuePair<string, int> property in properties)
		{
			GameObject gameObject = null;
			gameObject = UnityEngine.Object.Instantiate(ExistingActionPropertyItemPrefab);
			ExistingActionItems.Add(gameObject);
			gameObject.transform.SetParent(ExistingActionPropertyItemPrefab.transform.parent, worldPositionStays: false);
			gameObject.SetActive(value: true);
			UINPCEditItem component = gameObject.GetComponent<UINPCEditItem>();
			propertyItemList.Add(component);
			List<string> list = new List<string>();
			object value;
			if (actionData != null)
			{
				switch (property.Key)
				{
				case "ID":
				case "ApopID":
				case "QuestID":
				case "questID":
				case "QOID":
				case "NPCSpawnID":
				case "State":
				case "MapID":
				case "CellID":
				case "SpawnID":
				case "teamID":
				case "adjustment":
				case "warID":
				case "ItemID":
				case "Quantity":
				case "Message":
				case "cellID":
				case "key":
				case "value":
				case "level":
				case "duration":
				case "SpellId":
				case "SpellModeValue":
				case "CastAsLevelMultiplier":
				case "MachineID":
					value = ((actionData[property.Key] == null) ? "" : ((string?)actionData[property.Key]));
					break;
				case "MinDelay":
				case "MaxDelay":
					value = ((actionData[property.Key] == null) ? "0" : ((string?)actionData[property.Key]));
					break;
				case "AutoEquip":
				case "lockState":
				case "pathingActive":
				case "allowRegeneration":
				case "ShowConfirmation":
				case "SkipCompleteAction":
				case "IsAOE":
				case "PlayerOnly":
					value = ((actionData[property.Key] == null) ? ((object)false) : ((object)bool.Parse((string?)actionData[property.Key])));
					break;
				case "behavior":
					list = Enum.GetNames(typeof(NPCMovementBehavior)).ToList();
					value = ((actionData[property.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)actionData[property.Key])));
					break;
				case "TargetType":
					list = Enum.GetNames(typeof(CombatSolver.MachineTargetType)).ToList();
					value = ((actionData[property.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)actionData[property.Key])));
					break;
				case "SpellMode":
					list = Enum.GetNames(typeof(MachineSpellMode)).ToList();
					value = ((actionData[property.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)actionData[property.Key])));
					break;
				case "Reaction":
					list = Enum.GetNames(typeof(Entity.Reaction)).ToList();
					value = ((actionData[property.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)actionData[property.Key])));
					break;
				case "NotificationType":
					list = new List<string> { "Standard", "Chat", "Both" };
					value = ((actionData[property.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)actionData[property.Key])));
					break;
				case "CollisionMode":
					list = new List<string> { "Enter", "Exit", "Stay" };
					value = list.IndexOf(pressureMachineCollisionMode);
					break;
				default:
					if (property.Value == 0 || property.Value == 3)
					{
						value = "";
						break;
					}
					if (property.Value == 1)
					{
						value = false;
						break;
					}
					list = new List<string> { "None" };
					value = 0;
					break;
				}
			}
			else if (property.Value == 0 || property.Value == 3)
			{
				value = "";
			}
			else if (property.Value == 1)
			{
				value = false;
			}
			else
			{
				list = property.Key switch
				{
					"behavior" => Enum.GetNames(typeof(NPCMovementBehavior)).ToList(), 
					"Reaction" => Enum.GetNames(typeof(Entity.Reaction)).ToList(), 
					"NotificationType" => new List<string> { "Standard", "Chat", "Both" }, 
					"TargetType" => Enum.GetNames(typeof(CombatSolver.MachineTargetType)).ToList(), 
					"SpellMode" => Enum.GetNames(typeof(MachineSpellMode)).ToList(), 
					"CollisionMode" => new List<string> { "Enter", "Exit", "Stay" }, 
					_ => new List<string> { "=", ">=", "<=", ">", "<" }, 
				};
				value = 0;
			}
			component.ConfigurePropertyInputUI(property.Key, property.Value, value, list);
		}
		ExistingActionsTable.Reposition();
		ExistingActionsScrollView.ResetPosition();
	}

	public void BuildPropertyListForAction(JObject jData, Dictionary<string, int> props)
	{
		AddActionPropertiesScrollView.ResetPosition();
		foreach (KeyValuePair<string, int> prop in props)
		{
			GameObject obj = A_S_Pool.Get();
			obj.transform.SetParent(AddActionPropertyItemPrefab.transform.parent, worldPositionStays: false);
			obj.SetActive(value: true);
			UINPCEditItem component = obj.GetComponent<UINPCEditItem>();
			List<string> list = new List<string>();
			object value;
			if (jData != null)
			{
				switch (prop.Key)
				{
				case "ID":
				case "ApopID":
				case "QuestID":
				case "questID":
				case "QOID":
				case "NPCSpawnID":
				case "State":
				case "MapID":
				case "CellID":
				case "SpawnID":
				case "teamID":
				case "adjustment":
				case "warID":
				case "ItemID":
				case "Quantity":
				case "Message":
				case "cellID":
				case "key":
				case "value":
				case "level":
				case "duration":
				case "SpellId":
				case "SpellModeValue":
				case "CastAsLevelMultiplier":
				case "MachineID":
					value = ((jData[prop.Key] == null) ? "" : ((string?)jData[prop.Key]));
					break;
				case "MinDelay":
				case "MaxDelay":
					value = ((jData[prop.Key] == null) ? "0" : ((string?)jData[prop.Key]));
					break;
				case "AutoEquip":
				case "lockState":
				case "pathingActive":
				case "allowRegeneration":
				case "ShowConfirmation":
				case "SkipCompleteAction":
				case "IsAOE":
				case "PlayerOnly":
					value = ((jData[prop.Key] == null) ? ((object)false) : ((object)bool.Parse((string?)jData[prop.Key])));
					break;
				case "behavior":
					list = Enum.GetNames(typeof(NPCMovementBehavior)).ToList();
					value = ((jData[prop.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)jData[prop.Key])));
					break;
				case "TargetType":
					list = Enum.GetNames(typeof(CombatSolver.MachineTargetType)).ToList();
					value = ((jData[prop.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)jData[prop.Key])));
					break;
				case "SpellMode":
					list = Enum.GetNames(typeof(MachineSpellMode)).ToList();
					value = ((jData[prop.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)jData[prop.Key])));
					break;
				case "Reaction":
					list = Enum.GetNames(typeof(Entity.Reaction)).ToList();
					value = ((jData[prop.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)jData[prop.Key])));
					break;
				case "NotificationType":
					list = new List<string> { "Standard", "Chat", "Both" };
					value = ((jData[prop.Key] == null) ? ((object)0) : ((object)list.IndexOf((string?)jData[prop.Key])));
					break;
				case "CollisionMode":
					list = new List<string> { "Enter", "Exit", "Stay" };
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
				value = "0";
			}
			else if (prop.Value == 1)
			{
				value = false;
			}
			else
			{
				list = prop.Key switch
				{
					"behavior" => Enum.GetNames(typeof(NPCMovementBehavior)).ToList(), 
					"Reaction" => Enum.GetNames(typeof(Entity.Reaction)).ToList(), 
					"NotificationType" => new List<string> { "Standard", "Chat", "Both" }, 
					"TargetType" => Enum.GetNames(typeof(CombatSolver.MachineTargetType)).ToList(), 
					"SpellMode" => Enum.GetNames(typeof(MachineSpellMode)).ToList(), 
					"CollisionMode" => new List<string> { "Enter", "Exit", "Stay" }, 
					_ => new List<string> { "=", ">=", "<=", ">", "<" }, 
				};
				value = 0;
			}
			component.ConfigurePropertyInputUI(prop.Key, prop.Value, value, list);
			AddActionPropertyEditItemList.Add(component);
		}
		A_S_Table.Reposition();
		AddActionPropertiesScrollView.ResetPosition();
	}

	private Dictionary<string, int> GetActionProperties(string type)
	{
		Dictionary<string, int> dictionary = type switch
		{
			"CTADialogue" => new Dictionary<string, int>
			{
				{ "ID", 0 },
				{ "SkipCompleteAction", 1 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"CTAOpenApop" => new Dictionary<string, int>
			{
				{ "ApopID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"CTATransferMap" => new Dictionary<string, int>
			{
				{ "MapID", 0 },
				{ "CellID", 0 },
				{ "SpawnID", 0 },
				{ "ShowConfirmation", 1 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MATransferTeamToMapCore" => new Dictionary<string, int>
			{
				{ "MapID", 0 },
				{ "CellID", 0 },
				{ "SpawnID", 0 },
				{ "teamID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MACastSpellCore" => new Dictionary<string, int>
			{
				{ "SpellId", 0 },
				{ "SpellMode", 2 },
				{ "SpellModeValue", 0 },
				{ "CastAsLevelMultiplier", 0 },
				{ "IsAOE", 1 },
				{ "TargetType", 2 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MATransferCellCore" => new Dictionary<string, int>
			{
				{ "CellID", 0 },
				{ "SpawnID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetMachineStateCore" => new Dictionary<string, int>
			{
				{ "MachineID", 0 },
				{ "State", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MATriggerMachineCore" => new Dictionary<string, int>
			{
				{ "MachineID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MATeleportCore" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetPlayerSpawnCore" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetNPCRegenerationCore" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "allowRegeneration", 1 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetNPCReactionCore" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "Reaction", 2 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetNPCPathingCore" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "pathingActive", 1 },
				{ "duration", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetNPCMovementBehaviorCore" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "behavior", 2 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetNPCLevelCore" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "level", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetAreaLockCore" => new Dictionary<string, int>
			{
				{ "lockState", 1 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetAreaFlagCore" => new Dictionary<string, int>
			{
				{ "key", 0 },
				{ "value", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MARemoveItemCore" => new Dictionary<string, int>
			{
				{ "ItemID", 0 },
				{ "Quantity", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MANpcNotifyCellCore" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "cellID", 0 },
				{ "Message", 3 },
				{ "NotificationType", 2 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MANotifyTeamCellCore" => new Dictionary<string, int>
			{
				{ "Message", 0 },
				{ "NotificationType", 2 },
				{ "teamID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MANotifyCellCore" => new Dictionary<string, int>
			{
				{ "Message", 3 },
				{ "NotificationType", 2 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 },
				{ "PlayerOnly", 1 }
			}, 
			"MAGiveItemCore" => new Dictionary<string, int>
			{
				{ "ItemID", 0 },
				{ "Quantity", 0 },
				{ "AutoEquip", 1 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MAEquipItemCore" => new Dictionary<string, int>
			{
				{ "ItemID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MACloseInstanceCore" => new Dictionary<string, int>
			{
				{ "MapID", 0 },
				{ "CellID", 0 },
				{ "SpawnID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MAAdjustWarMeterCore" => new Dictionary<string, int>
			{
				{ "warID", 0 },
				{ "adjustment", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MAAdjustScoreCore" => new Dictionary<string, int>
			{
				{ "teamID", 0 },
				{ "adjustment", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MAQuestObjectiveCore" => new Dictionary<string, int>
			{
				{ "QuestID", 0 },
				{ "QOID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MASetNPCStateCore" => new Dictionary<string, int>
			{
				{ "NPCSpawnID", 0 },
				{ "State", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MAAbandonQuestCore" => new Dictionary<string, int> { { "questID", 0 } }, 
			"MATransferMapCore" => new Dictionary<string, int>
			{
				{ "MapID", 0 },
				{ "CellID", 0 },
				{ "SpawnID", 0 },
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MAExitInstanceCore" => new Dictionary<string, int>
			{
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			"MACompleteCellCore" => new Dictionary<string, int>
			{
				{ "MinDelay", 0 },
				{ "MaxDelay", 0 }
			}, 
			_ => new Dictionary<string, int>(), 
		};
		if (MachineTypeDropDownList.value.Equals("Pressure Machine"))
		{
			dictionary.Add("CollisionMode", 2);
			dictionary.Add("StayPauseDuration", 0);
		}
		return dictionary;
	}

	public void RefreshActions()
	{
		BuildCurrentActionsPage();
	}

	public void ShowAddNewActionPage()
	{
		CurrentActionsScreenV2.SetActive(value: false);
		AddActionsScreen.SetActive(value: true);
		EmptyAddActionPropertyList();
		if (uiPopupPageNumber == 1)
		{
			List<string> list = new List<string>(ActionLabelAndClassnameDictionary.Keys);
			list.Sort();
			AddActionDropDownList.items = list;
		}
		else
		{
			AddActionDropDownList.items = new List<string> { "Set NPC Movement Behavior", "Set NPC Pathing", "Set NPC Reaction", "Set NPC Regeneration", "Set NPC State", "Set Player Spawn", "Teleport", "Transfer Cell", "Transfer Map", "Transfer Team To Map" };
		}
		AddActionDropDownList.value = null;
		AddActionDropDownList.value = AddActionDropDownList.items[0];
	}

	private void EmptyAddActionPropertyList()
	{
		foreach (UINPCEditItem addActionPropertyEditItem in AddActionPropertyEditItemList)
		{
			A_S_Pool.Release(addActionPropertyEditItem.gameObject);
		}
		AddActionPropertyEditItemList.Clear();
	}

	public void OnAddActionDropdownListItemSelected()
	{
		if (!(UIPopupList.current == null))
		{
			EmptyAddActionPropertyList();
			if (!ActionLabelAndClassnameDictionary.ContainsKey(UIPopupList.current.value.Trim()))
			{
				Debug.LogError("ERROR: dictionary did not contain key: " + UIPopupList.current.value.Trim());
			}
			else
			{
				BuildPropertyListForAction(null, GetActionProperties(ActionLabelAndClassnameDictionary[UIPopupList.current.value.Trim()]));
			}
		}
	}

	public void AddActionEntry()
	{
		if (selectedMapEntityItem == null)
		{
			return;
		}
		dynamic val = JsonConvert.DeserializeObject<object>(selectedMapEntityItem.entity.Data);
		string text = ActionLabelAndClassnameDictionary[AddActionDropDownList.value.Trim()];
		JObject jObject = new JObject(new JProperty("$type", text));
		foreach (UINPCEditItem addActionPropertyEditItem in AddActionPropertyEditItemList)
		{
			object obj;
			switch (addActionPropertyEditItem.type)
			{
			case 0:
			case 3:
				obj = addActionPropertyEditItem.GetTextValue();
				if (string.IsNullOrEmpty(obj.ToString()))
				{
					Debug.LogError("Cannot leave value blank");
					return;
				}
				if (text.Equals("MATriggerMachineCore") && addActionPropertyEditItem.name.text.Equals("MachineID"))
				{
					int result = -1;
					if (int.TryParse(addActionPropertyEditItem.GetTextValue(), out result))
					{
						obj = BaseMachine.GetMachineIDByMapEditorMachineID(result);
					}
				}
				break;
			case 1:
				obj = (addActionPropertyEditItem.GetCheckValue() ? true : false);
				break;
			case 2:
				obj = addActionPropertyEditItem.GetDropValue();
				break;
			default:
				obj = "";
				break;
			}
			jObject.Add(new JProperty(addActionPropertyEditItem.name.text, obj));
		}
		if (!text.Substring(0, 3).Equals("CTA", StringComparison.OrdinalIgnoreCase))
		{
			val["actions"].Add(jObject);
		}
		else
		{
			val["ctaActions"].Add(jObject);
		}
		string data = JsonConvert.SerializeObject(val);
		AEC.getInstance().sendRequest(new RequestUpdateMapEntity(MapEntityTypes.Machine, data, selectedMapEntityItem.entity.ID, MoveToMe: false));
		ShowActionsPage();
	}

	public override void Close()
	{
		base.Close();
		EmptyList();
		AudioManager.Play2DSFX("UI_Bag_Close");
	}

	protected override void Destroy()
	{
		base.Destroy();
		EmptyList();
		Instance = null;
	}

	protected override void Resume()
	{
		base.Resume();
		Refresh();
	}
}
