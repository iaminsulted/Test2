using System;
using System.Collections.Generic;
using UnityEngine;

public class Setting<T> where T : IComparable, IConvertible
{
	private static List<Setting<T>> settings = new List<Setting<T>>();

	public readonly T Default;

	private readonly string playerPref;

	private T lastValue { get; set; }

	private T value { get; set; }

	private event Action<T> saveDelegate;

	public Setting(string playerPref, T defaultValue, Action<T> saveDelegate = null)
	{
		this.playerPref = playerPref;
		Default = defaultValue;
		lastValue = defaultValue;
		value = defaultValue;
		this.saveDelegate = saveDelegate;
		settings.Add(this);
	}

	public static implicit operator T(Setting<T> setting)
	{
		return setting.value;
	}

	private void LoadValueFromPlayerPref()
	{
		if (PlayerPrefs.HasKey(playerPref))
		{
			if (typeof(T) == typeof(int))
			{
				value = (T)(object)PlayerPrefs.GetInt(playerPref);
			}
			else if (typeof(T) == typeof(float))
			{
				value = (T)(object)PlayerPrefs.GetFloat(playerPref);
			}
			else if (typeof(T) == typeof(string))
			{
				value = (T)(object)PlayerPrefs.GetString(playerPref);
			}
			else if (typeof(T) == typeof(bool))
			{
				value = (T)(object)(PlayerPrefs.GetInt(playerPref) == 1);
			}
		}
	}

	public void ResetToDefault(bool updatePref, bool savePrefsToDisk, bool broadcastUpdate)
	{
		Set(Default, updatePref, savePrefsToDisk, broadcastUpdate);
	}

	public void Set(T newValue, bool updatePref = true, bool savePrefsToDisk = true, bool broadcastUpdate = true)
	{
		value = newValue;
		if (updatePref)
		{
			SaveToPrefs(savePrefsToDisk);
		}
		if (broadcastUpdate)
		{
			BroadcastUpdate();
		}
	}

	public void SaveToPrefs(bool savePrefsToDisk = true)
	{
		if (typeof(T) == typeof(int))
		{
			PlayerPrefs.SetInt(playerPref, (int)Convert.ChangeType(value, typeof(int)));
		}
		else if (typeof(T) == typeof(float))
		{
			PlayerPrefs.SetFloat(playerPref, (float)Convert.ChangeType(value, typeof(float)));
		}
		else if (typeof(T) == typeof(string))
		{
			PlayerPrefs.SetString(playerPref, (string)Convert.ChangeType(value, typeof(string)));
		}
		else if (typeof(T) == typeof(bool))
		{
			PlayerPrefs.SetInt(playerPref, (int)Convert.ChangeType((value.CompareTo(true) == 0) ? 1 : 0, typeof(int)));
		}
		if (savePrefsToDisk)
		{
			PlayerPrefs.Save();
		}
	}

	public void BroadcastUpdate()
	{
		this.saveDelegate?.Invoke(value);
	}

	public static void LoadAllValuesFromPlayerPrefs()
	{
		foreach (Setting<T> setting in settings)
		{
			setting.LoadValueFromPlayerPref();
			setting.BroadcastUpdate();
		}
	}
}
