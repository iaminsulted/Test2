using System;
using UnityEngine;

[AddComponentMenu("Interactivity/Client Triggers/Time Synchronization Client Trigger")]
public class TimeSynchronizationClientTrigger : ClientTrigger
{
	public float CycleTime = 10f;

	public float EventTime;

	public float EventDuration;

	public bool IsWithinEventDuration
	{
		get
		{
			float timeInCurrentCycle = GetTimeInCurrentCycle();
			if (timeInCurrentCycle >= EventTime)
			{
				return timeInCurrentCycle <= EventTime + EventDuration;
			}
			return false;
		}
	}

	public override void Awake()
	{
		base.Awake();
		float timeInCurrentCycle = GetTimeInCurrentCycle();
		float num = EventTime - timeInCurrentCycle;
		if (num < 0f)
		{
			num += CycleTime;
		}
		InvokeRepeating("TriggerEvent", num, CycleTime);
	}

	private void OnEnable()
	{
	}

	public float GetTimeInCurrentCycle()
	{
		DateTime serverTime = GameTime.ServerTime;
		DateTime dateTime = new DateTime(serverTime.Year, serverTime.Month, serverTime.Day);
		return (float)(serverTime - dateTime).TotalSeconds % CycleTime;
	}

	private void TriggerEvent()
	{
		Trigger(checkRequirements: true);
	}
}
