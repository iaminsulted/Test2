using System;
using System.Collections.Generic;
using Assets.Scripts.Game;
using UnityEngine;

public class NPCIATrigger : MonoBehaviour, IClickable, IInteractable
{
	public Texture2D[] textures;

	public List<int> ApopIDs;

	public List<NPCIA> Apops = new List<NPCIA>();

	public List<NPCIA> RelatedApops = new List<NPCIA>();

	public Transform CameraSpot;

	public Transform FocusSpot;

	public string Title;

	public int MapID;

	public int CellID;

	public int SpawnID;

	private MyPlayerData myPlayerData;

	private Game game;

	public NPCIA CurrentApop
	{
		get
		{
			if (Apops != null && Apops.Count > 0)
			{
				foreach (NPCIA apop in Apops)
				{
					if (apop.IsAvailable() || apop.DontHideWhenLocked)
					{
						return apop;
					}
				}
			}
			return null;
		}
	}

	public event Action Click;

	public event Action OnApopLoad;

	private void OnRequirementUpdated()
	{
		SetSprite(CurrentApop);
	}

	public void OnDestroy()
	{
		Clear();
	}

	private void Clear()
	{
		myPlayerData.QuestObjectiveUpdated -= OnQuestObjectiveUpdated;
		myPlayerData.QuestAdded -= OnQuestUpdated;
		myPlayerData.QuestRemoved -= OnQuestUpdated;
		game.QuestLoaded -= OnQuestLoaded;
		ApopMap.Cleared -= OnApopMapCleared;
		NPCIA.OnInitialize -= TryAddApop;
		foreach (NPCIA relatedApop in RelatedApops)
		{
			relatedApop.RequirementUpdated -= OnRequirementUpdated;
		}
		Apops.Clear();
		RelatedApops.Clear();
	}

	public void Init(List<int> apopIDs, string title, Transform cameraSpot, Transform focusSpot, int spawnID)
	{
		base.gameObject.SetActive(value: false);
		ApopIDs = apopIDs;
		SpawnID = spawnID;
		Title = title;
		MapID = Game.Instance.AreaData.id;
		CellID = Game.Instance.AreaData.currentCellID;
		CameraSpot = cameraSpot;
		FocusSpot = focusSpot;
		myPlayerData = Session.MyPlayerData;
		game = Game.Instance;
		myPlayerData.QuestObjectiveUpdated -= OnQuestObjectiveUpdated;
		myPlayerData.QuestAdded -= OnQuestUpdated;
		myPlayerData.QuestRemoved -= OnQuestUpdated;
		game.QuestLoaded -= OnQuestLoaded;
		ApopMap.Cleared -= OnApopMapCleared;
		myPlayerData.QuestObjectiveUpdated += OnQuestObjectiveUpdated;
		myPlayerData.QuestAdded += OnQuestUpdated;
		myPlayerData.QuestRemoved += OnQuestUpdated;
		game.QuestLoaded += OnQuestLoaded;
		ApopMap.Cleared += OnApopMapCleared;
		ApopDownloader.GetApops(ApopIDs, OnApopLoadEnded);
	}

	private void OnApopMapCleared()
	{
		Clear();
		Init(ApopIDs, Title, CameraSpot, FocusSpot, SpawnID);
	}

	private void OnApopLoadEnded(List<NPCIA> loadedApops)
	{
		if (ApopIDs == null || loadedApops == null || loadedApops.Count == 0)
		{
			return;
		}
		foreach (int apopID in ApopIDs)
		{
			TryAddApop(ApopMap.GetApop(apopID));
		}
		SetCameraAndFocus(CameraSpot, FocusSpot);
		this.OnApopLoad?.Invoke();
	}

	public void SetCameraAndFocus(Transform cameraSpot, Transform focusSpot)
	{
		CameraSpot = cameraSpot;
		FocusSpot = focusSpot;
	}

	private void TryAddApop(NPCIA apop)
	{
		if (apop == null || Apops.Count == ApopIDs.Count)
		{
			return;
		}
		if (ApopIDs.Contains(apop.ID) && !Apops.Contains(apop))
		{
			Apops.Add(apop);
			RelatedApops.AddRange(apop.GetSelfAndRelated());
		}
		if (Apops.Count != ApopIDs.Count)
		{
			return;
		}
		foreach (NPCIA relatedApop in RelatedApops)
		{
			relatedApop.RequirementUpdated += OnRequirementUpdated;
		}
		SetSprite(CurrentApop);
	}

	private void OnQuestObjectiveUpdated(int qid, int qoid)
	{
		SetSprite(CurrentApop);
	}

	private void OnQuestUpdated(int id)
	{
		SetSprite(CurrentApop);
	}

	private void OnQuestLoaded()
	{
		SetSprite(CurrentApop);
	}

	public void OnClick(Vector3 hitpoint)
	{
		if (this.Click != null)
		{
			this.Click();
		}
		else
		{
			Trigger();
		}
	}

	public void Trigger()
	{
		UINPCDialog.Load(Apops, Title, CameraSpot, FocusSpot);
	}

	public void OnDoubleClick()
	{
	}

	public void OnHover()
	{
		CursorManager.Instance.SetCursor(CursorManager.Icon.Talk);
	}

	private void SetSprite(NPCIA curApop)
	{
		if (myPlayerData != null && myPlayerData.HasTalkObjectiveInProgress(MapID, CellID, SpawnID))
		{
			base.gameObject.SetActive(value: true);
			GetComponent<MeshRenderer>().material.mainTexture = textures[6];
		}
		else if (curApop == null)
		{
			base.gameObject.SetActive(value: false);
		}
		else if (curApop.IsRequirementMet() && curApop.StaffAccessCheck())
		{
			base.gameObject.SetActive(value: true);
			GetComponent<MeshRenderer>().material.mainTexture = textures[(int)curApop.ApopState];
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void OnPress(Game.InteractableRaycastResult raycastResult, bool state)
	{
	}

	public bool IsInteractable(Game.InteractableRaycastResult raycastResult)
	{
		return true;
	}

	public int GetPriority()
	{
		return 0;
	}

	public void OnSecondTouchPress(Game.InteractableRaycastResult raycastResult, bool isPressed)
	{
	}
}
