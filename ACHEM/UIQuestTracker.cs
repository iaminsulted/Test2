using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIQuestTracker : MonoBehaviour
{
	public UILabel ShortText;

	public UILabel ObjectiveText;

	public UISprite sprQuest;

	public UISprite sprCheck;

	public QuestCompass Compass;

	public UITexture sprCompass;

	public PulseColor bgPulse;

	public UILabel lblDistance;

	private Quest current;

	private Transform playerTform;

	private bool isInitialized;

	private float timeElapsed;

	private int lastDistance;

	private ITrackable target;

	private bool visible = true;

	private bool isPulsing;

	private bool isCompassVisible;

	public bool Visible
	{
		get
		{
			return visible;
		}
		set
		{
			if (visible != value)
			{
				visible = value;
				base.gameObject.SetActive(visible);
			}
		}
	}

	public bool IsPulsing
	{
		get
		{
			return isPulsing;
		}
		set
		{
			if (isPulsing != value)
			{
				isPulsing = value;
				bgPulse.enabled = isPulsing;
			}
		}
	}

	public bool IsCompassVisible
	{
		get
		{
			return isCompassVisible;
		}
		set
		{
			if (isCompassVisible != value)
			{
				isCompassVisible = value;
				if (isCompassVisible)
				{
					sprQuest.gameObject.SetActive(value: false);
					Compass.gameObject.SetActive(value: true);
					sprCompass.gameObject.SetActive(value: true);
					lblDistance.gameObject.SetActive(value: true);
				}
				else
				{
					sprQuest.gameObject.SetActive(value: true);
					Compass.gameObject.SetActive(value: false);
					sprCompass.gameObject.SetActive(value: false);
					lblDistance.gameObject.SetActive(value: false);
				}
			}
		}
	}

	private void Start()
	{
		Init(Entities.Instance.me.wrapper.transform, Game.Instance.cam.transform);
		UpdateTracker();
	}

	public void InitCompass()
	{
		GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("UIElements/QuestCompass"));
		gameObject.transform.SetParent(Game.Instance.transform, worldPositionStays: false);
		Compass = gameObject.GetComponent<QuestCompass>();
		sprQuest.gameObject.SetActive(value: true);
		Compass.gameObject.SetActive(value: false);
		sprCompass.gameObject.SetActive(value: false);
		lblDistance.gameObject.SetActive(value: false);
	}

	private void Init(Transform player, Transform camera)
	{
		InitCompass();
		playerTform = player;
		Compass.Setup(player, camera);
		isInitialized = true;
	}

	public void OnEnable()
	{
		Session.MyPlayerData.QuestObjectiveUpdated += OnQuestObjectiveUpdated;
		current = Session.MyPlayerData.CurrentlyTrackedQuest;
		ApopDownloader.LoadEnd += OnApopLoaded;
		InteractiveMachine.AnyMachineActiveUpdated += OnAnyMachineActiveUpdated;
		NPC.AnyNPCStateUpdated += OnAnyNPCStateUpdated;
	}

	public void OnDisable()
	{
		current = null;
		SetTarget(null);
		Session.MyPlayerData.QuestObjectiveUpdated -= OnQuestObjectiveUpdated;
		ApopDownloader.LoadEnd -= OnApopLoaded;
		InteractiveMachine.AnyMachineActiveUpdated -= OnAnyMachineActiveUpdated;
		NPC.AnyNPCStateUpdated -= OnAnyNPCStateUpdated;
	}

	private void Update()
	{
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= 1f)
		{
			timeElapsed = 0f;
			UpdateTracker();
		}
		if (!target.IsNull() && !(target.TrackedTransform == null))
		{
			int num = (int)(target.TrackedTransform.position - playerTform.position).magnitude;
			if (num != lastDistance)
			{
				lblDistance.text = num.ToString();
				lastDistance = num;
			}
		}
	}

	private void OnAnyNPCStateUpdated()
	{
		UpdateTracker();
	}

	private void OnAnyMachineActiveUpdated(InteractiveMachine machine)
	{
		UpdateTracker();
	}

	private void OnAnyNPCSpawnActiveUpdated(NPCSpawn spawn)
	{
		UpdateTracker();
	}

	private void SetTarget(ITrackable value, IndicatorFX indicatorFX = IndicatorFX.None)
	{
		if (target != value)
		{
			if (!target.IsNull())
			{
				target.Untrack();
			}
			target = value;
			if (!target.IsNull())
			{
				target.Track(indicatorFX);
			}
		}
	}

	private void OnActiveUpdated(bool obj)
	{
		UpdateTracker();
	}

	private void OnQuestObjectiveUpdated(int questID, int qoid)
	{
		if (current != null && current.ID == questID)
		{
			UpdateTracker();
		}
	}

	public void ShowQuest(Quest quest)
	{
		current = quest;
		if (current == null)
		{
			Visible = false;
			return;
		}
		Visible = true;
		UpdateTracker();
	}

	private void UpdateTracker()
	{
		if (current == null || !isInitialized)
		{
			return;
		}
		ITrackable closestWaypoint = null;
		IndicatorFX indicatorFX = IndicatorFX.None;
		QuestObjective curObj = null;
		sprCheck.gameObject.SetActive(value: false);
		IsCompassVisible = false;
		IsPulsing = false;
		ShortText.text = current.Name;
		ObjectiveText.text = "";
		if (Session.MyPlayerData.IsQuestComplete(current.ID))
		{
			if (current.TurnInType == QuestTurnInType.QuestGiver)
			{
				if (Game.Instance.AreaData != null && Game.Instance.AreaData.id == current.MapEndID)
				{
					ObjectiveText.text = "Quest Ready for Turn In!";
					if (Game.Instance.AreaData.currentCellID == current.TurnInNpcCellId)
					{
						NPC npcBySpawnId = Entities.Instance.GetNpcBySpawnId(current.TurnInNpcSpawnId);
						if (npcBySpawnId != null && npcBySpawnId.wrapper != null)
						{
							closestWaypoint = npcBySpawnId;
							indicatorFX = IndicatorFX.Beam;
						}
					}
				}
				else
				{
					foreach (NPCIA allApop in ApopMap.GetAllApops())
					{
						if (RecursiveSearchApops(allApop, current.ID))
						{
							NPC nPCByApopID = GetNPCByApopID(allApop.ID);
							if (nPCByApopID != null)
							{
								ObjectiveText.text = "Quest Ready for Turn In!";
								closestWaypoint = nPCByApopID;
								indicatorFX = IndicatorFX.Beam;
								break;
							}
						}
					}
					if (closestWaypoint == null)
					{
						ObjectiveText.text = "Travel to " + current.EndMapName;
						Game.Instance.GetWaypointByTargetMapIDs(new List<int> { current.MapEndID }, out closestWaypoint, out var _);
						indicatorFX = IndicatorFX.Beam;
					}
				}
				if (current.HideTurnIn)
				{
					ObjectiveText.text = "Travel to ???";
					closestWaypoint = null;
					indicatorFX = IndicatorFX.None;
				}
			}
			else
			{
				ObjectiveText.text = "Quest Complete!";
				IsPulsing = true;
			}
		}
		else
		{
			string text = ((current.Objectives.Count > 1) ? " ..." : "");
			foreach (QuestObjective objective in current.Objectives)
			{
				if (Session.MyPlayerData.IsQuestObjectiveInProgress(current.ID, objective.ID))
				{
					curObj = objective;
					break;
				}
			}
			if (curObj != null)
			{
				if (curObj.IsMapHidden || (Game.Instance.AreaData != null && Game.Instance.AreaData.id == curObj.MapID))
				{
					ObjectiveText.text = Session.MyPlayerData.GetQuestObjectiveProgressText(curObj) + text;
					if (curObj.Type == QuestObjectiveType.Interact)
					{
						closestWaypoint = FindMachineByQOID(curObj.ID);
						indicatorFX = curObj.IndicatorFX;
					}
					else if ((curObj.Type == QuestObjectiveType.Talk || curObj.Type == QuestObjectiveType.Escort) && Game.Instance.AreaData.currentCellID == curObj.CellID)
					{
						if (curObj.RefIDs != null)
						{
							NPC npcBySpawnId2 = Entities.Instance.GetNpcBySpawnId(curObj.RefID);
							if (npcBySpawnId2 != null && npcBySpawnId2.wrapper != null)
							{
								closestWaypoint = npcBySpawnId2;
								indicatorFX = curObj.IndicatorFX;
							}
						}
						else if (AccessLevels.CanReceiveErrorMessages(Entities.Instance.me))
						{
							DisplayNullNPCIDError(curObj);
						}
					}
					else if (curObj.Type == QuestObjectiveType.Killcount)
					{
						float num = ((target.IsNull() || target.TrackedTransform == null) ? 2.1474836E+09f : Vector3.Distance(target.TrackedTransform.position, playerTform.position));
						if (curObj.RefIDs != null)
						{
							List<NPC> activeNpcsByNpcIds = Entities.Instance.GetActiveNpcsByNpcIds(curObj.RefIDs);
							if (activeNpcsByNpcIds.Count > 0)
							{
								foreach (NPC item2 in activeNpcsByNpcIds)
								{
									float num2 = Vector3.Distance(item2.position, playerTform.position);
									if ((num2 + 10f < num || (closestWaypoint == null && target == item2)) && item2.wrapper != null)
									{
										num = num2;
										closestWaypoint = item2;
									}
								}
							}
							else
							{
								foreach (KeyValuePair<int, List<int>> item3 in Game.Instance.AreaData.NpcSpawnInfos.Where((KeyValuePair<int, List<int>> x) => x.Value.Any((int y) => curObj.RefIDs.Contains(y))))
								{
									NPCSpawn npcSpawn = Game.Instance.GetNpcSpawn(item3.Key);
									if (npcSpawn == null)
									{
										continue;
									}
									NPC npcBySpawnId3 = Entities.Instance.GetNpcBySpawnId(npcSpawn.ID);
									if (npcBySpawnId3 != null && npcBySpawnId3.wrapper != null)
									{
										float num3 = Vector3.Distance(npcSpawn.transform.position, playerTform.position);
										if (num3 + 10f < num || (closestWaypoint == null && target == npcBySpawnId3))
										{
											num = num3;
											closestWaypoint = npcBySpawnId3;
										}
									}
								}
							}
						}
						else if (AccessLevels.CanReceiveErrorMessages(Entities.Instance.me))
						{
							DisplayNullNPCIDError(curObj);
						}
					}
					else if (curObj.Type == QuestObjectiveType.Turnin)
					{
						if (curObj.RefIDs != null)
						{
							float num4 = ((target.IsNull() || target.TrackedTransform == null) ? 2.1474836E+09f : Vector3.Distance(target.TrackedTransform.position, playerTform.position));
							foreach (NPC activeNPC in Entities.Instance.GetActiveNPCs())
							{
								if (activeNPC.questObjectiveItems == null)
								{
									continue;
								}
								foreach (int key in activeNPC.questObjectiveItems.Keys)
								{
									if (key != curObj.ID)
									{
										continue;
									}
									foreach (int item4 in activeNPC.questObjectiveItems[key])
									{
										if (curObj.RefIDs.Contains(item4))
										{
											float num6 = Vector3.Distance(activeNPC.position, playerTform.position);
											if ((num6 + 10f < num4 || (closestWaypoint == null && target == activeNPC)) && activeNPC.wrapper != null)
											{
												num4 = num6;
												closestWaypoint = activeNPC;
											}
											break;
										}
									}
									break;
								}
							}
							foreach (BaseMachine item5 in BaseMachine.Map.Values.Where((BaseMachine x) => x is ResourceMachine).ToList())
							{
								ResourceMachine resourceMachine = item5 as ResourceMachine;
								foreach (int refID in curObj.RefIDs)
								{
									if (resourceMachine.ContainsItem(refID))
									{
										float num7 = Vector3.Distance(resourceMachine.transform.position, playerTform.position);
										if (num7 + 10f < num4 || (closestWaypoint == null && target == resourceMachine))
										{
											num4 = num7;
											closestWaypoint = resourceMachine;
										}
									}
								}
							}
						}
						else if (AccessLevels.CanReceiveErrorMessages(Entities.Instance.me))
						{
							DisplayNullItemIDError(curObj);
						}
					}
				}
				else
				{
					ITrackable trackable = null;
					if (curObj.Type == QuestObjectiveType.Interact)
					{
						trackable = FindMachineByQOID(curObj.ID);
					}
					if (trackable != null)
					{
						ObjectiveText.text = Session.MyPlayerData.GetQuestObjectiveProgress(curObj) + "/" + curObj.Qty + " " + curObj.Desc + text;
						closestWaypoint = trackable;
						indicatorFX = curObj.IndicatorFX;
					}
					else
					{
						int mapIndex2 = 0;
						Game.Instance.GetWaypointByTargetMapIDs(curObj.MapIDs, out closestWaypoint, out mapIndex2);
						if (curObj.MapNames.Count > 0)
						{
							ObjectiveText.text = "Travel to " + curObj.MapNames[mapIndex2];
						}
						else if (AccessLevels.CanReceiveErrorMessages(Entities.Instance.me))
						{
							DisplayNullMapIDError(curObj);
						}
						indicatorFX = IndicatorFX.Beam;
					}
				}
				if (curObj.HideArrow)
				{
					closestWaypoint = null;
					indicatorFX = IndicatorFX.None;
				}
			}
		}
		SetTarget(closestWaypoint, indicatorFX);
		Compass.target = ((!target.IsNull()) ? target.TrackedTransform : null);
		IsCompassVisible = !target.IsNull();
	}

	private void OnApopLoaded(List<NPCIA> apops)
	{
		if (apops != null && current != null && Session.MyPlayerData.IsQuestComplete(current.ID))
		{
			UpdateTracker();
		}
	}

	private NPC GetNPCByApopID(int apopID)
	{
		foreach (NPC npc in Entities.Instance.NpcList)
		{
			if (npc.ApopIDs != null && npc.ApopIDs.Contains(apopID))
			{
				return npc;
			}
		}
		return null;
	}

	private bool RecursiveSearchApops(NPCIA npcia, int questID)
	{
		if (npcia is NPCIAAction)
		{
			NPCIAAction nPCIAAction = (NPCIAAction)npcia;
			if (nPCIAAction.Action is CTAQuestLoadCore)
			{
				return ((CTAQuestLoadCore)nPCIAAction.Action).TurnInQuestIDs.Contains(questID);
			}
		}
		if (npcia is NPCIAQuest)
		{
			return ((NPCIAQuest)npcia).QuestIDs.Contains(questID);
		}
		foreach (NPCIA child in npcia.children)
		{
			if (RecursiveSearchApops(child, questID))
			{
				return true;
			}
		}
		return false;
	}

	public void OnClick()
	{
		UIQuest.ShowLog(current);
	}

	private ITrackable FindMachineByQOID(int qoid)
	{
		List<BaseMachine> list = new List<BaseMachine>();
		foreach (BaseMachine value in BaseMachine.Map.Values)
		{
			if (value.IsActive && value is InteractiveMachine interactiveMachine && interactiveMachine.HasQuestObjective(qoid))
			{
				list.Add(interactiveMachine);
			}
		}
		if (list.Count == 1)
		{
			return list[0];
		}
		if (list.Count > 1)
		{
			BaseMachine result = null;
			float num = float.MaxValue;
			{
				foreach (BaseMachine item in list)
				{
					float num2 = Vector3.Distance(item.transform.position, playerTform.position);
					if (num2 < num)
					{
						result = item;
						num = num2;
					}
				}
				return result;
			}
		}
		return null;
	}

	private void DisplayNullNPCIDError(QuestObjective curObj)
	{
		ObjectiveText.text = "Quest Objective Error";
		Chat.SendAdminMessage("Error on Quest ID " + curObj.QuestID + " - Objective ID " + curObj.ID + " does not have at least one NPC ID assigned.");
	}

	private void DisplayNullItemIDError(QuestObjective curObj)
	{
		ObjectiveText.text = "Quest Objective Error";
		Chat.SendAdminMessage("Error on Quest ID " + curObj.QuestID + " - Objective ID " + curObj.ID + " does not have at least one item ID assigned.");
	}

	private void DisplayNullMapIDError(QuestObjective curObj)
	{
		ObjectiveText.text = "Quest Objective Error";
		Chat.SendAdminMessage("Error on Quest ID " + curObj.QuestID + " - Objective ID " + curObj.ID + " does not have at least one map ID assigned.");
	}
}
