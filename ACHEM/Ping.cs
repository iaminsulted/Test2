using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ping : MonoBehaviour
{
	private const int Timestamps_To_Keep = 7;

	private TimeStamp[] timeStamps = new TimeStamp[7];

	private int counter;

	private int IDs = int.MinValue;

	private TimeStamp temporary;

	private bool isResetting;

	private float ping;

	private Coroutine delayed;

	private float timeWhenFrameStarted;

	private float sessionPing;

	private int sessionPingCount;

	private void Start()
	{
		float @float = PlayerPrefs.GetFloat("SessionPing", -1f);
		SendAvgPing(@float);
		InvokeRepeating("CalculateAndSendAvgPing", 300f, 300f);
	}

	private void CalculateAndSendAvgPing()
	{
		float incomingPing = sessionPing / (float)sessionPingCount;
		SendAvgPing(incomingPing);
	}

	private void SendAvgPing(float incomingPing)
	{
		if (incomingPing > 0f)
		{
			AEC.getInstance().sendRequest(new RequestPingSession(incomingPing));
		}
	}

	private void Update()
	{
		timeWhenFrameStarted = GameTime.realtimeSinceServerStartup;
	}

	public void SendPingRequest()
	{
		isResetting = false;
		AEC.getInstance().sendRequest(new RequestPing(IDs));
		timeStamps[counter] = new TimeStamp(GameTime.realtimeSinceServerStartup, IDs++);
		delayed = StartCoroutine(CheckForDelay());
		Invoke("SendPingRequest", 3f);
	}

	public void OnPingResponse(int ID)
	{
		if (isResetting)
		{
			isResetting = false;
		}
		else
		{
			List<TimeStamp> list = timeStamps.Where((TimeStamp t) => t.ID == ID).ToList();
			if (list.Count != 0)
			{
				temporary = list.FirstOrDefault();
				temporary.timeArrived = GameTime.realtimeSinceServerStartup - ((GameTime.realtimeSinceServerStartup - timeWhenFrameStarted > 0.1f) ? (GameTime.realtimeSinceServerStartup - timeWhenFrameStarted) : 0f);
				if (temporary.GetPing() > 0f)
				{
					AddToAverage();
					sessionPingCount++;
					sessionPing += temporary.GetPing();
				}
			}
		}
		StopAllCoroutines();
	}

	private void AddToAverage()
	{
		timeStamps[counter++] = temporary;
		counter %= timeStamps.Length;
		CalculatePing();
	}

	private void CalculatePing()
	{
		float num = 0f;
		TimeStamp[] array = timeStamps;
		for (int i = 0; i < array.Length; i++)
		{
			TimeStamp timeStamp = array[i];
			if (timeStamp.timeArrived != -1f)
			{
				num += timeStamp.GetPing();
			}
		}
		int num2 = Math.Min(timeStamps.Length, sessionPingCount).Clamp(1, timeStamps.Length);
		ping = num / (float)num2 * 1000f;
		if (ping > (float)Session.MyPlayerData.LatencyDisconnectThreshold)
		{
			Debug.LogWarning("Entropy is looking for this. Ping: " + ping);
		}
	}

	public float GetPing()
	{
		return ping;
	}

	public void ResetPing()
	{
		isResetting = true;
		ping = 0f;
		for (int i = 0; i < timeStamps.Length; i++)
		{
			timeStamps[i].timeArrived = 0f;
			timeStamps[i].timeSent = 0f;
		}
		StopAllCoroutines();
	}

	private IEnumerator CheckForDelay()
	{
		yield return new WaitForSecondsRealtime((float)Session.MyPlayerData.LatencyNotifyThreshold * 2f / 1000f);
		ping = float.MaxValue;
		yield return new WaitForSecondsRealtime((float)Session.MyPlayerData.LatencyDisconnectThreshold / 1000f - (float)Session.MyPlayerData.LatencyNotifyThreshold * 2f / 1000f);
		Game.Instance.Logout("No Internet", "No Internet connection available. Please sure your device is connected to the Internet.");
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}

	private void OnDestroy()
	{
		PlayerPrefs.SetFloat("SessionPing", sessionPing / (float)sessionPingCount);
	}
}
