using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMachine : InteractiveObject
{
	public static Dictionary<int, BaseMachine> Map = new Dictionary<int, BaseMachine>();

	public int MachineID = -1;

	protected SFXPlayer sfxPlayer;

	public override void Awake()
	{
		base.Awake();
		if (ID >= 0)
		{
			AddMachineToMap();
		}
		sfxPlayer = base.gameObject.GetComponent<SFXPlayer>();
	}

	protected void playSfx()
	{
		if (sfxPlayer != null && sfxPlayer.MixerTracks.Count > 0)
		{
			sfxPlayer.Play(sfxPlayer.MixerTracks[0]);
		}
	}

	public static BaseMachine GetMachineByMachineID(int machineId)
	{
		foreach (KeyValuePair<int, BaseMachine> item in Map)
		{
			if (item.Value.MachineID == machineId)
			{
				return item.Value;
			}
		}
		return null;
	}

	public static int GetMachineIDByMapEditorMachineID(int mapEditorMachineId)
	{
		foreach (KeyValuePair<int, BaseMachine> item in Map)
		{
			if (item.Value.MachineID == mapEditorMachineId)
			{
				return item.Key;
			}
		}
		return -1;
	}

	public void AddMachineToMap()
	{
		if (Map.ContainsKey(ID))
		{
			Debug.LogWarning("Warning: Machine ID: " + ID + " already exists!");
		}
		Map[ID] = this;
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	protected override bool IsRequirementMet()
	{
		if (IsSingleUse && Game.Instance.UsedMachines.Contains(ID))
		{
			return false;
		}
		return base.IsRequirementMet();
	}
}
