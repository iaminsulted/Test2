using System.Collections.Generic;
using UnityEngine;

public class ServerInfo
{
	public int ID;

	public int SortIndex;

	public string Name;

	public string HostName;

	public int Port;

	public int WsPort;

	public int UserCount;

	public int MaxUsers;

	public bool State;

	public bool Chat;

	public string Language;

	public int AccessLevel;

	public string Region;

	public static List<ServerInfo> Servers = new List<ServerInfo>();

	public bool IsLocalhost => Name == "Localhost";

	public string DisplayName
	{
		get
		{
			if (!IsLocalhost)
			{
				return Name;
			}
			return Name + " - " + Main.Environment;
		}
	}

	public int ServerPort
	{
		get
		{
			if (!Platform.IsWebGL)
			{
				return Port;
			}
			return WsPort;
		}
	}

	public void Save()
	{
		PlayerPrefs.SetString("CurrentServer", Name);
		PlayerPrefs.SetInt("CurrentServerID", ID);
		PlayerPrefs.Save();
	}
}
