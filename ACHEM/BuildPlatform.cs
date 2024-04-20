public class BuildPlatform
{
	public enum BuildPlatforms
	{
		UNKNOWN,
		IOS,
		ANDROID,
		STEAM_PC,
		STEAM_MAC,
		STEAM_LINUX,
		PC,
		MAC,
		LINUX,
		WEBGL,
		XBOX,
		SONY,
		EDITOR
	}

	private static BuildPlatforms SteamPlatform => BuildPlatforms.STEAM_PC;

	private static BuildPlatforms StandalonePlatform => BuildPlatforms.PC;

	public static BuildPlatforms Get => SteamPlatform;
}
