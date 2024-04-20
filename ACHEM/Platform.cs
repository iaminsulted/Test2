public static class Platform
{
	private static bool isIOS = false;

	private static bool isAndroid = false;

	private static bool isPC = true;

	private static bool isMac = false;

	private static bool isLinux = false;

	private static bool isWebGL = false;

	private static bool isEditor = false;

	public static bool IsIOS => isIOS;

	public static bool IsAndroid => isAndroid;

	public static bool IsMobile
	{
		get
		{
			if (!IsAndroid)
			{
				return IsIOS;
			}
			return true;
		}
	}

	public static bool IsPC => isPC;

	public static bool IsMac => isMac;

	public static bool IsLinux => isLinux;

	public static bool IsDesktop
	{
		get
		{
			if (!IsPC && !IsMac)
			{
				return IsLinux;
			}
			return true;
		}
	}

	public static bool IsWebGL => isWebGL;

	public static bool IsEditor => isEditor;

	public static string GetPlatformName()
	{
		if (IsIOS)
		{
			return "iOS";
		}
		if (IsAndroid)
		{
			return "Android";
		}
		if (IsWebGL)
		{
			return "Web";
		}
		if (IsMac)
		{
			return "Mac";
		}
		if (IsPC)
		{
			return "Windows";
		}
		if (IsEditor)
		{
			return "Editor";
		}
		return "Unknown";
	}
}
