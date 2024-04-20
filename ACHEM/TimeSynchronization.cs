using System;
using UnityEngine;

public abstract class TimeSynchronization : MonoBehaviour
{
	public float EventTime;

	public float CycleTime;

	protected virtual void Start()
	{
		DateTime serverTime = GameTime.ServerTime;
		DateTime dateTime = new DateTime(serverTime.Year, serverTime.Month, serverTime.Day);
		float num = (float)(serverTime - dateTime).TotalSeconds % CycleTime;
		float num2 = EventTime - num;
		if (num2 < 0f)
		{
			SetState(0f - num2);
			num2 += CycleTime;
		}
		InvokeRepeating("TriggerEvent", num2, CycleTime);
	}

	private void TriggerEvent()
	{
		SetState(0f);
	}

	protected abstract void SetState(float time);
}
