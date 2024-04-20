using System.Collections.Generic;

public class DungeonData
{
	public class DungeonMap
	{
		public int ID;

		public string Name;

		public string DisplayName;

		public string Description;

		public int MinLevel;

		public int MaxUsers;

		public bool bStaff;

		public bool bActive;

		public int upgradeRestriction;

		public int levelRestriction;

		public int powerRestriction;

		public int questRestriction;

		public int questValue;

		public string RequirementText;

		public bool IsScaling;

		public bool IsChallenge;

		public bool IsSeasonal;

		public bool InvertDynamicScaling;

		public bool IsDungeon;

		public bool IsDynamicScaling
		{
			get
			{
				if (!IsDungeon || InvertDynamicScaling)
				{
					if (!IsDungeon)
					{
						return InvertDynamicScaling;
					}
					return false;
				}
				return true;
			}
		}
	}

	public DungeonMap map;

	public List<Item> items = new List<Item>();
}
