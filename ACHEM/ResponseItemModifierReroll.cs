internal class ResponseItemModifierReroll : Response
{
	public ItemModifier modifier;

	public ResponseItemModifierReroll(ItemModifier mod)
	{
		modifier = mod;
		type = 10;
		cmd = 20;
	}
}
