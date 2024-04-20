using System.Collections.Generic;

public class ApiResponseLootBoxData : APIResponse
{
	public int ItemID;

	public string Name;

	public string Info;

	public int Rarity;

	public Dictionary<int, List<Item>> items;
}
