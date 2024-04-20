using System.Collections.Generic;

public class Account
{
	public int UserID;

	public string strToken;

	public int AccessLevel;

	public string strCountryCode;

	public int LevelCap;

	public long EventBitTracker;

	public string strRegion;

	public List<Player> chars = new List<Player>();

	public Dictionary<string, BundleInfo> bundles = new Dictionary<string, BundleInfo>();

	public bool IsDcPromotion;

	public CellData CharSelectData;

	public BundleInfo Characters_Bundle
	{
		get
		{
			if (bundles.ContainsKey("Characters"))
			{
				return bundles["Characters"];
			}
			return null;
		}
	}

	public BundleInfo GameAssets_Bundle
	{
		get
		{
			if (bundles.ContainsKey("GameAssets"))
			{
				return bundles["GameAssets"];
			}
			return null;
		}
	}

	public BundleInfo Maps_Bundle
	{
		get
		{
			if (bundles.ContainsKey("Maps"))
			{
				return bundles["Maps"];
			}
			return null;
		}
	}
}
