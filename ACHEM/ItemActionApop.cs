public class ItemActionApop : ItemAction
{
	public int apopID;

	public ItemActionApop(int apopID)
		: base(ItemActionType.Apop)
	{
		this.apopID = apopID;
	}
}
