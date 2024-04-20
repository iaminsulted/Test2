public class SavedPrefs
{
	public enum LastLoginMethod
	{
		None,
		AQ3D,
		Facebook,
		Guest,
		Apple
	}

	public const string PREFS_USERNAME = "USERNAME";

	public const string PREFS_PASSWORD = "PASSWORD";

	public const string PREFS_PASSWORD_ENCODE = "PASSWORDENCODE";

	public const string PREFS_GUEST_GUID = "GUESTGUID";

	public const string PREFS_GUEST_GUID_CONVERTED = "GUESTGUIDCONVERTED";

	public const string PREFS_GUEST_GUID_CHAR_NAME = "GuestGuidCharName";

	public const string PREFS_FACEBOOK_ACCESS_TOKEN = "FBAT";

	public const string PREFS_APPLE_JSON_WEB_TOKEN = "JWT";

	public const string PREFS_CURRENT_SERVER = "CurrentServer";

	public const string PREFS_CURRENT_SERVER_ID = "CurrentServerID";

	public const string PREFS_ENVIRONMENT = "Environment";

	public const string PREFS_LAST_LOGIN_METHOD = "LastLoginMethod";

	public const string PREFS_DISABLE_AUTOLOGIN = "DisableAutoLogin";

	public const string PREFS_AQ3D_USER_ID = "HasAQ3DAccount";

	public const string PREFS_APPLICATION_HAS_LAUNCHED = "FIRSTLAUNCH";

	public const string PREFS_LINK_SOURCE = "LINKSOURCE";

	public const string PREFS_CHAT_ERROR = "CHATERROR";
}
