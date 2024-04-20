using System;

public class GameTime
{
	private static DateTime login;

	private static DateTime server;

	private static DateTime client;

	public static float realtimeSinceServerStartup => (float)((login - server).TotalSeconds + (DateTime.UtcNow - client).TotalSeconds);

	public static DateTime ServerTime => login.AddSeconds((DateTime.UtcNow - client).TotalSeconds);

	public static void Init(DateTime server, DateTime login)
	{
		GameTime.server = server;
		GameTime.login = login;
		client = DateTime.UtcNow;
	}
}
