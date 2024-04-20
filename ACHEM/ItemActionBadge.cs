public class ItemActionBadge : ItemAction
{
	public int BadgeID;

	public ItemActionBadge(int badgeID)
		: base(ItemActionType.Badge)
	{
		BadgeID = badgeID;
	}
}
