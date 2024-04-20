using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientTrigger : InteractiveObject
{
	public List<ClientTriggerAction> CTActions = new List<ClientTriggerAction>();

	public ClientTriggerStateType StateType = ClientTriggerStateType.Cell;

	public static Dictionary<int, ClientTrigger> Map = new Dictionary<int, ClientTrigger>();

	private Coroutine respawnIn;

	public override void Awake()
	{
		if (IsSingleUse)
		{
			ChangeState = true;
			RespawnTime = 0;
		}
		base.Awake();
		float savedState = GetSavedState();
		if (savedState > 0f)
		{
			if (savedState > GameTime.realtimeSinceServerStartup)
			{
				State = 0;
				if (RespawnTime > 0)
				{
					respawnIn = StartCoroutine(RespawnIn(savedState - GameTime.realtimeSinceServerStartup));
				}
			}
			else
			{
				State = 1;
			}
		}
		base.IsActive = IsInteractive();
		OnInitialize(State);
		AddAreaIDs();
		while (Map.ContainsKey(ID))
		{
			ID++;
		}
		Map[ID] = this;
	}

	public override void OnDestroy()
	{
		Map.Remove(ID);
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
			_ = nPCIAAction.Action;
		}
		foreach (NPCIA child in npcia.children)
		{
			RecursiveAddArea(child);
		}
	}

	public override void Trigger(bool checkRequirements)
	{
		if (!CanInteract())
		{
			return;
		}
		if (checkRequirements)
		{
			if (!IsInteractive())
			{
				return;
			}
		}
		else if (State == 0)
		{
			return;
		}
		if (ChangeState)
		{
			SetState((State != 1) ? ((byte)1) : ((byte)0), ID);
		}
		foreach (ClientTriggerAction cTAction in CTActions)
		{
			cTAction.Execute();
		}
		if (TriggerAudioSource != null)
		{
			TriggerAudioSource.Play();
		}
		else if (!string.IsNullOrEmpty(SfxTrigger))
		{
			AudioManager.Play2DSFX(SfxTrigger);
		}
		if (!string.IsNullOrEmpty(Animation))
		{
			Entities.Instance.me.PlayAnimation(EntityAnimations.Get(Animation), 0f, isCancellableByMovement: true);
		}
	}

	public override void SetState(byte state, int entityId)
	{
		base.SetState(state, entityId);
		if (state == 1)
		{
			if (respawnIn != null)
			{
				StopCoroutine(respawnIn);
			}
			ClearSavedState();
		}
		else if (RespawnTime > 0)
		{
			respawnIn = StartCoroutine(RespawnIn(RespawnTime));
			SetSavedState(GameTime.realtimeSinceServerStartup + (float)RespawnTime);
		}
		else
		{
			SetSavedState(float.MaxValue);
		}
	}

	private float GetSavedState()
	{
		if (StateType == ClientTriggerStateType.Session)
		{
			if (!Session.Has(GetSessionDataID()))
			{
				return -1f;
			}
			return Session.Get(GetSessionDataID());
		}
		return -1f;
	}

	private void SetSavedState(float timestamp)
	{
		if (StateType == ClientTriggerStateType.Session)
		{
			Session.Set(GetSessionDataID(), timestamp);
		}
	}

	private void ClearSavedState()
	{
		if (StateType == ClientTriggerStateType.Session)
		{
			Session.Clear(GetSessionDataID());
		}
	}

	private IEnumerator RespawnIn(float time)
	{
		yield return new WaitForSeconds(time);
		SetState(1, ID);
	}

	public string GetSessionDataID()
	{
		int num = ((ID == -1) ? GetInstanceID() : ID);
		return "Area: " + Game.CurrentAreaID + " Cell: " + Game.CurrentCellID + " ClientTrigger: " + num;
	}
}
