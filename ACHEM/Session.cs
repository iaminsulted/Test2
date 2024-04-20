using System;
using System.Collections.Generic;

public static class Session
{
	public static bool IsGuest;

	public static bool IsReconnectable;

	public static bool AppleUserHasAQ3DLogin;

	public static Account Account;

	public static MyPlayerData MyPlayerData;

	private static Dictionary<string, float> SessionData = new Dictionary<string, float>();

	public static string SummonWord;

	public static GotoInfo pendingGoto;

	public static ServerInfo pendingServer;

	public static bool AutoConnectServer;

	public static event Action<string> OnSessionDataUpdated;

	public static void Set(string key, float timeStamp)
	{
		if (!SessionData.ContainsKey(key))
		{
			SessionData.Add(key, timeStamp);
			if (Session.OnSessionDataUpdated != null)
			{
				Session.OnSessionDataUpdated(key);
			}
		}
	}

	public static bool Has(string key)
	{
		return SessionData.ContainsKey(key);
	}

	public static float Get(string key)
	{
		if (SessionData.ContainsKey(key))
		{
			return SessionData[key];
		}
		throw new Exception("Key not found.");
	}

	public static void Clear(string key)
	{
		if (SessionData.ContainsKey(key))
		{
			SessionData.Remove(key);
		}
	}

	public static void Clear()
	{
		SessionData.Clear();
		SummonWord = null;
		AutoConnectServer = false;
	}
}
