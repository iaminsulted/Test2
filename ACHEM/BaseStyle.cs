public class BaseStyle
{
	public int ID;

	public int QuestStringIndex;

	public int QuestStringValue;

	public string BitflagName;

	public byte BitflagIndex;

	public bool DontHideWhenLocked;

	public string RequirementText;

	public bool HasRequirement()
	{
		if (string.IsNullOrEmpty(BitflagName) || BitflagIndex <= 0)
		{
			return QuestStringIndex > 0;
		}
		return true;
	}
}
