public static class AccessLevels
{
	public const int Player = 0;

	public const int Tester = 50;

	public const int WhiteHat = 55;

	public const int Moderator = 60;

	public const int Admin = 100;

	public const int SuperAdmin = 101;

	public static bool IsAdmin(Player player)
	{
		if (player.AccessLevel != 100)
		{
			return player.AccessLevel == 101;
		}
		return true;
	}

	public static bool IsTester(Player player)
	{
		if (player.AccessLevel != 50)
		{
			return player.AccessLevel == 55;
		}
		return true;
	}

	public static bool CanReceiveAdminMessages(Player player)
	{
		return IsAdmin(player);
	}

	public static bool CanReceiveErrorMessages(Player player)
	{
		if (!IsAdmin(player))
		{
			return IsTester(player);
		}
		return true;
	}
}
