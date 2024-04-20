using System.Linq;

public class Badge : IUIItem
{
	public int ID;

	public string Name;

	public string Description;

	public string BitFlagName;

	public int BitFlagIndex;

	public int BadgeCategoryID;

	public int TitleCategoryID;

	public string ImageUrl;

	public string Title;

	public bool HideWhenLocked;

	public bool isNew;

	public virtual string ToolTipText
	{
		get
		{
			string text = "[894600]" + Name + "[-]";
			if (!string.IsNullOrEmpty(Description))
			{
				text = text + "\n[000000]" + Description + "[-]";
			}
			return text;
		}
	}

	public string Icon => "";

	public BadgeTitleCategory Category
	{
		get
		{
			if (!Badges.Categories.TryGetValue(TitleCategoryID, out var value))
			{
				return Badges.Categories.First().Value;
			}
			return value;
		}
	}

	public bool IsDaily
	{
		get
		{
			if (!string.IsNullOrEmpty(BitFlagName))
			{
				return BitFlagName.Substring(0, 2) == "id";
			}
			return false;
		}
	}
}
